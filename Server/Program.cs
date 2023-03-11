using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using Server;


void ListenForClients([In,Out] TcpListener listener)
{
    while (true)
    {
        
        listener.Start();
        TcpClient client = listener.AcceptTcpClient();
        byte[] buffer = new byte[256];
        client.GetStream().Read(buffer, 0, 256);
        Chatter chatter = new Chatter(Cipher.Decode(buffer), client);
        Lists.msgque.Add(new MessageData(chatter.name+ " just joined the chat room!", Statics.systemChatter));
        new Task(() => {RunMsgTask(chatter);}).Start();
        Lists.chatters.Add(chatter);
    }
      
       
}

void RunMsgTask([In,Out]Chatter chatter)
{
    byte[] buffer = new byte[256];
    while (true)
    {
        try
        {
            chatter.client.GetStream().Read(buffer, 0, 256);
            Lists.msgque.Add(new MessageData(Cipher.Decode(buffer), chatter));
                    
        }
        catch (IOException e)
        {
            Lists.msgque.Add(new MessageData(chatter.name + " discconected!", Statics.systemChatter));
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
Console.Clear();
Console.WriteLine("Started");
while (true)
{
    
    if (Lists.msgque.GetCount() >= 1 && Lists.msgque.GetAt(0) != null)
    {
        //Console.WriteLine(Lists.msgque.Count);
        //Console.WriteLine(Lists.msgque.Count());
        //Console.WriteLine(Lists.msgque[0].ToString());
        if ("" != Lists.msgque.GetAt(0).msg)
        {
            Console.WriteLine(Lists.msgque.GetAt(0).sender.name + ": " + Lists.msgque.GetAt(0).msg + " ");
        }
        else
        {
            Console.WriteLine(Lists.msgque.GetAt(0).sender.name + ":  ");
        }

        
        lock (Lists.chatters)
        {
            
            foreach (Chatter chatter in Lists.chatters.getCopyOfInternalList())
            {
                if (chatter != Lists.msgque.GetAt(0).sender)
                {
                    try
                    {
                        if (isClientAlive(chatter.client))
                            chatter.client.GetStream()
                                .Write(
                                    Cipher.Encode(Lists.msgque.GetAt(0).sender.name + ": " + Lists.msgque.GetAt(0).msg,
                                        256), 0, 256);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("ERROR: Tried to write on dead connectrion!");
                    }
                   
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }
        

        Lists.msgque.RemoveAt(0);
    }
}


bool isClientAlive(TcpClient client)
{
    try
    {
        bool test = client.GetStream().CanWrite;
        return true;
    }
    catch (ObjectDisposedException e)
    {
        return false;
    }
}
public static class Statics
{
    public static Chatter systemChatter = new Chatter("Server", null);
}
static class Lists
{
    
    public static SafeList<Chatter>chatters = new SafeList<Chatter>();
    public static SafeList<MessageData> msgque = new SafeList<MessageData>();
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
    public bool isSystem = false;
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