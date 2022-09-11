using UnityEngine;

namespace TrafficVolume.Extensions
{
    public static class ColorExtensions
    {
        public static Color ModifyHSV(this Color color, float h = 0f, float s = 0f, float v = 0f)
        {
            Color.RGBToHSV(color, out float origH, out float origS, out float origV);
            
            float newH = Mathf.Clamp(origH + h, 0f, 1f);
            float newS = Mathf.Clamp(origS + s, 0f, 1f);
            float newV = Mathf.Clamp(origV + v, 0f, 1f);

            Color newColor = Color.HSVToRGB(newH, newS, newV);
            
            return newColor;
        }
    }
}