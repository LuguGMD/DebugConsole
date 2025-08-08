using UnityEngine;
using UnityEditor;

namespace Lugu.Utils.Debug
{

    [CustomEditor(typeof(DebugController))]
    public class DebugControllerEditor : Editor
    {
        private DebugController m_debugController;
        private SerializedObject m_debugControllerSO;
        private SerializedProperty m_debugGroupsSP;

        private void OnEnable()
        {
            m_debugController = (DebugController)target;

            m_debugControllerSO = new SerializedObject(m_debugController);
            m_debugGroupsSP = m_debugControllerSO.FindProperty("m_debugGroups");
        }

        public override void OnInspectorGUI()
        {
            m_debugControllerSO.Update();

            EditorGUILayout.PropertyField(m_debugGroupsSP, true);

            if(m_debugControllerSO.ApplyModifiedProperties() && EditorApplication.isPlaying)
            {
                Undo.RecordObject(m_debugController, "Debug Groups Modified");
                m_debugController.UpdateDebugGroups();
            }
        }
    }

}
