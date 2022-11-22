using System.Collections.Generic;
using System.Linq;
using TrafficVolume.Extensions;
using TrafficVolume.Managers;
using TrafficVolume.Traffic;
using UnityEngine;

namespace TrafficVolume.TempGUI
{
    public class GlobalVolumeGUI : UniversalGUI
    {
        protected override Rect Rect { get; set; } = new Rect(Screen.width - 150 - 100, 100, 150, 140);
        protected override IEnumerable<KeyCode> EnableKeyCombination => Enumerable.Empty<KeyCode>();

        private string _dump;

        private void OnEnable()
        {
            Manager.Refresh += OnRefresh;
        }

        private void OnDisable()
        {
            Manager.Refresh -= OnRefresh;
        }

        protected override void Update()
        {
            if (Settings.GlobalTrafficKeybind.IsKeyDown())
            {
                if (Keymapping.SingleKeyPressBlock)
                {
                    Keymapping.SingleKeyPressBlock = false;
                }
                else
                {
                    SwitchVisibility();
                }
            }
        }

        protected override void OnOpened()
        {
            Refresh();
        }

        private void OnRefresh()
        {
            if (IsOpen)
            {
                Refresh();
            }
        }

        private void Refresh()
        {
            var volume = GlobalTraffic.CountVolume();

            _dump = volume.ToString();
        }

        protected override void DrawWindow(int windowID)
        {
            var text = $"Global Traffic Volume\n\n{_dump}";

            GUI.Label(new Rect(0, 0, Rect.width, Rect.height), text);
        }
    }
}