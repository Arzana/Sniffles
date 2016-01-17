using Snifles.Application_Layer;
using Snifles.Data;
using Snifles.Internet_Layer;
using Snifles.Transport_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Snifles
{
    public static class Program
    {
        static bool debug;

        public static void Main(string[] args)
        {
            NetworkSniffer.OnReceive += NS_OnReceive;
            NetworkSniffer.Sniff();

            Console.ReadKey();
            Console.WriteLine("Shutting down!");
            NetworkSniffer.WaitTillClosed();
        }

        public static void NS_OnReceive(Packet packet)
        {
            if (packet.Protocol == ProtocolType.Udp)
            {
                if ((packet.TransportHeader as UdpHeader).IsDns)
                {
                    if (!debug)
                    {
                        debug = true;
                        DnsPacket dns = new DnsPacket(packet);
                        for (int i = 0; i < dns.Questions.Length; i++)
                        {
                            Write(dns.Questions[i].QueriedDomainName);
                        }
                        debug = false;
                    }
                }
                else Write("UDP");
            }
            else if (packet.Protocol == ProtocolType.Tcp)
            {
                if ((packet.TransportHeader as TcpHeader).IsDns) Write("DNS(TCP)");
                else Write("TCP");
            }
            else if (packet.Protocol == ProtocolType.Icmp) Write($"ICMP({(packet.TransportHeader as IcmpHeader).Type})");
            else Write(packet.Protocol.ToString());
        }

        private static void Write(string msg)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("hh:mm:ss")}] - {msg}");
        }
    }
}
