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




        private static string[] Scopes = { GmailService.Scope.GmailReadonly };
        private static string ApplicationName = "Gmail API .NET Quickstart";

        private static GmailService GetGmailService()
        {
            UserCredential credential;

            // TODO: Path !!!
            using (var stream = new FileStream(@"C:\credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Gmail API service.
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }

        // RefreshTOken

        private static List<Message> GetMessages(GmailService service, string[] labels = null, string query = null, string userId = "me")
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

        private static Message GetMessageByID(GmailService service, string messageId, string userId = "me")
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
    }
}
