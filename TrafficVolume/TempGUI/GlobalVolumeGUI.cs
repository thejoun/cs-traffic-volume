using System.Collections.Generic;
using TrafficVolume.Traffic;
using UnityEngine;

namespace TrafficVolume.TempGUI
{
    public class GlobalVolumeGUI : UniversalGUI
    {
        protected override Rect Rect { get; set; } = new Rect(50, Screen.height - 400, 150, 200);
        protected override IEnumerable<KeyCode> EnableKeyCombination => new [] {KeyCode.LeftAlt, KeyCode.G};

        private string _dump;

        protected override void OnOpened()
        {
            GlobalTraffic.CountVolume();
            
            _dump = GlobalTraffic.Volume.ToString();
        }

        protected override void DrawWindow(int windowID)
        {
            var text = $"Global Traffic Volume\n\n{_dump}";

            UnityEngine.GUI.Label(new Rect(0, 0, Rect.width, Rect.height), text);
        }
    }
}