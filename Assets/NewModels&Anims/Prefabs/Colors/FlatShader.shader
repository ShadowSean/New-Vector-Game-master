Shader "URP/FlatShadingURP_Pop_Texture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _LightIntensity ("Light Intensity", Range(0,3)) = 1.2
        _AmbientIntensity ("Ambient Intensity", Range(0,2)) = 0.4
        _Contrast ("Contrast Boost", Range(0,2)) = 1.0
        _EdgeSharpness ("Edge Sharpness", Range(1,8)) = 1

    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 posWS       : TEXCOORD1;
                float2 uv          : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _LightIntensity;
                float _AmbientIntensity;
                float _Contrast;
                float _EdgeSharpness;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.posWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Flat-shaded normal
                float3 e1 = ddx(IN.posWS);
                float3 e2 = ddy(IN.posWS);
                half3 faceNormalWS = normalize(cross(e1, e2));

                // Light
                Light mainLight = GetMainLight();
                half3 lightDir = normalize(mainLight.direction);

                half NdotL = saturate(dot(faceNormalWS, -lightDir));
                NdotL = pow(NdotL, _EdgeSharpness);


                // Sample texture
                float4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);

                // Combine texture with base color
                half3 baseCol = tex.rgb * _BaseColor.rgb;

                // Diffuse
                half3 litColor = baseCol * NdotL * _LightIntensity;

                // Ambient
                half3 ambient = baseCol * _AmbientIntensity;

                // Combine lighting
                half3 color = litColor + ambient;

                // Contrast boost
                color = pow(color, 1.0 / _Contrast);

                return half4(color, tex.a * _BaseColor.a);
            }

            ENDHLSL
        }
    }
}
