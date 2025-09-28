Shader "Unlit/Crosshair"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Col ("Color", Color) = (1.,1.,1.,1.)
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent" "RenderType"="Transparent"
        }
        LOD 100
        ZWrite Off Cull Off
        ZTest Always

        Blend SrcAlpha OneMinusSrcAlpha
        BlendOp Add
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Col;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                // apply fog
                return clamp(col * _Col,0,1);
            }
            ENDCG
        }
    }
}
