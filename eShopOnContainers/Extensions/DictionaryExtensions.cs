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

        public static void ValueAsBool(this IDictionary<string, object> dictionary, string key, ref bool? value)
        {
            if ( dictionary.ContainsKey(key) && dictionary[key] is bool dictValue)
            {
                value = dictValue;
            }
        }

        public static (bool ContainsKeyAndValue, int Value) GetValueAsInt(this IDictionary<string, object> dictionary, string key)
        {
            return dictionary.ContainsKey(key)
                ? (true, dictionary[key] is int ? (int)dictionary[key] : default(int))
                : (false, default);
        }

        public static void ValueAsInt(this IDictionary<string, object> dictionary, string key, ref int? value)
        {
            if (dictionary.ContainsKey(key) && dictionary[key] is int intValue)
            {
                value = intValue;
            }
        }
    }
}