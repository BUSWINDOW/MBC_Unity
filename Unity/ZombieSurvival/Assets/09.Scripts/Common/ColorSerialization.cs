using ExitGames.Client.Photon;
using UnityEngine;

public class ColorSerialization {
    private static byte[] colorMemory = new byte[4 * 4];
    private static byte[] zombieMemory = new byte[4 * 4];

    public static short SerializeColor(StreamBuffer outStream, object targetObject) {
        Color color = (Color) targetObject;

        lock (colorMemory)
        {
            byte[] bytes = colorMemory;
            int index = 0;
            
            Protocol.Serialize(color.r, bytes, ref index);
            Protocol.Serialize(color.g, bytes, ref index);
            Protocol.Serialize(color.b, bytes, ref index);
            Protocol.Serialize(color.a, bytes, ref index);
            outStream.Write(bytes, 0, 4*4);
        }

        return 4 * 4;
    }

    public static object DeserializeColor(StreamBuffer inStream, short length)  {
        Color color = new Color();
  
        lock (colorMemory)
        {
            inStream.Read(colorMemory, 0, 4 * 4);
            int index = 0;
            
            Protocol.Deserialize(out color.r,colorMemory, ref index);
            Protocol.Deserialize(out color.g,colorMemory, ref index);
            Protocol.Deserialize(out color.b,colorMemory, ref index);
            Protocol.Deserialize(out color.a,colorMemory, ref index);
        }
        
        return color;
    }


/*    public static short SerializeZombieData(StreamBuffer outStream, object targetObject)
    {
        //Color color = (Color)targetObject;
        ZombieData data = (ZombieData)targetObject;

        lock (zombieMemory)
        {
            byte[] bytes = zombieMemory;
            int index = 0;

            Protocol.Serialize(data.hp, bytes, ref index);
            Protocol.Serialize(data.moveSpeed, bytes, ref index);
            Protocol.Serialize(data.damage, bytes, ref index);
            Protocol.Serialize(data.skinColor, bytes, ref index);
            outStream.Write(bytes, 0, 4 * 4);
        }

        return 4 * 4;
    }

    public static object DeserializeZombieData(StreamBuffer inStream, short length)
    {
        Color color = new Color();

        lock (colorMemory)
        {
            inStream.Read(colorMemory, 0, 4 * 4);
            int index = 0;

            Protocol.Deserialize(out color.r, colorMemory, ref index);
            Protocol.Deserialize(out color.g, colorMemory, ref index);
            Protocol.Deserialize(out color.b, colorMemory, ref index);
            Protocol.Deserialize(out color.a, colorMemory, ref index);
        }

        return color;
    }*/
}