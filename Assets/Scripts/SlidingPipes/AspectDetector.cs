using System;
using UnityEngine;

namespace SlidingPipes
{
    public class AspectDetector : MonoBehaviour
    {
        public static event Action<bool> AspectDetectorEvent;
        private static bool IsTablet()
        {

            bool con = false;
            float ssw;
            if (Screen.width > Screen.height)
            {
                ssw = Screen.width;
            }
            else
            {
                ssw = Screen.height;
            }

            if (ssw < 800) con= false;

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                var screenWidth = Screen.width / Screen.dpi;
                var screenHeight = Screen.height / Screen.dpi;
                var size = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
                if (size >= 6.5f) con= true;
            }
            AspectDetectorEvent?.Invoke(con);
            return con;
        }

        private void Start()
        {
            if (DeviceTypeChecker.GetDeviceType() == EnumDeviceType.Phone)
            {
                AspectDetectorEvent?.Invoke(false);
                 transform.localScale = Vector2.one;
            }
            else
            {
                AspectDetectorEvent?.Invoke(true);
                transform.localScale = Vector2.one * 0.7f;
            }
            // if (IsTablet())
            //     transform.localScale = Vector2.one * 0.8f;
            // else
            //     transform.localScale = Vector2.one;
        }
    }
    public enum EnumDeviceType
    {
        Tablet,
        Phone
    }
 
    public static class DeviceTypeChecker
    {
        public static bool IsTablet;
 
        private static float DeviceDiagonalSizeInInches()
        {
            var screenWidth = Screen.width / Screen.dpi;
            var screenHeight = Screen.height / Screen.dpi;
            var diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
 
            return diagonalInches;
        }
 
        public static EnumDeviceType GetDeviceType()
        {
#if UNITY_IOS
    bool deviceIsIpad = UnityEngine.iOS.Device.generation.ToString().Contains("iPad");
            if (deviceIsIpad)
            {
                return ENUM_Device_Type.Tablet;
            }
            bool deviceIsIphone = UnityEngine.iOS.Device.generation.ToString().Contains("iPhone");
            if (deviceIsIphone)
            {
                return ENUM_Device_Type.Phone;
            }
#elif UNITY_ANDROID
 
            // ReSharper disable once PossibleLossOfFraction
            float aspectRatio = Mathf.Max(Screen.width, Screen.height) / Mathf.Min(Screen.width, Screen.height);
            var isTablet = (DeviceDiagonalSizeInInches() > 6.5f && aspectRatio < 2f);
 
            return isTablet ? EnumDeviceType.Tablet : EnumDeviceType.Phone;
#endif
        }
    }
}