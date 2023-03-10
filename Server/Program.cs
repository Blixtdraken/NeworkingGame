
using System.Collections;
using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;


void ListenForClients([In,Out] TcpListener listener)
{
    while (true)
    {
        listener.Start();
        Console.WriteLine("Start Listening");
        TcpClient client = listener.AcceptTcpClient();
        Console.WriteLine("Client Accepted");
        new Task(() => {RunMsgTask(client);}).Start();
        
        Lists.clients.Add(client);
    }
      
       
}

void RunMsgTask([In,Out]TcpClient client)
{
    Console.WriteLine("Start Scanning Msg");
    byte[] buffer = new byte[256];
    while (true)
    {
        if (!client.Connected) break;
        try
        {
            client.GetStream().Read(buffer, 0, 256);
            //Console.WriteLine(Encoding.ASCII.GetString(buffer));
            Lists.msgque.Add(new MessageData(Encoding.ASCII.GetString(buffer), client));
                    
        }
        catch (IOException e)
        {
            Console.WriteLine("A Client discconected!");
            client.Close();
            
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
        Console.WriteLine(Lists.msgque[0].msg);
        byte[] bytes = Encoding.ASCII.GetBytes(Lists.msgque[0].msg);
                
        // client.GetStream().Write(bytes, 0, bytes.Length);
        foreach (TcpClient client in Lists.clients)
        {

            if (client == Lists.msgque[0].sender)
            {
                
            
                byte[] buffer = new byte[256];
                int i = 0;
                foreach (byte dataBit in bytes)
                {
                   
                buffer[i] = dataBit;
                i++;
                }
                client.GetStream().Write(buffer, 0, buffer.Length);
                
                Lists.msgque.RemoveAt(0);
        }
            else
            {
                Thread.Sleep(10);
            }
        }
    }
}






while (true)
{
/*
    byte[] buffer = new byte[256];
    while (true)
    {
        
        try
        {
            server.GetStream().Read(buffer, 0, 256);
                
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

static class Lists
{
    
    public static List<TcpClient> clients = new List<TcpClient>();
    public static List<MessageData> msgque = new List<MessageData>();
}

public class MessageData
{
    public MessageData(string msg, TcpClient sender)
    {
        this.msg = msg;
        this.sender = sender;
    }
    public string msg;
    public TcpClient sender;
}