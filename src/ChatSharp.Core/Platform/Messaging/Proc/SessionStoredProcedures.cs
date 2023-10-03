using ChatSharp.Core.Data;
using ChatSharp.Core.Platform.Messaging.Domain;
using ChatSharp.Core.Platform.Messaging.Dto;
using Microsoft.Data.SqlClient;

namespace ChatSharp.Core.Platform.Messaging.Proc
{
    public static class SessionStoredProcedures
    {
        public static IList<SessionDto> Session_GetList(this ChatSharpDbContext db,
            SessionDtoFilter filter)
        {
            var guidParam = filter.Guid.ToSqlParameter("Guid");
            var modelNameParam = filter.ModelName.ToSqlParameter("ModelName");

            var sessions = db.ExecStoreProcedure<SessionDto>($"{nameof(Session)}_GetList",
                guidParam,
                modelNameParam).ToList();

            return sessions;
        }

        public static SessionDto Session_GetByFilter(this ChatSharpDbContext db,
            SessionDtoFilter filter)
        {
            var sessions = db.Session_GetList(filter);
            return sessions.FirstOrDefault();
        }

        public static int Session_Insert(this ChatSharpDbContext db,
            Session session)
        {
            var guidParam = session.Guid.ToSqlParameter("Guid");
            var nameParam = session.Name.Replace("\n","").ToSqlParameter("Name");
            var autoDeleteAfterXDaysParam = session.AutoDeleteAfterXDays.ToSqlParameter("AutoDeleteAfterXDays");
            var modelNameParam = session.ModelName.ToSqlParameter("ModelName");

            var sessionId = db.ExecStoreProcedure<int>($"{nameof(Session)}_Insert",
                guidParam,
                nameParam,
                autoDeleteAfterXDaysParam,
                modelNameParam).FirstOrDefault();

            return sessionId;
        }
    }
}
