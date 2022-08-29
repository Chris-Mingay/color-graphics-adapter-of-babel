namespace CGAOB;

public class GenerateImageResponse
{
    public string Base64Image { get; }

    public GenerateImageResponse(string base64Image)
    {
        Base64Image = base64Image;
    }
}