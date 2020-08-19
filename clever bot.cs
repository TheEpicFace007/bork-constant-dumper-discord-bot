using System;
using System.IO;
using RestSharp;
using Newtonsoft.Json;

namespace quagmire_bot_9000
{
    // an answer for the cleverbot
    public class CleverAnswer
    {
        public bool sucess { get; set; }
        public string session { get; set; }
        public string response { get; set; }
        public double confidence { get; set; }
    }

    public class CleverMessage
    {
        public string query { get; }

        public CleverMessage(string messageQuery)
        {
            this.query = messageQuery ?? throw new ArgumentNullException(nameof(messageQuery));
        }
    }

    public static class CleverBot
    {
        const string ApiKey =
            "b1060a52b07c933b192298aede18a4e079e2cb3be98c2bd6fcd30e77c4c80cb9822937927db1797ccc9fdc6804e85777";

        public static CleverAnswer AskClever(string message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            var client = new RestClient("https://chatengine.xyz/api/ask");
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", ApiKey);
            request.AddJsonBody(JsonConvert.SerializeObject(new CleverMessage(message)));
            var jsonSerializer = new JsonSerializer();
            var response = client.Execute(request);
            var deserializedResponse = JsonConvert.DeserializeObject<CleverAnswer>(response.Content);
            return deserializedResponse;
        }
    }
}