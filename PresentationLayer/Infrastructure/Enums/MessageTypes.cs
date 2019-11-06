using System;
using System.Collections.Generic;
using System.Text;

namespace PresentationLayer.Infrastructure.Enums
{
    public enum MessageTypes
    {
        All = 1,
        None = 2,
        Inbox = 3,
        InboxNone = 4,
        InboxParticipant = 5,
        InboxSpeaker = 6,
        InboxListener = 7
    }
}
