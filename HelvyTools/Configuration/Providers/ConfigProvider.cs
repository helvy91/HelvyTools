namespace HelvyTools.Configuration.Providers
{
    public abstract class ConfigProvider
    {
        public T GetOrFail<T>(string key)
        {
            string value = GetValue(key);
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Provided configuration key does not have a value: " + key);
            }

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                throw new ArgumentException("Couldn't convert from string to " + typeof(T).Name + ", configuration build failed for key: " + key + ".");
            }
        }

        protected abstract string GetValue(string key);
    }
}
