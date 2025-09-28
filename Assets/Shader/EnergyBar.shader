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
        Stencil
        {
            Ref 1
            Comp Equal
            Pass Keep
        }
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
                col.a = (1 - (i.uv.y - _Percent) ) - 1;
                col.a -= (fbm(i.uv * 2 + _Time[1]) - .5) * .02;
                col.a = step( 0., col.a);
                // just invert the colors
                return col;
            }
            ENDCG
        }
    }
}
