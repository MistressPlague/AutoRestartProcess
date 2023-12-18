#if !AVTest
using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace DiscordWebhook
{
    [JsonObject]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = true)]
    public class Embed : IEmbedUrl
    {
        [JsonProperty("title")] public string Title { get; set; }

        [JsonProperty("type")] public string Type { get; set; } = "rich";

        [JsonProperty("description")] public string Description { get; set; }

        [JsonProperty("timestamp")] public DateTimeOffset? TimeStamp { get; set; }

        [JsonProperty("color")] public int Color { get; set; }

        [JsonProperty("footer")] public EmbedFooter Footer { get; set; }

        [JsonProperty("image")] public EmbedImage Image { get; set; }

        [JsonProperty("thumbnail")] public EmbedThumbnail Thumbnail { get; set; }

        [JsonProperty("video")] public EmbedVideo Video { get; set; }

        [JsonProperty("provider")] public EmbedProvider Provider { get; set; }

        [JsonProperty("author")] public EmbedAuthor Author { get; set; }

        [JsonProperty("fields")] public List<EmbedField> Fields { get; set; } = new();

        public string Url { get; set; }
    }

    [JsonObject]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = true)]
    public class EmbedFooter : IEmbedIconUrl, IEmbedIconProxyUrl
    {
        [JsonProperty("text")] public string Text { get; set; }

        public string ProxyIconUrl { get; set; }
        public string IconUrl { get; set; }
    }

    [JsonObject]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = true)]
    public class EmbedImage : EmbedProxyUrl, IEmbedDimension
    {
        public int Height { get; set; }
        public int Width { get; set; }
    }

    [JsonObject]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = true)]
    public class EmbedThumbnail : EmbedProxyUrl, IEmbedDimension
    {
        public int Height { get; set; }
        public int Width { get; set; }
    }

    [JsonObject]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = true)]
    public class EmbedVideo : EmbedUrl, IEmbedDimension
    {
        public int Height { get; set; }
        public int Width { get; set; }
    }

    [JsonObject]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = true)]
    public class EmbedProvider : EmbedUrl
    {
        [JsonProperty("name")] public string Name { get; set; }
    }

    [JsonObject]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = true)]
    public class EmbedAuthor : EmbedUrl, IEmbedIconUrl, IEmbedIconProxyUrl
    {
        [JsonProperty("name")] public string Name { get; set; }

        public string ProxyIconUrl { get; set; }
        public string IconUrl { get; set; }
    }

    [JsonObject]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = true)]
    public class EmbedField
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("value")] public string Value { get; set; }

        [JsonProperty("inline")] public bool Inline { get; set; }
    }

    [JsonObject]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = true)]
    public abstract class EmbedUrl : IEmbedUrl
    {
        public string Url { get; set; }
    }

    [JsonObject]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = true)]
    public abstract class EmbedProxyUrl : EmbedUrl, IEmbedProxyUrl
    {
        public string ProxyUrl { get; set; }
    }

    [JsonObject]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = true)]
    public interface IEmbedUrl
    {
        [JsonProperty("url")] string Url { get; set; }
    }

    [JsonObject]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = true)]
    public interface IEmbedProxyUrl
    {
        [JsonProperty("proxy_url")] string ProxyUrl { get; set; }
    }

    [JsonObject]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = true)]
    public interface IEmbedIconUrl
    {
        [JsonProperty("icon_url")] string IconUrl { get; set; }
    }

    [JsonObject]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = true)]
    public interface IEmbedIconProxyUrl
    {
        [JsonProperty("proxy_icon_url")] string ProxyIconUrl { get; set; }
    }

    [JsonObject]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = true)]
    public interface IEmbedDimension
    {
        [JsonProperty("height")] int Height { get; set; }

        [JsonProperty("width")] int Width { get; set; }
    }
}
#endif