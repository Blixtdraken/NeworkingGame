using System.Net;
using System.Net.Sockets;

namespace Connect_Test
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            TcpClient client = new TcpClient("130.61.171.190", 13000);
        }
    }
}