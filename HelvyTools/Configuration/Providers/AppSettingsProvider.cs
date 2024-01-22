using Microsoft.Extensions.Configuration;

namespace HelvyTools.Configuration.Providers
{
    public class AppSettingsProvider : ConfigProvider
    {
        protected readonly IConfiguration _config;

        public AppSettingsProvider(IConfiguration config)
        {
            _config = config;
        }

        protected override string GetValue(string key)
            => _config[key];
    }
}
