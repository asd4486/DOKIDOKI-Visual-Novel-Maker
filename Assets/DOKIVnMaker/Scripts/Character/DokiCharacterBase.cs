using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DokiVnMaker.Character
{
    public class DokiCharacterBase : MonoBehaviour
    {
        [SerializeField] CharacterObject currentCharacterObject;
        public CharacterObject CurrentCharacterObject { get { return currentCharacterObject; } }

        [SerializeField] Image bodySprite;
        RectTransform bodyRect;
        List<Image> faces = new List<Image>();
        public List<Image> Faces { get { return faces; } }

        private void Awake()
        {
            bodyRect = bodySprite.GetComponent<RectTransform>();
        }

        public void Init(CharacterObject charaObj)
        {
            currentCharacterObject = charaObj;

            gameObject.name = "character_" + charaObj.charaName;
            bodySprite.sprite = charaObj.bodySprite;

            bodyRect.anchoredPosition = charaObj.offsetPos;
            bodyRect.sizeDelta = charaObj.autoSize ? new Vector2(charaObj.bodySprite.texture.width, charaObj.bodySprite.texture.height) : charaObj.size;

            for (int i = 0; i < charaObj.faces.Count; i++)
            {
                var face = charaObj.faces[i];
                if (face.sprite != null)
                {
                    var newFaceImg = new GameObject("face_" + i).AddComponent<Image>();
                    newFaceImg.transform.SetParent(bodyRect, false);
                    newFaceImg.sprite = face.sprite;
                    var faceRect = newFaceImg.GetComponent<RectTransform>();
                    faceRect.anchoredPosition = face.offsetPos;
                    faceRect.sizeDelta = face.autoSize ? new Vector2(face.sprite.texture.width, face.sprite.texture.height) : face.size;
                }
            }
        }
    }
}