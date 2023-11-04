using System.Text.Json.Serialization;

namespace ClashNet.Models;

public class AppBackground
{
    [JsonPropertyName("Opacity")]
    public double Opacity { get; set; } = 0.3;

    [JsonPropertyName("Gaussian")]
    public double Gaussian { get; set; } = 0;

    [JsonPropertyName("Image_Path")]
    public string? ImagePath { get; set; }
}

public record AppAbout(string TitleName,string SubName,string About,string Url);
