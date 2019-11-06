using UnityEngine;
using UnityEditor;

namespace DokiVnMaker
{
    [CreateAssetMenu(order = 0, fileName = "NewTextStyle", menuName = "Doki VN Maker/Create TextStyle")]
    public class TextStyleObject : ScriptableObject
    {
        public Font font /*= Resources.Load("ARLRDBD") as Font*/;
        public int fontSize = 20;
        public Color textColor = new Color(0.8f, 1, 1);
        public float displaySpeed = 3;
    }
}