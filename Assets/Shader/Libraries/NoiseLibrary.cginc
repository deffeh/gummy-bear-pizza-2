float rand21(float2 p){
    float3 p3  = frac(float3(p.xyx) * .1031);
    p3 += dot(p3, p3.yzx + 33.33);
    return frac((p3.x + p3.y) * p3.z);
}

float2 rand22(float2 p)
{
    float3 p3 = frac(float3(p.xyx) * float3(.1031, .1030, .0973));
    p3 += dot(p3, p3.yzx+33.33);
    return frac((p3.xx+p3.yz)*p3.zy);

}

float rand31(float3 p){
    p *= float3(21.234,42.31,47.23);
         
    return frac(sin(length(p)) * 884.31);
}

float3 rand33(float3 p){
    p =  mul(float3x3(21.234,17.31,15.23,
              13.2113, 3.36, 12.545,
              2.334, 14.4335, 9.56), p);
         
    return frac(sin(p) * 94.31);
}
float noise(float3 p){
    float3 id = floor(p);
    float3 f  = frac(p);
    float2 z = float2(1.,0.);
    float a = rand31(id);
    float b = rand31(id + z.xyy);
    float c = rand31(id + z.yxy);
    float d = rand31(id + z.xxy);
    
    float a2 = rand31(id + z.yyx);
    float b2 = rand31(id + z.xyx);
    float c2 = rand31(id + z.yxx);
    float d2 = rand31(id + z.xxx);
    
    f = f*f *(3. - 2.*f);
    
    return lerp(lerp(lerp(a,b,f.x),
               lerp(c,d,f.x),f.y),
           lerp(lerp(a2,b2,f.x),
               lerp(c2,d2,f.x),f.y),f.z);
}

float noise(float2 st){
    float2 i = floor(st);
    float2 f = frac(st);

    float2 u = f*f*(3.-2.*f);
    
    float a = rand21(i);
    float b = rand21(i + float2(1.,0));
    float c = rand21(i + float2(0,1.));
    float d = rand21(i + float2(1.,1.));
    
    float rand = lerp(lerp(a,b,u.x),
                        lerp(c,d,u.x),u.y);
    return rand;
    
}

float noise3(float3 p){
    float3 id = floor(p);
    float3 f  = frac(p);
    float2 z = float2(1.,0.);
    float a = rand31(id);
    float b = rand31(id + z.xyy);
    float c = rand31(id + z.yxy);
    float d = rand31(id + z.xxy);
    
    float a2 = rand31(id + z.yyx);
    float b2 = rand31(id + z.xyx);
    float c2 = rand31(id + z.yxx);
    float d2 = rand31(id + z.xxx);
    
    f = f*f *(3. - 2.*f);
    
    return lerp(lerp(lerp(a,b,f.x),
                lerp(c,d,f.x),f.y),
           lerp(lerp(a2,b2,f.x),
                lerp(c2,d2,f.x),f.y),f.z);
}

float fbm(float2 st){
    int octaves = 2;
    float rand = 0.;
    float amp = .5;
    float freq = 1.;
    float2x2 rot = float2x2(cos(.5), sin(.5),
                    -sin(0.5), cos(0.50));
    float shift = 100.;
    for(int i = 0; i < octaves; i++)
    {
        rand += amp * noise(st * freq);
        amp *= .5;
        st = mul(rot,st) * 2.0 + shift;
    }
    return rand;
}

float fbmO(float2 st, int octaves){
    float rand = 0.;
    float amp = .5;
    float freq = 1.;
    float2x2 rot = float2x2(cos(.5), sin(.5),
                    -sin(0.5), cos(0.50));
    float shift = 100.;
    for(int i = 0; i < octaves; i++)
    {
        rand += amp * noise(st * freq);
        amp *= .5;
        st = mul(rot,st) * 2.0 + shift;
    }
    return rand;
}


