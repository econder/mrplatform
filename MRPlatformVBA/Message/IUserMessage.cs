using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;

namespace MRPlatformVBA.Message
{
    [Guid("22C3035B-0B7D-461D-9FE1-802F762B108F")]
    public interface IUserMessage
    {
        void Send(string sender, string recipient, string message, int priority = 2);

        //void Send(string sender, List<string> recipients, string message, int priority = 2);

        DataSet GetMessages(string recipient);
        DataSet GetMessages(string recipient, int priority);
        DataSet GetUnreadMessages(string recipient);
        DataSet GetUnreadMessages(string recipient, int priority);
        DataSet GetArchivedMessages(string recipient);
        DataSet GetArchivedMessages(string recipient, int priority);
        void MarkAsUnread(string recipient, long msgId);
        void MarkAsRead(string recipient, long msgId);
        void Archive(string recipient, long msgId);
        void UnArchive(string recipient, long msgId);
        void DeleteMessage(long msgId);
        void DeleteArchivedMessage(long msgId);
    }


    [Guid("48677FF9-D71A-45ED-915B-CF86D2F22DC5")]
    public interface IUserMessageEvents
    {

    }
}