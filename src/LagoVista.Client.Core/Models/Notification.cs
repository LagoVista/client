using System;
using LagoVista.Core;
using Newtonsoft.Json;
using LagoVista.Core.Models;

namespace LagoVista.Client.Core.Models
{
    public class Notification
    {
        public Notification()
        {
            MessageId = Guid.NewGuid().ToId();
            DateStamp = DateTime.Now.ToJSONString();
        }

        public enum NotificationVerbosity
        {
            Diagnostics,
            Normal,
            Errors,
        }

        [JsonProperty("messageId")]
        public String MessageId { get; set; }
        [JsonProperty("dateStamp")]
        public String DateStamp { get; set; }

        [JsonProperty("channel")]
        public EntityHeader Channel { get; set; }

        [JsonProperty("verbosity")]
        public EntityHeader<NotificationVerbosity> Verbosity { get; set; }

        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

        [JsonProperty("title")]
        public String Title { get; set; }

        [JsonProperty("text")]
        public String Text { get; set; }

        [JsonProperty("payloadType")]
        public String PayloadType { get; set; }

        [JsonProperty("payloadJSON")]
        public String Payload { get; set; }
    }
}
