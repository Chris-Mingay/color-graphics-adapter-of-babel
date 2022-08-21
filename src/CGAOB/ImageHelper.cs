using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace CGAOB;

public static class ImageHelper
{
    
    public static GenerateImageResponse GenerateImage(byte[] bytes, Color[] palette, int imageWidth = 160, int imageHeight = 200)
    {

        using (MemoryStream outStream = new MemoryStream())
        {
            var image = new Image<Rgba32>(imageWidth, imageHeight);
            var brushes = palette.Select(x => Brushes.Solid(x)).ToArray();

            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 0; y < imageHeight; y++)
                {
                    var pixelIndex = (y * imageWidth) + x;
                    var brushIndex = bytes[pixelIndex];
                    var brush = brushes[brushIndex];
                    var rectangle = new Rectangle(x, y, 1, 1);
            
                    image.Mutate(img => img.Fill(brush, rectangle));
                }
            }

            image.SaveAsPng(outStream);

            var response =
                new GenerateImageResponse("data:image/png;base64, " + Convert.ToBase64String(outStream.ToArray()),
                    CompressionHelper.CompressImageData(bytes));
            return response;
        }
        
    }
}