using System;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    public class OptionButton : MonoBehaviour
    {
        public event Action<OptionProp> ButtonClickEvent;
        public static event Action<OptionProp> InternalClickEvent;
        [SerializeField] private Button button;
        [SerializeField] private RTLTextMeshPro text;
        [SerializeField] private OptionProp optionProp;
        [SerializeField] private bool isAnswer;


        public void SetData(OptionProp option)
        {
            optionProp = option;
            text.text = option.GetOption().Key;
            isAnswer = option.GetOption().Value;
            button.interactable = true;
            button.image.color = Color.white;
        }

        private void OnDestroy()
        {
            // ButtonClickEvent -= OnButtonClickEvent;
            // ButtonClickEvent -= InternalClickEvent;
            InternalClickEvent -= OnButtonClickEvent;
        }

        private void Awake()
        {
            // ButtonClickEvent += OnButtonClickEvent;
            // ButtonClickEvent += InternalClickEvent;
            InternalClickEvent += OnButtonClickEvent;
        }

        private void Start()
        {
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            if (optionProp is null)
                return;
            Debug.Log($"it is pushed button and color is {button.image.color}");
            ButtonClickEvent?.Invoke(optionProp);
            InternalClickEvent?.Invoke(optionProp);
        }

        private void OnButtonClickEvent(OptionProp prop)
        {
            button.interactable = false;
            Debug.Log($"it is pushed button and color is {button.image.color}");
            if (optionProp == prop)
            {
                button.image.color = isAnswer ? Color.green : Color.red;
                Debug.Log($"it is pushed button and color is {button.image.color}");
            }
            if (isAnswer)
            {
                button.image.color = Color.green;
                Debug.Log($"it is correct button and color is {button.image.color}");
            }
        }
    }
}