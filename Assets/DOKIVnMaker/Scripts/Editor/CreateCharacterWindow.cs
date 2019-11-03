//using DokiVnMaker.MyEditor.Items;
//using DokiVnMaker.Tools;
//using UnityEditor;
//using UnityEngine;
//using UnityEngine.UI;

//namespace DokiVnMaker.Character
//{
//    public class CreateCharacterWindow : EditorWindow
//    {
//        [MenuItem("DokiDoki VN Maker/New Character")]
//        static void Init()
//        {
//            CreateCharacterWindow window =
//                (CreateCharacterWindow)EditorWindow.GetWindow(typeof(CreateCharacterWindow), true, "Create Character");
//        }

//        Vector2 Scroller { get; set; }

//        CharacterObject charaObj = ScriptableObject.CreateInstance<CharacterObject>();
//        bool dropDownFaces;
//        private void OnGUI()
//        {
//            GUILayout.Space(10);
//            //character name
//            charaObj.charaName = EditorGUILayout.TextField("Name:", charaObj.charaName);
//            GUILayout.Space(10);
//            GUILayout.Label("Add character sprites");

//            //character sprite editor
//            Scroller = GUILayout.BeginScrollView(Scroller);
//            //for (int i = 0; i < SpriteCount; i++)
//            //{
//            //    var mySprite = CharacterSpriteList[i];
//            GUILayout.BeginVertical("box");
//            //auto name if string is empty
//            //if (string.IsNullOrEmpty(mySprite.Name)) mySprite.Name = "Sprite_" + i;

//            //mySprite.Name = EditorGUILayout.TextField("Sprite name:", mySprite.Name);
//            //body sprite
//            charaObj.sprite = EditorGUILayout.ObjectField("Body", charaObj.sprite, typeof(Sprite), true) as Sprite;

//            //face sprite
//            if (dropDownFaces == EditorGUILayout.Foldout(dropDownFaces, "Faces"))
//            {
//                GUILayout.BeginVertical("box");

//                var faceGroup = charaObj.faces;

//                for (int j = 0; j < faceGroup.Count; j++)
//                {
//                    var myFace = faceGroup[j];
//                    GUILayout.BeginHorizontal();
//                    //auto name if string is empty
//                    if (string.IsNullOrEmpty(myFace.faceName)) myFace.faceName = "emotion_" + j;

//                    myFace.faceName = EditorGUILayout.TextField(myFace.faceName, GUILayout.Width(90));
//                    myFace.sprite = EditorGUILayout.ObjectField(myFace.sprite, typeof(Sprite), true) as Sprite;
//                    GUILayout.EndHorizontal();
//                }

//                GUILayout.BeginHorizontal();
//                GUILayout.FlexibleSpace();
//                if (GUILayout.Button("+", GUILayout.Width(50)))
//                {
//                    faceGroup.Add(new CharacterFace());
//                }
//                if (GUILayout.Button("-", GUILayout.Width(50)) && faceGroup.Count > 0)
//                {
//                    var index = faceGroup.Count - 1;
//                    faceGroup.RemoveAt(index);

//                }
//                GUILayout.EndHorizontal();

//                GUILayout.EndVertical();
//            }
//            GUILayout.EndVertical();
//            //}
//            GUILayout.EndScrollView();

//            //add new character maximun 10
//            //GUILayout.BeginHorizontal("box");
//            //if (GUILayout.Button("+") && SpriteCount < 6)
//            //{
//            //    CharacterSpriteList.Add(new CharacterSprite());
//            //    SpriteCount += 1;
//            //}
//            //if (GUILayout.Button("-") && SpriteCount > 0)
//            //{
//            //    SpriteCount -= 1;
//            //    CharacterSpriteList.RemoveAt(SpriteCount);
//            //}
//            //GUILayout.EndHorizontal();

//            GUILayout.Space(10);
//            GUILayout.BeginHorizontal();
//            GUILayout.FlexibleSpace();
//            if (GUILayout.Button("Create new", GUILayout.Width(100), GUILayout.Height(30)))
//            {
//                AddNewSpriteGroup();
//            }
//            GUILayout.EndHorizontal();
//        }

//        void AddNewSpriteGroup()
//        {
//            //conditions
//            if (string.IsNullOrEmpty(charaObj.charaName))
//            {
//                EditorUtility.DisplayDialog("OOOOOps", "Character missing a DOKI DOKI name!!", "OK");
//                return;
//            }
//            if (charaObj.sprite == null)
//            {
//                EditorUtility.DisplayDialog("OOOOOps", "Character missing a DOKI DOKI sprite!!", "OK");
//                return;
//            }
//            //bool isSpriteEmpty = true;
//            //foreach (var sp in CharacterSpriteList)
//            //{
//            //    if (sp.Sprite != null) isSpriteEmpty = false;
//            //}
//            //if (isSpriteEmpty)
//            //{
//            //    EditorUtility.DisplayDialog("OOOOOps", "Character missing a DOKI DOKI sprite!!", "OK");
//            //    return;
//            //}

//            //character component
//            GameObject chara = new GameObject();
//            chara.name = "chara_" + charaObj.charaName;
//            chara.AddComponent<RectTransform>();

//            //for (int i = 0; i < SpriteCount; i++)
//            //{
//            //var spriteInfo = CharacterSpriteList[i];
//            //create sprites for character
//            //if (spriteInfo.Sprite != null)
//            //{

//            GameObject mySprite = new GameObject();
//            mySprite.name = charaObj.charaName + "Sprite";
//            var spRendere = mySprite.AddComponent<Image>();

//            //face group
//            GameObject faces = new GameObject();
//            faces.name = "faces";
//            //set parent
//            faces.transform.parent = mySprite.transform;

//            //create faces
//            for (int j = 0; j < charaObj.faces.Count; j++)
//            {
//                var faceInfo = charaObj.faces[j];
//                if (faceInfo.sprite != null)
//                {
//                    GameObject face = new GameObject();
//                    face.name = faceInfo.faceName;
//                    var faceRendere = face.AddComponent<Image>();
//                    faceRendere.sprite = faceInfo.sprite;

//                    var facePos = face.AddComponent<CharaFaceSetting>();
//                    facePos.xPos = face.transform.localPosition.x;
//                    facePos.yPos = face.transform.localPosition.y;
//                    face.transform.SetParent(faces.transform);
//                }
//            }
//            //add sprite to character object
//            mySprite.transform.SetParent(chara.transform);
//            //float adjusY = spRendere.bounds.size.y / 2;
//            //mySprite.transform.localPosition = new Vector2(0, adjusY);

//            //}
//            //}

//            //save to game source folder
//            string charaPath = ValueManager.CharaPath + chara.name;
//            CreateObjHelper.SaveGameobj(chara, charaPath + ".prefab");


//            charaObj.currentCharacterObject = chara;
//            CreateObjHelper.SaveGameobj(charaObj, charaPath + ".asset");

//            //destroy Temporary File in scene
//            DestroyImmediate(chara);

//            EditorUtility.DisplayDialog("WOAH!!", "A new dokidoki character are created !!", "OK");
//        }
//    }
//}