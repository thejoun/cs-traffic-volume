using System.Collections.Generic;
using ColossalFramework.UI;
using UnityEngine;

namespace TrafficVolume
{
    public class UIUtils
    {
        private static UIColorField _colorFIeldTemplate;
        private static Dictionary<string, UITextureAtlas> _atlases;

        public static UIButton CreateButton(UIComponent parent)
        {
            UIButton uiButton = parent.AddUIComponent<UIButton>();
            uiButton.atlas = UIUtils.GetAtlas("Ingame");
            uiButton.size = new Vector2(90f, 30f);
            uiButton.textScale = 0.9f;
            uiButton.normalBgSprite = "ButtonMenu";
            uiButton.hoveredBgSprite = "ButtonMenuHovered";
            uiButton.pressedBgSprite = "ButtonMenuPressed";
            uiButton.disabledBgSprite = "ButtonMenuDisabled";
            uiButton.disabledTextColor = new Color32((byte) 80, (byte) 80, (byte) 80, (byte) 128);
            uiButton.canFocus = false;
            uiButton.playAudioEvents = true;
            return uiButton;
        }

        public static UICheckBox CreateCheckBox(UIComponent parent, string label= " ")
        {
            var checkBox = parent.AddUIComponent<UICheckBox>();
            checkBox.width = 300f;
            checkBox.height = 20f;
            checkBox.clipChildren = true;

            var sprite = checkBox.AddUIComponent<UISprite>();
            sprite.atlas = GetAtlas("Ingame");
            sprite.spriteName = "ToggleBase";
            sprite.size = new Vector2(16f, 16f);
            sprite.relativePosition = Vector3.zero;

            checkBox.checkedBoxObject = sprite.AddUIComponent<UISprite>();
            ((UISprite) checkBox.checkedBoxObject).atlas = GetAtlas("Ingame");
            ((UISprite) checkBox.checkedBoxObject).spriteName = "ToggleBaseFocused";
            checkBox.checkedBoxObject.size = new Vector2(16f, 16f);
            checkBox.checkedBoxObject.relativePosition = Vector3.zero;

            checkBox.label = checkBox.AddUIComponent<UILabel>();
            checkBox.label.text = label;
            checkBox.label.textScale = 0.9f;
            checkBox.label.relativePosition = new Vector3(22f, 2f);
            return checkBox;
        }

        public static UITextField CreateTextField(UIComponent parent)
        {
            UITextField uiTextField = parent.AddUIComponent<UITextField>();
            uiTextField.atlas = UIUtils.GetAtlas("Ingame");
            uiTextField.size = new Vector2(90f, 20f);
            uiTextField.padding = new RectOffset(6, 6, 3, 3);
            uiTextField.builtinKeyNavigation = true;
            uiTextField.isInteractive = true;
            uiTextField.readOnly = false;
            uiTextField.horizontalAlignment = UIHorizontalAlignment.Center;
            uiTextField.selectionSprite = "EmptySprite";
            uiTextField.selectionBackgroundColor = new Color32((byte) 0, (byte) 172, (byte) 234, byte.MaxValue);
            uiTextField.normalBgSprite = "TextFieldPanelHovered";
            uiTextField.disabledBgSprite = "TextFieldPanelHovered";
            uiTextField.textColor = new Color32((byte) 0, (byte) 0, (byte) 0, byte.MaxValue);
            uiTextField.disabledTextColor = new Color32((byte) 80, (byte) 80, (byte) 80, (byte) 128);
            uiTextField.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            return uiTextField;
        }

        public static UIDropDown CreateDropDown(UIComponent parent)
        {
            UIDropDown dropDown = parent.AddUIComponent<UIDropDown>();
            dropDown.atlas = UIUtils.GetAtlas("Ingame");
            dropDown.size = new Vector2(90f, 30f);
            dropDown.listBackground = "GenericPanelLight";
            dropDown.itemHeight = 30;
            dropDown.itemHover = "ListItemHover";
            dropDown.itemHighlight = "ListItemHighlight";
            dropDown.normalBgSprite = "TextFieldPanel";
            dropDown.focusedBgSprite = "TextFieldPanelHovered";
            dropDown.hoveredBgSprite = "TextFieldPanelHovered";
            dropDown.listPosition = UIDropDown.PopupListPosition.Above;
            dropDown.listWidth = 90;
            dropDown.listHeight = 500;
            dropDown.foregroundSpriteMode = UIForegroundSpriteMode.Stretch;
            dropDown.popupColor = new Color32((byte) 45, (byte) 52, (byte) 61, byte.MaxValue);
            dropDown.popupTextColor = new Color32((byte) 170, (byte) 170, (byte) 170, byte.MaxValue);
            dropDown.zOrder = 1;
            dropDown.textColor = new Color32((byte) 0, (byte) 0, (byte) 0, byte.MaxValue);
            dropDown.textScale = 0.8f;
            dropDown.verticalAlignment = UIVerticalAlignment.Middle;
            dropDown.horizontalAlignment = UIHorizontalAlignment.Left;
            dropDown.selectedIndex = 0;
            dropDown.textFieldPadding = new RectOffset(8, 0, 8, 0);
            dropDown.itemPadding = new RectOffset(14, 0, 8, 0);
            dropDown.builtinKeyNavigation = true;
            UIButton button = dropDown.AddUIComponent<UIButton>();
            dropDown.triggerButton = (UIComponent) button;
            button.atlas = UIUtils.GetAtlas("Ingame");
            button.text = "";
            button.size = dropDown.size;
            button.relativePosition = new Vector3(0.0f, 0.0f);
            button.textVerticalAlignment = UIVerticalAlignment.Middle;
            button.textHorizontalAlignment = UIHorizontalAlignment.Left;
            button.normalFgSprite = "OptionsScrollbarThumb";
            button.spritePadding = new RectOffset(0, 3, 3, 0);
            button.foregroundSpriteMode = UIForegroundSpriteMode.Fill;
            button.horizontalAlignment = UIHorizontalAlignment.Right;
            button.verticalAlignment = UIVerticalAlignment.Middle;
            button.zOrder = 0;
            button.textScale = 0.8f;
            dropDown.eventSizeChanged += (PropertyChangedEventHandler<Vector2>) ((c, t) =>
            {
                button.size = t;
                dropDown.listWidth = (int) t.x;
            });
            return dropDown;
        }

