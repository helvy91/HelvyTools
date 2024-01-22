using HelvyTools.Configuration.Providers;

namespace HelvyTools.Configuration
{
    public class Configuration
    {
        protected readonly ConfigProvider _configProvider;

        public Configuration(ConfigProvider configProvider)
        {
            _configProvider = configProvider;
        }

        #region Smtp

        public class SmtpConfiguration : Configuration
        {
            public SmtpConfiguration(ConfigProvider provider) : base(provider) { }

            public string Username
                => GetOrFail<string>("AppSettings:Smtp:Username");
            public string Password
                => GetOrFail<string>("AppSettings:Smtp:Password");
            public string FromAddress
                => GetOrFail<string>("AppSettings:Smtp:FromAddress");
            public string Host
                => GetOrFail<string>("AppSettings:Smtp:Host");
            public int Port
                => GetOrFail<int>("AppSettings:Smtp:Port");
        }

        #endregion

        public T GetOrFail<T>(string key)
            => _configProvider.GetOrFail<T>(key);
    }   
}
