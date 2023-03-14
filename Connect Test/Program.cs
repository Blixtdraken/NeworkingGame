
using System;
using System.Net.Mime;

namespace Connect_Test
{
    internal class Program
    {
        
        public  static void Main(string[] args)
        {

            byte bit = 1;
            int num = bit;
            Test test = new Test();
            test.MyEvent += TriggerThis;
            Console.WriteLine("Started!");
            test.InvokeMyEvent();
            test.InvokeMyEvent();
            test.MyEvent -= TriggerThis;
            test.InvokeMyEvent();
            test.InvokeMyEvent();
            test.MyEvent -= TriggerThis;
            test.InvokeMyEvent();
            Console.ReadLine();

        }
        static void TriggerThis (object sender, EventArgs args)
        {
            Console.WriteLine(((MyEventArgs)args).text);
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