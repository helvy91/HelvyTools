using HelvyTools.Sql.Attributes;

namespace HelvyTools.Configuration.Providers.Data.Sql
{
    public class DbConfigValue
    {
        [MySqlPropertyName("id")]
        public int Id { get; set; }

        [MySqlPropertyName("key")]
        public string Key { get; set; }

        [MySqlPropertyName("value")]
        public string Value { get; set; }

        [MySqlPropertyName("type")]
        public string Type { get; set; }

        [MySqlPropertyName("name")]
        public string Name { get; set; }
    }
}
