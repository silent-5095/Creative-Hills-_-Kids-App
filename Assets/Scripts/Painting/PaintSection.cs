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
        [SerializeField] private int id;
        [HideInInspector] [SerializeField] private SolidPaintingTexture solidPaintingTextures;
        [HideInInspector] [SerializeField] private BrushPaintingTexture brushPaintingTextures;
        [SerializeField] private AudioSource source;

        [HideInInspector] [SerializeField] private new SpriteRenderer renderer;

        [HideInInspector] [SerializeField] private SpriteMask mask;

        [SerializeField] private float touchDlyTime;
        private float _touchTime;
        private bool _beginTouch, _correct;
        private Vector2 _bTouchPos;
        private PaintSectionProp _pSectionProp;

        [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
        private void OnDestroy()
        {
            PaintController.ResetAllEvent -= OnResetAllEvent;
        }

        private void Start()
        {
            PaintController.ResetAllEvent += OnResetAllEvent;
            touchDlyTime = PaintController.Instance.touchDly;
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

        private void Update()
        {
            if (_beginTouch)
            {
                _touchTime += Time.deltaTime;
            }
        }

        public void OnBeganTouchHandler()
        {
            _touchTime = 0;
            _beginTouch = true;
            _bTouchPos = Input.touches[0].position;
        }

        public void OnMoveTouchHandler(Vector3 position)
        {
            if (_bTouchPos.magnitude - Input.touches[0].position.magnitude > 1)
            {
                _correct = false;
            }
        }

        public void OnStationaryTouchHandler(Vector3 position)
        {
        }

        public void OnMoveTouchHandler()
        {
            _correct = false;
        }

        public void OnEndTouchHandler()
        {
                _correct = _touchTime <touchDlyTime;
            _beginTouch = false;
            _touchTime = 0;
            if (!_correct || Input.touchCount >= 2)
                return;
            source.Play();
            solidPaintingTextures.ResetTextures();
            brushPaintingTextures.ResetTextures();
            switch (PaintController.GetPaintType())
            {
                case PaintType.None:
                    break;
                case PaintType.Color:
                    var tempTexture = brushPaintingTextures.GetTexture(PaintController.GetBrushIndex());
                    mask.enabled = tempTexture is not null;
                    if (tempTexture is null)
                        break;
                    tempTexture.Active(true);
                    tempTexture.SetColor(PaintController.GetColor());
                    break;
                case PaintType.Texture:
                    mask.enabled = true;
                    renderer.color = Color.white;
                    solidPaintingTextures.GetTexture(PaintController.GetPatternIndex()).Active(true);

                    break;
                case PaintType.Brush:
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
        
    }

    [Serializable]
    public class PaintSectionProp
    {
        public Color SolidColor { get; set; }
        public int BrushId { get; set; }
        public int TextureId { get; set; }
    }
}