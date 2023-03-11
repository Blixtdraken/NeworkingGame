using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Connect_Test
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                
                    Console.Write(key.KeyChar + "");
                    
                    if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                
            }
            Console.ReadLine();
        }
    }
}