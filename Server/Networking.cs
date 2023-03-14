using System.Net.Sockets;

namespace Server;


//Might remove later, not thinking of using!
public static class Networking
{
    private const int bufferSize = 256;

    public static void SendMsg(TcpClient client, string msg)
    {
        byte packet = (byte) PacketTypes.Msg;
        byte[] bytes = new byte[bufferSize];
        byte[] encodedMsg = Cipher.Encode(msg, bufferSize - 1);
        
        for(int i = 0; i<bufferSize; i++)
        {
            if (i == 0) bytes[0] = packet;
            else bytes[i] = encodedMsg[i + 1];

        }
        client.GetStream().Write(bytes, 0, bufferSize);
    }
    
    public static string WaitForReceiveMsg(TcpClient client)
    {
        while (true)
        {
            byte[] buffer = new byte[bufferSize];
           // client.GetStream().
        }
    }



    
} 
