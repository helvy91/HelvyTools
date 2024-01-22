using MySqlConnector;
using System.Reflection;
using HelvyTools.Sql.Attributes;
using HelvyTools.Utils.Attributes;

namespace HelvyTools.Sql
{
    public class MySqlConnector
    {
        private readonly string _connectionString;

        public MySqlConnector(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task ExecuteCommandAsync(string command)
        {
            using (var conn = GetConnection())
            {
                await conn.OpenAsync();
                using (var sqlCommand = GetCommand(command, conn))
                {
                    await sqlCommand.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<T>> ExecuteCommandAsync<T>(string command)
            where T : new()
        {
            var result = new List<T>();
            using (var conn = GetConnection())
            {
                await conn.OpenAsync();
                using (var sqlCommand = GetCommand(command, conn))
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    var props = AttributeReader.ReadPropertyNames<MySqlPropertyName,T>();
                    if (reader.FieldCount > props.Count)
                    {
                        throw new ArgumentException($"Provided class: {typeof(T).Name} has less properties than SQL columns in row.");
                    }

                    while (await reader.ReadAsync())
                    {
                        result.Add(ConvertData<T>(reader, props));
                    }
                }
            }
            
            return result;
        }

        private T ConvertData<T>(MySqlDataReader reader, Dictionary<string, PropertyInfo> props)
            where T : new()
        {
            var obj = new T();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var name = reader.GetName(i);
                if (!props.TryGetValue(name, out var prop))
                {
                    throw new ArgumentException($"Provided class {typeof(T).Name} doesn't have corresponding property name for column {name}");
                }

                var value = reader.GetValue(i);
                if (!(prop.PropertyType.IsAssignableFrom(value.GetType()) || prop.PropertyType.IsAssignableTo(value.GetType())))
                {
                    throw new ArgumentException($"Provided class {typeof(T).Name} property type {prop.PropertyType.Name} " +
                                                 $"is different than column {name} type {value.GetType().Name}");
                }

                prop.SetValue(obj, value);
            }

            return obj;
        }

        private MySqlCommand GetCommand(string command, MySqlConnection conn)
            => new MySqlCommand(command, conn);
        private MySqlConnection GetConnection()
            => new MySqlConnection(_connectionString);
    }
}
