using Newtonsoft.Json;
using System.Collections.Generic;
using MSExtension.CommandsGen;

namespace MSExtension.Models.Utils
{
    public class TranslatedMessage
    {
        public ResponseData responseData { get; set; }
        public bool quotaFinished { get; set; }
        public object mtLangSupported { get; set; }
        public string responseDetails { get; set; }
        public int responseStatus { get; set; }
        public object responderId { get; set; }
        public object exception_code { get; set; }
        public List<Match> matches { get; set; }
    }
    public class Match
    {
        public string id { get; set; }
        public string segment { get; set; }
        public string translation { get; set; }
        public string source { get; set; }
        public string target { get; set; }
        public string quality { get; set; }
        public object reference { get; set; }

        [JsonProperty("usage-count")]
        public int usagecount { get; set; }
        public string subject { get; set; }

        [JsonProperty("created-by")]
        public string createdby { get; set; }

        [JsonProperty("last-updated-by")]
        public string lastupdatedby { get; set; }

        [JsonProperty("create-date")]
        public string createdate { get; set; }

        [JsonProperty("last-update-date")]
        public string lastupdatedate { get; set; }
        public double match { get; set; }
    }

    public class ResponseData
    {
        public string translatedText { get; set; }
        public double match { get; set; }
    }
}