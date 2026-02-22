Shader "Alvaro/URP/SimpleDiffuseWithAmbientAndSpecularURP"
{
    Properties
    {
        // Base color tint of the surface
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)

        // Base texture (albedo) applied to the model
        _MainTex ("Base Texture", 2D) = "white" {}

        // Color of the specular reflection
        _SpecColor ("Specular Color", Color) = (1, 1, 1, 1)

        // Controls the sharpness of the specular highlight (shininess exponent)
        _Shininess ("Shininess", Range(0.1, 100)) = 16
    }

    SubShader
    {
        // Tags tell Unity how and when to render this shader
        Tags { 
            "RenderPipeline" = "UniversalPipeline"    // Works with URP
            "RenderType" = "Opaque"                         // Opaque surface
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Include URP libraries for transforms and lighting
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // =============================
            //  Vertex Input (from the mesh)
            // =============================
            struct Attributes
            {
                float4 positionOS : POSITION;  // Object-space vertex position
                float3 normalOS   : NORMAL;    // Object-space surface normal
                float2 uv         : TEXCOORD0; // Texture coordinates
            };

            // ===================================
            //  Data passed to the Fragment Shader
            // ===================================
            struct Varyings
            {
                float4 positionHCS : SV_POSITION; // Clip-space position (for rasterization)
                float3 normalWS    : TEXCOORD1;   // World-space normal (for lighting)
                float3 viewDirWS   : TEXCOORD2;   // World-space view direction (for specular)
                float2 uv          : TEXCOORD0;   // Texture coordinates
            };

            // ===================================
            //  Texture and Sampler Declarations
            // ===================================
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            // ===================================
            //  Per-material properties (URP SRP Batcher)
            // ===================================
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;   // Base color tint
                float4 _SpecColor;   // Specular highlight color
                float  _Shininess;   // Controls specular sharpness
            CBUFFER_END

            // =============================
            //  Vertex Shader
            // =============================
            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                // Convert object-space position → clip-space for rendering
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);

                // Convert object-space normal → world-space for correct lighting
                OUT.normalWS = normalize(TransformObjectToWorldNormal(IN.normalOS));

                // Compute world-space view direction (camera position - world position)
                float3 worldPosWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.viewDirWS = normalize(GetCameraPositionWS() - worldPosWS);

                // Pass UV coordinates to fragment shader
                OUT.uv = IN.uv;

                return OUT;
            }

            // =============================
            //  Fragment Shader
            // =============================
            half4 frag (Varyings IN) : SV_Target
            {
                // --- Step 1: Sample the base texture ---
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);

                // --- Step 2: Get main directional light ---
                Light mainLight = GetMainLight();
                half3 lightDir = normalize(mainLight.direction);

                // --- Step 3: Normalize the surface normal ---
                half3 normalWS = normalize(IN.normalWS);

                // --- Step 4: Lambertian Diffuse Lighting ---
                // NdotL = how much surface faces the light
                half NdotL = saturate(dot(normalWS, lightDir));

                // --- Step 5: Ambient Lighting using Spherical Harmonics ---
                half3 ambientSH = SampleSH(normalWS);

                // --- Step 6: Combine base color, texture, and diffuse lighting ---
                // We multiply texColor × _BaseColor × NdotL
                // (texture color tinted by material color and scaled by light intensity)
                half3 diffuse = texColor.rgb * _BaseColor.rgb * NdotL;

                // --- Step 7: Specular Lighting (Blinn-Phong/Phong reflection) ---
                // reflectDir = direction light bounces off surface
                half3 reflectDir = reflect(-lightDir, normalWS);

                // viewDir = direction from surface to camera
                half3 viewDir = normalize(IN.viewDirWS);

                // specFactor = (dot between reflection and view)^shininess
                // Higher shininess = smaller, sharper highlights
                half specFactor = pow(saturate(dot(reflectDir, viewDir)), _Shininess);

                // specular = specular color × specular intensity
                half3 specular = _SpecColor.rgb * specFactor;

                // --- Step 8: Combine all lighting components ---
                // finalColor = diffuse + (ambient × base) + specular
                half3 finalColor = diffuse + (ambientSH * texColor.rgb * _BaseColor.rgb) + specular;

                // --- Step 9: Return final shaded color ---
                return half4(finalColor, 1.0);
            }

            ENDHLSL
        }
    }
}
