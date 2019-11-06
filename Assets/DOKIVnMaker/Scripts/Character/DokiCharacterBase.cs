using DG.Tweening;
using DokiVnMaker.Story;
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
        RectTransform myRect;
        List<Image> faces = new List<Image>();
        public List<Image> Faces { get { return faces; } }

        private void Awake()
        {
            myRect = GetComponent<RectTransform>();
            bodySprite.color = new Color(1, 1, 1, 0);
        }

        public void Init(CharacterObject charaObj)
        {
            currentCharacterObject = charaObj;

            gameObject.name = "character_" + charaObj.charaName;
            bodySprite.sprite = charaObj.bodySprite;

            var bodyRect = bodySprite.GetComponent<RectTransform>();
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
                    faces.Add(newFaceImg);

                    newFaceImg.color = new Color(1, 1, 1, 0);
                }
            }
        }

        public void FadeIn(CharacterShowBase node)
        {
            bodySprite.DOColor(Color.white, node.duration);
            foreach (var f in faces)
                if (f.color.a > 0) f.DOColor(new Color(1, 1, 1, 0), node.duration);

            if (node.faceIndex >= 0 && faces.Count > node.faceIndex)
                faces[node.faceIndex].DOColor(Color.white, node.duration);

            switch (node.displayPos)
            {
                case DisplayPositions.Left:
                    myRect.anchoredPosition = new Vector2(-400, 0);
                    break;
                case DisplayPositions.Center:
                    myRect.anchoredPosition = Vector2.zero;
                    break;
                case DisplayPositions.Right:
                    myRect.anchoredPosition = new Vector2(400, 0);
                    break;
                case DisplayPositions.Custom:
                    myRect.anchoredPosition = node.customPos;
                    break;
            }
        }

        public void FadeOut(float duration)
        {
            bodySprite.DOColor(new Color(1, 1, 1, 0), duration);
            foreach (var f in faces)   
                if (f.color.a > 0) f.DOColor(new Color(1, 1, 1, 0), duration);   
        }
    }
}