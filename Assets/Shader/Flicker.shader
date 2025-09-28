Shader "Hidden/EnergyBar"
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
                fixed4 col = i.color;
                i.uv -= .5;
                i.uv *= 2;
                col.a *= smoothstep(0., 2., 1 - dot(i.uv, i.uv));
                col.a *= lerp(0.5,1., fbm(_Time[1] * 1) * fbm(_Time[1] * 10));
                return col;
            }
            ENDCG
        }
    }
}
