using UnityEngine;
using UnityEditor;

namespace DokiVnMaker.Character
{
    [CustomEditor(typeof(CharacterObject))]
    public class CharacterObjectEditor : Editor
    {
        CharacterObject oldCharaObj;
        CharacterObject charaObj;
        private void OnEnable()
        {
            charaObj = target as CharacterObject;
            if (string.IsNullOrEmpty(charaObj.charaName)) charaObj.charaName = charaObj.name;
            oldCharaObj = Instantiate(charaObj);
        }

        private void OnDisable()
        {
            if (!oldCharaObj.Equals(charaObj))
            {
                if (!EditorUtility.DisplayDialog("change not applied",
                        "Do you want to apply your changes of " + charaObj.name + "?",
                        "Yes", "No"))
                {
                    charaObj.Revert(oldCharaObj);
                }
            }
        }

        public override void OnInspectorGUI()
        {
            charaObj.charaName = EditorGUILayout.TextField("Name", charaObj.charaName);
            charaObj.bodySprite = (Sprite)EditorGUILayout.ObjectField("Body sprite", charaObj.bodySprite, typeof(Sprite), false, GUILayout.Height(65f));
            charaObj.offsetPos = EditorGUILayout.Vector2Field("Offset", charaObj.offsetPos);

            charaObj.autoSize = EditorGUILayout.Toggle("Auto size", charaObj.autoSize);
            if (!charaObj.autoSize)
                charaObj.size = EditorGUILayout.Vector2Field("Size", charaObj.size);
            GUILayout.Space(10);

            charaObj.dropDownFaces = EditorGUILayout.Foldout(charaObj.dropDownFaces, "Faces");
            //face sprite
            if (charaObj.dropDownFaces)
            {
                GUILayout.BeginVertical("box");
                for (int j = 0; j < charaObj.faces.Count; j++)
                {
                    GUILayout.BeginVertical("box");
                    var myFace = charaObj.faces[j];
                    GUILayout.BeginHorizontal();

                    GUILayout.BeginVertical();
                    //auto name if string is empty
                    if (string.IsNullOrEmpty(myFace.faceName)) myFace.faceName = "emotion_" + j;
                    myFace.faceName = EditorGUILayout.TextField(myFace.faceName);
                    myFace.offsetPos = EditorGUILayout.Vector2Field("Offset", myFace.offsetPos);
                    myFace.autoSize = EditorGUILayout.Toggle("Auto size", myFace.autoSize);
                    if (!myFace.autoSize)
                        EditorGUILayout.Vector2Field("Size", myFace.size);
                    GUILayout.EndVertical();

                    myFace.sprite = EditorGUILayout.ObjectField(myFace.sprite, typeof(Sprite), false, GUILayout.Width(65f), GUILayout.Height(65f)) as Sprite;
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                }

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("+", GUILayout.Width(50))) charaObj.AddFace();
                if (GUILayout.Button("-", GUILayout.Width(50))) charaObj.RemoveFace();
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Apply", GUILayout.Width(50), GUILayout.Height(20)))
            {
                oldCharaObj = Instantiate(charaObj);
            }
            EditorGUI.BeginDisabledGroup(charaObj.Equals(oldCharaObj));
            if (GUILayout.Button("Revert", GUILayout.Width(50), GUILayout.Height(20)))
            {
                charaObj.Revert(oldCharaObj);
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();

            EditorUtility.SetDirty(charaObj);
        }
    }
}