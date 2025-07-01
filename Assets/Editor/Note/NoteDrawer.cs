using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NoteComponent))]
public class NoteDrawer : Editor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    static void DrawNoteGizmo(NoteComponent note, GizmoType gizmoType)
    {
        if (string.IsNullOrEmpty(note.noteText)) return;

        Vector3 pos = note.transform.position + Vector3.up * 2f;

        GUIStyle style = new GUIStyle();
        style.fontSize = note.fontSize;
        style.normal.textColor = note.textColor;
        style.fontStyle = note.fontStyle;
        style.alignment = note.alignment;

        if (note.showBackground)
        {
            Texture2D bg = new Texture2D(1, 1);
            bg.SetPixel(0, 0, note.backgroundColor);
            bg.Apply();
            style.normal.background = bg;
            style.padding = new RectOffset(4, 4, 2, 2);
        }

        Handles.Label(pos, note.noteText, style);
    }
}