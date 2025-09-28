Shader "Hidden/Smoke"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Percent ("Percent", Float) = 1
    }
    SubShader
    {
        // No culling or depth
        Tags
        {
            "Queue" = "Transparent" "RenderType"="Transparent"
        }
        LOD 100

        ZWrite Off Cull Off
        Blend SrcAlpha OneMinusSrcAlpha
        BlendOp Add
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Libraries/NoiseLibrary.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
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
            float _Percent;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 startUV = i.uv;
                float f = smoothstep(0.,.8, 1 - i.uv.y);
                f *= smoothstep(0.,.3, i.uv.y);
                f *= smoothstep(0.,.9, 1 - i.uv.x);
                f *= smoothstep(0.,.1, i.uv.x);
                
                
                i.uv.x -= .5;
                i.uv.x -= (i.uv.x * i.uv.y)*.5;
                i.uv.x *= 1.4;
                i.uv.x += .5;
                i.uv += fbm(i.uv * float2(10.,3.) + _Time[1]) * .1 * (startUV.y * 2 + .3);
                i.uv += fbm(i.uv * float2(2.,1.) + _Time[1] * float2(-.5,.5)) * .2 * (startUV.y * 2 + .3);
                
                fixed4 col = tex2D(_MainTex, saturate(i.uv));
                col.a *= f;
                col.a *= smoothstep(0.,.4,voronoise(i.uv * 2 - _Time[1] * .3)) + smoothstep(0.7,1.,voronoise(i.uv * 4 - _Time[1] * 1)) * .3;
                // col.a *= 1.3;
                return col;
            }
            ENDCG
        }
    }
}
