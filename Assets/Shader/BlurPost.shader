Shader "Hidden/BlurPost"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Blur ("Blur Strength", float) = .1
        _Radius ("Blur Radius", int) = 1
        _Sigma ("Sigma", float) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Blur;
            int _Radius;
            float _Sigma;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = 0;
                int samples = 0;
                for(float k = -_Radius; k < _Radius; k++)
                {
                    for(float j = -_Radius; j < _Radius; j++)
                    {
                        col += tex2D(_MainTex, i.uv + float2(k,j) * _Blur * .001) ;
                        samples++;
                    }
                }
                col /= samples;
                i.uv -= .5;
                col.rgb *= lerp(1., 1 - dot(i.uv, i.uv) - .2, _Blur*2);
                // just invert the colors
                // col.rgb = 1 - col.rgb;
                return col;
            }
            ENDCG
        }
    }
}
