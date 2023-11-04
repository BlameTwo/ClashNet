using System.Text.Json.Serialization;

namespace Clash.CoreNet.Models.Clash;

internal class ClashCoreUpdateData
{
    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("assets_url")]
    public string AssetsUrl { get; set; }


    [JsonPropertyName("upload_url")]
    public string UpLoadUrl { get; set; }

    [JsonPropertyName("html_url")]
    public string HtmlUrl { get; set; }

    [JsonPropertyName("id")]
    public int ID { get; set; }

    [JsonPropertyName("tag_name")]
    public string TagName { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedTime { get; set; }

    [JsonPropertyName("published_at")]
    public DateTime PublishTime { get; set; }

    [JsonPropertyName("assets")]
    public List<ClashCoreAssets> Assets { get; set; }
}


public class ClashCoreAssets
{
    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("download_count")]
    public int DownloadCount { get; set; }

    [JsonPropertyName("browser_download_url")]
    public string DownLoadUrl { get; set; }
}