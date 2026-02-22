Shader "Alvaro/URP_LUTColorGrading"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LUT ("LUT", 2D) = "white" {}
        _Contribution ("Contribution", Range(0, 1)) = 1
    }

    SubShader
    {
        Cull Off ZWrite Off ZTest LEqual

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #define COLORS 32.0

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionHCS : SV_POSITION;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_LUT);
            SAMPLER(sampler_LUT);

            float4 _LUT_TexelSize;
            float _Contribution;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                
                float maxColor = COLORS - 1.0;
                
                float halfColX = 0.5 / _LUT_TexelSize.z;
                float halfColY = 0.5 / _LUT_TexelSize.w;
                float threshold = maxColor / COLORS;

                float xOffset = halfColX + col.r * threshold / COLORS;
                float yOffset = halfColY + col.g * threshold;
                float cell = floor(col.b * maxColor);

                float2 lutPos = float2(cell / COLORS + xOffset, yOffset);
                half4 gradedCol = SAMPLE_TEXTURE2D(_LUT, sampler_LUT, lutPos);

                return lerp(col, gradedCol, _Contribution);
            }

            ENDHLSL
        }
    }
}