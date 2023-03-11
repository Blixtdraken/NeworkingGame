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

            String consoleBuffer = "";
            
            
            void RunMsgTask([In,Out]TcpClient client)
            {
                byte[] buffer = new byte[256];
                while (true){
                
                    if (!client.Connected) break;
                    try
                    {
                        client.GetStream().Read(buffer, 0, 256);
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write(Cipher.Decode(buffer));
                        Console.SetCursorPosition(0, Console.CursorTop+1);
                        Console.Write(consoleBuffer);
                        
                    
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine("A Client discconected!");
                        client.Close();
            
                        break;
                    }
        
                }
            }
            Console.WriteLine("This is Blixt's chatroom/networking test! Select a name for you to be refernced by!");
            Console.Write("Enter Name: ");  string name = Console.ReadLine();
            Console.Clear();
            //Console.Write("Input host adress: ");
            //string ipAdress = Console.ReadLine();
            string ipAdress = "130.61.171.190";
            
            if(args.Length != 0 && args[0] == "local")
             ipAdress= "127.0.0.1";
            
            //Console.WriteLine(ipAdress);
            Console.WriteLine("Client: Connecting....");
            TcpClient client = new TcpClient(ipAdress, 4587);
            client.GetStream().Write(Cipher.Encode(name, 256), 0 ,256);
            new Task(() => {RunMsgTask(client);}).Start();
            //client.Connect(IPAddress.Parse("127.0.0.1"), 4587);
            Console.WriteLine("Client: Connected");
            Console.Clear();
            Console.WriteLine("Welcome to the chatroom " + name + "!");
            

            while (true)
            {
                while (true)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                   

                    if (key.Key == ConsoleKey.Backspace && consoleBuffer != "")
                    {
                        consoleBuffer = consoleBuffer.Remove(consoleBuffer.Length-1, 1);
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write(consoleBuffer);
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write("Me: " + consoleBuffer);
                        Console.SetCursorPosition(0, Console.CursorTop+1);
                        break;
                    }
                    else
                    {
                        consoleBuffer += key.KeyChar;
                        Console.Write(key.KeyChar);
                    }
                
                }
                byte[] buffer = new byte[256];
                buffer = Cipher.Encode(consoleBuffer, 256);
                client.GetStream().Write(buffer, 0, buffer.Length);
                consoleBuffer = "";


            }

            
        }

        
        }
    
    

}
