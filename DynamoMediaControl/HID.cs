/*
 
using Autodesk.DesignScript.Runtime;
using System.Collections.Generic;


namespace DynamoMediaControl
{
    
    public class HID
    {
        [IsVisibleInDynamoLibrary(false)]
        private static HidSharp.HidDevice device = null;
        private static HidSharp.DeviceStream stream = null;

        [IsVisibleInDynamoLibrary(false)]
        private static void MaybeInitDevice()
        {
            if (device == null)
            {
                int vendorId = 1118; // Microsoft
                int deviceId = 767;  // Xbox One Controller (wired mini edition?)
                device = HidSharp.DeviceList.Local.GetHidDeviceOrNull(vendorId, deviceId);
                if (device != null)
                {
                    if (!device.TryOpen(out stream))
                    {
                        device = null;
                        stream = null;
                    }
                }
                else
                {
                    stream = null;
                }
            }
        }

        [IsVisibleInDynamoLibrary(true)]
        [CanUpdatePeriodically(true)]
        static Dictionary<string, double> OutputReport()
        {
            var result = new Dictionary<string, double>();

            MaybeInitDevice();
            
            if (stream != null && stream.CanRead)
            {
                strea
            }

            return result;
        }

    }
}
*/