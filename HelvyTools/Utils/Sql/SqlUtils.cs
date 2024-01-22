using System.Transactions;

namespace HelvyTools.Utils.Sql
{
    public static class SqlUtils
    {
        public static async Task ExecuteInTransaction(Func<Task> func)
        {
            using (var scope = new TransactionScope())
            {
                await func();
            }
        }

        public static string GetInClauseQuery(params int[] values)
        {
            return GetInClauseQuery(values.Select(x => x.ToString()).ToArray());
        }

        public static string GetInClauseQuery(params string[] values)
        {
            var items = new List<string>();
            for (int i = 0; i < values.Length; i++)
            {
                items.Add($"'{values[i]}'");
            }

            return string.Join(',', items);
        }

        public static string GetMySqlDateTime(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd hh:mm:ss");
        }
    }
}
