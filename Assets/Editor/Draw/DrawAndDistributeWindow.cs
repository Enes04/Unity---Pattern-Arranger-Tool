using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

public class DrawAndDistributeWindow : EditorWindow
{
    private GameObject prefabToPlace;
    private bool isDrawing = false;
    private float spacing = 1f;
    private List<Vector3> points = new List<Vector3>();

    [MenuItem("Tools/Draw & Distribute")]
    public static void ShowWindow()
    {
        GetWindow<DrawAndDistributeWindow>("Draw & Distribute");
    }

    private void OnGUI()
    {
        GUILayout.Label("Draw & Distribute Tool", EditorStyles.boldLabel);
        prefabToPlace = (GameObject)EditorGUILayout.ObjectField("Prefab to Place:", prefabToPlace, typeof(GameObject), false);
        spacing = EditorGUILayout.FloatField("Spacing:", spacing);

        if (!isDrawing)
        {
            if (GUILayout.Button("Çizime Başla"))
                isDrawing = true;
        }
        else
        {
            if (GUILayout.Button("Çizimi Bitir"))
                isDrawing = false;
        }

        if (points.Count > 1 && GUILayout.Button("Dağıt"))
            DistributeAlongPath();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += DuringSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= DuringSceneGUI;
    }

    private void DuringSceneGUI(SceneView sv)
    {
        if (!isDrawing) return;

        var e = Event.current;
        int id = GUIUtility.GetControlID(FocusType.Passive);
        // Sol fareyle çiz
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            points.Clear();
            e.Use();
        }
        else if (e.type == EventType.MouseDrag && e.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                // Yalnızca yeni nokta ekle
                if (points.Count == 0 || Vector3.Distance(points[points.Count - 1], hit.point) > 0.1f)
                    points.Add(hit.point);
                e.Use();
            }
        }
        else if (e.type == EventType.MouseUp && e.button == 0)
        {
            e.Use();
        }

        // Çizgiyi görselleştir
        Handles.color = Color.green;
        for (int i = 1; i < points.Count; i++)
            Handles.DrawLine(points[i - 1], points[i]);

        // Sahneyi güncelle
        if (points.Count > 1)
            sv.Repaint();
    }

    private void DistributeAlongPath()
    {
        if (prefabToPlace == null)
        {
            Debug.LogWarning("Lütfen bir prefab seçin!");
            return;
        }

        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Distribute Along Path");

        float distanceAccumulator = 0f;
        int segmentIndex = 0;

        while (segmentIndex < points.Count - 1)
        {
            Vector3 start = points[segmentIndex];
            Vector3 end   = points[segmentIndex + 1];
            float segmentLength = Vector3.Distance(start, end);

            // Halen bu segment içindeysek
            if (distanceAccumulator <= segmentLength)
            {
                // Segment üzerindeki konum
                float t = distanceAccumulator / segmentLength;
                Vector3 position = Vector3.Lerp(start, end, t);

                // Prefab'ı sahneye instantiate et
                var instance = (GameObject)PrefabUtility.InstantiatePrefab(
                    prefabToPlace,
                    EditorSceneManager.GetActiveScene()
                );

                // Undo kaydı ekle
                Undo.RegisterCreatedObjectUndo(instance, "Distribute Along Path");

                // Pozisyonu ayarla
                instance.transform.position = position;

                // Bir sonraki objeyi yerleştirmek için mesafeyi artır
                distanceAccumulator += spacing;
            }
            else
            {
                // Bu segment'teki yeri aştık, bir sonraki segmente geç
                distanceAccumulator -= segmentLength;
                segmentIndex++;
            }
        }

        points.Clear();
    }
}
