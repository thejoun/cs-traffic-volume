using ColossalFramework;
using UnityEngine;

namespace TrafficVolume.Extensions
{
    public static class SavedInputKeyExtensions
    {
        public static bool IsKeyDown(this SavedInputKey input)
        {
            int num = input.value;
            KeyCode key = (KeyCode) (num & 268435455);
            return key != KeyCode.None && Input.GetKeyDown(key) && (Input.GetKey(KeyCode.LeftControl) ? 1 : (Input.GetKey(KeyCode.RightControl) ? 1 : 0)) == ((num & 1073741824) != 0 ? 1 : 0) && ((Input.GetKey(KeyCode.LeftShift) ? 1 : (Input.GetKey(KeyCode.RightShift) ? 1 : 0)) == ((num & 536870912) != 0 ? 1 : 0) && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt) ? 1 : (Input.GetKey(KeyCode.AltGr) ? 1 : 0)) == ((num & 268435456) != 0 ? 1 : 0));
        }
    }
}