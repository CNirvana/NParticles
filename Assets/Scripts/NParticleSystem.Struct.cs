using UnityEngine;

namespace Nirvana
{
    public partial class NParticleSystem
    {
        public struct Particle
        {
            public Vector3 position;
            public Vector3 velocity;
            public Color color;
            public Vector2 lifeTime;
            public float size;
            public bool alive;
        }

        public struct MeshData
        {
            public Vector3 vertex;
            public Vector2 uv;

            public MeshData(Vector3 vertex, Vector2 uv)
            {
                this.vertex = vertex;
                this.uv = uv;
            }
        }
    }
}