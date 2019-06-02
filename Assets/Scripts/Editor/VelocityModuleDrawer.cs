using UnityEngine;
using UnityEditor;

namespace Nirvana
{
    [CustomPropertyDrawer(typeof(NParticleSystem.VelocityModule))]
    public class VelocityModuleDrawer : AbstractModuleDrawer
    {
        protected override void DrawProperty(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUILayout.PropertyField(property.FindPropertyRelative("velocity"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("acceleration"));
        }
    }
}