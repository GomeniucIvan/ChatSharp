using ChatSharp.Core.Data;
using ChatSharp.Core.Platform.Messaging.Domain;
using ChatSharp.Core.Platform.Messaging.Dto;

namespace ChatSharp.Core.Platform.Messaging.Proc
{
    public static class SessionMessageStoredProcedures
    {
        public static int SessionMessage_Insert(this ChatSharpDbContext db,
            SessionMessageDto sessionMessage)
        {
            var sessionIdParam = sessionMessage.SessionId.ToSqlParameter("SessionId");
            var isMineParam = sessionMessage.IsMine.ToSqlParameter("IsMine");
            var messageParam = sessionMessage.Message.ToSqlParameter("Message");

            var sessionMessageId = db.ExecStoreProcedure<int>($"{nameof(SessionMessage)}_Insert",
                sessionIdParam,
                isMineParam,
                messageParam).FirstOrDefault();

            return sessionMessageId;
        }

        public static IList<SessionMessageDto> SessionMessage_GetList(this ChatSharpDbContext db,
            int sessionId)
        {
            var sessionIdParam = sessionId.ToSqlParameter("SessionId");

            return db.ExecStoreProcedure<SessionMessageDto>($"{nameof(SessionMessage)}_GetList",
                sessionIdParam).ToList();
        }
    }
}
