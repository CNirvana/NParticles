using System;
using UnityEngine;

namespace Nirvana
{
    public partial class NParticleSystem
    {
        public abstract class AbstractModule
        {
            public virtual void Initialize() {}
            public virtual void Destroy() {}
        }

        [Serializable]
        public class MainModule : AbstractModule
        {
            public SimulationSpace simulationSpace = SimulationSpace.World;
            public float startLifeTime = 5.0f;
        }

        [Serializable]
        public class EmissionModule : AbstractModule
        {
            public bool enabled = true;
            public EmissionShape shape = EmissionShape.Edge;
            public Vector3 startEdge;
            public Vector3 endEdge;
            public float radius = 1.0f;

            public float rateOverTime = 20.0f;
        }

        [Serializable]
        public class VelocityModule : AbstractModule
        {
            public float startSpeed;
            public Vector3 acceleration;
        }

        [Serializable]
        public class ColorModule : AbstractModule
        {
            public Gradient color;
            public ColorMode mode = ColorMode.Random;
            public Texture2D colorTexture;
            public int textureResolution = 256;

            public override void Initialize()
            {
                this.colorTexture = new Texture2D(this.textureResolution, 1);
                float step = 1.0f / this.textureResolution;

                for(int i = 0; i < this.textureResolution; i++)
                {
                    colorTexture.SetPixel(i, 0, this.color.Evaluate(i * step));
                }
                this.colorTexture.wrapMode = TextureWrapMode.Clamp;
                this.colorTexture.Apply();
            }

            public override void Destroy()
            {
                if(this.colorTexture != null)
                {
                    GameObject.Destroy(this.colorTexture);
                }
            }
        }

        [Serializable]
        public class SizeModule : AbstractModule
        {
            public AnimationCurve size = new AnimationCurve(new Keyframe(0.0f, 1.0f), new Keyframe(1.0f, 1.0f));
            public SizeMode mode = SizeMode.Random;
            public ComputeBuffer buffer;
            public int resolution = 256;

            public override void Initialize()
            {
                this.buffer = new ComputeBuffer(this.resolution, sizeof(float));

                float[] data = new float[this.resolution];
                float step = 1.0f / this.resolution;
                for(int i = 0; i < this.resolution; i++)
                {
                    data[i] = this.size.Evaluate(i * step);
                }
                this.buffer.SetData(data);
            }

            public override void Destroy()
            {
                if(this.buffer != null)
                {
                    this.buffer.Release();
                }

                this.buffer = null;
            }
        }

        [Serializable]
        public class RenderingModule : AbstractModule
        {
            public bool enabled = true;
            public Material material;
        }
    }
}