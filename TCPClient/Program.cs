using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Security;

namespace TCPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = "localhost";
            int port = 7;
            int tempPort;
            string message = String.Empty;
            Console.WriteLine("Siema, default port = 7, Would you like to change it? If yes press y, otherwise click any key");
            if(Console.ReadLine().Equals("y")) 
            {
                do
                {
                    Console.WriteLine("Give me the port");
                    if(int.TryParse(Console.ReadLine(),out tempPort))
                    {
                        port = tempPort;
                    }
                } while(port < 0);
            }
			try
			{
				Socket socket = Connect1(host, port);
				while (true)
				{
					Console.WriteLine("Insert data to send, 0 will close everything");
					message = Console.ReadLine();
					if (!message.Equals("0"))
					{
						SendMessage(message, socket);
						GetMessage(socket);
					}
					else
					{
						Console.WriteLine("End of data exchange, closing socket");
						socket.Shutdown(SocketShutdown.Both);
						socket.Close();
						Console.WriteLine("Socket closed");
						break;
					}
				}
			}	catch (ArgumentNullException e)
			{
				Console.WriteLine("Address jest null");
			}	catch (SocketException e)
			{
				Console.WriteLine("Niepoowodzenie dostępu do gniazda");
			}	catch (ObjectDisposedException e)
			{
				Console.WriteLine("Gniazdo zostało zamkniete");
			} 
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
            foreach (IPAddress address in IPs)
            {
                if (IPAddress.Parse(address.ToString()).AddressFamily == AddressFamily.InterNetwork)
                {
					try
					{
						s.Connect(address, port);
					}	catch (ArgumentOutOfRangeException e)
					{
						Console.WriteLine("Błedny numer portu");
					}	catch (NotSupportedException e)
					{
						Console.WriteLine("Proba polaczenia do gniazd i protokołów innych niż InterNetwork lub InterNetwortkV6");
					}	catch (ArgumentException e)
					{
						Console.WriteLine("Długość address jest zero");
					}	catch (InvalidOperationException e)
					{
						Console.WriteLine("Gniazdo jest słuchające - listener");
					}
                   
                    break;
                }
            }
            Console.WriteLine("Connection established");
            return s;
        }
        public static void SendMessage(string textToSend, Socket s)
        {
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
            s.Send(bytesToSend);
        }
        public static void GetMessage(Socket s)
        {
            byte[] bytes = new byte[512];
            try
            {
                s.Receive(bytes);
                Console.WriteLine("Received data:");
                Console.WriteLine(Encoding.ASCII.GetString(bytes).TrimEnd('\0'));
            }
            catch (SecurityException e)
            {
                Console.WriteLine("Brak uprawnień przy wykonywaniu metody");
            }
        }
    }
}
