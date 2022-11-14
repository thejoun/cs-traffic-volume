using System.Collections.Generic;
using System.Linq;
using ColossalFramework.UI;
using UnityEngine;

namespace TrafficVolume.UI
{
    public class TrafficRadialChart : UIRadialChart
    {
        private const int HoverBoxWidth = 43;
        private const int HoverBoxHeight = 20;

        private const int HoverBoxPadding = 3;

        private static readonly Vector2 HoverBoxOffset = new Vector2(0f, -2f);
        private static readonly Color LightTextColor = new Color(0.85f, 0.85f, 0.9f);
        
        private readonly Dictionary<TransportType, Texture2D> _textures = new Dictionary<TransportType, Texture2D>();

        private GUIStyle _hoverBoxStyle;

        public override void Awake()
        {
            base.Awake();
            
            PrepareStyles();
        }

        private void OnGUI()
        {
            if (m_IsMouseHovering)
            {
                DrawHoverBox();
            }
        }

        public void Create(IEnumerable<CheckboxData> checkboxes)
        {
            foreach (var checkbox in checkboxes)
            {
                AddSlice(checkbox);
            }
        }

        public void DisplayVolume(Volume volume)
        {
            var counts = volume.Values.ToArray();
            
            var sum = counts.Sum(c => c);
            var percentages = counts.Select(c => 1f * c / sum).ToArray();

            Manager.Log.WriteLog($"Displaying chart, sum is {sum}");
            
            gameObject.SetActive(sum != 0);

            if (sum == 0)
            {
                return;
            }
            
            transform.localPosition = Vector2.zero;
            
            var a = 0f;
            
            for (int index = 0; index < sliceCount; index++)
            {
                var percentage = percentages[index];
                var slice = GetSlice(index);

                // ensures that no weird clamps are applied later
                slice.startValue = 0f;
                slice.endValue = 1f;
                
                slice.startValue = Mathf.Clamp(a, 0f, 1f);
                slice.endValue = Mathf.Clamp(a + percentage, 0f, 1f);

                a += percentage;
            }
            
            Invalidate();
        }

        public void PrepareColors(IEnumerable<CheckboxData> checkboxes)
        {
            _textures.Clear();

            foreach (var checkbox in checkboxes)
            {
                var transport = checkbox.Transport;
                
                var texture = PrepareTexture(checkbox);
                _textures.Add(transport, texture);

                var slice = GetSlice(transport);
                slice.innerColor = checkbox.PrimaryColor;
                slice.outterColor = checkbox.SecondaryColor;
            }
        }

        private void AddSlice(CheckboxData checkbox)
        {
            var transport = checkbox.Transport;

            AddSlice();

            var slice = GetSlice(transport);

            slice.endValue = 0f;

            // colors in checkboxes may not have been set properly yet
            // need to call PrepareColors later
            
            slice.innerColor = checkbox.PrimaryColor;
            slice.outterColor = checkbox.SecondaryColor;

            var texture = PrepareTexture(checkbox);
            _textures.Add(transport, texture);
        }

        private void DrawHoverBox()
        {
            var cam = GetCamera();
            var centerPos = (Vector2) cam.WorldToScreenPoint(center);
            var mousePos = (Vector2) Input.mousePosition;
            var direction = mousePos - centerPos;

            if (direction.magnitude > UIManager.TrafficChartSize / 2f)
            {
                return;
            }

            var negative = direction.x < 0;

            // old Unity does not have SignedAngle
            var angle = Vector2.Angle(Vector2.up, direction);

            if (negative) angle = -angle + 360;

            var fraction = angle / 360f;

            var index = m_Slices
                .FindIndex(slice => slice.startValue < fraction && slice.endValue > fraction);

            if (index == -1)
            {
                return;
            }

            var transport = (TransportType) index;

            var hoveredSlice = m_Slices[index];

            var boxSize = new Vector2(HoverBoxWidth, HoverBoxHeight);

            var rectPosX = Input.mousePosition.x - boxSize.x / 2f + HoverBoxOffset.x;
            var rectPosY = Screen.height - Input.mousePosition.y - boxSize.y + HoverBoxOffset.y;

            var percent = (hoveredSlice.endValue - hoveredSlice.startValue) * 100f;
            var text = $"{percent:F0}%";

            var style = new GUIStyle(_hoverBoxStyle)
            {
                normal = {background = _textures[transport]}
            };

            var guiColor = GUI.color;
            var guiContentColor = GUI.contentColor;

            GUI.color = Color.white;
            GUI.contentColor = Color.white;
            GUI.Box(new Rect(rectPosX, rectPosY, boxSize.x, boxSize.y), text, style);
            GUI.color = guiColor;
            GUI.contentColor = guiContentColor;
        }

        private void PrepareStyles()
        {
            _hoverBoxStyle = new GUIStyle()
            {
                fontSize = 13,
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                normal = {textColor = LightTextColor}
            };
        }

        private SliceSettings GetSlice(TransportType transport)
        {
            return GetSlice((int) transport);
        }

        private Texture2D PrepareTexture(CheckboxData checkbox)
        {
            var texture = new Texture2D(HoverBoxWidth, HoverBoxHeight);

            var primaryColor = checkbox.PrimaryColor;
            
            var topColor = UIManager.BackgroundTopColor;
            var bottomColor = UIManager.BackgroundBottomColor;

            for (int y = 0; y < HoverBoxHeight; y++)
            {
                for (int x = 0; x < HoverBoxWidth; x++)
                {
                    if (x < HoverBoxPadding 
                        || y < HoverBoxPadding 
                        || x > HoverBoxWidth - HoverBoxPadding 
                        || y > HoverBoxHeight - HoverBoxPadding)
                    {
                        // border
                        var pixelColor = primaryColor.gamma;
                        texture.SetPixel(x, y, pixelColor);
                    }
                    else
                    {
                        // inside
                        var gradient = 1f * y / HoverBoxHeight;
                        var pixelColor = Color.Lerp(topColor, bottomColor, gradient).gamma;
                        texture.SetPixel(x, y, pixelColor);
                    }

                }
            }
            
            texture.Apply();

            return texture;
        }
    }
}