using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UndertaleStyleText
{
    [System.Serializable]
    public struct ImageOrSpriteRenderer
    {
        public enum Type { Image, SpriteRenderer }

        [Tooltip("Type of visual element that this object holds")]
        public Type type;
        [Tooltip("UI image for the object")]
        public Image image;
        [Tooltip("Sprite renderer for the object")]
        public SpriteRenderer spriteRenderer;

        public void SetSprite(Sprite sprite)
        {
            if (type == Type.Image) image.sprite = sprite;
            else spriteRenderer.sprite = sprite;
        }
    }
}
