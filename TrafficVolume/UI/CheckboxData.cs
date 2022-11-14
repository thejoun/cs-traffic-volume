using System;
using ColossalFramework.UI;
using UnityEngine;

namespace TrafficVolume.UI
{
    public class CheckboxData
    {
        public UICheckBox UiCheckbox { get; private set; }
        public string OriginalText { get; private set; }
        public TransportType Transport { get; private set; }
        public Color PrimaryColor { get; private set; } = Color.white;
        public Color SecondaryColor { get; private set; } = Color.white;

        public bool IsChecked => UiCheckbox.isChecked;

        public event Action<CheckboxData> CheckChanged; 
        public event Action<CheckboxData> Checked;
        public event Action<CheckboxData> Unchecked; 

        public CheckboxData(UICheckBox ui, TransportType transport)
        {
            UiCheckbox = ui;
            OriginalText = ui.text;
            
            Transport = transport;

            ui.eventCheckChanged += OnCheckChanged;
        }

        public void SetColors(Color primary, Color secondary)
        {
            PrimaryColor = primary;
            SecondaryColor = secondary;
        }

        private void OnCheckChanged(UIComponent ui, bool isChecked)
        {
            if (isChecked)
            {
                Checked?.Invoke(this);
            }
            else
            {
                Unchecked?.Invoke(this);
            }
            
            CheckChanged?.Invoke(this);
        }
    }
}