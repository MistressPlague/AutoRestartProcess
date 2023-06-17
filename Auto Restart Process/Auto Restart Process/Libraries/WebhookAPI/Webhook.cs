#if !AVTest
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace DiscordWebhook
{
    [JsonObject]
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = true)]
    public class Webhook
    {
        private readonly HttpClient _httpClient;
        private readonly string _webhookUrl;

        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
        // ReSharper disable once InconsistentNaming
        [JsonProperty("tts")]
        public bool IsTTS { get; set; }
        [JsonProperty("embeds")]
        public List<Embed> Embeds { get; set; } = new();

        public Webhook(string webhookUrl)
        {
            _httpClient = new HttpClient();
            _webhookUrl = webhookUrl;
        }

        public Webhook(ulong id, string token) : this($"https://discord.com/api/webhooks/{id}/{token}")
        {

        }

        public void Send()
        {
            var content = new StringContent(JsonConvert.SerializeObject(this), Encoding.UTF8, "application/json");
            _httpClient.PostAsync(_webhookUrl, content);
        }

        // ReSharper disable once InconsistentNaming
        public void Send(string content, string username = null, string avatarUrl = null, bool isTTS = false, IEnumerable<Embed> embeds = null)
        {
            Content = content;
            Username = username;
            AvatarUrl = avatarUrl;
            IsTTS = isTTS;

            Embeds.Clear();

            if (embeds != null)
            {
                Embeds.AddRange(embeds);
            }

            Send();
        }
    }
}
#endif