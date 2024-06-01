using UnityEngine;

namespace Game.Scripts.Foundation
{
    public static class ScreenInfo
    {
        public static float ScreenRight => Camera.main.ViewportToWorldPoint(Vector3.one).x;
        public static float ScreenLeft => Camera.main.ViewportToWorldPoint(Vector3.zero).x;
        
        public static float ScreenTop => Camera.main.ViewportToWorldPoint(Vector3.one).y;
        public static float ScreenBottom => Camera.main.ViewportToWorldPoint(Vector3.zero).y;
    }
}