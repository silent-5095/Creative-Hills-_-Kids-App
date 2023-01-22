using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Painting
{
    [RequireComponent(typeof(SpriteRenderer), typeof(SpriteMask), typeof(PolygonCollider2D))]
    public class PaintSection : MonoBehaviour, ITouchable
    {
        [HideInInspector]
        [SerializeField] private SolidPaintingTexture solidPaintingTextures;
        [HideInInspector]
        [SerializeField] private BrushPaintingTexture brushPaintingTextures;
        [SerializeField] private AudioSource source;
        
        [HideInInspector]
        [SerializeField] private new SpriteRenderer renderer;
        
        [HideInInspector]
        [SerializeField] private SpriteMask mask;

        [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
        private void OnDestroy()
        {
            PaintController.ResetAllEvent -= OnResetAllEvent;
        }

        private void Start()
        {
            PaintController.ResetAllEvent += OnResetAllEvent;
            renderer = GetComponent<SpriteRenderer>();
            mask = GetComponent<SpriteMask>();
            renderer.sortingLayerName = "Player";
            mask.isCustomRangeActive = true;
            mask.frontSortingLayerID = renderer.sortingLayerID;
            mask.frontSortingOrder = renderer.sortingOrder + 1;
            mask.backSortingLayerID = renderer.sortingLayerID;
            mask.backSortingOrder = renderer.sortingOrder;
            solidPaintingTextures = GetComponentInChildren<SolidPaintingTexture>();
            brushPaintingTextures = GetComponentInChildren<BrushPaintingTexture>();
            solidPaintingTextures.SetTextureOrder(mask.frontSortingOrder);
            brushPaintingTextures.SetTextureOrder(mask.frontSortingOrder);
        }

        private void OnResetAllEvent()
        {
            solidPaintingTextures.ResetTextures();
            brushPaintingTextures.ResetTextures();
            renderer.color = Color.white;
        }

        public void OnBeganTouchHandler()
        {
            source.Play();
            Debug.Log("OnBegan Touch Handler");
            solidPaintingTextures.ResetTextures();
            brushPaintingTextures.ResetTextures();
            switch (PaintController.GetPaintType())
            {
                case PaintType.None:
                    break;
                case PaintType.Color:
                    // renderer.color = PaintController.GetColor();
                        Debug.Log("color");
                        var tempTexture = brushPaintingTextures.GetTexture(PaintController.GetBrushIndex());
                    mask.enabled = tempTexture is not null;
                        if (tempTexture is null)
                            break;
                        tempTexture.Active(true);
                        tempTexture.SetColor(PaintController.GetColor());
                    break;
                case PaintType.Texture:
                    Debug.Log("Texture");
                    mask.enabled = true;
                    renderer.color = Color.white;
                    solidPaintingTextures.GetTexture(PaintController.GetPatternIndex()).Active(true);

                    break;
                case PaintType.Brush:
                    // renderer.color = PaintController.GetColor();
                    Debug.Log("Brush");
                    var tTexture = brushPaintingTextures.GetTexture(PaintController.GetBrushIndex());
                    mask.enabled = tTexture is not null;
                    if (tTexture is null)
                        break;
                    tTexture.Active(true);
                    tTexture.SetColor(PaintController.GetColor());
                    break;
                case PaintType.Erase:
                    renderer.color = Color.white;
                    mask.enabled = false;
                    break;
                default:
                    renderer.color = PaintController.GetColor();
                    mask.enabled = false;
                    break;
            }
        }

        public void OnMoveTouchHandler(Vector3 position)
        {
        }

        public void OnMoveTouchHandler()
        {
        }

        public void OnEndTouchHandler()
        {
        }
    }
}