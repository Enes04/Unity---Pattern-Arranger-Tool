using UnityEditor;
using UnityEngine;

public class NoteEditorMenu
{
    [MenuItem("GameObject/Add Note", false, 0)]
    public static void AddNote(MenuCommand command)
    {
        GameObject obj = command.context as GameObject;
        if (obj != null && obj.GetComponent<NoteComponent>() == null)
        {
            obj.AddComponent<NoteComponent>();
        }
    }
}