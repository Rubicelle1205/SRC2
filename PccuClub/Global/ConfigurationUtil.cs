namespace WebPccuClub.Global
{
    public class ConfigurationUtil
    {
        private readonly IConfiguration _config;

        public ConfigurationUtil()
        {
            var builder = new ConfigurationBuilder().AddJsonFile(@"appsettings.json");
            _config = builder.Build();
        }

        /// <summary>
        /// 取得ConnectionString
        /// </summary>
        public string getDefaultDatabase()
        {
            return _config.GetConnectionString("DefaultDatabase");
        }

        /// <summary>
        /// 系統網址
        /// </summary>
        public bool isDebug()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}
