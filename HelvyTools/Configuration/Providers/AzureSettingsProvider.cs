namespace HelvyTools.Configuration.Providers
{
    public class AzureSettingsProvider : ConfigProvider
    {
        protected override string GetValue(string key)
            => Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process);
    }
}
