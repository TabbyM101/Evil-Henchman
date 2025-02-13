#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DayObj))]
public class DayObjEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DayObj myTarget = (DayObj)target;

        myTarget.DayName = EditorGUILayout.TextField("Day Name", myTarget.DayName);
        myTarget.IsTimed = EditorGUILayout.Toggle("Is Timed", myTarget.IsTimed);
        if (myTarget.IsTimed)
        {
            myTarget.Duration = EditorGUILayout.FloatField("Duration", myTarget.Duration);
        }

        SerializedProperty minigamesProperty = serializedObject.FindProperty("Minigames");
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(minigamesProperty, true);
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif