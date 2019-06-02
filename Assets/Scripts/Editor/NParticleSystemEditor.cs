using UnityEditor;
using UnityEngine;

namespace Nirvana
{
    [CustomEditor(typeof(NParticleSystem))]
    public class NParticleSystemEditor : Editor
    {
        private NParticleSystem _particleSystem;
        private SerializedProperty _maxParticlesProperty;
        private SerializedProperty _randomSeedProperty;
        private SerializedProperty _mainModuleProperty;
        private SerializedProperty _emissionModuleProperty;
        private SerializedProperty _velocityModuleProperty;
        private SerializedProperty _colorModuleProperty;
        private SerializedProperty _sizeModuleProperty;
        private SerializedProperty _renderingModule;

        private void OnEnable()
        {
            _particleSystem = this.target as NParticleSystem;
            _maxParticlesProperty = this.serializedObject.FindProperty("maxParticles");
            _randomSeedProperty = this.serializedObject.FindProperty("randomSeed");
            _mainModuleProperty = this.serializedObject.FindProperty("mainModule");
            _emissionModuleProperty = this.serializedObject.FindProperty("emissionModule");
            _velocityModuleProperty = this.serializedObject.FindProperty("velocityModule");
            _colorModuleProperty = this.serializedObject.FindProperty("colorModule");
            _sizeModuleProperty = this.serializedObject.FindProperty("sizeModule");
            _renderingModule = this.serializedObject.FindProperty("renderingModule");
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            EditorGUILayout.PropertyField(_maxParticlesProperty);
            EditorGUILayout.LabelField("Alive Count: " + _particleSystem.AliveCount);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PropertyField(_randomSeedProperty);
                if(GUILayout.Button("Generate", GUILayout.Width(60.0f)))
                {
                    _randomSeedProperty.intValue = Random.Range(-100000, 100000);
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(_mainModuleProperty);
            EditorGUILayout.PropertyField(_emissionModuleProperty);
            EditorGUILayout.PropertyField(_velocityModuleProperty);
            EditorGUILayout.PropertyField(_colorModuleProperty);
            EditorGUILayout.PropertyField(_sizeModuleProperty);
            EditorGUILayout.PropertyField(_renderingModule);

            this.serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            var shape = _particleSystem.emissionModule.shape;
            switch (shape)
            {
                case NParticleSystem.EmissionShape.Edge:
                    {
                        var startPoint = _particleSystem.transform.TransformPoint(_particleSystem.emissionModule.startEdge);
                        var endPoint = _particleSystem.transform.TransformPoint(_particleSystem.emissionModule.endEdge);
                        Handles.DrawLine(startPoint, endPoint);
                    }
                    break;
                case NParticleSystem.EmissionShape.Sphere:
                    {
                        Handles.DrawWireDisc(_particleSystem.transform.position, Vector3.up, _particleSystem.emissionModule.radius);
                        Handles.DrawWireDisc(_particleSystem.transform.position, Vector3.right, _particleSystem.emissionModule.radius);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}