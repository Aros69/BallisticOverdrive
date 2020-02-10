Shader "HUD/ammoHUD"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AmmoMax("Max Ammo", float) = 1
        _AmmoLeft("Ammo", float) = 1
        _Smoother("Smoother", float) = 0.1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _AmmoMax;
            float _AmmoLeft; 
            float _Smoother;
            
            float rectangle(float2 uv, float2 pos, float2 dimensions)
            {
                float p = smoothstep(pos.x-_Smoother,pos.x+_Smoother , uv.x); 

                p *= smoothstep(uv.x-_Smoother,uv.x+_Smoother , pos.x + dimensions.x );
                p *= step(pos.y, uv.y);
                p *= step(uv.y, pos.y + dimensions.y);
                return p;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                float uv = i.uv;
                float2 muv = uv;
                muv.x *= _AmmoMax;
                muv.x = muv.x % 1.0;
                
                float p;
                p = rectangle(muv, float2(0.1, 0), float2(0.9, 1.0)); // Draw all the slots
                
                // Remove the slots that are not full
                if(uv.x  < _AmmoLeft/_AmmoMax)
                    p *= 1.0;
                else
                    p *= 0.0;
                
                col = fixed4(p,p,p,p);
                return col;
            }
            ENDCG
        }
    }
}
