using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using Microsoft.Extensions.Logging;

namespace DataAccessLayer.DBContext
{
    /// <summary>
    /// Не используется
    /// </summary>
    public class MessageContextSeed
    {
        public static async Task SeedAsync(MessageContext messageContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            try
            {
                if (!messageContext.MessageTypes.Any())
                {
                    messageContext.MessageTypes.AddRange(GetMessageTypes());
                    await messageContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<MessageContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(messageContext, loggerFactory, retryForAvailability);
                }
            }
        }

        static IEnumerable<MessageType> GetMessageTypes()
        {
            return new List<MessageType>()
            {
                
            };
        }
    }
}
