using System;

namespace ArmorGamesDownloader.Extensions
{
    public static class EnumExtension
    {
        // TODO Метод использует рефлексию, что медленно, но допустимо для редких обращений
        public static String GetStringValue(this Enum value)
        {
            string output = null;
            var attrs = value
                .GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(StringValue), false) as StringValue[];

            if (attrs != null && attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }

        public class StringValue : Attribute
        {
            private string _value;

            public StringValue(string value)
            {
                _value = value;
            }

            public string Value
            {
                get { return _value; }
            }
        }
    }
}