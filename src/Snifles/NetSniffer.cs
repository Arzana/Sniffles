using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Snifles
{
    public static class NetworkSniffer
    {
        public delegate void NetHandler(Packet packet);

        public static bool Stop;
        public static event NetHandler OnReceive;

        private static Socket socket;
        private static byte[] buffer;
        private static bool working;

        public static void Sniff(IPAddress address = null, int port = 0)
        {
            if (address == null)
            {
                IPHostEntry entry = Dns.GetHostEntry(Dns.GetHostName());
                address = entry.AddressList.First(a => a.AddressFamily == AddressFamily.InterNetwork);
            }
            IPEndPoint localEP = new IPEndPoint(address, port);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
            socket.Bind(localEP);

            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);

            byte[] byTrue = new byte[4] { 1, 0, 0, 0 };
            byte[] byOut = new byte[4];
            socket.IOControl(IOControlCode.ReceiveAll, byTrue, byOut);

            buffer = new byte[4096];
            working = true;
            socket.BeginReceive(buffer, 0, buffer.Length, 0, HandlePackage, null);
        }

        public static void WaitTillClosed(bool setStop = true)
        {
            if (setStop) Stop = true;
            while (working) Thread.Sleep(50);

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        private static void HandlePackage(IAsyncResult ar)
        {
            int count = socket.EndReceive(ar);
            Packet p = new Packet(buffer, count);

            if (!Stop)
            {
                buffer = new byte[4096];
                socket.BeginReceive(buffer, 0, buffer.Length, 0, HandlePackage, null);
            }
            else working = false;

            if (OnReceive != null) OnReceive.Invoke(p);
        }
    }
}