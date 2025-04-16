using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.Logging.Prime.Database.Configuration
{
    public sealed record PrimeOptionsConfig
    {
        public static string RouteMatch { get; set; } = string.Empty;
        public static string HttpLogIgnoreRouteMatch { get; set; } = string.Empty;

        public static string UserName { get; set; } = string.Empty;
        public static string Password { get; set; } = string.Empty;

        public static string ConnectionString { get; set; } = string.Empty;

        public static DatabaseType DbProvider { get; set; }
    }

    public enum DatabaseType
    {
        SqlServer,
        MySql,
        PostgreSQL
    }
}