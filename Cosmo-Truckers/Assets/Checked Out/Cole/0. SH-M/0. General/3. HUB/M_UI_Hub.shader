Shader "SH_UI_MovingTexture"
{
    Properties
    {
        [NoScaleOffset] _MainTex("MainTex", 2D) = "white" {}
        [NoScaleOffset]_UniqueTexture("UniqueTexture", 2D) = "white" {}
        _ScrollSpeed("ScrollSpeed", Range(0, 1)) = 0.1
        _ScrollDirection("ScrollDirection", Vector) = (3, 1, 0, 0)
        _TilingScale("TilingScale", Vector) = (2, 1, 0, 0)
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}


        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255
        _ColorMask("Color Mask", Float) = 15

    }
        SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "UniversalMaterialType" = "Lit"
            "Queue" = "Transparent"
            "ShaderGraphShader" = "true"
            "ShaderGraphTargetId" = ""
        }

        Stencil
        {
        Ref[_Stencil]
        Comp[_StencilComp]
        Pass[_StencilOp]
        ReadMask[_StencilReadMask]
        WriteMask[_StencilWriteMask]
        }

        Pass
        {
            Name "Sprite Lit"
            Tags
            {
                "LightMode" = "Universal2D"
            }

        // Render State
        Cull Off
    Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
    ZTest LEqual
    ZWrite Off

        // Debug
        // <None>

        // --------------------------------------------------
        // Pass

        HLSLPROGRAM

        // Pragmas
        #pragma target 2.0
    #pragma exclude_renderers d3d11_9x
    #pragma vertex vert
    #pragma fragment frag

        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>

        // Keywords
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_0
    #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_1
    #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_2
    #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_3
    #pragma multi_compile_fragment _ DEBUG_DISPLAY
        // GraphKeywords: <None>

        // Defines
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define VARYINGS_NEED_SCREENPOSITION
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITELIT
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

        // Includes
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */

        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

        struct Attributes
    {
         float3 positionOS : POSITION;
         float3 normalOS : NORMAL;
         float4 tangentOS : TANGENT;
         float4 uv0 : TEXCOORD0;
         float4 color : COLOR;
        #if UNITY_ANY_INSTANCING_ENABLED
         uint instanceID : INSTANCEID_SEMANTIC;
        #endif
    };
    struct Varyings
    {
         float4 positionCS : SV_POSITION;
         float3 positionWS;
         float4 texCoord0;
         float4 color;
         float4 screenPosition;
        #if UNITY_ANY_INSTANCING_ENABLED
         uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
         uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
         uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
         FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };
    struct SurfaceDescriptionInputs
    {
         float4 uv0;
         float3 TimeParameters;
    };
    struct VertexDescriptionInputs
    {
         float3 ObjectSpaceNormal;
         float3 ObjectSpaceTangent;
         float3 ObjectSpacePosition;
    };
    struct PackedVaryings
    {
         float4 positionCS : SV_POSITION;
         float3 interp0 : INTERP0;
         float4 interp1 : INTERP1;
         float4 interp2 : INTERP2;
         float4 interp3 : INTERP3;
        #if UNITY_ANY_INSTANCING_ENABLED
         uint instanceID : CUSTOM_INSTANCE_ID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
         uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
         uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
         FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
        #endif
    };

        PackedVaryings PackVaryings(Varyings input)
    {
        PackedVaryings output;
        ZERO_INITIALIZE(PackedVaryings, output);
        output.positionCS = input.positionCS;
        output.interp0.xyz = input.positionWS;
        output.interp1.xyzw = input.texCoord0;
        output.interp2.xyzw = input.color;
        output.interp3.xyzw = input.screenPosition;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }

    Varyings UnpackVaryings(PackedVaryings input)
    {
        Varyings output;
        output.positionCS = input.positionCS;
        output.positionWS = input.interp0.xyz;
        output.texCoord0 = input.interp1.xyzw;
        output.color = input.interp2.xyzw;
        output.screenPosition = input.interp3.xyzw;
        #if UNITY_ANY_INSTANCING_ENABLED
        output.instanceID = input.instanceID;
        #endif
        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
        #endif
        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
        #endif
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        output.cullFace = input.cullFace;
        #endif
        return output;
    }


    // --------------------------------------------------
    // Graph

    // Graph Properties
    CBUFFER_START(UnityPerMaterial)
float4 _MainTex_TexelSize;
float _ScrollSpeed;
float2 _ScrollDirection;
float4 _UniqueTexture_TexelSize;
float2 _TilingScale;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);
TEXTURE2D(_UniqueTexture);
SAMPLER(sampler_UniqueTexture);

// Graph Includes
// GraphIncludes: <None>

// -- Property used by ScenePickingPass
#ifdef SCENEPICKINGPASS
float4 _SelectionID;
#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Graph Functions

void Unity_Multiply_float_float(float A, float B, out float Out)
{
    Out = A * B;
}

void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
{
    Out = A * B;
}

void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
{
    Out = UV * Tiling + Offset;
}

void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
{
    RGBA = float4(R, G, B, A);
    RGB = float3(R, G, B);
    RG = float2(R, G);
}

/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

// Graph Vertex
struct VertexDescription
{
    float3 Position;
    float3 Normal;
    float3 Tangent;
};

VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
{
    VertexDescription description = (VertexDescription)0;
    description.Position = IN.ObjectSpacePosition;
    description.Normal = IN.ObjectSpaceNormal;
    description.Tangent = IN.ObjectSpaceTangent;
    return description;
}

    #ifdef FEATURES_GRAPH_VERTEX
Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
{
return output;
}
#define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
#endif

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float Alpha;
    float4 SpriteMask;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_ac961c9859004e1cb4df77a49c5e64b4_Out_0 = UnityBuildTexture2DStructNoScale(_UniqueTexture);
    float2 _Property_05fcc2f014044718a0dda62f4b9ec2a8_Out_0 = _TilingScale;
    float2 _Property_db983ba1bc1e4a7dbb10a097bae7514d_Out_0 = _ScrollDirection;
    float _Split_cdc6c0effcb946d39d816d6267fb639c_R_1 = _Property_db983ba1bc1e4a7dbb10a097bae7514d_Out_0[0];
    float _Split_cdc6c0effcb946d39d816d6267fb639c_G_2 = _Property_db983ba1bc1e4a7dbb10a097bae7514d_Out_0[1];
    float _Split_cdc6c0effcb946d39d816d6267fb639c_B_3 = 0;
    float _Split_cdc6c0effcb946d39d816d6267fb639c_A_4 = 0;
    float _Property_ddfe3b56adf64a4a92449991bf93c29e_Out_0 = _ScrollSpeed;
    float _Multiply_bf8d87b56e6649c2ae83aefc57d403d6_Out_2;
    Unity_Multiply_float_float(IN.TimeParameters.x, _Property_ddfe3b56adf64a4a92449991bf93c29e_Out_0, _Multiply_bf8d87b56e6649c2ae83aefc57d403d6_Out_2);
    float4 _Multiply_a48234a34b0f4da389782d572538f5f7_Out_2;
    Unity_Multiply_float4_float4((_Split_cdc6c0effcb946d39d816d6267fb639c_R_1.xxxx), (_Multiply_bf8d87b56e6649c2ae83aefc57d403d6_Out_2.xxxx), _Multiply_a48234a34b0f4da389782d572538f5f7_Out_2);
    float4 _Multiply_3a5d8375d3144f9c8b61ee3e9fa958fc_Out_2;
    Unity_Multiply_float4_float4((_Split_cdc6c0effcb946d39d816d6267fb639c_G_2.xxxx), (_Multiply_bf8d87b56e6649c2ae83aefc57d403d6_Out_2.xxxx), _Multiply_3a5d8375d3144f9c8b61ee3e9fa958fc_Out_2);
    float2 _Vector2_a1c5d9a8828f4ceab8ace5c490322ca9_Out_0 = float2((_Multiply_a48234a34b0f4da389782d572538f5f7_Out_2).x, (_Multiply_3a5d8375d3144f9c8b61ee3e9fa958fc_Out_2).x);
    float2 _TilingAndOffset_a2d82528d5c64b9190f9d4811691a91d_Out_3;
    Unity_TilingAndOffset_float(IN.uv0.xy, _Property_05fcc2f014044718a0dda62f4b9ec2a8_Out_0, _Vector2_a1c5d9a8828f4ceab8ace5c490322ca9_Out_0, _TilingAndOffset_a2d82528d5c64b9190f9d4811691a91d_Out_3);
    float4 _SampleTexture2D_2ff3c2c93878457881ff144050529bcc_RGBA_0 = SAMPLE_TEXTURE2D(_Property_ac961c9859004e1cb4df77a49c5e64b4_Out_0.tex, _Property_ac961c9859004e1cb4df77a49c5e64b4_Out_0.samplerstate, _Property_ac961c9859004e1cb4df77a49c5e64b4_Out_0.GetTransformedUV(_TilingAndOffset_a2d82528d5c64b9190f9d4811691a91d_Out_3));
    float _SampleTexture2D_2ff3c2c93878457881ff144050529bcc_R_4 = _SampleTexture2D_2ff3c2c93878457881ff144050529bcc_RGBA_0.r;
    float _SampleTexture2D_2ff3c2c93878457881ff144050529bcc_G_5 = _SampleTexture2D_2ff3c2c93878457881ff144050529bcc_RGBA_0.g;
    float _SampleTexture2D_2ff3c2c93878457881ff144050529bcc_B_6 = _SampleTexture2D_2ff3c2c93878457881ff144050529bcc_RGBA_0.b;
    float _SampleTexture2D_2ff3c2c93878457881ff144050529bcc_A_7 = _SampleTexture2D_2ff3c2c93878457881ff144050529bcc_RGBA_0.a;
    float4 _Combine_e8b3e1ae4417406bb024b4cfe765ed50_RGBA_4;
    float3 _Combine_e8b3e1ae4417406bb024b4cfe765ed50_RGB_5;
    float2 _Combine_e8b3e1ae4417406bb024b4cfe765ed50_RG_6;
    Unity_Combine_float((_SampleTexture2D_2ff3c2c93878457881ff144050529bcc_RGBA_0).x, (_SampleTexture2D_2ff3c2c93878457881ff144050529bcc_RGBA_0).x, (_SampleTexture2D_2ff3c2c93878457881ff144050529bcc_RGBA_0).x, 0, _Combine_e8b3e1ae4417406bb024b4cfe765ed50_RGBA_4, _Combine_e8b3e1ae4417406bb024b4cfe765ed50_RGB_5, _Combine_e8b3e1ae4417406bb024b4cfe765ed50_RG_6);
    surface.BaseColor = _Combine_e8b3e1ae4417406bb024b4cfe765ed50_RGB_5;
    surface.Alpha = _SampleTexture2D_2ff3c2c93878457881ff144050529bcc_A_7;
    surface.SpriteMask = IsGammaSpace() ? float4(1, 1, 1, 1) : float4 (SRGBToLinear(float3(1, 1, 1)), 1);
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);







    output.uv0 = input.texCoord0;
    output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

    return output;
}

    // --------------------------------------------------
    // Main

    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteLitPass.hlsl"

    ENDHLSL
}
    }
        CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
        FallBack "Hidden/Shader Graph/FallbackError"
}