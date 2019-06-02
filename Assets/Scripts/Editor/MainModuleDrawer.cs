using UnityEngine;
using UnityEditor;

namespace Nirvana
{
    [CustomPropertyDrawer(typeof(NParticleSystem.MainModule))]
    public class MainModuleDrawer : AbstractModuleDrawer
    {
        protected override void DrawProperty(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUILayout.PropertyField(property.FindPropertyRelative("simulationSpace"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("startLifeTime"));
        }
    }
}