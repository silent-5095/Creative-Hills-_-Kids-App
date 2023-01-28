using UnityEngine;

namespace Painting
{
    public static class PaintingColorHandler
    {
        private static int HexToDec(string hex)
        {
            var dec = System.Convert.ToInt32(hex, 16);
            return dec;
        }

        private static string DecToHex(int value)
        {
            return value.ToString("X2");
        }

        private static string FloatNormalizeToHex(float value)
        {
            return DecToHex(Mathf.RoundToInt(value * 255));
        }

        private static float HexToFloatNormalize(string hex)
        {
            return HexToDec(hex) / 255f;
        }

        public static Color GetColorFromString(string hexString)
        {
            var red = HexToFloatNormalize(hexString.Substring(0, 2));
            var green = HexToFloatNormalize(hexString.Substring(2, 2));
            var blue = HexToFloatNormalize(hexString.Substring(4, 2));
            return new Color(red, green, blue);
        }

        public static string GetStringFromColor(Color color)
        {
            var red = FloatNormalizeToHex(color.r);
            var green = FloatNormalizeToHex(color.g);
            var blue = FloatNormalizeToHex(color.b);
            return red + green + blue;
        }
    }
}