using UnityEngine;
using UnityEditor;

public class PatternArrangerTool: EditorWindow
{
    private enum PatternType { Grid, Circle, Triangle ,Staircase}

    private PatternType selectedPattern = PatternType.Grid;
    private float spacing = 2f;
    private float radius = 5f;
    private float stairHeight = 1f;

    [MenuItem("Tools/Pattern Arranger Tool")]
    public static void ShowWindow()
    {
        GetWindow<PatternArrangerTool>("Pattern Arranger");
    }

    private void OnGUI()
    {
        GUILayout.Label("Objeleri Diz", EditorStyles.boldLabel);

        selectedPattern = (PatternType)EditorGUILayout.EnumPopup("Dizilim Türü", selectedPattern);
        spacing = EditorGUILayout.FloatField("Boşluk", spacing);
        if (selectedPattern == PatternType.Circle)
        {
            radius = EditorGUILayout.FloatField("Yarıçap (Çember için)", radius);
        }
        if (selectedPattern == PatternType.Staircase)
        {
            stairHeight = EditorGUILayout.FloatField("Basamak Yüksekliği", stairHeight);
        }
        if (GUILayout.Button("Diz!"))
        {
            ArrangeObjects();
        }
    }

    private void ArrangeObjects()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("Hiç obje seçmedin!");
            return;
        }

        Undo.RecordObjects(selectedObjects, "Pattern Arrange");

        switch (selectedPattern)
        {
            case PatternType.Grid:
                ArrangeGrid(selectedObjects);
                break;
            case PatternType.Circle:
                ArrangeCircle(selectedObjects);
                break;
            case PatternType.Triangle:
                ArrangeTriangle(selectedObjects);
                break;
            case PatternType.Staircase:
                ArrangeStaircase(selectedObjects);
                break;
        }
    }
    private void ArrangeStaircase(GameObject[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            Vector3 newPos = new Vector3(i * spacing, i * stairHeight, 0);
            objects[i].transform.position = newPos;
        }
    }

    private void ArrangeGrid(GameObject[] objects)
    {
        int count = objects.Length;
        int columns = Mathf.CeilToInt(Mathf.Sqrt(count));

        for (int i = 0; i < count; i++)
        {
            int row = i / columns;
            int col = i % columns;

            Vector3 newPos = new Vector3(col * spacing, 0, row * spacing);
            objects[i].transform.position = newPos;
        }
    }

    private void ArrangeCircle(GameObject[] objects)
    {
        int count = objects.Length;
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            objects[i].transform.position = newPos;
        }
    }

    private void ArrangeTriangle(GameObject[] objects)
    {
        int index = 0;
        int row = 1;

        while (index < objects.Length)
        {
            for (int i = 0; i < row && index < objects.Length; i++)
            {
                float x = (i - row / 2f) * spacing;
                float z = -row * spacing;
                Vector3 newPos = new Vector3(x, 0, z);
                objects[index].transform.position = newPos;
                index++;
            }
            row++;
        }
    }
}
