using UnityEngine;
using UnityEditor;

namespace Nirvana
{
    public class AbstractModuleDrawer : PropertyDrawer
    {
        private static GUIStyle _headerStyle;
        protected static GUIStyle HeaderStyle
        {
            get
            {
                if(_headerStyle == null)
                {
                    _headerStyle = new GUIStyle("ToolbarButton");
                    _headerStyle.fontStyle = FontStyle.Bold;
                }

                return _headerStyle;
            }
        }
        private Rect _headerRect;
        private bool _foldout;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            this.DrawHeader(label);

            if(_foldout)
            {
                EditorGUI.indentLevel++;
                this.DrawProperty(position, property, label);
                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        private void DrawHeader(GUIContent label)
        {
            EditorGUILayout.LabelField(label, HeaderStyle, GUILayout.Height(30.0f));
            if(Event.current.type == EventType.Repaint)
            {
                _headerRect = GUILayoutUtility.GetLastRect();
            }

            _foldout = EditorGUI.Foldout(new Rect(_headerRect.x, _headerRect.y, 30.0f, _headerRect.height), _foldout, "");
        }

        protected virtual void DrawProperty(Rect position, SerializedProperty property, GUIContent label) { }
    }
}