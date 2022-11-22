using ColossalFramework;
using ColossalFramework.Globalization;
using ColossalFramework.UI;
using UnityEngine;

namespace TrafficVolume
{
    public class Keymapping : UICustomControl
    {
        private SavedInputKey _editedBinding;
        private int _count;

        private const string KeyBindingTemplate = "KeyBindingTemplate";
        private const string KeyName = "KEYNAME";

        private static bool IsControlDown => Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        private static bool IsShiftDown => Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        private static bool IsAltDown => Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);

        public static bool SingleKeyPressBlock { get; set; }
        
        public void AddKeymapping(string label, SavedInputKey savedInputKey)
        {
            var uiPanel = component.AttachUIComponent(UITemplateManager.GetAsGameObject(KeyBindingTemplate)) as UIPanel;

            if (_count++ % 2 == 1)
            {
                uiPanel.backgroundSprite = null;
            }

            var uiLabel = uiPanel.Find<UILabel>("Name");
            var uiButton = uiPanel.Find<UIButton>("Binding");

            uiLabel.text = label;

            uiButton.eventKeyDown += OnBindingKeyDown;
            uiButton.eventMouseDown += OnBindingMouseDown;
            uiButton.text = savedInputKey.ToLocalizedString(KeyName);
            uiButton.objectUserData = savedInputKey;
            uiButton.eventVisibilityChanged += ButtonVisibilityChanged;
        }

        private void OnEnable()
        {
            LocaleManager.eventLocaleChanged += OnLocaleChanged;
        }

        private void OnDisable()
        {
            LocaleManager.eventLocaleChanged -= OnLocaleChanged;
        }

        private void OnLocaleChanged()
        {
            RefreshBindableInputs();
        }

        private bool IsModifierKey(KeyCode code)
        {
            return code == KeyCode.LeftControl || code == KeyCode.RightControl ||
                   code == KeyCode.LeftShift || code == KeyCode.RightShift || code == KeyCode.LeftAlt ||
                   code == KeyCode.RightAlt;
        }

        private bool IsUnbindableMouseButton(UIMouseButton code)
        {
            return code == UIMouseButton.Left || code == UIMouseButton.Right;
        }

        private KeyCode ButtonToKeycode(UIMouseButton button)
        {
            switch (button)
            {
                case UIMouseButton.Left:
                    return KeyCode.Mouse0;
                case UIMouseButton.Right:
                    return KeyCode.Mouse1;
                case UIMouseButton.Middle:
                    return KeyCode.Mouse2;
                case UIMouseButton.Special0:
                    return KeyCode.Mouse3;
                case UIMouseButton.Special1:
                    return KeyCode.Mouse4;
                case UIMouseButton.Special2:
                    return KeyCode.Mouse5;
                case UIMouseButton.Special3:
                    return KeyCode.Mouse6;
                case UIMouseButton.None:
                    break;
                default:
                    return KeyCode.None;
            }

            return KeyCode.None;
        }

        private static void ButtonVisibilityChanged(UIComponent component, bool isVisible)
        {
            if (!isVisible || !(component.objectUserData is SavedInputKey objectUserData))
                return;

            ((UIButton) component).text = objectUserData.ToLocalizedString(KeyName);
        }

        private void OnBindingKeyDown(UIComponent comp, UIKeyEventParameter p)
        {
            if (!(_editedBinding != null) || IsModifierKey(p.keycode))
                return;
            p.Use();
            UIView.PopModal();
            KeyCode keycode = p.keycode;
            InputKey inputKey = p.keycode == KeyCode.Escape
                ? _editedBinding.value
                : SavedInputKey.Encode(keycode, p.control, p.shift, p.alt);
            if (p.keycode == KeyCode.Backspace)
                inputKey = SavedInputKey.Empty;
            _editedBinding.value = inputKey;
            ((UITextComponent) p.source).text = _editedBinding.ToLocalizedString(KeyName);
            _editedBinding = null;

            SingleKeyPressBlock = true;
        }

        private void OnBindingMouseDown(UIComponent comp, UIMouseEventParameter p)
        {
            if (_editedBinding == null)
            {
                p.Use();
                this._editedBinding = (SavedInputKey) p.source.objectUserData;
                UIButton source = p.source as UIButton;
                source.buttonsMask = UIMouseButton.Left | UIMouseButton.Right | UIMouseButton.Middle |
                                     UIMouseButton.Special0 | UIMouseButton.Special1 | UIMouseButton.Special2 |
                                     UIMouseButton.Special3;
                source.text = "Press any key";
                p.source.Focus();
                UIView.PushModal(p.source);
            }
            else
            {
                if (IsUnbindableMouseButton(p.buttons))
                    return;
                p.Use();
                UIView.PopModal();
                _editedBinding.value = SavedInputKey.Encode(this.ButtonToKeycode(p.buttons), IsControlDown,
                    IsShiftDown, IsAltDown);
                UIButton source = p.source as UIButton;
                source.text = _editedBinding.ToLocalizedString(KeyName);
                source.buttonsMask = UIMouseButton.Left;
                _editedBinding = null;
            }
        }

        private void RefreshBindableInputs()
        {
            foreach (UIComponent componentsInChild in component.GetComponentsInChildren<UIComponent>())
            {
                UITextComponent uiTextComponent = componentsInChild.Find<UITextComponent>("Binding");
                if (uiTextComponent != null)
                {
                    SavedInputKey objectUserData = uiTextComponent.objectUserData as SavedInputKey;
                    if (objectUserData != null)
                        uiTextComponent.text = objectUserData.ToLocalizedString(KeyName);
                }

                UILabel uiLabel = componentsInChild.Find<UILabel>("Name");
                if (uiLabel != null)
                    uiLabel.text = Locale.Get("KEYMAPPING", uiLabel.stringUserData);
            }
        }
    }
}