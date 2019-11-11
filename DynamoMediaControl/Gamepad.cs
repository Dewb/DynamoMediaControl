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

        [IsVisibleInDynamoLibrary(false)]
        private Gamepad()
        {
        }

        [IsVisibleInDynamoLibrary(false)]
        private static void FindFirstConnectedController()
        {
            if (controller == null || !controller.IsConnected)
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
                    result.Add(Enum.GetName(typeof(GamepadButtonFlags), button), ((short)buttonFlags & button) != 0);
                }
            }

            return result;
        }

        [IsVisibleInDynamoLibrary(false)]
        private static int DeadZoneTest(int value, int deadzone)
        {
            if (Math.Abs(value) <= deadzone * 0.5)
            {
                return 0;
            }
            else
            {
                return value;
            }
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

            FindFirstConnectedController();

            if (controller != null && controller.IsConnected)
            {
                var newState = controller.GetState();

                result["Buttons"]      = ParseButtonFlagsToDictionary(newState.Gamepad.Buttons);

                result["LeftTrigger"]  = DeadZoneTest(newState.Gamepad.LeftTrigger,  SharpDX.XInput.Gamepad.TriggerThreshold);
                result["RightTrigger"] = DeadZoneTest(newState.Gamepad.RightTrigger, SharpDX.XInput.Gamepad.TriggerThreshold);
                result["LeftThumbX"]   = DeadZoneTest(newState.Gamepad.LeftThumbX,   SharpDX.XInput.Gamepad.LeftThumbDeadZone);
                result["LeftThumbY"]   = DeadZoneTest(newState.Gamepad.LeftThumbY,   SharpDX.XInput.Gamepad.LeftThumbDeadZone);
                result["RightThumbX"]  = DeadZoneTest(newState.Gamepad.RightThumbX,  SharpDX.XInput.Gamepad.RightThumbDeadZone);
                result["RightThumbY"]  = DeadZoneTest(newState.Gamepad.RightThumbY,  SharpDX.XInput.Gamepad.RightThumbDeadZone);
            }

            return result;
        }
    }
}