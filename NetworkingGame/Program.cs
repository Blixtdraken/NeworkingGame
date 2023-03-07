using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace NetworkingGame
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Client: Connecting....");
            TcpClient client = new TcpClient("127.0.0.01", 4587);
            //client.Connect(IPAddress.Parse("127.0.0.1"), 4587);
            Console.WriteLine("Client: Connected");


            while (true)
            {
                
                byte[] bytes = Encoding.ASCII.GetBytes(Console.ReadLine());
                
               // client.GetStream().Write(bytes, 0, bytes.Length);
               byte[] buffer = new byte[256];

               int i = 0;
               foreach (byte dataBit in bytes)
               {
                   
                   buffer[i] = dataBit;
                   i++;
               }
                
                
                
                client.GetStream().Write(buffer, 0, buffer.Length);
                //client.GetStream().WriteByte(Byte.Parse(Console.ReadLine()));
                
                
                Console.WriteLine("Added Byte");
            }

            
        }

        
        }

}
