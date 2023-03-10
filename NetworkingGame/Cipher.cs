using System.Text;

namespace NetworkingGame
{
    static public class Cipher
    {
        static public byte[] Encode(string text, int size)
        {

            byte[] encodedText = Encoding.ASCII.GetBytes(text);

            byte[] output = new byte[size];
            int i = 0;
            foreach (byte bit in encodedText)
            {
                output[i] = bit;
                i++;
            }
            return output;
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
        
            return Encoding.ASCII.GetString(decoding);
        }
    }
}