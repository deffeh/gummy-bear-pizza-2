Shader "Hidden/Color"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _F("Hue", float) = 1
        _B("Brightness", float) = 1
        _C("Contrast", float) = 1
    }
    SubShader
    {
        // No culling or depth
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }

        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            sampler2D _MainTex;
            float _F;
            float _B;
            float _C;

            //credit https://gist.github.com/mairod/a75e7b44f68110e1576d77419d608786
            float3 hueShift( float3 color, float hueAdjust ){

                const float3  kRGBToYPrime = float3 (0.299, 0.587, 0.114);
                const float3  kRGBToI      = float3 (0.596, -0.275, -0.321);
                const float3  kRGBToQ      = float3 (0.212, -0.523, 0.311);

                const float3  kYIQToR     = float3 (1.0, 0.956, 0.621);
                const float3  kYIQToG     = float3 (1.0, -0.272, -0.647);
                const float3  kYIQToB     = float3 (1.0, -1.107, 1.704);

                float   YPrime  = dot (color, kRGBToYPrime);
                float   I       = dot (color, kRGBToI);
                float   Q       = dot (color, kRGBToQ);
                float   hue     = atan2 (Q, I);
                float   chroma  = sqrt (I * I + Q * Q);

                hue += hueAdjust;

                Q = chroma * sin (hue);
                I = chroma * cos (hue);

                float3    yIQ   = float3 (YPrime, I, Q);

                return float3( dot (yIQ, kYIQToR), dot (yIQ, kYIQToG), dot (yIQ, kYIQToB) );

            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb *= _B;
                col.rgb = pow(col.rgb, _C);
                // just invert the colors
                col.rgb = clamp(hueShift( col.rgb, _F),0.,1.);
                col *= i.color;
                return col;
            }
            ENDCG
        }
    }
}
