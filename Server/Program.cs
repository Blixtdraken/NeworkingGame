
using System.Collections;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using Server;


Console.WriteLine(Cipher.Encode("bob", 256));
Console.WriteLine(Cipher.Decode(Cipher.Encode("bob var bra", 256)));

void ListenForClients([In,Out] TcpListener listener)
{
    while (true)
    {
        
        listener.Start();
        Console.WriteLine("Start Listening");
        TcpClient client = listener.AcceptTcpClient();
        Chatter chatter = new Chatter("", client);
        Console.WriteLine("Client Accepted");
        new Task(() => {RunMsgTask(chatter);}).Start();
        
        Lists.chatters.Add(chatter);
    }
      
       
}

void RunMsgTask([In,Out]Chatter chatter)
{
    Console.WriteLine("Start Scanning Msg");
    byte[] buffer = new byte[256];
    while (true)
    {
        try
        {
            chatter.client.GetStream().Read(buffer, 0, 256);
            //Console.WriteLine(Encoding.ASCII.GetString(buffer));
            Lists.msgque.Add(new MessageData(Encoding.ASCII.GetString(buffer), chatter));
                    
        }
        catch (IOException e)
        {
            Console.WriteLine("A Client discconected!");
            chatter.client.Close();
            Lists.chatters.Remove(chatter);
            break;
        }
        
    }
}

void CommandEvents()
{
    while (true)
    {
        if (Console.ReadLine() == "stop")
        {
            
            System.Environment.Exit(0);
            Console.WriteLine("Closed");
        }
    }
}

Console.WriteLine("Init");
new Task(() => {CommandEvents();}).Start();
TcpListener listener = new TcpListener(IPAddress.Any, 4587);

Console.WriteLine("Init Listener");
//ListenForClients(listener, connections);
new Task(() => { ListenForClients(listener);}).Start();

Console.WriteLine("Start applying messages");
while (true)
{
    
    if (Lists.msgque.Count >= 1)
    {
        byte[] bytes = new byte[256];
        if (string.Empty != Lists.msgque[0].msg)
        {
            Console.WriteLine(Lists.msgque[0].msg + " ");
            bytes = Encoding.ASCII.GetBytes(Lists.msgque[0].msg);
        }


        // client.GetStream().Write(bytes, 0, bytes.Length);
        foreach (Chatter chatter in Lists.chatters)
        {
            if (chatter != Lists.msgque[0].sender)
            {
                byte[] buffer = new byte[256];
                int i = 0;
                foreach (byte dataBit in bytes)
                {
                    buffer[i] = dataBit;
                     i++;
                }
                
                if(isClientAlive(chatter.client))chatter.client.GetStream().Write(buffer, 0, buffer.Length);
            }
            else
            {
                Thread.Sleep(10);
            }
        }

        Lists.msgque.RemoveAt(0);
    }
}






while (true)
{
/*
    byte[] buffer = new byte[256];
    while (true)
    {
        
        tr        {            server.GetStream().Read(buffer, 0, 256);
                
            Console.WriteLine("Buffer: " + Encoding.ASCII.GetString(buffer));
                    
        }
        catch (IOException e)
        {
            Console.WriteLine("Client discconected!");
            server.Close();
            break;
        }
        
    }
        */
                
}

bool isClientAlive(TcpClient client)
{
    try
    {
        bool test = client.Connected;
        return true;
    }
    catch (ObjectDisposedException e)
    {
        Console.WriteLine("Tried using a dead client");
        return false;
    }
}
static class Lists
{
    
    public static List<Chatter> chatters = new List<Chatter>();
    public static List<MessageData> msgque = new List<MessageData>();
}

public class MessageData
{
    public MessageData(string msg, Chatter sender)
    {
        this.msg = msg;
        this.sender = sender;
    }
    public string msg;
    public Chatter sender;
}

public class Chatter
{
    public Chatter(string name, TcpClient client)
    {
        this.name = name;
        this.client = client;
    }
    public TcpClient client;
    public string name;
}