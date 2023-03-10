using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NetworkingGame
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            
            
            void RunMsgTask([In,Out]TcpClient client)
            {
                Console.WriteLine("Start Scanning Msg");
                byte[] buffer = new byte[256];
                while (true)
                {
                    if (!client.Connected) break;
                    try
                    {
                        client.GetStream().Read(buffer, 0, 256);
                        Console.WriteLine(Encoding.ASCII.GetString(buffer));
                        
                    
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine("A Client discconected!");
                        client.Close();
            
                        break;
                    }
        
                }
            }

            Console.Write("Enter Name: ");  string name = Console.ReadLine();
            //Console.Write("Input host adress: ");
            //string ipAdress = Console.ReadLine();
            string ipAdress = "130.61.171.190";
            
            if(args.Length != 0 && args[0] == "local")
             ipAdress= "127.0.0.1";
            
            Console.WriteLine(ipAdress);
            Console.WriteLine("Client: Connecting....");
            TcpClient client = new TcpClient(ipAdress, 4587);
            new Task(() => {RunMsgTask(client);}).Start();
            //client.Connect(IPAddress.Parse("127.0.0.1"), 4587);
            Console.WriteLine("Client: Connected");
            

            while (true)
            {
                
                byte[] bytes = Encoding.ASCII.GetBytes(name + ": " + Console.ReadLine());
                
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
                
            }

            
        }

        
        }

}
