﻿using Microsoft.AspNet.SignalR;
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

        public void Send(string name, string message)
        {
            var id = Context.ConnectionId;

            if (SignalRClients.Count(x => x.ConnectionId == id) == 0)
            {
                SignalRClients.Add(new Adviser { ConnectionId = id, Name = name });
            }

            Clients.All.addNewMessageToPage(name, message);

            var client = new RestClient("https://rschatbot.azurewebsites.net/qnamaker");
            var request = new RestRequest("/knowledgebases/900dd98e-4901-4140-9f0b-a2c7cb21b53f/generateAnswer", Method.POST);
            request.AddHeader("authorization", "EndpointKey 91522118-9724-4391-82d9-c7cb11f2f08c");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\"question\": \"" + message + "\"}", ParameterType.RequestBody);
            var response = client.Execute(request);

            var result = JsonConvert.DeserializeObject<QnAMakerModel>(response.Content);

            var answear = result.Answers.OrderByDescending(t => t.Score).FirstOrDefault();
            if (answear != null)
            {
                Clients.All.addNewMessageToPage("Bot", answear.Answer);
            }


        }

        public List<Adviser> GetAllActiveConnections()
        {
            return SignalRClients.ToList();
        }
    }
}