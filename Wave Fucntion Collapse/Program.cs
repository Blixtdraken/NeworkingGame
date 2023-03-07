using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace Wave_Fucntion_Collapse
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

                byte[] buffer = new byte[256];

                //foreach (byte VARIABLE in bytes)
                // {
              //      
              //  } 
                
                client.GetStream().Write(bytes, 1, 256);
                //client.GetStream().WriteByte(Byte.Parse(Console.ReadLine()));
                
                
                Console.WriteLine("Added Byte");
            }

            
        }

        
        }

}
