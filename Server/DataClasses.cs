using System.Net.Sockets;

namespace Server;

//Info about player/user/chatter
public class Chatter
{
    public Chatter(string name, ChatterClient client)
    {
        this.name = name;
        this.client = client;
    }
    public ChatterClient client;
    public string name;
}

//Self explantory
public class MessageData
{
    public MessageData(string msg, Chatter sender)
    {
        this.msg = msg;
        this.sender = sender;
    }
    
    public string msg;
    public Chatter sender;
    //public bool isSystem = false;
}
