using UnityEngine;
using UnityEditor;

namespace Nirvana
{
    [CustomPropertyDrawer(typeof(NParticleSystem.SizeModule))]
    public class SizeModuleDrawer : AbstractModuleDrawer
    {
        protected override void DrawProperty(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUILayout.PropertyField(property.FindPropertyRelative("size"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("mode"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("resolution"));
        }
    }
}