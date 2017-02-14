using System;
using System.Data;
using System.Runtime.InteropServices;

namespace MRPlatform.Message
{
    [Guid("FF4433E8-41EF-4D00-8C40-C349FD6FDE3C")]
    public interface IUserMessage
    {
        void Send(string sender, string recipient, string message, int priority = 2);
        void Send(string sender, Array recipients, string message, int priority = 2);
        DataSet GetMessages(string sender);
        DataSet GetMessages(string sender, int priority);
        DataSet GetMessages(string sender, int priority, bool unread);
        DataSet GetMessages(string sender, int priority, bool unread, bool archived);
        void MarkAsUnread(string hmiUserName, int msgId);
        void MarkAsRead(string hmiUserName, int msgId);
        void Archive(string hmiUserName, int msgId);
        void UnArchive(string hmiUserName, int msgId);
    }


    [Guid("48677FF9-D71A-45ED-915B-CF86D2F22DC5")]
    public interface IUserMessageEvents
    {

    }
}