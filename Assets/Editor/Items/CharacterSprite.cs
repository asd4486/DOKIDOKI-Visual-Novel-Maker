using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Editor.Items
{
    public class CharacterSprite
    {
        public Sprite Sprite;
        public string Name;
        public List<CharacterSprite_Face> Faces = new List<CharacterSprite_Face>();
        public bool OpenFaceDropDown;
    }
}

