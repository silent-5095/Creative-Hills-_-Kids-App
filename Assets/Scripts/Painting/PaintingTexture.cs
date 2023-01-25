using System;
using System.Linq;
using UnityEngine;

namespace Painting
{
    public class PaintingTexture : MonoBehaviour
    {
        [SerializeField] internal  TextureProp[] brushTextures;

        public virtual bool IsActive()
        {
            var isActive = false;
            foreach (var brushTexture in brushTextures)
            {
                isActive = brushTexture.IsActive();
                if (isActive)
                    break;
            }

            return isActive;
        }

        internal virtual void Start()
        {
            ResetTextures();
        }

        public virtual void ResetTextures()
        {
            foreach (var brushTexture in brushTextures)
            {
                brushTexture.Active(false);
            }
        }

        public virtual void SetTextureOrder(int index)
        {
            foreach (var brushTexture in brushTextures)
            {
                brushTexture.SetOrder(index);
            }
        }

        public virtual TextureProp GetTexture(int id) => brushTextures.FirstOrDefault(x => x.Id == id);
    }

    [Serializable]
    public class TextureProp
    {
        [SerializeField] private int id;
        [SerializeField] private SpriteRenderer renderer;

        public void SetOrder(int index)
        {
            renderer.sortingLayerName = "Player";
            renderer.sortingOrder = index;
        }

        public void Active(bool con) => renderer.enabled = con;

        public bool IsActive()
        {
            return renderer.enabled;
        }

        public void SetColor(Color color) => renderer.color = color;

        public int Id => id;
    }
}