using Google.Protobuf;
using SoulKnightProtocol;
using System;
using System.Linq;

public class Message
{
    private byte[] buffer = new byte[2048];
    public byte[] Buffer => buffer;
    private int startIndex;
    public int StartIndex => startIndex;
    public int RemainSize => buffer.Length - startIndex;
    public void ReadBuffer(int len, Action<MainPack> Callback)
    {
        startIndex += len;
        if (startIndex <= 4)
        {
            return;
        }
        int Count = BitConverter.ToInt32(buffer, 0);
        while (true)
        {
            if (startIndex >= Count + 4)
            {
                MainPack pack = (MainPack)MainPack.Descriptor.Parser.ParseFrom(buffer, 4, Count);
                Callback(pack);
                Array.Copy(buffer, Count + 4, buffer, 0, startIndex - Count - 4);
                startIndex -= Count + 4;
            }
            else
            {
                break;
            }
        }
    }
    public static byte[] ConvertToByteArray(MainPack pack)
    {
        byte[] data = pack.ToByteArray();
        byte[] head = BitConverter.GetBytes(data.Length);
        return head.Concat(data).ToArray();
    }
    public static PlayerControlInput ConvertToPlayerInput(InputPack pack)
    {
        PlayerControlInput input = new PlayerControlInput();
        input.Vertical = pack.Vertical;
        input.Horizontal = pack.Horizontal;
        input.isAttackKeyDown = pack.IsAttackKeyDown;
        input.MouseWorldPos.Set(pack.MousePosX, pack.MousePosY);
        input.CharacterPos.Set(pack.CharacterPosX, pack.CharacterPosY);
        return input;
    }
    public static byte[] PackDataUDP(MainPack pack)
    {
        return pack.ToByteArray();
    }
}
