Shader "Unlit/WorldReflection_URP"
{
    Properties
    {
        // Optional base layer (the texture you can tint)
        _BaseMap   ("Base Map", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)

        // Environment reflection parameters
        _EnvCube     ("Reflection Cubemap", Cube) = "" {}         // Cubemap used for reflections
        _EnvIntensity("Reflection Intensity", Range(0,2)) = 1.0   // Brightness of reflection
        _EnvBlend    ("Reflection Blend (0=Base,1=Env)", Range(0,1)) = 1.0 // Mix between texture and reflection
        _FresnelPow  ("Fresnel Power", Range(0.1, 8)) = 5.0       // Controls sharpness of edge reflection
        _FresnelBoost("Fresnel Boost", Range(0, 2)) = 1.0         // Amplifies edge reflection
    }

    SubShader
    {
        // URP-compatible, opaque geometry rendering
        Tags { "RenderType"="Opaque" "Queue"="Geometry" "RenderPipeline"="UniversalRenderPipeline" }
        LOD 200

        Pass
        {
            Name "Unlit"
            Tags { "LightMode"="UniversalForward" } // Works in the main lighting pass

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Include core URP shader library for math, transforms, etc.
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // ===== Vertex Input from Mesh =====
            struct Attributes
            {
                float4 positionOS : POSITION; // Vertex position in Object Space
                float3 normalOS   : NORMAL;   // Vertex normal in Object Space
                float2 uv0        : TEXCOORD0; // Texture coordinates
            };

            // ===== Data passed from Vertex to Fragment =====
            struct Varyings
            {
                float4 positionHCS : SV_POSITION; // Position in Homogeneous Clip Space (needed for rasterization)
                float3 positionWS  : TEXCOORD0;   // Position in World Space
                float3 normalWS    : TEXCOORD1;   // Normal in World Space
                float2 uv          : TEXCOORD2;   // UVs for texture
            };

            // ===== Texture Declarations =====
            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            TEXTURECUBE(_EnvCube);       // Environment reflection texture (cubemap)
            SAMPLER(sampler_EnvCube);

            // ===== Per-material Variables (SRP Batcher compatible) =====
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;     // Color tint
                float4 _BaseMap_ST;    // Tiling and offset for texture
                float  _EnvIntensity;  // Brightness of reflection
                float  _EnvBlend;      // Mix between texture and reflection
                float  _FresnelPow;    // Sharpness of Fresnel
                float  _FresnelBoost;  // Strength of Fresnel reflection
            CBUFFER_END

            // ===== Vertex Shader =====
            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                // Convert vertex position and normal from object → world space
                float3 posWS = TransformObjectToWorld(IN.positionOS.xyz);
                float3 nrmWS = TransformObjectToWorldNormal(IN.normalOS);

                // Store data for fragment shader
                OUT.positionWS  = posWS;                            // For reflection & view direction
                OUT.normalWS    = nrmWS;                            // For reflection angle
                OUT.positionHCS = TransformWorldToHClip(posWS);     // For rendering position on screen
                OUT.uv          = TRANSFORM_TEX(IN.uv0, _BaseMap);  // Apply tiling/offset to UV
                return OUT;
            }

            // ===== Fragment Shader =====
            half4 frag (Varyings IN) : SV_Target
            {
                // --- Step 1: Base texture sampling ---
                // Read color from the texture and multiply by the base color tint
                half3 baseCol = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv).rgb * _BaseColor.rgb;

                // --- Step 2: Compute view direction (camera → surface) ---
                // GetWorldSpaceViewDir gives direction from pixel to camera
                float3 V = SafeNormalize(GetWorldSpaceViewDir(IN.positionWS));

                // --- Step 3: Get the surface normal (in world space) ---
                float3 N = SafeNormalize(IN.normalWS);

                // --- Step 4: Compute reflection vector ---
                // reflect() expects the INCIDENT vector (from surface toward light or eye)
                // So we use -V (from surface to camera) to reflect around N
                float3 R = reflect(-V, N);

                // --- Step 5: Sample reflection cubemap ---
                // Use reflection direction to fetch environment color
                half3 envCol = SAMPLE_TEXTURECUBE(_EnvCube, sampler_EnvCube, R).rgb * _EnvIntensity;

                // --- Step 6: Fresnel effect ---
                // Gives stronger reflection at edges (like metals or glass)
                float ndotv   = saturate(dot(N, V));                     // Angle between surface and view direction
                float fresnel = pow(1.0 - ndotv, _FresnelPow) * _FresnelBoost; // Increases reflection near edges
                envCol *= (1.0 + fresnel);

                // --- Step 7: Blend reflection with base texture ---
                // _EnvBlend = 0 → only base texture
                // _EnvBlend = 1 → only reflection
                half3 finalCol = lerp(baseCol, envCol, _EnvBlend);

                // Output final color (opaque alpha)
                return half4(finalCol, 1.0);
            }

            ENDHLSL
        }
    }

    FallBack Off
}
