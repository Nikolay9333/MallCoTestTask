using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using RestSharp;
using TestTaskMoloko.Interfaces;
using TestTaskMoloko.Models;
using TestTaskMoloko.Extensions;

namespace TestTaskMoloko.Helpers
{
    public class SmsSender : ISmsSender
    {
        #region Fields

        private IConfiguration Configuration { get; set; }

        #endregion

        #region Constructors

        public SmsSender(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        #endregion

        #region Public Methods

        public string TrySendMessages(List<Message> messages)
        {
            var smsCip = Configuration["SMSCip"];
            var smsCipPort = int.Parse(Configuration["SMSCPort"]);
            var package = new Package()
            {
                Login = Configuration["SMSCLogin"],
                Password = Configuration["SMSCPassword"],
                Messages = messages
            };

            var client = new RestClient($"http://{smsCip}:{smsCipPort}");
            var request = new RestRequest(Configuration["SMSCMethodUrl"], Method.POST);
            var body = package.Serialize();

            var d = XmlHelper.Deserialize<Package>(body);

            request.AddParameter("text/xml", body, ParameterType.RequestBody);
            var response = client.Execute(request);

            return response.Content;
        }

        #endregion
    }
}