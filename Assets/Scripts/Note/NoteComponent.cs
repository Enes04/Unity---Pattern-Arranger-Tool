using UnityEditor;
using UnityEngine;

public class NoteComponent : MonoBehaviour
{
    [TextArea(2, 5)]
    public string noteText = "Notunuzu buraya yazın.";

    [Header("Yazı Stili")]
    public Color textColor = Color.yellow;
    public int fontSize = 14;
    public FontStyle fontStyle = FontStyle.Normal;
    public TextAnchor alignment = TextAnchor.MiddleCenter;
    public bool showBackground = false;
    public Color backgroundColor = new Color(0, 0, 0, 0.5f);
}

