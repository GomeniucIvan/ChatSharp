using ChatSharp.Core.Data;
using ChatSharp.Core.Platform.Configuration.Dto;
using ChatSharp.Core.Platform.Confirguration.Domain;

namespace ChatSharp.Core.Platform.Configuration.Proc
{
    public static class SettingStoredProcedures
    {
        public static IList<SettingDto> Settings_GetList(this ChatSharpDbContext db)
        {
            return db.ExecStoreProcedure<SettingDto>($"{nameof(Setting)}_GetList").ToList();
        }
    }
}
