using System;
using UnityEngine;

namespace TrafficVolume
{
    public class UnityHook : MonoBehaviour
    {
        public event Action UnityUpdate;

        private void Update()
        {
            UnityUpdate?.Invoke();
        }
    }
}