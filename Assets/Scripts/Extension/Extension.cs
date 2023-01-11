using TMPro;
using UnityEngine.UI;

namespace Extension
{
    public static class Extension
    {
        public static void SetButtonText(this Button button, string value)
        {
            if (button.TryGetComponent(typeof(Text), out var c))
            {
                ((Text) c).text = value;
            }
            else if (button.TryGetComponent(typeof(TextMeshProUGUI), out var meshC))
            {
                ((TextMeshProUGUI) meshC).text = value;
            }
        }
    }
}