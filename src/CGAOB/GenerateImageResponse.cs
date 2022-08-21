namespace CGAOB;

public class GenerateImageResponse
{
    public string Base64Image { get; }
    public string Address { get; }

    public GenerateImageResponse(string base64Image, string address)
    {
        Base64Image = base64Image;
        Address = address;
    }
}