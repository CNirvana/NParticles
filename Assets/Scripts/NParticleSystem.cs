using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Nirvana
{
    public partial class NParticleSystem : MonoBehaviour
    {
        private const int GROUP_SIZE = 256;

        public int maxParticles = 1000;
        public int randomSeed = 1;
        public MainModule mainModule;
        public EmissionModule emissionModule;
        public VelocityModule velocityModule;
        public ColorModule colorModule;
        public SizeModule sizeModule;
        public RenderingModule renderingModule;
        [SerializeField]
        [HideInInspector]
        private ComputeShader _computeShader = null;

        public int AliveCount { get { return _bufferSize - _poolCount; } }

        private ComputeBuffer _meshBuffer;
        private ComputeBuffer _particlesBuffer;
        private ComputeBuffer _poolBuffer;
        private ComputeBuffer _countBuffer;
        private int _poolCount;
        private uint[] _args;
        private int _groupCount;
        private int _bufferSize;
        private int _initKernel;
        private int _emitKernel;
        private int _updateKernel;
        private float _time;
        private float _deltaTime;
        private float _emissionInterval;

        private void Awake()
        {
            this.ReleaseResources();
            this.Initialize();
        }

        private void OnDestroy()
        {
            this.ReleaseResources();
            this.DestroyAllModules();
        }

        private void Update()
        {
            this.UpdateTime();
            this.UpdatePoolCount();
            this.UpdateEmission();
            this.UpdateParticles();
        }

        private void OnRenderObject()
        {
            if(this.renderingModule.enabled)
            {
                this.renderingModule.material.SetBuffer("particles", _particlesBuffer);
                this.renderingModule.material.SetBuffer("meshDatas", _meshBuffer);
                this.renderingModule.material.SetPass(0);

                Graphics.DrawProceduralNow(MeshTopology.Quads, 4, _bufferSize);
            }
        }

        private void ReleaseResources()
        {
            this.ReleaseComputeBuffer(_particlesBuffer);
            this.ReleaseComputeBuffer(_meshBuffer);
            this.ReleaseComputeBuffer(_poolBuffer);
            this.ReleaseComputeBuffer(_countBuffer);
        }

        private void ReleaseComputeBuffer(ComputeBuffer computeBuffer)
        {
            if(computeBuffer != null)
            {
                computeBuffer.Release();
            }

            computeBuffer = null;
        }

        private void Initialize()
        {
            _time = 0.0f;
            _emissionInterval = 0.0f;

            _groupCount = Mathf.CeilToInt((float)this.maxParticles / GROUP_SIZE);
            _bufferSize = _groupCount * GROUP_SIZE;

            _particlesBuffer = new ComputeBuffer(_bufferSize, Marshal.SizeOf(typeof(Particle)));

            _poolBuffer = new ComputeBuffer(_bufferSize, sizeof(uint), ComputeBufferType.Append);
            _poolBuffer.SetCounterValue(0);

            _countBuffer = new ComputeBuffer(1, 4 * sizeof(uint), ComputeBufferType.IndirectArguments);
            _args = new uint[]{ 0, 1, 0, 0 };

            this.UpdateMeshBuffer();
            this.InitializeAllModules();

            _initKernel = _computeShader.FindKernel("Init");
            _emitKernel = _computeShader.FindKernel("Emit");
            _updateKernel = _computeShader.FindKernel("Update");
            _computeShader.SetBuffer(_initKernel, "particles", _particlesBuffer);
            _computeShader.SetBuffer(_initKernel, "deadList", _poolBuffer);
            _computeShader.Dispatch(_initKernel, _groupCount, 1, 1);

            _poolCount = _bufferSize;
        }

        private void InitializeAllModules()
        {
            this.mainModule.Initialize();
            this.emissionModule.Initialize();
            this.velocityModule.Initialize();
            this.colorModule.Initialize();
            this.sizeModule.Initialize();
            this.renderingModule.Initialize();
        }

        private void DestroyAllModules()
        {
            this.mainModule.Destroy();
            this.emissionModule.Destroy();
            this.velocityModule.Destroy();
            this.colorModule.Destroy();
            this.sizeModule.Destroy();
            this.renderingModule.Destroy();
        }

        private void UpdateMeshBuffer()
        {
            var meshDatas = new MeshData[4];
            meshDatas[0] = new MeshData(new Vector3(-0.5f, -0.5f, 0.0f), new Vector2(0.0f, 0.0f));
            meshDatas[1] = new MeshData(new Vector3(-0.5f, 0.5f, 0.0f), new Vector2(0.0f, 1.0f));
            meshDatas[2] = new MeshData(new Vector3(0.5f, 0.5f, 0.0f), new Vector2(1.0f, 1.0f));
            meshDatas[3] = new MeshData(new Vector3(0.5f, -0.5f, 0.0f), new Vector2(1.0f, 0.0f));

            _meshBuffer = new ComputeBuffer(4, Marshal.SizeOf(typeof(MeshData)));
            _meshBuffer.SetData(meshDatas);
        }

        private void UpdateTime()
        {
            _deltaTime = Time.deltaTime;
            _time += _deltaTime;
            _computeShader.SetFloat("time", _time);
            _computeShader.SetFloat("deltaTime", _deltaTime);
        }

        private void UpdateEmission()
        {
            if(!this.emissionModule.enabled)
            {
                return;
            }

            int count = this.CalculateEmissionCount();
            if(count > 0)
            {
                _computeShader.SetMatrix("modelMatrix", this.transform.localToWorldMatrix);
                _computeShader.SetInt("randomSeed", this.randomSeed);

                _computeShader.SetFloat("startLifeTime", this.mainModule.startLifeTime);

                _computeShader.SetInt("emissionShape", (int)this.emissionModule.shape);
                _computeShader.SetFloat("radius", this.emissionModule.radius);
                _computeShader.SetVector("startEdge", this.transform.TransformPoint(this.emissionModule.startEdge));
                _computeShader.SetVector("endEdge", this.transform.TransformPoint(this.emissionModule.endEdge));

                _computeShader.SetFloat("startSpeed", this.velocityModule.startSpeed);

                _computeShader.SetInt("colorMode", (int)this.colorModule.mode);
                _computeShader.SetTexture(_emitKernel, "colorTex", this.colorModule.colorTexture);

                _computeShader.SetInt("sizeMode", (int)this.sizeModule.mode);
                _computeShader.SetBuffer(_emitKernel, "size", this.sizeModule.buffer);
                _computeShader.SetInt("sizeBufferResolution", this.sizeModule.resolution);

                _computeShader.SetBuffer(_emitKernel, "particles", _particlesBuffer);
                _computeShader.SetBuffer(_emitKernel, "poolList", _poolBuffer);

                _computeShader.Dispatch(_emitKernel, count, 1, 1);
            }
        }

        private void UpdateParticles()
        {
            _computeShader.SetVector("acceleration", this.velocityModule.acceleration * _deltaTime);

            _computeShader.SetInt("colorMode", (int)this.colorModule.mode);
            _computeShader.SetTexture(_updateKernel, "colorTex", this.colorModule.colorTexture);

            _computeShader.SetInt("sizeMode", (int)this.sizeModule.mode);
            _computeShader.SetBuffer(_updateKernel, "size", this.sizeModule.buffer);
            _computeShader.SetInt("sizeBufferResolution", this.sizeModule.resolution);

            _computeShader.SetBuffer(_updateKernel, "particles", _particlesBuffer);
            _computeShader.SetBuffer(_updateKernel, "deadList", _poolBuffer);
            _computeShader.Dispatch(_updateKernel, _groupCount, 1, 1);
        }

        private void UpdatePoolCount()
        {
            _countBuffer.SetData(_args);
            ComputeBuffer.CopyCount(_poolBuffer, _countBuffer, 0);
            _countBuffer.GetData(_args);
            _poolCount = (int)_args[0];
        }

        private int CalculateEmissionCount()
        {
            float emissionRate = 1.0f / this.emissionModule.rateOverTime;
            _emissionInterval -= _deltaTime;

            if(_emissionInterval < emissionRate)
            {
                int count = Mathf.CeilToInt((emissionRate - _emissionInterval) / emissionRate);
                _emissionInterval += count * emissionRate;
                count = Mathf.Min(count, this.maxParticles - this.AliveCount);
                return count;
            }

            return 0;
        }
    }
}