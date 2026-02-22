Shader "URP/URP_RimLighting"
{
    Properties
    {
        // Base color of the surface
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)

        // Color of the rim (edge highlight)
        _RimColor ("Rim Color", Color) = (0, 0.5, 0.5, 1)

        // Controls the sharpness/intensity of the rim glow
        _RimPower ("Rim Power", Range(0.5, 8.0)) = 3.0

        _RimIntensity ("Rim Intensity", Range(0.0, 10.0)) = 0

        [Toggle] _RimBreathe ("Rim Breathe", Float) = 1

        _RimBreatheSpeed ("Rim Breathing Speed", Range(1.0, 10.0)) = 1.0
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

            // Include URP libraries for transforms and math
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // =============================
            //  Vertex Input (from the mesh)
            // =============================
            struct Attributes
            {
                float4 positionOS : POSITION;  // Object-space vertex position
                float3 normalOS   : NORMAL;    // Object-space surface normal
                float4 tangentOS  : TANGENT;   // Tangent (not used here but included for URP standards)
            };

            // ===================================
            //  Data passed to the Fragment Shader
            // ===================================
            struct Varyings
            {
                float4 positionHCS : SV_POSITION; // Clip-space position (for rendering)
                float3 viewDirWS   : TEXCOORD0;   // World-space view direction
                float3 normalWS    : TEXCOORD1;   // World-space normal
            };

            // ===================================
            //  Per-material properties (URP SRP Batcher)
            // ===================================
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;   // Base surface color
                float4 _RimColor;    // Color of rim glow
                float  _RimPower;    // Controls rim falloff
                float _RimIntensity;
                float _RimBreathe;
                float _RimBreatheSpeed;
            CBUFFER_END

            // =============================
            //  Vertex Shader
            // =============================
            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                // Convert object-space position → clip-space for rendering
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);

                // Convert normal → world-space
                OUT.normalWS = normalize(TransformObjectToWorldNormal(IN.normalOS));

                // Compute view direction in world space (camera - vertex position)
                float3 worldPosWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.viewDirWS = normalize(GetCameraPositionWS() - worldPosWS);

                return OUT;
            }

            // =============================
            //  Fragment Shader
            // =============================
            half4 frag (Varyings IN) : SV_Target
            {
                // --- Step 1: Normalize normal and view direction ---
                half3 normalWS = normalize(IN.normalWS);
                half3 viewDirWS = normalize(IN.viewDirWS);

                // --- Step 2: Compute rim factor ---
                // dot(viewDir, normal) measures how much the surface faces the camera.
                // At edges, this value is small → gives rim highlight.
                // We invert it: (1 - dot) so edges become bright.
                half rimFactor = 1.0 - saturate(dot(viewDirWS, normalWS));

                // --- Step 3: Adjust rim width/intensity ---
                // Raising to a power (_RimPower) makes the rim sharper or softer.
                half rimLighting = pow(rimFactor, _RimPower);

                // step 3.5 make it breathe if its enabled
                half3 rimPixel = (_RimColor.rgb * rimLighting) * _RimIntensity;

                if (_RimBreathe) rimPixel *= saturate(sin(_Time.y*_RimBreatheSpeed)+1);

                // --- Step 4: Combine base color with rim lighting ---
                // Multiply rim color × rimLighting → gives rim intensity in rim color.
                // Then add to base color for final glow effect.
                half3 finalColor = _BaseColor.rgb + rimPixel;
                // (We multiply _RimColor.rgb × rimLighting = rim glow intensity)
                // (Then we add + _BaseColor.rgb = final combined color)

                // --- Step 5: Return the final color ---
                return half4(finalColor, _BaseColor.a);
            }

            ENDHLSL
        }
    }
}