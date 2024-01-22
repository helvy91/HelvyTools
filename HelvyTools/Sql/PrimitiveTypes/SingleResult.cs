using HelvyTools.Sql.Attributes;

namespace HelvyTools.PrimitiveTypes
{
    public class SingleResult<T>
    {
        [MySqlPropertyName("Result")]
        public T Value { get; set; }
    }
}
