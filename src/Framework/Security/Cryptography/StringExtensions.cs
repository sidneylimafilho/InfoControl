using System;
using System.Collections.Generic;
using System.Text;
using InfoControl.Security.Cryptography;

namespace InfoControl
{
    public static partial class StringExtensions
    {
        #region Decrypt
        public static string DecryptFromBase64(this string identifier)
        {
            return Decrypt(identifier, Algorithm.DES, EncodingType.Base64);
        }

        public static string DecryptFromHex(this string identifier)
        {
            return Decrypt(identifier, Algorithm.DES, EncodingType.BinHex);
        }

        public static string Decrypt(this string identifier)
        {
            return Decrypt(identifier, Algorithm.DES);
        }

        public static string Decrypt(this string identifier, Algorithm algorithm)
        {
            return Decrypt(identifier, algorithm, null);
        }

        public static string Decrypt(this string identifier, string key)
        {
            return Decrypt(identifier, Algorithm.DES, key);
        }

        public static string Decrypt(this string identifier, Algorithm algorithm, EncodingType encType)
        {
            return Decrypt(identifier, algorithm, null, encType);
        }

        public static string Decrypt(this string identifier, EncodingType encType)
        {
            return Decrypt(identifier, Algorithm.DES, null, encType);
        }

        public static string Decrypt(this string identifier, Algorithm algorithm, string key)
        {
            return Decrypt(identifier, algorithm, key, EncodingType.Base64);
        }

        public static string Decrypt(this string identifier, Algorithm algorithm, string key, EncodingType encType)
        {
            Cypher cypher = new Cypher();
            cypher.EncryptionAlgorithm = algorithm;
            cypher.Encoding = encType;

            if (key != null)
                cypher.Key = key;

            return cypher.Decrypt(identifier);
        }
        #endregion

        #region Encrypt
        public static string EncryptToBase64(this string identifier)
        {
            return Encrypt(identifier, Algorithm.DES, EncodingType.Base64);
        }

        public static string EncryptToHex(this string identifier)
        {
            return Encrypt(identifier, Algorithm.DES, EncodingType.BinHex);
        }

        public static string Encrypt(this string identifier)
        {
            return Encrypt(identifier, Algorithm.DES);
        }

        public static string Encrypt(this string identifier, Algorithm algorithm)
        {
            return Encrypt(identifier, algorithm, null);
        }

        public static string Encrypt(this string identifier, string key)
        {
            return Encrypt(identifier, Algorithm.DES, key);
        }

        public static string Encrypt(this string identifier, Algorithm algorithm, EncodingType encType)
        {
            return Encrypt(identifier, algorithm, null, encType);
        }

        public static string Encrypt(this string identifier, Algorithm algorithm, string key)
        {
            return Encrypt(identifier, algorithm, key, EncodingType.Base64);
        }

        public static string Encrypt(this string identifier, EncodingType encType)
        {
            return Encrypt(identifier, Algorithm.DES, null, encType);
        }

        public static string Encrypt(this string identifier, Algorithm algorithm, string key, EncodingType encType)
        {
            Cypher cypher = new Cypher();

            cypher.EncryptionAlgorithm = algorithm;

            if (key != null)
                cypher.Key = key;


            cypher.Encoding = encType;

            return cypher.Encrypt(identifier);
        }
        #endregion

        #region Encrypt Object
        public static string EncryptToBase64(this Object identifier)
        {
            return EncryptToBase64(Convert.ToString(identifier));
        }

        public static string EncryptToHex(this Object identifier)
        {
            return EncryptToHex(Convert.ToString(identifier));
        }

        public static string Encrypt(this Object identifier)
        {
            return Encrypt(Convert.ToString(identifier));
        }

        public static string Encrypt(this Object identifier, Algorithm algorithm)
        {
            return Encrypt(Convert.ToString(identifier), algorithm);
        }

        public static string Encrypt(this Object identifier, string key)
        {
            return Encrypt(Convert.ToString(identifier), key);
        }

        public static string Encrypt(this Object identifier, Algorithm algorithm, EncodingType encType)
        {
            return Encrypt(Convert.ToString(identifier), algorithm, encType);
        }

        public static string Encrypt(this Object identifier, Algorithm algorithm, string key)
        {
            return Encrypt(Convert.ToString(identifier), algorithm, key);
        }

        public static string Encrypt(this Object identifier, EncodingType encType)
        {
            return Encrypt(Convert.ToString(identifier), encType);
        }

        public static string Encrypt(this Object identifier, Algorithm algorithm, string key, EncodingType encType)
        {
            return Encrypt(Convert.ToString(identifier), algorithm, key, encType);
        }
        #endregion

    }
}
