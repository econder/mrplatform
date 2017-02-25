using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;

namespace MRPlatform.Message
{
    [Guid("FF4433E8-41EF-4D00-8C40-C349FD6FDE3C")]
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