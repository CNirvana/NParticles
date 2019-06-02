using UnityEngine;
using UnityEditor;

namespace Nirvana
{
    [CustomPropertyDrawer(typeof(NParticleSystem.ColorModule))]
    public class ColorModuleDrawer : AbstractModuleDrawer
    {
        protected override void DrawProperty(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUILayout.PropertyField(property.FindPropertyRelative("color"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("mode"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("textureResolution"));
        }
    }
}