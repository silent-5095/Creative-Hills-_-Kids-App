using System;
using RTLTMPro;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    public class OptionButton : MonoBehaviour
    {
        public event Action<OptionProp> ButtonClickEvent;
        public static event Action<OptionProp> InternalClickEvent;
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private ArabicFixerTMPRO fixer;
        [SerializeField] private OptionProp optionProp;
        [SerializeField] private bool isAnswer;


        public void SetData(OptionProp option)
        {
            optionProp = option;
            fixer.fixedText = option.GetOption().Key;
            isAnswer = option.GetOption().Value;
            button.interactable = true;
            button.image.color = Color.white;
        }

        private void OnDestroy()
        {
            InternalClickEvent -= OnButtonClickEvent;
        }

        private void Awake()
        {
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
            ButtonClickEvent?.Invoke(optionProp);
            InternalClickEvent?.Invoke(optionProp);
        }

        private void OnButtonClickEvent(OptionProp prop)
        {
            button.interactable = false;
            if (optionProp == prop)
            {
                button.image.color = isAnswer ? Color.green : Color.red;
            }
            if (isAnswer)
            {
                button.image.color = Color.green;
            }
        }
    }
}