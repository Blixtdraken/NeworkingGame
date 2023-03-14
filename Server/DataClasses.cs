using System.Net.Sockets;

namespace Server;

//Info about player/user/chatter

[Obsolete("Now storing this info directly in chatterclient")]
public class Chatter
{
    public Chatter(ChatterClient client)
    {
        this.client = client;
    }
    public ChatterClient client;
    public string name = "ERROR: NAME NOT SET WEWOOO WEOOOO";
}

//Self explantory
public class MessageData
{
    public MessageData(string msg, ChatterClient sender)
    {
        this.msg = msg;
        this.sender = sender;
    }
    
    public string msg;
    public ChatterClient sender;
    //public bool isSystem = false;
}
