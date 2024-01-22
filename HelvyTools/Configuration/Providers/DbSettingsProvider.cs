using HelvyTools.Configuration.Providers.Data.Sql;

namespace HelvyTools.Configuration.Providers
{
    public abstract class DbSettingsProvider : ConfigProvider
    {
        private readonly Sql.MySqlConnector _sqlConnector;

        protected DbSettingsProvider(Sql.MySqlConnector sqlConnector)
        {
            _sqlConnector = sqlConnector;
        }

        protected override string GetValue(string key)
        {
            var query = GetConfigQuery(key);
            var dbConfigValues = _sqlConnector.ExecuteCommandAsync<DbConfigValue>(query).Result;
            if (!dbConfigValues.Any())
            {
                throw new ArgumentException($"Provided configuration key {key} was not present in database.", nameof(key));
            }

            var value = dbConfigValues[0];
            return value.Value;
        }

        protected abstract string GetConfigQuery(string key);
    }
}
