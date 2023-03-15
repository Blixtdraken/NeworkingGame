
using System;
using System.Net.Mime;
using System.Net.Sockets;

namespace Connect_Test
{
    internal class Program
    {
        
        public  static void Main(string[] args)
        {
            TcpClient client = new TcpClient("130.61.171.190", 4587);
            Console.WriteLine(client);
            Console.ReadLine();

        }
       



      

       

       
    }

    class Test
    {
        public event EventHandler MyEvent;
        public void InvokeMyEvent()
        {
            MyEventArgs args = new MyEventArgs();
            args.text = "Working";
            MyEvent?.Invoke(this, args);
        }
    }

    class MyEventArgs : EventArgs
    {
        public string text { get; set; }
    }
}