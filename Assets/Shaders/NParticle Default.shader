Shader "Nirvana/Particles/NParticle Default"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
            "IgnoreProjector"="True"
            "PreviewType"="Plane"
        }

        LOD 100

        Cull Back
        ZWrite Off
        Lighting Off
        Blend SrcAlpha One

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma target 4.5

            #include "UnityCG.cginc"
            #include "Common.cginc"

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            StructuredBuffer<Particle> particles;
            StructuredBuffer<MeshData> meshDatas;

            v2f vert (uint id : SV_VertexID, uint inst : SV_InstanceID)
            {
                Particle p = particles[inst];

                v2f o;
                float4 viewPos = mul(UNITY_MATRIX_V, float4(p.position, 1.0));
                o.vertex = mul(UNITY_MATRIX_P, float4(viewPos.xyz + meshDatas[id].vertex * p.size, 1.0));
                o.uv = meshDatas[id].uv;
                o.color = p.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= i.color;
                col.rgb *= col.a;
                return col;
            }
            ENDCG
        }
    }
}
