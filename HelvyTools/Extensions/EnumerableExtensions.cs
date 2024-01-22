namespace HelvyTools.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsSupersetOf<T>(this IEnumerable<T> superset, IEnumerable<T> subset)
        {
            int count = 0;
            foreach (T superItem in superset)
            {
                foreach (T subItem in subset)
                {
                    
                    if (subItem.Equals(superItem))
                    {
                        count++;
                    }
                }
            }

            return count == subset.Count();
        }
    }
}
