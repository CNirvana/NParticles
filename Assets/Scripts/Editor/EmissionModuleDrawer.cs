using UnityEngine;
using UnityEditor;

namespace Nirvana
{
    [CustomPropertyDrawer(typeof(NParticleSystem.EmissionModule))]
    public class EmissionModuleDrawer : AbstractModuleDrawer
    {
        protected override void DrawProperty(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUILayout.PropertyField(property.FindPropertyRelative("enabled"));
            var shapeProperty = property.FindPropertyRelative("shape");
            EditorGUILayout.PropertyField(shapeProperty);

            var shape = (NParticleSystem.EmissionShape)shapeProperty.enumValueIndex;
            EditorGUI.indentLevel++;
            switch (shape)
            {
                case NParticleSystem.EmissionShape.Sphere:
                    {
                        EditorGUILayout.PropertyField(property.FindPropertyRelative("radius"));
                    }
                    break;
                case NParticleSystem.EmissionShape.Edge:
                    {
                        EditorGUILayout.PropertyField(property.FindPropertyRelative("startEdge"));
                        EditorGUILayout.PropertyField(property.FindPropertyRelative("endEdge"));
                    }
                    break;
                default:
                    break;
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.PropertyField(property.FindPropertyRelative("rateOverTime"));
        }
    }
}