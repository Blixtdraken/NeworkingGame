using System;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleListener
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 4587);
            TcpClient server;
            listener.Start();
            Console.WriteLine("Server: Trying to connect...");
            server = listener.AcceptTcpClient();
            Console.WriteLine("Server: Connected!");
            listener.Stop();
            
           
                
          


            byte[] buffer = new byte[256];
            while (true)
            {
                try
                {
                    server.GetStream().Read(buffer, 0, 256);
                
                    Console.Write("Buffer: " + Encoding.ASCII.GetString(buffer));
                    
                }
                catch (IOException e)
                {
                    Console.WriteLine("Client discconected!");
                    server.Close();
                    break;
                }
                
            }
            }
        }
    }
}