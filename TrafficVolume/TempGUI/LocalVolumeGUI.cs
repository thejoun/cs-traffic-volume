using System.Collections.Generic;
using UnityEngine;

namespace TrafficVolume.TempGUI
{
    public class LocalVolumeGUI : UniversalGUI
    {
        protected override Rect Rect { get; set; } = new Rect(50, Screen.height - 400, 150, 200);
        // protected override IEnumerable<KeyCode> EnableKeyCombination => new [] {KeyCode.LeftAlt, KeyCode.L};
        protected override IEnumerable<KeyCode> EnableKeyCombination => new List<KeyCode>();

        private string _dump;

        protected override void OnOpened()
        {
            _dump = LocalTraffic.Volume.ToString();
        }

        protected override void DrawWindow(int windowID)
        {
            var text = $"Local Traffic Volume\n\n{_dump}";

            UnityEngine.GUI.Label(new Rect(0, 0, Rect.width, Rect.height), text);
        }
    }
}