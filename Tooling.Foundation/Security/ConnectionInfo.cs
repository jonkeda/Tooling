using System.Data.SqlClient;

namespace Foundations.Security
{
    public class ConnectionInfo : IImpersonationInfo
    {
        public ConnectionInfo()
        {
        }

        public ConnectionInfo(string server, string database, string userName, string domain, string password, LogonType logonType)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                IntegratedSecurity = true,
                DataSource = server
            };

            if (string.IsNullOrEmpty(Database))
            {
                builder.InitialCatalog = database;
            }

            Server = server;
            Database = database;

            ConnectionString = builder.ConnectionString;
            Domain = domain;
            UserName = userName;
            Password = password;
            LogonType = logonType;
        }

        public string Server { get; set; }

        public string Database { get; set; }

        public string ConnectionString { get; set; }

        public string Domain { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public LogonType LogonType { get; set; }
    }
}