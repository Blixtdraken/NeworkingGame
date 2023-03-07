using System;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleListener
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 4587);
            TcpClient server;
            listener.Start();
            while (true)
            {
                try
                {
                    Console.WriteLine("Trying to connect...");
                    server = listener.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    break;
                }
                catch (Exception e)
                {
                
                }
            }


            byte[] buffer = new byte[256];
            while (true)
            {
                server.GetStream().Read(buffer, 0, 256);
                
                Console.WriteLine(Encoding.ASCII.GetString(buffer));
            }
            Console.ReadLine();
        }
    }
}