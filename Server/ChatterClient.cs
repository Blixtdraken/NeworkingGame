using System.Net.Sockets;

namespace Server;

public class ChatterClient: TcpClient
{
    private const int bufferSize = 256;
    public string chatterName { get; set; }
    public bool beenNamed { get; set; }
    
    public ChatRoom currentRoom { get; set; }

    public EventHandler ReceivedMsgEvent;
    public EventHandler ClientDisconnectEvent;

    public ChatterClient(Socket socket)
    {
        Client = socket;
        new Task(()=>ByteReadingTask()).Start();
    }
    private void ByteReadingTask()
    {
        while (!Connected);
        while (true)
        {
            byte[] bytes = ReceiveBytes();
            if (bytes != null)
            {
                string text = Cipher.Decode(bytes);
                ReceivedMsgEvent?.Invoke(this, new ReceivedMsgEventArgs(text, this));
            }
           
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
            ClientDisconnectEvent?.Invoke(this, new ClientDisconnectEventArgs(this));
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
        try
        {
            GetStream().Read(buffer, 0, bufferSize);
        }
        catch (Exception e)
        {
            ClientDisconnectEvent?.Invoke(this, new ClientDisconnectEventArgs(this));
            return null;
        }
       
        return buffer;
    }

    private bool isReadBusy = false;
   
    
}

class ReceivedMsgEventArgs:EventArgs
{
    public ReceivedMsgEventArgs(string msg, ChatterClient sender)
    {
        this.msg = msg;
        this.sender = sender;
    }
    public string msg { get; }
    public ChatterClient sender { get; }
}

class ClientDisconnectEventArgs:EventArgs
{
    public ClientDisconnectEventArgs(ChatterClient client)
    {
        this.client = client;
    }
    public ChatterClient client { get; }
}