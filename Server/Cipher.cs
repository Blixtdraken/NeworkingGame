using System.Text;

namespace Server;

static public class Cipher
{
    static public byte[] Encode(string text, int size)
    {

        byte[] encodedText = new byte[size];
        Encoding.ASCII.GetBytes(text, encodedText);
        
        return encodedText;
    }

    static public string Decode(byte[] encoding)
    {
        byte[] decoding = new byte[0];
        for (int i = encoding.Length -1; i < 1; i--)
        {
            if (encoding[i] != 00)
            {
                decoding = new byte[i+1];
                int f = 0;
                foreach (byte bit in decoding)
                {
                    decoding[f] = bit;
                    f++;
                }

                break;
            }
        }
        
        return Encoding.ASCII.GetString(decoding);
    }
}