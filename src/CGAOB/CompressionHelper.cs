using System.IO.Compression;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace CGAOB;

public static class CompressionHelper
{
    public static byte[] Compress(byte[] data)
    {
        MemoryStream memStream = new MemoryStream();
        using (DeflateStream dstream = new DeflateStream(memStream, CompressionLevel.Fastest))
        {
            dstream.Write(data, 0, data.Length);
        }

        var output = memStream.ToArray();
        return output;
    }

    public static byte[] Decompress(byte[] data)
    {
        MemoryStream input = new MemoryStream(data);
        MemoryStream output = new MemoryStream();
        using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
        {
            dstream.CopyTo(output);
        }
        return output.ToArray();
    }
    
    
    public static byte[] DecompressImageData(string data)
    {
        var compressedByteData = Convert.FromBase64String(data);
        var decompressedByteData = CompressionHelper.Decompress(compressedByteData);
        var outputData = new byte[160 * 200];

        int outputIndex = 0;
        foreach (var b in decompressedByteData)
        {
            outputData[outputIndex++] = (byte)(b & 0b_0000_0011);
            outputData[outputIndex++] = (byte)((b & 0b_0000_1100) >> 2);
            if(outputIndex < 32000) outputData[outputIndex++] = (byte)((b & 0b_0011_0000) >> 4);
        }
        return outputData;
    }

    public static string CompressImageData(byte[] data)
    {
        List<byte> output = new();
        var byteIndex = 0;
        byte i = 0;
        foreach (var b in data)
        {

            switch (byteIndex)
            {
                case 0:
                    i |= (byte)(b);
                    break;
                case 1:
                    i |= (byte)(b << 2);
                    break;
                case 2: 
                    i |= (byte)(b << 4);
                    break;
            }
                
            byteIndex++;
            if (byteIndex > 2)
            {
                output.Add(i);
                byteIndex = 0;
                i = 0;
            }
        }

        if (byteIndex > 0)
        {
            output.Add(i);
        }
        
        var compressed = Compress(output.ToArray());

        return Convert.ToBase64String(compressed);
    }

}