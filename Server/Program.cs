using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Server;

//This function listens for clients trying to connect, if one is found the remote client is expected to send a 256 bytes long 


void ListenForClients([In,Out] TcpListener listener)
{
    while (true)
    {
      
            ChatterClient client = new ChatterClient(listener.AcceptTcpClient().Client);
            Console.WriteLine("Client Connected!");
            new Task(() =>GetName(client)).Start();
            Console.WriteLine("Started sesssion with new client to get name!");

        
       
    }
}

void GetName(ChatterClient client)
{
    client.SendString("cmd clear");
    client.SendString("Please Input your name: ");
    client.ReceivedMsgEvent += GetNameTrigger;
}

void GetNameTrigger(object sender, EventArgs a)
{
    ReceivedMsgEventArgs args = (ReceivedMsgEventArgs)a;
    ChatterClient client = args.sender;
    if (!client.beenNamed)
    {
        client.beenNamed = true;
        client.chatterName = args.msg;
        Console.WriteLine("Got name from " + client.chatterName);
        Lobby.RoomSelectText(client);
        client.ReceivedMsgEvent += Lobby.RoomSelectTrigger;
    }
    client.ReceivedMsgEvent -= GetNameTrigger;
    
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

Console.WriteLine("Init");
new Task(() => {CommandEvents();}).Start();
TcpListener listener = new TcpListener(IPAddress.Any, 4587);
Console.Clear();
Console.WriteLine("Started");
listener.Start();
ListenForClients(listener);



public static class Lobby
{
    public static void RoomSelectText(ChatterClient client)
    {
        SafeList<ChatRoom> rooms = GData.chatRooms;
        client.SendString("cmd clear");
        client.SendString("Type \"join <Room Name>\" to join a room, if room doesn't yet exist it will be created for you!");
        client.SendString("Existing Rooms:");
        foreach (ChatRoom room in rooms.GetCopyOfInternalList())
        {
            client.SendString(room.GetRoomName() + "    Chatters: " + room.GetListOfChatters().Count);
        }

        client.SendString("  ");
    }

    public static void RemoveRoom(ChatRoom room)
    {
        GData.chatRooms.Remove(room);
    }

    public static void RoomSelectTrigger(object sender, EventArgs a)
    {
        ReceivedMsgEventArgs args = (ReceivedMsgEventArgs)a;
        ChatterClient client = args.sender;
        Console.WriteLine(client.chatterName + " tried select");
        string msg = args.msg; Console.WriteLine(client.chatterName + " wrote " + msg);
        try
        {
            string[] cutMsg = msg.Split(" ");
            if (cutMsg[0] == "join" && cutMsg.Length >=2)
            {
                
                if (GData.GetChatRoomByName(cutMsg[1]) != null)
                {
                    ChatRoom room = GData.GetChatRoomByName(cutMsg[1]);
                    room.AddChatter(client);
                }
                else
                {
                    ChatRoom room = new ChatRoom(cutMsg[1]);
                    GData.chatRooms.Add(room);
                    room.AddChatter(client);
                }
                
                client.ReceivedMsgEvent -= RoomSelectTrigger;
            
            }
        }catch(Exception e){}
    
    }
}

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
    
}

[Obsolete("This does literally nothing anymore! Just kept around incase I need it for later??????????? Bad choice.?  . . . yes!")]
static class Lists
{
    
   // public static SafeList<Chatter>chatters = new SafeList<Chatter>();
    //public static SafeList<MessageData> msgque = new SafeList<MessageData>();
}



