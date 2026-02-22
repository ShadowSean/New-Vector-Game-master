Shader "Custom/ToggleShader"
{
    Properties
    {
        // Base color tint for the surface
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)

        // Base texture (albedo)
        _MainTex ("Base Texture", 2D) = "white" {}

        // Specular reflection color
        _SpecColor ("Specular Color", Color) = (1, 1, 1, 1)

        // Controls highlight sharpness
        _Shininess ("Shininess", Range(0.1, 100)) = 16

        _myBump ("Bump Texture", 2D) = "bump" {}
        _mySlider ("Bump Amount", Range(0,10)) = 1

        _RimPower ("Rim Power", Range(0.5, 8.0)) = 3.0

        _RimIntensity ("Rim Intensity", Range(0.0, 10.0)) = 0

        // Toggles to enable or disable lighting components
        [Toggle] _UseDiffuse ("Enable Diffuse", Float) = 1
        [Toggle] _UseAmbient ("Enable Ambient", Float) = 1
        [Toggle] _UseSpecular ("Enable Specular", Float) = 1
        [Toggle] _UseBhong_phong ("Enable Phong", Float) = 1
        [Toggle] _UseRim ("Enable Rim", Float) = 1
    }

    SubShader
    {
        // Tags tell Unity when and how to render this shader
        Tags { 
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Include URP helper files
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // =============================
            //  Vertex Input (from the mesh)
            // =============================
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
                float4 tangentOS : TANGENT;
            };

            // ===================================
            //  Data passed to the Fragment Shader
            // ===================================
            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD1;
                float3 tangentWS : TEXCOORD2;
                float2 uv : TEXCOORD0;
                float3 bitangentWS : TEXCOORD3;
                float3 viewDirWS : TEXCOORD4;
            };

            // ===================================
            //  Textures and Samplers
            // ===================================
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_myBump);
            SAMPLER(sampler_myBump);

            // ===================================
            //  Material Properties (URP Batcher)
            // ===================================
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;   // Base color tint
                float4 _SpecColor;   // Specular color
                float  _Shininess;   // Shininess (specular exponent)
                float _mySlider;
                float4 _myBump_ST;
                float  _RimPower;    // Controls rim falloff
                float _RimIntensity;

                // Lighting toggles
                float _UseDiffuse;
                float _UseAmbient;
                float _UseSpecular;
                float _UseBhong_phong;
                float _UseRim;
            CBUFFER_END

            // =============================
            //  Vertex Shader
            // =============================
            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.normalWS = normalize(TransformObjectToWorldNormal(IN.normalOS));
                OUT.tangentWS = normalize(TransformObjectToWorldNormal(IN.tangentOS.xyz));
                OUT.bitangentWS = cross(OUT.normalWS, OUT.tangentWS) * IN.tangentOS.w;

                float3 worldPosWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.viewDirWS = normalize(GetCameraPositionWS() - worldPosWS);

                OUT.uv = IN.uv;
                return OUT;
            }

            // =============================
            //  Fragment Shader
            // =============================
            half4 frag (Varyings IN) : SV_Target
            {
                // --- Step 1: Sample the base texture and multiply with base color ---
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                half3 base = texColor.rgb * _BaseColor.rgb;

                half3 normalWS;
                if (_UseBhong_phong > 0.5)
                {
                    float2 bumpUV = IN.uv * _myBump_ST.xy + _myBump_ST.zw;   // <<< NEW
                    half3 normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_myBump, sampler_myBump, bumpUV));
                    normalTS.xy *= _mySlider;

                    half3x3 TBN = half3x3(IN.tangentWS, IN.bitangentWS, IN.normalWS);
                    normalWS = normalize(mul(normalTS, TBN));
                }
                else
                {
                    normalWS = normalize(IN.normalWS);
                }
                

                // --- Normal map sample with tiling/offset applied ---
               

                // --- Step 2: Fetch light and normalize directions ---
                Light mainLight = GetMainLight();
                half3 lightDir = normalize(mainLight.direction);
               
                half NdotL = saturate(dot(normalWS, lightDir)); // Lambert term

                // --- Step 3: Compute lighting components ---
                // Diffuse term (Lambert)
                half3 diffuse = base * NdotL;

                // Ambient term (Spherical Harmonics)
                half3 ambient = SampleSH(normalWS) * base;

                half3 ambientSH = SampleSH(normalWS);
                // Specular term (Blinn-Phong / Phong style)
                half3 reflectDir = reflect(-lightDir, normalWS);
                half3 viewDir = normalize(IN.viewDirWS);
                half specFactor = pow(saturate(dot(reflectDir, viewDir)), _Shininess);
                half3 specular = _SpecColor.rgb * specFactor;

                half rimFactor = 1.0 - saturate(dot(viewDir, normalize(IN.normalWS)));

                half rimLighting = pow(rimFactor, _RimPower);

                half3 rimColor = _SpecColor.rgb * rimLighting * _RimIntensity;

                half3 finalColor = 0;

                if (_UseDiffuse > 0.5) finalColor += diffuse;   // Add diffuse if enabled
                if (_UseAmbient > 0.5) finalColor += ambient;   // Add ambient if enabled
                if (_UseSpecular > 0.5) finalColor += specular; // Add specular if enabled
                if (_UseRim > 0.5) finalColor += rimColor;
                

                // --- Step 5: Return final color ---
                return half4(finalColor, 1.0);
            }

            ENDHLSL
        }
    }
}
