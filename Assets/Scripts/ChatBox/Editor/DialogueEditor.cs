using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Dialogue))]
public class DialogueEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Dialogue dialogue = (Dialogue)target;

        SerializedProperty dialogueLinesProperty = serializedObject.FindProperty("dialogueLines");
        for (int i = 0; i < dialogueLinesProperty.arraySize; i++)
        {
            SerializedProperty element = dialogueLinesProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(element, new GUIContent($"Element {i}"), true);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Move Up") && i > 0)
            {
                dialogueLinesProperty.MoveArrayElement(i, i - 1);
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Move Down") && i < dialogueLinesProperty.arraySize - 1)
            {
                dialogueLinesProperty.MoveArrayElement(i, i + 1);
            }
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Add Dialogue Line"))
        {
            AddElement(dialogueLinesProperty, typeof(DialogueLine));
        }

        if (GUILayout.Button("Remove Last Element"))
        {
            dialogueLinesProperty.arraySize--;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void AddElement(SerializedProperty list, System.Type type)
    {
        list.arraySize++;
        SerializedProperty element = list.GetArrayElementAtIndex(list.arraySize - 1);
        element.managedReferenceValue = System.Activator.CreateInstance(type);
    }
}
