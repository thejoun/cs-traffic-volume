using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TrafficVolume.GUI
{
    public abstract class UniversalGUI : MonoBehaviour
    {
        private bool m_open;

        protected virtual bool AllowDrag => true;

        protected abstract Rect Rect { get; set; } 
        protected abstract IEnumerable<KeyCode> EnableKeyCombination { get; }

        protected abstract void OnOpened();
        protected abstract void DrawWindow(int windowID);

        protected virtual void Update()
        {
            if (EnableKeyCombination.Any())
            {
                var lastKey = EnableKeyCombination.Last();
                var previousKeys = EnableKeyCombination.Except(new []{lastKey});
		
                if (previousKeys.All(Input.GetKey) && Input.GetKeyDown(lastKey))
                {
                    SwitchVisibility();
                }
            }
        }

        protected virtual void OnGUI()
        {
            if (!m_open)
            {
                return;
            }
            
            Rect = UnityEngine.GUI.Window(0, Rect, DrawDraggableWindow, "");
        }

        protected void SwitchVisibility()
        {
            m_open = !m_open;

            if (m_open)
            {
                OnOpened();
            }
        }

        public void Open()
        {
            m_open = true;
            
            OnOpened();
        }

        public void Close()
        {
            m_open = false;
        }
        
        private void DrawDraggableWindow(int windowID)
        {
            DrawWindow(windowID);
	
            if (AllowDrag)
            {
                UnityEngine.GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
            }
        }
    }
}