using System.Text;

namespace Server;

static public class Cipher
{
    static public byte[] Encode(string text, int size)
    {

        byte[] encodedText = new byte[size];
        Encoding.UTF8.GetBytes(text, encodedText);
        
        return encodedText;
    }

    static public string Decode(byte[] encoding)
    {
        byte[] decoding = new byte[0];
        for (int i = encoding.Length -1; i >= 0; i--)
        {
            if (encoding[i] != 00)
            {
                decoding = new byte[i+1];
                
                for(int j = 0; j<decoding.Length; j++)
                {
                    decoding[j] = encoding[j];
                }

                break;
            }
        }
        
        return Encoding.UTF8.GetString(decoding);
    }
}