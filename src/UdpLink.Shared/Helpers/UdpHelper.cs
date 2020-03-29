using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace UdpLink.Shared.Helpers
{
    public class UdpHelper
    {
        public static void ConfigureSocket(UdpClient listener)
        {
            const int SIO_UDP_CONNRESET = -1744830452;
            byte[] inValue = new byte[] { 0 };
            byte[] outValue = new byte[] { 0 };
            listener.Client.IOControl(SIO_UDP_CONNRESET, inValue, outValue);
        }
    }
}
