using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Util
{
    public static string ConvertToHex(byte SingleByte)
    {
        string returnString = string.Empty;
        returnString = SingleByte.ToString("x2").ToUpper();
        return returnString;
    }
    public static string ConvertToBin(byte SingleByte)
    {
        string returnString = string.Empty;
        returnString = Convert.ToString(SingleByte, 2);
        return returnString;
    }
    public static int ConvertBinToInt(string BinString)
    {
        int returnInt = 0;
        returnInt = Convert.ToInt32(BinString, 2);
        return returnInt;
    }
    public static int ConvertHexToInt(string HexString)
    {
        int returnInt = 0;
        returnInt = Convert.ToInt32(HexString, 16);
        return returnInt;
    }

    public static UInt16 GetUshortFromLittleEndian(ushort ushortvalue)
    {
        byte[] bytes = BitConverter.GetBytes(ushortvalue);
        byte[] newbytes = {
			bytes[1],
			bytes[0]
		};
        return (UInt16)BitConverter.ToInt16(newbytes, 0);
    }

    public static List<byte> UnEscapeUartBytes(List<byte> ListOfBytes)
    {
        List<byte> list = new List<byte>();
        int countEscapteByte = ListOfBytes.Count(item => item == (byte)0x7d);

        if (countEscapteByte > 0)
        {
            for (int i = 0; i < ListOfBytes.Count; i++)
            {
                byte Currentbyte = ListOfBytes[i];
                if (Currentbyte == (byte)0x7d)
                {
                    byte singlebyte = ListOfBytes[i + 1];
                    singlebyte = (byte)(singlebyte ^ 0x20); //bitwise XOR
                    list.Add(singlebyte);
                    i++;
                }
                else
                {
                    list.Add(Currentbyte);
                }
            }
        }
        else
        {
            return ListOfBytes;
        }
        return list;
    }

    public static List<byte> EscapeUartBytes(List<byte> ListOfBytes)
    {
        List<byte> list = new List<byte>();

        foreach (byte abyte in ListOfBytes)
        {
            switch (abyte)
            {
                case (byte)0x7e: // default delimitor
                    if (list.Count == 0)
                    {
                        list.Add(abyte);
                    }
                    else
                    {
                        list.Add(0x7d);
                        byte XoredByte = (byte)(abyte ^ 0x20);
                        list.Add(XoredByte);
                    }
                    break;

                case (byte)0x11:
                    list.Add(0x7d);
                    byte XoredByte11 = (byte)(abyte ^ 0x20);
                    list.Add(XoredByte11);
                    break;

                case (byte)0x13:
                    list.Add(0x7d);
                    byte XoredByte13 = (byte)(abyte ^ 0x20);
                    list.Add(XoredByte13);
                    break;

                case (byte)0x7d:
                    list.Add(0x7d);
                    byte XoredByte7d = (byte)(abyte ^ 0x20);
                    list.Add(XoredByte7d);
                    break;

                default:
                    list.Add(abyte);
                    break;
            }
        }
        return list;
    }

    public static List<byte> EscapeUartBytes(byte[] ArrayOfBytes)
    {
        List<byte> NewList = new List<byte>();
        NewList.AddRange(ArrayOfBytes);
        return Util.EscapeUartBytes(NewList);
    }
}
