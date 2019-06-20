using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RSChatter.Models;
using RestSharp;
using Newtonsoft.Json;
using RSChatter.Models.QnAMaker;

namespace RSChatter.SignalR.Hubs
{
    public class ChatHub: Hub
    {
        public static List<Adviser> SignalRClients = new List<Adviser>();
        private int _answerThresholdMax = 100;
        private int _answerThresholdMMin = 90;
        private int _minThreshold = 50;

        public void Send(string name, string message)
        {
            var id = Context.ConnectionId;

            if (SignalRClients.Count(x => x.ConnectionId == id) == 0)
            {
                SignalRClients.Add(new Adviser { ConnectionId = id, Name = name });
            }

            Clients.All.addNewMessageToPage(name, message);

            var answear = GetAnswearFromBot(message);
            int type = 0;
            answear.Metadata.FirstOrDefault(t => Int32.TryParse(t.Value, out type));
            if (answear != null)
            {
                if (answear.Score >= _answerThresholdMMin && answear.Score <= _answerThresholdMax)
                {
                    Clients.All.addNewMessageToPage("Bot", $"{(AdvisorType)type} | {answear.Score} | {answear.Answer}");
                } 
                else if (answear.Score > _minThreshold)
                {
                    Clients.All.addNewMessageToPage("Bot", $"{(AdvisorType)type} | {answear.Score} | {answear.Answer}");
                }

                Clients.All.addNewMessageToPage("Bot", $"{(AdvisorType)type} | {answear.Score} | {answear.Answer}");
            }


        }

        public Answers GetAnswearFromBot(string message)
        {
            var client = new RestClient("https://rschatter.azurewebsites.net/qnamaker");
            var request = new RestRequest("/knowledgebases/c215c7ca-cc53-488c-a131-1de6bc06d62c/generateAnswer", Method.POST);
            request.AddHeader("authorization", "EndpointKey 83f79251-cd6e-4481-ba64-eb5229a60413");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\"question\": \"" + message + "\"}", ParameterType.RequestBody);
            var response = client.Execute(request);

            var result = JsonConvert.DeserializeObject<QnAMakerModel>(response.Content);

            return result.Answers.OrderByDescending(t => t.Score).FirstOrDefault();
        }


        public List<Adviser> GetAllActiveConnections()
        {
            return SignalRClients.ToList();
        }
    }
}