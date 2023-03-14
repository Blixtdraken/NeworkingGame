using System.Net.Sockets;

namespace Server;

public class ChatterClient: TcpClient
{
    private const int bufferSize = 256;
    private SafeList<byte[]> localPacketBuffer = new SafeList<byte[]>();

    public ChatterClient(Socket socket)
    {
        Client = socket;
        new Task(()=>ByteReadingTask()).Start();
       // new Task(()=>Debug()).Start();

        
        
    }

    void Debug()
    {
        while (true)
        {
            Console.WriteLine("BUffer Size: " + localPacketBuffer.GetCount());
            Thread.Sleep(10);
        }
    }
    void ByteReadingTask()
    {
        while (!Connected);
        while (true)
        {
            byte[] bytes = ReceiveBytes();
            while (isReadBusy);
            localPacketBuffer.Add(bytes);
            Console.WriteLine("Got TEXT " + Cipher.Decode(bytes));
            if (bytes == null)
            {
                Console.WriteLine("                       ewigfhwohfbukwrbfiowrgorwgorbgrngorngiwr                        Got Null Bytes");
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
            return false;
        }
        
    }

    public bool SendString(string text)
    {
        return SendBytes(Cipher.Encode(text, bufferSize));
    }

    byte[] ReceiveBytes()
    {
        byte[] buffer = new byte[bufferSize];
        GetStream().Read(buffer, 0, bufferSize);
        return buffer;
    }

    void StreamClosed(ChatterClient client)
    {
        
    }

    private bool isReadBusy = false;
    public byte[] ReadBuffer()                      //Randomlly Returns null bytes
    {
        byte[] bytes = null;
        while (bytes == null)
        {
            while (localPacketBuffer.GetCount() == 0 || isReadBusy);
            isReadBusy = true;
            Console.WriteLine(localPacketBuffer.GetAt(0)[0]);
            bytes = localPacketBuffer.GetAt(0); Console.WriteLine("Size Before: " + localPacketBuffer.GetCount());
            localPacketBuffer.RemoveAt(0);  Console.WriteLine("Removed Bytes");
            isReadBusy = false; Console.WriteLine("Buffer After: " + localPacketBuffer.GetCount());
            if(bytes == null)Console.WriteLine("It was null");
        }
        Console.WriteLine("Bytes Read as: "+ Cipher.Decode(bytes));
        return bytes;
    }
    public byte[] PeekAtBuffer()
    {
        return localPacketBuffer.GetAt(0);
    }
    
    
}