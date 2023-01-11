using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    public class OptionButton : MonoBehaviour
    {
        public event Action<OptionProp> ButtonClickEvent;
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private OptionProp optionProp;
        [SerializeField] private bool isAnswer;
        public OptionProp OptionProp => optionProp;


        public void SetData(OptionProp option)
        {
            optionProp = option;
            text.text = option.GetOption().Key;
            isAnswer = option.GetOption().Value;
        }

        public void IsCompleted(bool isPressed)
        {
            button.interactable = false;
            if (isAnswer)
                button.image.color = Color.green;
            else if (isPressed)
                button.image.color = Color.red;
        }

        private void Start()
        {
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            if (optionProp is null)
                return;
            if (isAnswer)
            {
                // Reaction true answer
                Debug.Log("Correct Answer");
            }
            else
            {
                // Reaction false answer
                Debug.Log("False Answer");
            }

            ButtonClickEvent?.Invoke(optionProp);
        }
    }
}