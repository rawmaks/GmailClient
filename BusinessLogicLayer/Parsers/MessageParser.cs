using BusinessLogicLayer.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Parsers
{
    public class MessageParser
    {
        // Singleton
        public static MessageParser Instance { get; } = new MessageParser();

        public ParticipantMessageDTO ParseToParticipant(string body)
        {
            ParticipantMessageDTO participant = new ParticipantMessageDTO();

            participant.CreateDate = DateTime.Now;
            participant.FirstName = "Максим";

            return participant;
        }
    }
}
