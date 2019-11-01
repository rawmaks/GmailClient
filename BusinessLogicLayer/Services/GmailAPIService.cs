using BusinessLogicLayer.DataTransferObjects;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class GmailAPIService
    {
        public static GmailAPIService Instance { get; } = new GmailAPIService();

        private GmailService _service;

        public GmailAPIService()
        {
            _service = GetGmailService();
        }


        public List<Message> GetMessages(string[] labels = null, string query = null, string userId = "me")
        {
            return GetMessages(_service, labels, query, userId);
        }

        public Message GetMessageByID(string messageId, string userId = "me")
        {
            return GetMessageByID(_service, messageId, userId);
        }

        public async Task<List<Message>> GetMessagesAsync(string[] labels = null, string query = null, string userId = "me")
        {
            return await GetMessagesAsync(_service, labels, query, userId);
        }

        public async Task<Message> GetMessageByIDAsync(string messageId, string userId = "me")
        {
            return await GetMessageByIDAsync(_service, messageId, userId);
        }




        private string[] scopes = { GmailService.Scope.GmailReadonly, GmailService.Scope.GmailSend };
        private string applicationName = "RawmaksGmailClient";
        private string credentialsPath = "credentials.json";
        private string tokenPath = "token.json";

        private GmailService GetGmailService()
        {
            UserCredential credential;

            // TODO: Path !!!
            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                //ClientSecrets secrets = new ClientSecrets();
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(tokenPath, true)).Result;
            }

            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });

            return service;
        }

        private List<Message> GetMessages(GmailService service, string[] labels = null, string query = null, string userId = "me")
        {
            List<Message> result = new List<Message>();
            UsersResource.MessagesResource.ListRequest request = service.Users.Messages.List(userId);
            if (labels != null) request.LabelIds = labels;
            if (!string.IsNullOrWhiteSpace(query)) request.Q = query;

            do
            {
                try
                {
                    ListMessagesResponse response = request.Execute();
                    result.AddRange(response.Messages);
                    request.PageToken = response.NextPageToken;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                }
            } while (!string.IsNullOrEmpty(request.PageToken));

            return result;
        }

        private Message GetMessageByID(GmailService service, string messageId, string userId = "me")
        {
            try
            {
                return service.Users.Messages.Get(userId, messageId).Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }

            return null;
        }

        private async Task<List<Message>> GetMessagesAsync(GmailService service, string[] labels = null, string query = null, string userId = "me")
        {
            List<Message> result = new List<Message>();
            UsersResource.MessagesResource.ListRequest request = service.Users.Messages.List(userId);
            if (labels != null) request.LabelIds = labels;
            if (!string.IsNullOrWhiteSpace(query)) request.Q = query;

            do
            {
                try
                {
                    ListMessagesResponse response = await request.ExecuteAsync();
                    result.AddRange(response.Messages);
                    request.PageToken = response.NextPageToken;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                }
            } while (!string.IsNullOrEmpty(request.PageToken));

            return result;
        }

        private async Task<Message> GetMessageByIDAsync(GmailService service, string messageId, string userId = "me")
        {
            try
            {
                return await service.Users.Messages.Get(userId, messageId).ExecuteAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }

            return null;
        }
    }
}
