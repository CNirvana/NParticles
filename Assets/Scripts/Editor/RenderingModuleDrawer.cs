using UnityEngine;
using UnityEditor;

namespace Nirvana
{
    [CustomPropertyDrawer(typeof(NParticleSystem.RenderingModule))]
    public class RenderingModuleDrawer : AbstractModuleDrawer
    {
        protected override void DrawProperty(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUILayout.PropertyField(property.FindPropertyRelative("material"));
        }
    }
}