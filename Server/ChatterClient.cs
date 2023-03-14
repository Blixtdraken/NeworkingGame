using System.Net.Sockets;

namespace Server;

public class ChatterClient: TcpClient
{
    private const int bufferSize = 256;
    public string chatterName { get; set; }
    public bool beenNamed { get; set; }
    
    public ChatRoom currentRoom { get; set; }

    public EventHandler ReceivedMsgEvent;

    public ChatterClient(Socket socket)
    {
        Client = socket;
        new Task(()=>ByteReadingTask()).Start();
       // new Task(()=>Debug()).Start();

        
        
    }
    private void ByteReadingTask()
    {
        while (!Connected);
        while (true)
        {
            byte[] bytes = ReceiveBytes();
        }
        
    }
    public bool isClientAlive()
    {
        try
        {
            bool test = GetStream().CanWrite;
            return true;
        }
        catch (ObjectDisposedException e)
        {
            return false;
        }
    }
    
    public bool SendBytes(byte[] bytes)
    {
        try
        {
            GetStream().Write(bytes, 0, bufferSize);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
        
    }

    public bool SendString(string text)
    {
        return SendBytes(Cipher.Encode(text, bufferSize));
    }

    private byte[] ReceiveBytes()
    {
        byte[] buffer = new byte[bufferSize];
        GetStream().Read(buffer, 0, bufferSize);
        return buffer;
    }

    private bool isReadBusy = false;
   
    
}

class ReceivedMsgEventArgs:EventArgs
{
    public string msg { get; set; }
    public ChatterClient sender { get; set; }
}