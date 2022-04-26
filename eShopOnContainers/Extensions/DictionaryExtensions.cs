namespace eShopOnContainers.Extensions
{
    public static class DictionaryExtensions
    {
        public static (bool ContainsKeyAndValue, bool Value) GetValueAsBool(this IDictionary<string, object> dictionary, string key)
        {
            return dictionary.ContainsKey(key)
                ? (true, dictionary[key] is bool ? (bool)dictionary[key] : default(bool))
                : (false, default);
        }

        public static (bool ContainsKeyAndValue, int Value) GetValueAsInt(this IDictionary<string, object> dictionary, string key)
        {
            return dictionary.ContainsKey(key)
                ? (true, dictionary[key] is int ? (int)dictionary[key] : default(int))
                : (false, default);
        }
    }
}