using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = "127.0.0.1";
            int port = 7;
            Socket socket = Connect1(host, port);

            SendMessage("Michal", socket);
            Console.Read();
        }
        public static Socket Connect1(string host, int port)
        {
            IPAddress[] IPs = Dns.GetHostAddresses(host);

            Socket s = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);

            Console.WriteLine("Establishing Connection to {0}",
                host);
            s.Connect(IPs[0], port);
            Console.WriteLine("Connection established");
            return s;
        }
        public static void SendMessage(string textToSend, Socket s)
        {
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
            s.Send(bytesToSend);
        }
    }
}
