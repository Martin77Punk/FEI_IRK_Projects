using System;
using System.Collections.Generic;
using System.Text;

namespace FEI.IRK.HM.HMIvR
{
    public class DeviceSensors
    {
        private static Lazy<IDeviceSensors> Implementation = new Lazy<IDeviceSensors>(() => CreateDeviceSensors(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        private static IDeviceSensors CreateDeviceSensors()
        {
#if PORTABLE
            return null;
#else
            return new DeviceSensorsImpl();
#endif
        }

        public static IDeviceSensors Current
        {
            get
            {
                return Implementation.Value;
            }
        }

    }
}