float fbm(float3 p, int octaves){
    float3x3 q = float3x3(cos(.5), sin(.5), 0.,
                  -sin(.5), cos(.5),  0.,
                  0.,    0.,     1.);
    
    float rand = 0.;
    float amp = .5;
    float freq = 1.;
    float shift = 100.;
    for(int i = 0; i < octaves; i++)
    {
        rand += amp * noise3(p * freq);
        amp *= .5;
        p = mul(p, q) * 2.0 + shift;
    }
    return rand;
}

float3 voronoise(float2 uv)
{
    float2 f = frac(uv);
    f -= .5;
    float2 i = floor(uv);
    float2 pos = i;
    float dist = distance(f,rand22(i) - .5);
    for(int x=-1; x<=1; x++)
    {
        for(int y=-1; y<=1; y++)
        {
            float2 p = i + float2(x,y);
            float nDist = distance(f,rand22(p) + float2(x,y)  - .5);
            //TODO remove branch
            if(nDist < dist){
                dist = nDist;
                pos = p;
            }
        }
    }
    return float3(dist, pos);
}

float3 voronoiDistance(in float3 x)
{
    int3 p = int3(floor(x));
    float3 f = frac(x);

    int3 mb;
    float3 mr;

    float res = 8.0f;
    
    // Iterate over the neighboring cells in 3D space
    for (int k = -1; k <= 1; k++)
    {
        for (int j = -1; j <= 1; j++)
        {
            for (int i = -1; i <= 1; i++)
            {
                int3 b = int3(i, j, k);
                float3 r = float3(b) + rand31(p + b) - f;
                float d = dot(r, r);

                if (d < res)
                {
                    res = d;
                    mr = r;
                    mb = b;
                }
            }
        }
    }

    res = 8.0f;

    // Iterate over a larger neighborhood in 3D space
    for (int k2 = -2; k2 <= 2; k2++)
    {
        for (int j2 = -2; j2 <= 2; j2++)
        {
            for (int i2 = -2; i2 <= 2; i2++)
            {
                int3 b = mb + int3(i2, j2, k2);
                float3 r = float3(b) + rand31(p + b) - f;
                float d = dot(0.5f * (mr + r), normalize(r - mr));

                res = min(res, d);
            }
        }
    }

    return mb + p;
}

float3 voronoise3(in float3 x)
{
    int3 p = int3(floor(x));
    float3 f = frac(x);

    int3 mb;

    float res = 8.0f;
    
    // Iterate over the neighboring cells in 3D space
    for (int k = -1; k <= 1; k++)
    {
        for (int j = -1; j <= 1; j++)
        {
            for (int i = -1; i <= 1; i++)
            {
                int3 b = int3(i, j, k);
                float3 r = float3(b) + rand31(p + b) - f;
                float d = dot(r, r);

                if (d < res)
                {
                    res = d;
                    mb = b;
                }
            }
        }
    }

    return mb + p;
}

float3 getBorder(float3 p)
{
    float3 d = voronoiDistance(p);

    return d;
}

float pFbm(float3 p){
    float3x3 q = float3x3(4./5., -3./5., 0.,
                  3./5., 4./5.,  0.,
                  0.,    0.,     1.);
    int octaves = 8;
    
    float f = 0.;
    float freq = 1.;
    float a = .5;
    for(int i = 0; i < octaves; i++){
        f += (1. - noise(p * freq)) * a;
        a /= 2.;
        freq *= 2.;
        p = mul(q,p);
    }
    return f;
}

float2 rot(float2 uv, float a )
{
    float2x2 mat = float2x2(cos(a), -sin(a),
                            sin(a),  cos(a));
    return mul(mat, uv);
}

float getGooDepth(float4 col)
{
    return col.g;
}
fixed getGooAlpha(fixed4 col)
{
    return 1 - col.a;
}

struct v2fTransparent
{
    float2 uv : TEXCOORD0;
    float3 normal : NORMAL;
    float4 vertex : SV_POSITION;
    float3 worldPos : TEXCOORD1;
    float3 objPos : TEXCOORD2;
    float4 scrPos : TEXCOORD3;
    float4 color : COLOR;
};
