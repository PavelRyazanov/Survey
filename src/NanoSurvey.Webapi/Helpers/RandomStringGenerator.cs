using System;
using System.Security.Cryptography;

namespace NanoSurvey.Webapi.Test
{
    public class RandomStringGenerator
    {
        public const string AllowedChars = "ABCDEFGHIJKLMNOPQRSTasdfghjklzxcvbnmqwertyuiop";
        public static readonly int QuantityAllowedChars = AllowedChars.Length;

        public static string GenerateRandomString(int length)
        {
            if (length <= 0)
                throw new ArgumentException($"{nameof(length)} должно иметь значение больше 0.");

            var result = new char[length];
            for (int i = 0; i < length; i++)
                result[i] = AllowedChars[RNGCryptoServiceProvider.GetInt32(0, QuantityAllowedChars)];

            return new string(result);
        } 
    }
}
