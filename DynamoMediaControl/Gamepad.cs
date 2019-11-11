using Autodesk.DesignScript.Runtime;
using SharpDX.XInput;
using System;
using System.Collections.Generic;

namespace DynamoMediaControl
{
    public class Gamepad
    {
        [IsVisibleInDynamoLibrary(false)]
        private static Controller controller = null;
        private static State lastState;

        [IsVisibleInDynamoLibrary(false)]
        private Gamepad()
        {
        }

        [IsVisibleInDynamoLibrary(false)]
        private static void MaybeInitDevice()
        {
            if (controller == null)
            {
                var controllers = new[] 
                {
                    new Controller(UserIndex.One),
                    new Controller(UserIndex.Two),
                    new Controller(UserIndex.Three),
                    new Controller(UserIndex.Four)
                };

                foreach (var c in controllers)
                {
                    if (c.IsConnected)
                    {
                        controller = c;
                        break;
                    }
                }
            }
        }

        [IsVisibleInDynamoLibrary(false)]
        private static Dictionary<string, bool> ParseButtonFlagsToDictionary(GamepadButtonFlags buttonFlags)
        {
            var result = new Dictionary<string, bool>();

            foreach (short button in Enum.GetValues(typeof(GamepadButtonFlags)))
            {
                if (button != (short)GamepadButtonFlags.None)
                {
                    result.Add(Enum.GetName(typeof(GamepadButtonFlags), button), ((int)buttonFlags & button) != 0);
                }
            }

            return result;
        }
    
        [CanUpdatePeriodically(true)]
        [MultiReturn(new[] { "Buttons", "LeftTrigger", "RightTrigger", "LeftThumbX", "LeftThumbY", "RightThumbX", "RightThumbY" })]
        public static Dictionary<string, object> State()
        {
            var result = new Dictionary<string, object>
            {
                { "Buttons", null },
                { "LeftTrigger", null },
                { "RightTrigger", null },
                { "LeftThumbX", null },
                { "LeftThumbY", null },
                { "RightThumbX", null },
                { "RightThumbY", null },
            };

            MaybeInitDevice();

            if (controller != null && controller.IsConnected)
            {
                var newState = controller.GetState();

                result["Buttons"] = ParseButtonFlagsToDictionary(newState.Gamepad.Buttons);
                result["LeftTrigger"] = newState.Gamepad.LeftTrigger;
                result["RightTrigger"] = newState.Gamepad.RightTrigger;
                result["LeftThumbX"] = newState.Gamepad.LeftThumbX;
                result["LeftThumbY"] = newState.Gamepad.LeftThumbY;
                result["RightThumbX"] = newState.Gamepad.RightThumbX;
                result["RightThumbY"] = newState.Gamepad.RightThumbY;

                lastState = newState;
            }

            return result;
        }
    }
}