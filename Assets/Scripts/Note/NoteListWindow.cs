using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NoteListWindow : EditorWindow
{
    private Vector2 scrollPos;
    private List<NoteComponent> notes;

    [MenuItem("Window/Note List")]
    public static void ShowWindow()
    {
        GetWindow<NoteListWindow>("Notlar");
    }

    private void OnFocus()
    {
        RefreshNotes();
    }

    private void OnHierarchyChange()
    {
        RefreshNotes();
    }

    void RefreshNotes()
    {
        notes = new List<NoteComponent>(FindObjectsOfType<NoteComponent>());
        Repaint();
    }

    private void OnGUI()
    {
        if (notes == null) RefreshNotes();

        GUILayout.Label("Sahnedeki Notlar", EditorStyles.boldLabel);

        scrollPos = GUILayout.BeginScrollView(scrollPos);

        if (notes.Count == 0)
        {
            GUILayout.Label("Not bulunamadı.");
        }
        else
        {
            foreach (var note in notes)
            {
                GUILayout.BeginHorizontal();

                if (GUILayout.Button("🔍", GUILayout.Width(25)))
                {
                    // Objeye odaklan
                    Selection.activeGameObject = note.gameObject;
                    SceneView.FrameLastActiveSceneView();
                }

                // Notu düzenle
                string newText = GUILayout.TextField(note.noteText);

                if (newText != note.noteText)
                {
                    Undo.RecordObject(note, "Not Düzenlendi");
                    note.noteText = newText;
                    EditorUtility.SetDirty(note);
                }

                GUILayout.EndHorizontal();
            }
        }

        GUILayout.EndScrollView();
    }
}