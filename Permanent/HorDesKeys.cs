using System.Collections.Generic;
using UnityEngine;

namespace Assets._NETWORK
{
    public static class HorDesKeys
    {
        public static KeyCode forward = KeyCode.W;
        public static KeyCode back = KeyCode.S;
        public static KeyCode left = KeyCode.A;
        public static KeyCode right = KeyCode.D;
        public static KeyCode sprint = KeyCode.LeftShift;
        public static KeyCode jump = KeyCode.Space;
        public static KeyCode interactKey = KeyCode.E;
        public static KeyCode crouch = KeyCode.LeftControl;
        public static KeyCode zoomKey = KeyCode.Mouse1;

        private static List<KeyCode> movableKeyCodes = new List<KeyCode> { forward, back, left, right };

        public static bool IsMovableKey(KeyCode currentInput)
        {
            bool value = false;
            movableKeyCodes.ForEach(x =>
            {
                if (x == currentInput)
                {
                    value = true;
                }
            });

            return value;
        }
    }
}
