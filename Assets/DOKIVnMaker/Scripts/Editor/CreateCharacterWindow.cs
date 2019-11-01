using DokiVnMaker.MyEditor.Items;
using DokiVnMaker.Tools;
using Editor.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DokiVnMaker.MyEditor
{
    public class CreateCharacterWindow : EditorWindow
    {
        [MenuItem("DokiDoki VN Maker/New Character")]
        static void Init()
        {
            CreateCharacterWindow window =
                (CreateCharacterWindow)EditorWindow.GetWindow(typeof(CreateCharacterWindow), true, "Create Character");
        }

        public string CharaName;

        public int SpriteCount;

        Vector2 Scroller { get; set; }

        public List<CharacterSprite> CharacterSpriteList = new List<CharacterSprite>();

        private void OnGUI()
        {
            GUILayout.Space(10);
            //character name
            CharaName = EditorGUILayout.TextField("Character Name:", CharaName);
            GUILayout.Space(10);
            GUILayout.Label("Add character sprites");

            //character sprite editor
            Scroller = GUILayout.BeginScrollView(Scroller);
            for (int i = 0; i < SpriteCount; i++)
            {
                var mySprite = CharacterSpriteList[i];
                GUILayout.BeginVertical("box");
                //auto name if string is empty
                if (string.IsNullOrEmpty(mySprite.Name)) mySprite.Name = "Sprite_" + i;

                mySprite.Name = EditorGUILayout.TextField("Sprite name:", mySprite.Name);
                //body sprite
                mySprite.Sprite = EditorGUILayout.ObjectField("Body", mySprite.Sprite, typeof(Sprite), true) as Sprite;

                //face sprite
                if (mySprite.OpenFaceDropDown = EditorGUILayout.Foldout(mySprite.OpenFaceDropDown, "Faces"))
                {
                    GUILayout.BeginVertical("box");

                    var faceGroup = mySprite.Faces;

                    for (int j = 0; j < faceGroup.Count; j++)
                    {
                        var myFace = faceGroup[j];
                        GUILayout.BeginHorizontal();
                        //auto name if string is empty
                        if (string.IsNullOrEmpty(myFace.Name)) myFace.Name = "emotion_" + j;

                        myFace.Name = EditorGUILayout.TextField(myFace.Name, GUILayout.Width(90));
                        myFace.Sprite = EditorGUILayout.ObjectField(myFace.Sprite, typeof(Sprite), true) as Sprite;
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("+", GUILayout.Width(50)))
                    {
                        faceGroup.Add(new CharacterSprite_Face());
                    }
                    if (GUILayout.Button("-", GUILayout.Width(50)) && faceGroup.Count > 0)
                    {
                        var index = faceGroup.Count - 1;
                        faceGroup.RemoveAt(index);

                    }
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndScrollView();

            //add new character maximun 10
            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("+") && SpriteCount < 6)
            {
                CharacterSpriteList.Add(new CharacterSprite());
                SpriteCount += 1;
            }
            if (GUILayout.Button("-") && SpriteCount > 0)
            {
                SpriteCount -= 1;
                CharacterSpriteList.RemoveAt(SpriteCount);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Create new", GUILayout.Width(100), GUILayout.Height(30)))
            {
                AddNewSpriteGroup();
            }
            GUILayout.EndHorizontal();
        }

        void AddNewSpriteGroup()
        {
            //conditions
            if (string.IsNullOrEmpty(CharaName))
            {
                EditorUtility.DisplayDialog("OOOOOps", "Character missing a DOKI DOKI name!!", "OK");
                return;
            }
            if (SpriteCount <= 0)
            {
                EditorUtility.DisplayDialog("OOOOOps", "Character missing a DOKI DOKI sprite!!", "OK");
                return;
            }
            bool isSpriteEmpty = true;
            foreach (var sp in CharacterSpriteList)
            {
                if (sp.Sprite != null) isSpriteEmpty = false;
            }
            if (isSpriteEmpty)
            {
                EditorUtility.DisplayDialog("OOOOOps", "Character missing a DOKI DOKI sprite!!", "OK");
                return;
            }

            //character component
            GameObject chara = new GameObject();
            chara.name = "chara_" + CharaName;
            chara.AddComponent<SpriteRenderer>();

            for (int i = 0; i < SpriteCount; i++)
            {
                var spriteInfo = CharacterSpriteList[i];
                //create sprites for character
                if (spriteInfo.Sprite != null)
                {
                    GameObject mySprite = new GameObject();
                    mySprite.name = spriteInfo.Name;
                    var spRendere = mySprite.AddComponent<SpriteRenderer>();
                    spRendere.sortingLayerName = "character";
                    spRendere.sprite = spriteInfo.Sprite;
                    var spInfos = mySprite.AddComponent<CharaSpriteSetting>();

                    //face group
                    GameObject faces = new GameObject();
                    faces.name = "faces";
                    //set parent
                    faces.transform.parent = mySprite.transform;

                    //create faces
                    for (int j = 0; j < spriteInfo.Faces.Count; j++)
                    {
                        var faceInfo = spriteInfo.Faces[j];
                        if (faceInfo.Sprite != null)
                        {
                            GameObject face = new GameObject();
                            face.name = faceInfo.Name;
                            var faceRendere = face.AddComponent<SpriteRenderer>();
                            faceRendere.sortingLayerName = "character";
                            faceRendere.sortingOrder = 9;
                            faceRendere.sprite = faceInfo.Sprite;

                            var facePos = face.AddComponent<CharaFaceSetting>();
                            facePos.xPos = face.transform.localPosition.x;
                            facePos.yPos = face.transform.localPosition.y;
                            face.transform.parent = faces.transform;
                        }
                    }
                    //add sprite to character object
                    mySprite.transform.parent = chara.transform;
                    float adjusY = spRendere.bounds.size.y / 2;
                    mySprite.transform.localPosition = new Vector2(0, adjusY);
                }
            }

            //save to game source folder
            string path = ValueManager.CharaPath + chara.name + ".prefab";
            CreateObjHelper.SaveGameobj(chara, path);

            //destroy Temporary File in scene
            DestroyImmediate(chara);

            EditorUtility.DisplayDialog("WOAH!!", "A new dokidoki character are created !!", "OK");
        }

    }
}