        public static void CreateDropDownScrollBar(UIDropDown dropDown, Vector3 relativePosition)
        {
            dropDown.listScrollbar = dropDown.AddUIComponent<UIScrollbar>();
            dropDown.listScrollbar.width = 20f;
            dropDown.listScrollbar.height = (float) dropDown.listHeight;
            dropDown.listScrollbar.orientation = UIOrientation.Vertical;
            dropDown.listScrollbar.pivot = UIPivotPoint.TopRight;
            dropDown.listScrollbar.thumbPadding = new RectOffset(0, 0, 5, 5);
            dropDown.listScrollbar.minValue = 0.0f;
            dropDown.listScrollbar.value = 0.0f;
            dropDown.listScrollbar.incrementAmount = 50f;
            dropDown.listScrollbar.AlignTo((UIComponent) dropDown, UIAlignAnchor.TopRight);
            dropDown.listScrollbar.autoHide = true;
            dropDown.listScrollbar.isVisible = false;
            relativePosition.x += 50000f;
            relativePosition.y += 50000f;
            dropDown.listScrollbar.relativePosition = relativePosition;
            UISlicedSprite uiSlicedSprite1 = dropDown.listScrollbar.AddUIComponent<UISlicedSprite>();
            uiSlicedSprite1.relativePosition = (Vector3) Vector2.zero;
            uiSlicedSprite1.autoSize = true;
            uiSlicedSprite1.size = uiSlicedSprite1.parent.size;
            uiSlicedSprite1.fillDirection = UIFillDirection.Vertical;
            uiSlicedSprite1.spriteName = "ScrollbarTrack";
            dropDown.listScrollbar.trackObject = (UIComponent) uiSlicedSprite1;
            UISlicedSprite uiSlicedSprite2 = uiSlicedSprite1.AddUIComponent<UISlicedSprite>();
            uiSlicedSprite2.relativePosition = (Vector3) Vector2.zero;
            uiSlicedSprite2.fillDirection = UIFillDirection.Vertical;
            uiSlicedSprite2.autoSize = true;
            uiSlicedSprite2.width = uiSlicedSprite2.parent.width - 8f;
            uiSlicedSprite2.spriteName = "ScrollbarThumb";
            dropDown.listScrollbar.thumbObject = (UIComponent) uiSlicedSprite2;
        }

        public static void DestroyDropDownScrollBar(UIDropDown dropDown)
        {
            foreach (Component componentsInChild in dropDown.GetComponentsInChildren<UIScrollbar>())
                Object.DestroyImmediate((Object) componentsInChild.gameObject);
        }

