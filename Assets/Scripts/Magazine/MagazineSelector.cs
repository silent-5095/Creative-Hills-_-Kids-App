using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Magazine
{
    public class MagazineSelector : MonoBehaviour
    {
        public event Action<MagazineData> SelectorEvent;
        [SerializeField] private Button button;
        [SerializeField] private Image image;
        [Range(0,1)]
        [SerializeField] private float clickAnimationDuration,scaleInPush;

        private MagazineData _data;

        public void SetData(MagazineData data)
        {
            _data = data;
            image.sprite = _data.Icon;
        }

        private void Start()
        {
            button.onClick.AddListener(OnButtonClick);
        }


        private void OnButtonClick()
        {
            var tween=transform.DOScale(Vector3.one*scaleInPush,clickAnimationDuration/2);
            tween.onComplete += () =>
            {
                var tween1 = transform.DOScale(Vector3.one, clickAnimationDuration/2);
                tween1.onComplete += () =>
                {
                    SelectorEvent?.Invoke(_data);
                    tween.Kill();
                    tween1.Kill();
                };
            };
        }
    }
}
