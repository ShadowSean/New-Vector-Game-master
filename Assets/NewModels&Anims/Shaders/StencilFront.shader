Shader "Custom/URP_StencilWrite_FrontObject"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}  // Main texture
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry" }

        // Disable color output (so the object doesn't appear visually)
        ColorMask 0
        ZWrite Off

        // Stencil operations
        Stencil
        {
            Ref 1  // Set stencil reference value
            Comp Always  // Always pass stencil test
            Pass Replace  // Replace stencil buffer value with reference value
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Include URP core functionality
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;  // Object space position
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;  // Clip-space position
            };

            // Vertex shader: simple pass-through
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            // Fragment shader: return black (but ColorMask 0 prevents it from being rendered)
            half4 frag(Varyings IN) : SV_Target
            {
                return half4(0, 0, 0, 1);  // Black color (will not be visible due to ColorMask 0)
            }

            ENDHLSL
        }
    }
}