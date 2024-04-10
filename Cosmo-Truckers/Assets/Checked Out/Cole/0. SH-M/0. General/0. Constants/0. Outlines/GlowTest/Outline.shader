Shader "Custom/SimpleOutlineShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineWidth("Outline Width", Range(0.0, 0.1)) = 0.01
    }

        SubShader
        {
            Tags { "Queue" = "Overlay" "RenderType" = "Transparent" }
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            Lighting Off

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _OutlineColor;
                float _OutlineWidth;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // Sample the texture
                    fixed4 texColor = tex2D(_MainTex, i.uv);

                // Calculate the average alpha value of neighboring pixels
                float alpha = texColor.a;
                float sumAlpha = 0;
                int count = 0;

                // Radius for sampling neighboring pixels
                float2 texelSize = 1 / _ScreenParams.zw;
                int radius = int(_OutlineWidth * texelSize.x);

                for (int x = -radius; x <= radius; x++)
                {
                    for (int y = -radius; y <= radius; y++)
                    {
                        float2 offset = float2(x, y) * texelSize;
                        float4 neighborColor = tex2D(_MainTex, i.uv + offset);
                        sumAlpha += neighborColor.a;
                        count++;
                    }
                }

                // Calculate the average alpha value
                float avgAlpha = sumAlpha / count;

                // Determine if the current pixel is part of the outline
                if (abs(alpha - avgAlpha) > 0.1)
                {
                    // Outline color
                    return _OutlineColor;
                }
                else
                {
                    // Inside the sprite, return original color
                    return texColor;
                }
            }
            ENDCG
        }
        }

            FallBack "Diffuse"
}