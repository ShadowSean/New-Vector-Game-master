Shader "Custom/ProceduralDoubleScrollingNoise"
{
    Properties
    {
        _ScrollA ("Noise Scroll A (X,Y)", Vector) = (0.1, 0.0, 0, 0)
        _ScrollB ("Noise Scroll B (X,Y)", Vector) = (0.0, 0.1, 0, 0)
        _NoiseScaleA ("Noise Scale A", Float) = 10.0
        _NoiseScaleB ("Noise Scale B", Float) = 20.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float4 _ScrollA;
            float4 _ScrollB;
            float _NoiseScaleA;
            float _NoiseScaleB;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos( v.vertex );
                o.uv  = v.uv;
                return o;
            }

            //---------------------------------------
            // 2D Value Noise function (fast)
            //---------------------------------------
            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 53758.5453123);
            }

            float valueNoise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);

                float a = hash(i);
                float b = hash(i + float2(1,0));
                float c = hash(i + float2(0,1));
                float d = hash(i + float2(1,1));

                float2 u = f * f * (3.0 - 2.0 * f);

                return lerp( lerp(a, b, u.x),
                             lerp(c, d, u.x),
                             u.y );
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float t = _Time.x;

                // FIRST NOISE (A)
                float2 uvA = i.uv * _NoiseScaleA + _ScrollA.xy * t;
                float noiseA = valueNoise(uvA);

                // Clamp noise A: anything < 0.9 becomes 0
                noiseA *= step(0.9, noiseA);

                // SECOND NOISE (B)
                float2 uvB = i.uv * _NoiseScaleB + _ScrollB.xy * t;
                float noiseB = valueNoise(uvB);

                // Clamp noise B
                noiseB *= step(0.8, noiseB);

                // Multiply AFTER thresholding
                float result = noiseA * noiseB;

                return fixed4(result, result, result, 1.0);
            }

            ENDCG
        }
    }
}
