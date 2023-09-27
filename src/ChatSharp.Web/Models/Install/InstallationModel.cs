using System.ComponentModel.DataAnnotations;

namespace ChatSharp.Web.Models.Install
{
    public partial class InstallationModel
    {
        [DataType(DataType.EmailAddress)]
        public string AdminEmail { get; set; }

        [DataType(DataType.Password)]
        public string AdminPassword { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string DataProvider { get; set; } // mysql

        public bool CreateDatabase { get; set; } = true;

        public string DbServer { get; set; }

        public string DbName { get; set; }

        public string DbUserId { get; set; }

        [DataType(DataType.Password)]
        public string DbPassword { get; set; }

        public bool UseRawConnectionString { get; set; }

        public string DbRawConnectionString { get; set; }

        public string DbAuthType { get; set; } = "sqlserver"; // sqlserver

        public bool IsInstalled { get; set; }
    }
}
