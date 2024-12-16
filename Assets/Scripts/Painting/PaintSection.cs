using System;
using System.Diagnostics.CodeAnalysis;
using Interfaces;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using ColorUtility = UnityEngine.ColorUtility;

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
            Invoke(nameof(SetData), 0.1f);
        }

        private void SetData()
        {
            _pSectionProp = LoadData();
            brushPaintingTextures.GetTexture(_pSectionProp.BrushId).Active(true);
            brushPaintingTextures.GetTexture(_pSectionProp.BrushId).SetColor(_pSectionProp.GetColor());
            if (_pSectionProp.TextureId > -1)
                solidPaintingTextures.GetTexture(_pSectionProp.TextureId).Active(true);
            mask.enabled = true;
        }

        private void OnResetAllEvent()
        {
            solidPaintingTextures.ResetTextures();
            brushPaintingTextures.ResetTextures();
            Debug.Log("OnResetAll");
            PlayerPrefs.DeleteKey(gameObject.name+SceneManager.GetActiveScene().name);
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
            _correct = _touchTime < touchDlyTime;
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
                    _pSectionProp.BrushId = PaintController.GetBrushIndex();
                    _pSectionProp.SetColor(PaintController.GetColor());
                    _pSectionProp.TextureId = -1;
                    break;
                case PaintType.Texture:
                    mask.enabled = true;
                    renderer.color = Color.white;
                    solidPaintingTextures.GetTexture(PaintController.GetPatternIndex()).Active(true);
                    _pSectionProp.BrushId = 0;
                    _pSectionProp.TextureId = PaintController.GetPatternIndex();
                    _pSectionProp.SetColor(Color.white);

                    break;
                case PaintType.Brush:
                    var tTexture = brushPaintingTextures.GetTexture(PaintController.GetBrushIndex());
                    mask.enabled = tTexture is not null;
                    if (tTexture is null)
                        break;
                    tTexture.Active(true);
                    tTexture.SetColor(PaintController.GetColor());
                    _pSectionProp.BrushId = PaintController.GetBrushIndex();
                    _pSectionProp.SetColor(PaintController.GetColor());
                    _pSectionProp.TextureId = -1;
                    break;
                case PaintType.Erase:
                    renderer.color = Color.white;
                    mask.enabled = false;
                    _pSectionProp.BrushId = 0;
                    _pSectionProp.SetColor(Color.white);
                    _pSectionProp.TextureId = -1;
                    break;
                default:
                    renderer.color = PaintController.GetColor();
                    _pSectionProp.BrushId = 0;
                    _pSectionProp.SetColor(Color.white);
                    _pSectionProp.TextureId = -1;
                    mask.enabled = false;
                    break;
            }

            SaveProp();
        }

        private void SaveProp()
        {
            var dataString = _pSectionProp.ToString();
            // Debug.Log(_pSectionProp.SolidColor);
            // Debug.Log(gameObject.name, gameObject);
            PlayerPrefs.SetString(gameObject.name+SceneManager.GetActiveScene().name, dataString);
            Debug.Log($"{gameObject.name}  {_pSectionProp is null}  {dataString}", gameObject);
            Debug.Log(PlayerPrefs.GetString(gameObject.name));
        }

        private PaintSectionProp LoadData()
        {
            var dataString = PlayerPrefs.GetString(gameObject.name+SceneManager.GetActiveScene().name, string.Empty);

            var prop = JsonConvert.DeserializeObject<PaintSectionProp>(dataString);
            Debug.Log($"{gameObject.name}  {dataString} | {prop?.SolidColor}", gameObject);
            return string.IsNullOrEmpty(dataString)
                ? new PaintSectionProp()
                : JsonConvert.DeserializeObject<PaintSectionProp>(dataString);
        }
    }

    [Serializable]
    public class PaintSectionProp
    {
        public PaintSectionProp()
        {
            SolidColor = ColorUtility.ToHtmlStringRGB(Color.white);
            BrushId = 0;
            TextureId = -1;
        }

        public string SolidColor { get; set; }
        public int BrushId { get; set; }
        public int TextureId { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public Color GetColor()
        {
            return PaintingColorHandler.GetColorFromString(SolidColor);
        }

        public void SetColor(Color color)
        {
            SolidColor = PaintingColorHandler.GetStringFromColor(color);
        }
    }
}