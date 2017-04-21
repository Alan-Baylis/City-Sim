using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareTime {
    static class InputManager {
        static MouseState currentMState;
        static MouseState lastMState;
        public static float DeltaScroll { get;private set; }
        static KeyboardState currentKState;
        static KeyboardState lastKState;
        public static void Update() {
            lastKState = currentKState;
            lastMState = currentMState;
            currentMState = Mouse.GetState();
            currentKState = Keyboard.GetState();
            DeltaScroll = currentMState.ScrollWheelValue - lastMState.ScrollWheelValue;
        }
        public static float getScrollState() {// fix me
            if (DeltaScroll != 0) return DeltaScroll;
            else {
                if (currentKState.IsKeyDown(Keys.End)) return 50;
                if (currentKState.IsKeyDown(Keys.Home)) return -50;
                else return 0;
            }
        }
        public static bool getMouseClick() {
            return (currentMState.LeftButton == ButtonState.Pressed) && (lastMState.LeftButton == ButtonState.Released);
        }
        public static bool getMouseUp() {
            return (currentMState.LeftButton == ButtonState.Released) && (lastMState.LeftButton == ButtonState.Pressed); 
        }
        public static bool getKey(Keys key) {
            return currentKState.IsKeyDown(key);
        }
        public static Vector2 getMousePos() {
            return currentMState.Position.ToVector2();
        }
        public static Vector2 getLastMousePos() {
            return lastMState.Position.ToVector2();
        }
    }
}
