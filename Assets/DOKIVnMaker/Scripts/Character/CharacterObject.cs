using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DokiVnMaker.Tools;

namespace DokiVnMaker.Character
{
    [CreateAssetMenu(order = 0, fileName = "NewCharacter", menuName = "Doki VN Maker/Create Character")]
    public class CharacterObject : ScriptableObject
    {
        public GameObject currentCharacterObject;

        public string charaName;
        public List<FaceSprite> faces = new List<FaceSprite>();
        public Sprite bodySprite;
        public bool autoSize = true;
        public Vector2 size;
        public Vector2 offsetPos;

        [System.Serializable]
        public class FaceSprite
        {
            public string faceName;
            public Sprite sprite;
            public Vector2 offsetPos;
            public bool autoSize = true;
            public Vector2 size;

            public override bool Equals(object obj)
            {
                if (!(obj is FaceSprite)) return false;
                var o = obj as FaceSprite;

                bool isEqual = faceName == o.faceName && offsetPos == o.offsetPos && autoSize == o.autoSize && size == o.size;

                if (sprite == null)
                    return isEqual && sprite == o.sprite;
                return isEqual && sprite.GetInstanceID() == o.sprite.GetInstanceID();
            }
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        public bool dropDownFaces;

        public void AddFace()
        {
            if (faces.Count > 10) return;
            faces.Add(new FaceSprite());
        }

        public void RemoveFace()
        {
            if (faces.Count < 1) return;
            faces.RemoveAt(faces.Count - 1);
        }

        public void Revert(CharacterObject charaObj)
        {
            bodySprite = charaObj.bodySprite;
            charaName = charaObj.charaName;
            faces = charaObj.faces;
            autoSize = charaObj.autoSize;
            size = charaObj.size;
        }

        public override bool Equals(object other)
        {
            if (!(other is CharacterObject)) return false;
            var o = other as CharacterObject;

            if (faces.Count != o.faces.Count) return false;
            foreach (var f in faces)
            {
                if (!o.faces.Contains(f))
                    return false;
            }

            return currentCharacterObject == o.currentCharacterObject && bodySprite == o.bodySprite && charaName == o.charaName &&
                autoSize == o.autoSize && size == o.size && offsetPos ==o.offsetPos;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}