Shader "Unlit/Hollogram"
{
            Properties
            {
                _MainTex ("Main Texture", 2D) = "white" {} // Main texture
                _LineColor ("Line Color", Color) = (0, 1, 1, 1) // Color of the scan lines
                _FresnelColor ("Fresnel Color", Color) = (0, 0.8, 1, 1) // Color of the rim effect
                _RimIntensity ("Rim Intensity", Float) = 1.5 // Strength of the rim lighting
                _FresnelPower ("Fresnel Power", Range(1, 5)) = 2.0 // Power for the fresnel rim effect
                _LineSpeed ("Line Speed", Float) = 1.0 // Speed of the scanning lines
                _LineFrequency ("Line Frequency", Float) = 10.0 // Frequency of scan lines
                _Transparency ("Transparency", Range(0, 1)) = 0.5 // Transparency control
            }
            SubShader
            {
            Tags { "RenderPipeline" = "UniversalPipeline" "Queue" = "Transparent" "RenderType" = "Transparent" }

            // Transparency blending
            Blend SrcAlpha OneMinusSrcAlpha

            Pass
            {
                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

                // Vertex input structure
                struct Attributes
                {
                    float4 positionOS : POSITION; // Object space position
                    float3 normalOS : NORMAL; // Object space normal
                    float2 uv : TEXCOORD0; // Texture coordinates
                };
                // Data passed to fragment shader
                struct Varyings
                {
                    float4 positionHCS : SV_POSITION; // Clip-space position
                    float3 normalWS : TEXCOORD1; // World space normal
                    float3 viewDirWS : TEXCOORD2; // World space view direction
                    float2 uv : TEXCOORD0; // Texture coordinates
                };
                // Material properties
                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);
                float4 _LineColor;
                float4 _FresnelColor;
                float _RimIntensity;
                float _FresnelPower;
                float _LineSpeed;
                float _LineFrequency;
                float _Transparency;


            // Vertex shader
                Varyings vert(Attributes IN)
                {
                    Varyings OUT;
                    // Transform object space position to clip space
                    OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                    // Transform normal to world space
                    OUT.normalWS = normalize(TransformObjectToWorldNormal(IN.normalOS));
                    // Calculate view direction in world space
                    OUT.viewDirWS = normalize(GetWorldSpaceViewDir(IN.positionOS.xyz));
                    // Pass through the UV coordinates
                    OUT.uv = IN.uv;
                    return OUT;
                }
                // Fragment shader
                half4 frag(Varyings IN) : SV_Target
                {
                    // Sample the main texture
                    half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                    // Fresnel effect (rim lighting)
                    half3 normalWS = normalize(IN.normalWS);
                    half3 viewDirWS = normalize(IN.viewDirWS);
                    half fresnel = pow(1.0 - saturate(dot(viewDirWS, normalWS)), _FresnelPower);
                    half3 fresnelColor = _FresnelColor.rgb * fresnel * _RimIntensity;
                    // Scrolling lines effect
                    float lineValue = sin(IN.uv.y * _LineFrequency + _Time.y * _LineSpeed);
                    half3 lineColor = _LineColor.rgb * step(0.5, lineValue); // Creates sharp scan lines
                    // Combine the texture color, fresnel rim, and scan lines
                    half3 finalColor = texColor.rgb + fresnelColor + lineColor;
                    // Apply transparency
                    return half4(finalColor, _Transparency);
                }
                ENDHLSL
        }
    }
}
