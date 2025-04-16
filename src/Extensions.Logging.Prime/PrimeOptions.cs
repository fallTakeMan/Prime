using Extensions.Logging.Prime.Database.Configuration;

namespace Extensions.Logging.Prime
{
    public class PrimeOptions
    {
        public PrimeOptions()
        {
            RouteMatch = "/prime";
        }

        public string RouteMatch { get; set; }

        /// <summary>
        /// 忽略记录日志的路由
        /// <para>多个路由用逗号分隔</para>
        /// </summary>
        /// <remarks>示例：/swagger</remarks>
        public string HttpLogIgnoreRouteMatch { get; set; } = string.Empty;

        public string ConnectionString { get; set; } = string.Empty;

        public DatabaseType DbProvider { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string PrimeUserName { get; set; } = "prime";
        /// <summary>
        /// 密码
        /// </summary>
        public string PrimePassword { get; set; } = "prime";
    }
}
