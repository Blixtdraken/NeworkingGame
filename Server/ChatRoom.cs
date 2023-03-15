using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Server;

public class ChatRoom
{
    private string roomName; 
    private SafeList<ChatterClient> chatters = new SafeList<ChatterClient>();
    private SafeList<MessageData> msgque = new SafeList<MessageData>();
    private Task msgQueScrollerTask;
    
    private bool running = true;

    public ChatRoom(string roomName)
    {
        this.roomName = roomName;
        Init();
    }

    public string GetRoomName()
    {
        return this.roomName;
    }
    public void Init()
    {

        msgQueScrollerTask = new Task(() => { MsgQueScrollerTask(); });
        msgQueScrollerTask.Start();
    }


    public Task MsgQueScrollerTask()
    {
        while (running)
        {
    
            if (msgque.GetCount() >= 1 && msgque.GetAt(0) != null)
            {
                if ("" != msgque.GetAt(0).msg)
                {
                    if (msgque.GetAt(0).sender != null)
                    {
                        Console.WriteLine(msgque.GetAt(0).sender.chatterName + ": " + msgque.GetAt(0).msg);
                    }
                    else
                    {
                        Console.WriteLine(msgque.GetAt(0).msg);
                    }
                    
                }
                else
                {
                    Console.WriteLine(msgque.GetAt(0).sender.chatterName + ":  ");
                }

        
                lock (chatters)
                {
            
                    foreach (ChatterClient client in chatters.GetCopyOfInternalList())
                    {
                        if (client != msgque.GetAt(0).sender)
                        {
                            try
                            {
                                if (msgque.GetAt(0).sender != null)
                                {
                                    client.SendString(msgque.GetAt(0).sender.chatterName + ": " + msgque.GetAt(0).msg);
                                }
                                else
                                {
                                    client.SendString(msgque.GetAt(0).msg);
                                }
                                
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("ERROR: Tried to write on dead connection!");
                            }
                   
                        }
                        else
                        {
                            Thread.Sleep(100);
                        }
                    }
                }
        

                msgque.RemoveAt(0);
            }
        }

        return Task.CompletedTask;
    }
    
    

    public void AddChatter(ChatterClient client)
    {
        chatters.Add(client);
        client.SendString("You just joined the room with name " + this.roomName + "!");
        //new Task(() => { MsgReceiveTask(element);}).Start();  
        msgque.Add(new MessageData(client.chatterName + " joined the room!", null));
        client.ReceivedMsgEvent += MsgReceiveTrigger;
        client.ClientDisconnectEvent += LeaveTrigger;
    }

    void LeaveTrigger(object sender, EventArgs a)
    {
        running = false;
        ClientDisconnectEventArgs args = (ClientDisconnectEventArgs) a;
        ChatterClient client = args.client;
        client.ReceivedMsgEvent -= MsgReceiveTrigger;
        client.ClientDisconnectEvent -= LeaveTrigger;
        chatters.Remove(client);
        msgque.Add(new MessageData(client.chatterName + " left the room!!", null));

        if (chatters.GetCount() == 0)
        {
            msgQueScrollerTask.Wait();
            msgQueScrollerTask.Dispose();
            Lobby.RemoveRoom(this);
        }
        
    }
    void MsgReceiveTrigger(object sender, EventArgs a)
    {
        ReceivedMsgEventArgs args = (ReceivedMsgEventArgs) a;
        ChatterClient client = args.sender;
        msgque.Add(new MessageData(args.msg, client));
    }
    
  
}