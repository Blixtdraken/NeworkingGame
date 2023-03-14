using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Server;

public class ChatRoom
{
    private string roomName; 
    private SafeList<ChatterClient> chatters = new SafeList<ChatterClient>();
    private SafeList<MessageData> msgque = new SafeList<MessageData>();

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
        
        
        new Task(() => { MsgQueScrollerTask(); }).Start();
    }


    public void MsgQueScrollerTask()
    {
        while (true)
        {
    
            if (msgque.GetCount() >= 1 && msgque.GetAt(0) != null)
            {
                if ("" != msgque.GetAt(0).msg)
                {
                    Console.WriteLine(msgque.GetAt(0).sender.chatterName + ": " + msgque.GetAt(0).msg + " ");
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
                                if (client.isClientAlive())
                                    client.SendString(msgque.GetAt(0).sender.chatterName + ": " + msgque.GetAt(0).msg);
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
    }
    
    

    public void AddChatter(ChatterClient element)
    {
        chatters.Add(element);
        //new Task(() => { MsgReceiveTask(element);}).Start();  
        msgque.Add(new MessageData(element.name + " joined!", GData.systemChatter));
    }
    void MsgReceiveTrigger(object sender, EventArgs a)
    {
        byte[] buffer = new byte[256];
        while (true)
        {
            try
            {
                
                msgque.Add(new MessageData(Cipher.Decode(chatter.client.ReadBuffer()), chatter));
                    
            }
            catch (IOException e)
            {
                msgque.Add(new MessageData(chatter.name + " discconected!", GData.systemChatter));
                chatter.client.Close();
                chatters.Remove(chatter); 
            
                break;
            }
        
        }
    }
    
  
}