        public static UICheckBox CreateIconToggle(
            UIComponent parent,
            string atlas,
            string checkedSprite,
            string uncheckedSprite,
            float disabledSpriteOpacity = 1f,
            float tabSize = 35f)
        {
            UICheckBox checkBox = parent.AddUIComponent<UICheckBox>();
            disabledSpriteOpacity = 0.3f;
            checkBox.width = tabSize;
            checkBox.height = tabSize;
            checkBox.clipChildren = true;
            UIPanel panel = checkBox.AddUIComponent<UIPanel>();
            panel.atlas = UIUtils.GetAtlas("Ingame");
            panel.backgroundSprite = "IconPolicyBaseRect";
            panel.size = checkBox.size;
            panel.relativePosition = Vector3.zero;
            UISprite sprite = panel.AddUIComponent<UISprite>();
            sprite.atlas = UIUtils.GetAtlas(atlas);
            sprite.spriteName = uncheckedSprite;
            sprite.size = checkBox.size;
            sprite.relativePosition = Vector3.zero;
            checkBox.checkedBoxObject = (UIComponent) sprite.AddUIComponent<UISprite>();
            ((UISprite) checkBox.checkedBoxObject).atlas = sprite.atlas;
            ((UISprite) checkBox.checkedBoxObject).spriteName = checkedSprite;
            checkBox.checkedBoxObject.size = checkBox.size;
            checkBox.checkedBoxObject.relativePosition = Vector3.zero;
            checkBox.eventCheckChanged += (PropertyChangedEventHandler<bool>) ((c, b) =>
            {
                if (checkBox.isChecked)
                {
                    panel.backgroundSprite = "IconPolicyBaseRect";
                    sprite.opacity = 1f;
                }
                else
                {
                    panel.backgroundSprite = "IconPolicyBaseRectDisabled";
                    sprite.opacity = disabledSpriteOpacity;
                }

                panel.Invalidate();
            });
            checkBox.eventMouseEnter += (MouseEventHandler) ((c, p) =>
            {
                panel.backgroundSprite = "IconPolicyBaseRectHovered";
                sprite.spriteName = checkedSprite;
                sprite.opacity = 1f;
            });
            checkBox.eventMouseLeave += (MouseEventHandler) ((c, p) =>
            {
                if (checkBox.isChecked)
                {
                    panel.backgroundSprite = "IconPolicyBaseRect";
                    sprite.opacity = 1f;
                }
                else
                {
                    panel.backgroundSprite = "IconPolicyBaseRectDisabled";
                    sprite.opacity = disabledSpriteOpacity;
                }

                sprite.spriteName = uncheckedSprite;
            });
            return checkBox;
        }

        public static UIColorField CreateColorField(UIComponent parent)
        {
            if ((Object) UIUtils._colorFIeldTemplate == (Object) null)
            {
                UIComponent uiComponent = UITemplateManager.Get("LineTemplate");
                if ((Object) uiComponent == (Object) null)
                    return (UIColorField) null;
                UIUtils._colorFIeldTemplate = uiComponent.Find<UIColorField>("LineColor");
                if ((Object) UIUtils._colorFIeldTemplate == (Object) null)
                    return (UIColorField) null;
            }

            UIColorField component = Object.Instantiate<GameObject>(UIUtils._colorFIeldTemplate.gameObject)
                .GetComponent<UIColorField>();
            parent.AttachUIComponent(component.gameObject);
            component.size = new Vector2(40f, 26f);
            component.pickerPosition = UIColorField.ColorPickerPosition.LeftAbove;
            return component;
        }

        public static UISlider CreateSlider(
            UIComponent parent,
            float value,
            float min,
            float max,
            float step)
        {
            UISlider uiSlider = parent.AddUIComponent<UISlider>();
            uiSlider.minValue = min;
            uiSlider.maxValue = max;
            uiSlider.stepSize = step;
            uiSlider.value = value;
            uiSlider.atlas = UIUtils.GetAtlas("Ingame");
            uiSlider.backgroundSprite = "WhiteRect";
            uiSlider.color = new Color32((byte) 110, (byte) 110, (byte) 110, byte.MaxValue);
            uiSlider.size = new Vector2(350f, 10f);
            uiSlider.scrollWheelAmount = 5f;
            UISprite uiSprite = uiSlider.AddUIComponent<UISprite>();
            uiSprite.atlas = UIUtils.GetAtlas("Ingame");
            uiSprite.spriteName = "SliderFill";
            uiSprite.size = new Vector2(10f, 15f);
            uiSlider.thumbObject = (UIComponent) uiSprite;
            return uiSlider;
        }

        public static void ResizeIcon(UISprite icon, Vector2 maxSize)
        {
            icon.width = icon.spriteInfo.width;
            icon.height = icon.spriteInfo.height;
            if ((double) icon.height == 0.0)
                return;
            float num = icon.width / icon.height;
            if ((double) icon.width > (double) maxSize.x)
            {
                icon.width = maxSize.x;
                icon.height = maxSize.x / num;
            }

            if ((double) icon.height <= (double) maxSize.y)
                return;
            icon.height = maxSize.y;
            icon.width = maxSize.y * num;
        }

        public static UITextureAtlas GetAtlas(string name)
        {
            if (_atlases != null && _atlases.ContainsKey(name))
            {
                return _atlases[name];
            }

            _atlases = new Dictionary<string, UITextureAtlas>();

            var objectsOfTypeAll = Resources.FindObjectsOfTypeAll(typeof(UITextureAtlas)) as UITextureAtlas[];

            if (objectsOfTypeAll == null)
            {
                Debug.Log("UIUtils: ObjectsOfTypeAll is null");
                return null;
            }
            
            foreach (var atlas in objectsOfTypeAll)
            {
                if (!_atlases.ContainsKey(atlas.name))
                {
                    _atlases.Add(atlas.name, atlas);
                }
            }

            return _atlases[name];
        }
    }
}