using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using Server;

//This function listens for clients trying to connect, if one is found the remote client is expected to send a 256 bytes long 


void ListenForClients([In,Out] TcpListener listener)
{
    while (true)
    {
        
       
        ChatterClient client = new ChatterClient(listener.AcceptTcpClient().Client);

        Console.WriteLine("Client Connected!");
        string clientName = Cipher.Decode(client.ReadBuffer());
        Console.WriteLine("Client name of " + clientName);
        Chatter chatter = new Chatter(clientName, client);
        //new Task(() => {RunMsgTask(chatter);}).Start();
        new Task(() =>StartSession(chatter)).Start();
        Console.WriteLine("Started sesssion with " + clientName);
       
    }
}

void StartSession(Chatter chatter)
{
    chatter.client.SendString("There are " + GData.chatRooms.GetCount() + " rooms! Type \" join <Room Name (No Spaces)>\" ");
    byte[] bytes;
    string msgBuffer;
    while (true)
    {
        bytes = chatter.client.ReadBuffer();
        msgBuffer = Cipher.Decode(bytes);
        string[] cutMsg = msgBuffer.Split(" ");
        try
        {
            if (cutMsg[0] == "join")
            {
                ChatRoom room = GData.GetChatRoomByName(cutMsg[1]);
                room.AddChatter(chatter);
                chatter.client.SendString("Joined Test");
                break;
            }
        }catch(Exception e){}
        

    }
   
    
}

void CommandEvents()
{
    while (true)
    {
        if (Console.ReadLine() == "stop")
        {
            
            Environment.Exit(0);
            Console.WriteLine("Closed");
        }
    }
}
GData.chatRooms.Add(new ChatRoom("test"));
Console.WriteLine("Init");
new Task(() => {CommandEvents();}).Start();
TcpListener listener = new TcpListener(IPAddress.Any, 4587);
//GData.chatRoom = new ChatRoom("Default");
Console.Clear();
Console.WriteLine("Started");
listener.Start();
ListenForClients(listener);




public static class GData
{
    public static ChatRoom GetChatRoomByName(string name)
    {
        foreach (ChatRoom room in chatRooms.GetCopyOfInternalList())
        {
            if (room.GetRoomName() == name) return room;
        }

        return null;
    }
    public static SafeList<ChatRoom> chatRooms = new SafeList<ChatRoom>();
    //public static ChatRoom chatRoom;
    public static Chatter systemChatter = new Chatter("Server", null);
}

[Obsolete("This does literally nothing anymore! Just kept around incase I need it for later??????????? Bad choice.?  . . . yes!")]
static class Lists
{
    
   // public static SafeList<Chatter>chatters = new SafeList<Chatter>();
    //public static SafeList<MessageData> msgque = new SafeList<MessageData>();
}



