Shader "Custom/OutlineShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth("Outline Width", Range(0.0, 0.1)) = 0.01
        _OutlineTransparencyStart("Outline Transparency Start", Range(0.0, 1.0)) = 0.5
        _OutlineTransparencyEnd("Outline Transparency End", Range(0.0, 1.0)) = 0.1
        _PixelThreshold("Pixel Threshold", Range(0.0, 1.0)) = 0.5
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

            float _OutlineWidth;
            float _OutlineTransparencyStart;
            float _OutlineTransparencyEnd;
            float _PixelThreshold;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _Color;
            float4 _OutlineColor;

            fixed4 frag(v2f i) : SV_Target
            {
                float distance = 1 - saturate(length(i.uv - 0.5) / 0.5);
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;

                // Calculate outline based on distance from center
                float outline = smoothstep(_OutlineWidth, _OutlineWidth + _PixelThreshold, distance);
                fixed4 outlineCol = _OutlineColor;
                outlineCol.a *= lerp(_OutlineTransparencyStart, _OutlineTransparencyEnd, outline);

                // Blend outline with original color
                fixed4 finalCol = lerp(outlineCol, col, step(0.5, distance));

                return finalCol;
            }
            ENDCG
        }
    }
}