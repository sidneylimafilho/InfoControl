using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.ComponentModel;
using InfoControl.Properties;


namespace InfoControl.Security.Cryptography
{
    public enum AlgorithmKeySize
    {
        Rijndael = 128,
        TripleDES = 192,
        RC2 = 64,
        DES = 64,
        RSA = 2048
    }
    public enum Algorithm
    {
        SHA1,
        SHA256,
        SHA384,
        SHA512,
        Rijndael,
        TripleDES,
        RC2,
        DES,
        MD5,
        RSA
    }
    public enum EncodingType
    {
        BinHex = 0,
        Base64 = 1
    }

    public class Cypher
    {
        #region Members

        //Initialization Vectors that we will use for symmetric encryption/decryption. These
        //byte arrays are completely arbitrary, and you can change them to whatever you like.

        private byte[] _iv8 = new byte[] { 2, 63, 9, 36, 235, 174, 78, 12 };

        private byte[] _iv16 = new byte[] {15, 199, 56, 77, 244, 126, 107, 239, 
                                           9, 10, 88, 72, 24, 202, 31, 108};

        private byte[] _iv24 = new byte[] {37, 28, 19, 44, 25, 170, 122, 25, 
                                           25, 57, 127, 5, 22, 1, 66, 65, 
                                           14, 155, 224, 64, 9, 77, 18, 251};

        private byte[] _iv32 = new byte[] {133, 206, 56, 64, 110, 158, 132, 22, 
                                           99, 190, 35, 129, 101, 49, 204, 248, 
                                           251, 243, 13, 194, 160, 195, 89, 152,
                                           149, 227, 245, 5, 218, 86, 161, 124};

        //Salt value used to encrypt a plain text key. Again, this can be whatever you like
        private byte[] _saltBytes = new byte[] { 162, 27, 98, 1, 28, 239, 64, 30, 156, 102, 223 };


        //Values used for RSA-based asymmetric encryption
        private const int RSA_BLOCKSIZE = 58;
        private const int RSA_DECRYPTBLOCKSIZE = 128;



        //Initialization variables
        private string _key = Resources.Cypher_DefaultKey;

        private Algorithm _algorithm = Algorithm.DES;
        private EncodingType _encodingType = EncodingType.Base64;

        #endregion

        #region Properties
        /// <summary> The key that is used to encrypt and decrypt data
        /// </summary>        
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private string _publicKey;
        /// <summary> The key that is used to encrypt data in RSA Algorithm
        /// </summary>
        public string PublicKey
        {
            get
            {
                if (_publicKeyFile != null && _publicKey == null)
                {
                    _publicKey = File.OpenText(_publicKeyFile).ReadToEnd();
                }

                if (_publicKey == null)
                {
                    return Resources.Cypher_PublicKey;//.Decrypt(Algorithm.Rijndael, EncodingType.Base64);
                }

                return _publicKey;
            }
            set { _publicKey = value; }
        }

        private string _publicKeyFile;
        /// <summary>
        /// 
        /// </summary>
        public string PublicKeyFile
        {
            get { return _publicKeyFile; }
            set { _publicKeyFile = value; }
        }


        private string _privateKeyFile;
        /// <summary>
        /// 
        /// </summary>
        public string PrivateKeyFile
        {
            get { return _privateKeyFile; }
            set { _privateKeyFile = value; }
        }


        private string _privateKey;
        /// <summary> The key that is used to decrypt data in RSA Algorithm
        /// </summary>
        public string PrivateKey
        {
            get
            {
                if (_privateKeyFile != null && _privateKey == null)
                {
                    _privateKey = File.OpenText(_privateKeyFile).ReadToEnd();
                }

                if (_privateKey == null)
                {
                    return Resources.Cypher_PrivateKey;//.Decrypt(Algorithm.Rijndael, EncodingType.Base64);
                }

                return _privateKey;
            }
            set { _privateKey = value; }
        }



        /// <summary> The algorithm that will be used for encryption and decryption
        /// </summary>
        public Algorithm EncryptionAlgorithm
        {
            get { return _algorithm; }
            set { _algorithm = value; }
        }

        /// <summary> The format in which content is returned after encryption, or provided for decryption
        /// </summary>
        public EncodingType Encoding
        {
            get { return _encodingType; }
            set { _encodingType = value; }
        }




        /// <summary> Determines whether the currently specified algorithm is a hash
        /// </summary>
        public bool IsHashAlgorithm
        {
            get
            {
                switch (_algorithm)
                {
                    case Algorithm.MD5:
                        return true;
                    case Algorithm.SHA1:
                        return true;
                    case Algorithm.SHA256:
                        return true;
                    case Algorithm.SHA384:
                        return true;
                    case Algorithm.SHA512:
                        return true;
                    default:
                        return false;
                }
            }
        }
        #endregion

        #region Cryptographic Functions


        /// <summary> Encryption a selected file
        /// </summary>
        /// <param name="filename">Will go to Encryped contet file</param>
        /// <param name="target">File name</param>
        /// <returns>True or False</returns>
        public bool EncryptFile(string fileName, string target)
        {
            if (!File.Exists(fileName))
            {
                throw new CryptographicException(Resources.Cypher_NoFile);
            }

            //Make sure the target file can be written
            try
            {
                using (FileStream fs = File.Create(target))
                {
                    fs.Close();
                }
                File.Delete(target);
            }
            catch (Exception ex)
            {
                throw new CryptographicException(Resources.Cypher_NotPermissionFileWrite, ex);
            }

            byte[] inStream = null;
            byte[] cipherBytes;

            try
            {
#if !CompactFramework
                inStream = File.ReadAllBytes(fileName);
#else
                FileStream stream = File.OpenRead(fileName);
                stream.Read(inStream, 0, (int)stream.Length);
#endif
            }
            catch (Exception ex)
            {
                throw new CryptographicException(Resources.Cypher_NotPermissionFileRead, ex);
            }

            cipherBytes = Encrypt(inStream);

            string encodedString = String.Empty;

            if (_encodingType == EncodingType.Base64)
            {
                encodedString = System.Convert.ToBase64String(cipherBytes);
            }
            else
            {
                encodedString = BytesToHex(cipherBytes);
            }

            byte[] encodedBytes = System.Text.Encoding.UTF8.GetBytes(encodedString);

            //Create the encrypted file
            using (FileStream outStream = File.Create(target))
            {
                outStream.Write(encodedBytes, 0, encodedBytes.Length);
                outStream.Close();
            }

            return true;
        }

        /// <summary> Decryption a selected file
        /// </summary>
        /// <param name="filename">Will go to Decryped contet file</param>
        /// <param name="target">File name</param>
        /// <returns>True or False</returns>
        public bool DecryptFile(string fileName, string target)
        {

            if (!File.Exists(fileName))
            {
                throw new CryptographicException(Resources.Cypher_NoFile);
            }

            try
            {
                using (FileStream fs = File.Create(target))
                {
                    fs.Close();
                }
                File.Delete(target);
            }
            catch (Exception ex)
            {
                throw new CryptographicException(Resources.Cypher_NotPermissionFileWrite, ex);
            }


            byte[] inStream = null;
            byte[] clearBytes;
            try
            {
#if !CompactFramework
                inStream = File.ReadAllBytes(fileName);
#else
                FileStream stream = File.OpenRead(fileName);
                stream.Read(inStream, 0, (int)(stream.Length - 1));
#endif
            }
            catch (Exception ex)
            {
                throw new CryptographicException(Resources.Cypher_NotPermissionFileRead, ex);
            }


            clearBytes = Decrypt(inStream);


            //Create the decrypted file
            using (FileStream outStream = File.Create(target))
            {
                outStream.Write(clearBytes, 0, clearBytes.Length);
                outStream.Close();
            }

            return true;
        }

        /// <summary> Generator of Hash from a content
        /// </summary>
        /// <param name="content">Content is being hash code</param>
        /// <returns>True or False</returns>
        public string GenerateHash(string content)
        {
            if (content == null || String.IsNullOrEmpty(content))
            {
                throw new CryptographicException(Resources.Cypher_NoContent);
            }

            HashAlgorithm hashAlgorithm = null;
            switch (_algorithm)
            {
                case Algorithm.SHA1:
                    hashAlgorithm = new SHA1CryptoServiceProvider();
                    break;
#if !CompactFramework
                case Algorithm.SHA256:
                    hashAlgorithm = new SHA256Managed();
                    break;
                case Algorithm.SHA384:
                    hashAlgorithm = new SHA384Managed();
                    break;
                case Algorithm.SHA512:
                    hashAlgorithm = new SHA512Managed();
                    break;
#endif
                case Algorithm.MD5:
                    hashAlgorithm = new MD5CryptoServiceProvider();
                    break;
                default:
                    throw new CryptographicException(Resources.Cypher_InvalidProvider);

            }

            try
            {
                byte[] hash = ComputeHash(hashAlgorithm, content);
                if (_encodingType == EncodingType.BinHex)
                {
                    content = BytesToHex(hash);
                }
                else
                {
                    content = System.Convert.ToBase64String(hash);
                }
            }
            finally
            {
                hashAlgorithm.Clear();
            }
            return content;
        }

        /// <summary> Clear variables
        /// </summary>
        public void Clear()
        {
            _algorithm = Algorithm.MD5;
            _encodingType = EncodingType.BinHex;
        }

        /// <summary> Function to encrypt an one content bytes
        /// </summary>
        /// <param name="content">Function to encrypt an one content bytes</param>
        /// <returns>Array of bytes</returns>
        public byte[] Encrypt(byte[] content)
        {
            if (!IsHashAlgorithm && _key == null)
            {
                throw new CryptographicException(Resources.Cypher_NoKey);
            }

            if (content == null || content.Equals(String.Empty))
            {
                throw new CryptographicException(Resources.Cypher_NoContent);
            }

            byte[] cipherBytes = null;
            int NumBytes = 0;
#if !CompactFramework
            if (_algorithm == Algorithm.RSA)
            {
                //This is an asymmetric call, which has to be treated differently
                cipherBytes = RSAEncrypt(content);
            }
            else
#endif
                if (IsHashAlgorithm)
                {
                    string hash = GenerateHash(System.Text.Encoding.UTF8.GetString(content, 0, content.Length));
                    cipherBytes = System.Text.Encoding.UTF8.GetBytes(hash);
                }
                else
                {
                    SymmetricAlgorithm provider;
                    switch (_algorithm)
                    {
                        case Algorithm.DES:
                            provider = new DESCryptoServiceProvider();
                            NumBytes = Convert.ToInt32(AlgorithmKeySize.DES);
                            break;
                        case Algorithm.TripleDES:
                            provider = new TripleDESCryptoServiceProvider();
                            NumBytes = Convert.ToInt32(AlgorithmKeySize.TripleDES);
                            break;
                        case Algorithm.Rijndael:
                            provider = new RijndaelManaged();
                            NumBytes = Convert.ToInt32(AlgorithmKeySize.Rijndael);
                            break;
                        case Algorithm.RC2:
                            provider = new RC2CryptoServiceProvider();
                            NumBytes = Convert.ToInt32(AlgorithmKeySize.RC2);
                            break;
                        default:
                            throw new CryptographicException(Resources.Cypher_InvalidProvider);
                    }

                    try
                    {
                        //Encrypt the string
                        cipherBytes = SymmetricEncrypt(provider, content, _key, NumBytes);
                    }
                    finally
                    {
                        //Free any resources held by the SymmetricAlgorithm provider
                        provider.Clear();
                    }
                }
            return cipherBytes;
        }

        /// <summary> Function to encrypt an one content string
        /// </summary>
        /// <param name="content">Function to encrypt an one content string</param>
        /// <returns>Array of bytes</returns>
        public string Encrypt(string content)
        {
            if (!String.IsNullOrEmpty(content))
            {
                byte[] cipherBytes = System.Text.Encoding.UTF8.GetBytes(content);
                cipherBytes = Encrypt(cipherBytes);
                content = (_encodingType == EncodingType.Base64) ? 
                    System.Convert.ToBase64String(cipherBytes) : 
                    BytesToHex(cipherBytes);               
            }

            return content;
        }

        /// <summary> Function to decrypt an one content bytes
        /// </summary>
        /// <param name="content">To be encrypted content</param>
        /// <returns>Array of bytes</returns>
        public byte[] Decrypt(byte[] content)
        {
            if (IsHashAlgorithm)
            {
                throw new CryptographicException(Resources.Cypher_DecryptHash);
            }

            if (_key == null)
            {
                throw new CryptographicException(Resources.Cypher_NoKey);
            }

            if (content == null || content.Equals(String.Empty))
            {
                throw new CryptographicException(Resources.Cypher_NoContent);
            }

            string encText = System.Text.Encoding.UTF8.GetString(content, 0, content.Length);

            if (_encodingType == EncodingType.Base64)
            {
                //We need to convert the content to Hex before decryption
                encText = BytesToHex(System.Convert.FromBase64String(encText));
            }

            byte[] clearBytes = null;
            int NumBytes = 0;

#if !CompactFramework
            if (_algorithm == Algorithm.RSA)
            {
                clearBytes = RSADecrypt(encText);

            }
            else
#endif
            {
                SymmetricAlgorithm provider;
                switch (_algorithm)
                {
                    case Algorithm.DES:
                        provider = new DESCryptoServiceProvider();
                        NumBytes = Convert.ToInt32(AlgorithmKeySize.DES);
                        break;
                    case Algorithm.TripleDES:
                        provider = new TripleDESCryptoServiceProvider();
                        NumBytes = Convert.ToInt32(AlgorithmKeySize.TripleDES);
                        break;
                    case Algorithm.Rijndael:
                        provider = new RijndaelManaged();
                        NumBytes = Convert.ToInt32(AlgorithmKeySize.Rijndael);
                        break;
                    case Algorithm.RC2:
                        provider = new RC2CryptoServiceProvider();
                        NumBytes = Convert.ToInt32(AlgorithmKeySize.RC2);
                        break;
                    default:
                        throw new CryptographicException(Resources.Cypher_InvalidProvider);
                }
                try
                {
                    clearBytes = SymmetricDecrypt(provider, encText, _key, NumBytes);
                }
                finally
                {
                    //Free any resources held by the SymmetricAlgorithm provider
                    provider.Clear();
                }
            }
            //Now return the plain text content
            return clearBytes;
        }

        /// <summary> Function to decrypt an one content string
        /// </summary>
        /// <param name="content">To be decrypted content</param>
        /// <returns>Array of bytes</returns>
        public string Decrypt(string content)
        {
            if (!String.IsNullOrEmpty(content))
            {
                byte[] cipherBytes = System.Text.Encoding.UTF8.GetBytes(content);
                cipherBytes = Decrypt(cipherBytes);
                content = System.Text.Encoding.UTF8.GetString(cipherBytes, 0, cipherBytes.Length);
            }
            return content;

        }

        /// <summary> Mechanisms inherit from the HashAlgorithm base class so we can use that to cast the crypto service provider
        /// </summary>
        /// <param name="provider">Type of provider</param>
        /// <param name="plainText">To be hash content</param>
        /// <returns>Array of bytes</returns>
        private byte[] ComputeHash(HashAlgorithm provider, string plainText)
        {
            //All hashing mechanisms inherit from the HashAlgorithm base class so we can use that to cast the crypto service provider
            byte[] hash = provider.ComputeHash(System.Text.Encoding.UTF8.GetBytes(plainText));
            provider.Clear();
            return hash;
        }

        /// <summary> Symmetrical function to Encrypt
        /// </summary>
        /// <param name="provider">Type of provider</param>
        /// <param name="encText">To be encrypted content</param>
        /// <param name="key">Type key</param>
        /// <param name="keySize">Size of key</param>
        /// <returns>Array of byte</returns>
        private byte[] SymmetricEncrypt(SymmetricAlgorithm provider, byte[] plainText, string key, int keySize)
        {
            //All symmetric algorithms inherit from the SymmetricAlgorithm base class, to which we can cast from the original crypto service provider
            byte[] ivBytes = GetInitializationVector(keySize);
            provider.KeySize = keySize;

            //Generate a secure key based on the original password by using SALT
            byte[] keyStream = DerivePassword(key, keySize / 8);

            //Initialize our encryptor object
            ICryptoTransform trans = provider.CreateEncryptor(keyStream, ivBytes);

            //Perform the encryption on the textStream byte array
            byte[] result = trans.TransformFinalBlock(plainText, 0, plainText.GetLength(0));

            //Release cryptographic resources
            provider.Clear();
            trans.Dispose();

            return result;
        }

        /// <summary> Symmetrical function to Decrypt
        /// </summary>
        /// <param name="provider">Type of provider</param>
        /// <param name="encText">To be decrypted content</param>
        /// <param name="key">Type key</param>
        /// <param name="keySize">Size of key</param>
        /// <returns></returns>
        private byte[] SymmetricDecrypt(SymmetricAlgorithm provider, string encText, string key, int keySize)
        {
            //All symmetric algorithms inherit from the SymmetricAlgorithm base class, to which we can cast from the original crypto service provider
            byte[] ivBytes = GetInitializationVector(keySize);

            //Generate a secure key based on the original password by using SALT
            byte[] keyStream = DerivePassword(key, keySize / 8);

            //Convert our hex-encoded cipher text to a byte array
            byte[] textStream = HexToBytes(encText);
            provider.KeySize = keySize;

            //Initialize our decryptor object
            ICryptoTransform trans = provider.CreateDecryptor(keyStream, ivBytes);

            //Initialize the result stream
            byte[] result = null;
            try
            {
                //Perform the decryption on the textStream byte array
                result = trans.TransformFinalBlock(textStream, 0, textStream.Length);
            }
            catch (Exception ex)
            {

                throw new System.Security.Cryptography.CryptographicException("The following exception occurred during decryption: " + ex.Message);
            }
            finally
            {
                //Release cryptographic resources
                provider.Clear();
                trans.Dispose();
            }
            return result;
        }

        private byte[] GetInitializationVector(int keySize)
        {
            switch (keySize / 8) //Determine which initialization vector to use
            {
                case 8:
                    return _iv8;

                case 16:
                    return _iv16;

                case 24:
                    return _iv24;

                case 32:
                    return _iv32;

                default:
                    //TODO: Throw an error because an invalid key length has been passed
                    throw new Exception();

            }
        }

#if !CompactFramework
        #region RSA Algorithm
        /// <summary>
        /// RSA Encryptator
        /// </summary>
        /// <param name="plainText">Text to Encrypted</param>
        /// <returns></returns>
        private byte[] RSAEncrypt(byte[] plainText)
        {
            //The RSA algorithm works on individual blocks of unencoded bytes. In this case, the
            //maximum is 58 bytes. Therefore, we are required to break up the text into blocks and
            //encrypt each one individually. Each encrypted block will give us an output of 128 bytes.
            //If we do not break up the blocks in this manner, we will throw a "key not valid for use
            //in specified state" exception

            //Get the size of the final block

            int lastBlockLength = plainText.Length % RSA_BLOCKSIZE;
            int blockCount = (int)Math.Floor((double)plainText.Length / RSA_BLOCKSIZE);
            bool hasLastBlock = false;
            if (!(lastBlockLength.Equals(0)))
            {
                //We need to create a final block for the remaining characters
                blockCount += 1;
                hasLastBlock = true;
            }



            //Initialize the result buffer
            byte[] result = new byte[] { };

            //Initialize the RSA Service Provider with the public key
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider(Convert.ToInt32(Algorithm.RSA));

            provider.FromXmlString(PublicKey);


            //Break the text into blocks and work on each block individually
            for (int blockIndex = 0; blockIndex < blockCount; blockIndex++)
            {
                int thisBlockLength;
                //If this is the last block and we have a remainder, then set the length accordingly
                if (blockCount.Equals(blockIndex + 1) && hasLastBlock)
                {
                    thisBlockLength = lastBlockLength;
                }
                else
                {
                    thisBlockLength = RSA_BLOCKSIZE;
                }

                int startChar = blockIndex * RSA_BLOCKSIZE;

                //Define the block that we will be working on
                byte[] currentBlock = new byte[thisBlockLength];
                Array.Copy(plainText, startChar, currentBlock, 0, thisBlockLength);

                //Encrypt the current block and append it to the result stream
                byte[] encryptedBlock = provider.Encrypt(currentBlock, false);
                int originalResultLength = result.Length;
                Array.Resize<byte>(ref result, originalResultLength + encryptedBlock.Length);
                encryptedBlock.CopyTo(result, originalResultLength);
            }

            //Release any resources held by the RSA Service Provider
            provider.Clear();

            return result;
        }

        /// <summary>
        /// RSA Decryptator
        /// </summary>
        /// <param name="encText">Text to decrypted</param>
        /// <returns></returns>
        private byte[] RSADecrypt(string encText)
        {
            //When we encrypt a string using RSA, it works on individual blocks of up to
            //58 bytes. Each block generates an output of 128 encrypted bytes. Therefore, to
            //decrypt the message, we need to break the encrypted stream into individual
            //chunks of 128 bytes and decrypt them individually

            //Determine how many bytes are in the encrypted stream. The input is in hex format,
            //so we have to divide it by 2
            int maxBytes = encText.Length / 2;

            //Ensure that the length of the encrypted stream is divisible by 128
            if (!(maxBytes % RSA_DECRYPTBLOCKSIZE).Equals(0))
            {
                throw new System.Security.Cryptography.CryptographicException("Encrypted text is an invalid length");

            }

            //Calculate the number of blocks we will have to work on
            int blockCount = maxBytes / RSA_DECRYPTBLOCKSIZE;

            //Initialize the result buffer
            byte[] result = new byte[] { };

            //Initialize the RSA Service Provider
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider(Convert.ToInt32(Algorithm.RSA));
            provider.FromXmlString(PrivateKey);

            //Iterate through each block and decrypt it
            string currentBlockHex;
            for (int blockIndex = 0; blockIndex < blockCount; blockIndex++)
            {
                //Get the current block to work on
                currentBlockHex = encText.Substring(blockIndex * (RSA_DECRYPTBLOCKSIZE * 2), RSA_DECRYPTBLOCKSIZE * 2);
                byte[] currentBlockBytes = HexToBytes(currentBlockHex);

                //Decrypt the current block and append it to the result stream
                byte[] currentBlockDecrypted = provider.Decrypt(currentBlockBytes, false);
                int originalResultLength = result.Length;

                Array.Resize<byte>(ref result, originalResultLength + currentBlockDecrypted.Length);
                currentBlockDecrypted.CopyTo(result, originalResultLength);

            }

            //Release all resources held by the RSA service provider
            provider.Clear();

            return result;
        }
        #endregion
#endif

        #endregion

        #region Utility Functions

        /// <summary>
        /// BytesToHex: Converts a byte array to a hex-encoded string
        /// </summary>
        /// <param name="bytes">BytesToHex: Converts a byte array to a hex-encoded string</param>
        /// <returns></returns>
        private string BytesToHex(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder();
            for (int n = 0; n < bytes.Length; n++)
            {
                hex.AppendFormat(null, "{0:X2}", bytes[n]);
            }

            return hex.ToString();
        }

        /// <summary>
        /// Converts a hex-encoded string to a byte array
        /// </summary>
        /// <param name="hex"></param>
        /// <returns>Array of byte</returns>
        private byte[] HexToBytes(string hex)
        {
            int numBytes = hex.Length / 2;
            byte[] bytes = new byte[numBytes];

            for (int n = 0; n < bytes.Length; n++)
            {
                bytes[n] = byte.Parse(hex.Substring(n * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }

            return bytes;
        }

        /// <summary>
        /// ClearBuffer: Clears a byte array to ensure that it cannot be read from memory
        /// </summary>
        /// <param name="bytes"></param>
        private void ClearBuffer(byte[] bytes)
        {
            if (bytes == null) { return; }
            for (int n = 0; n < bytes.Length - 1; n++)
            {
                bytes[n] = 0;
            }
        }


        /// <summary>
        /// GenerateSalt: No, this is not a culinary routine. This generates a random salt value for password generation
        /// </summary>
        /// <param name="saltLength"></param>
        /// <returns></returns>
        private byte[] GenerateSalt(int saltLength)
        {
            byte[] salt = new byte[saltLength];
            RNGCryptoServiceProvider seed = new RNGCryptoServiceProvider();
            seed.GetBytes(salt);
            return salt;
        }


        /// <summary>
        /// DerivePassword: This takes the original plain text key and creates a secure key using SALT
        /// </summary>
        /// <param name="originalPassword"></param>
        /// <param name="passwordLength"></param>
        /// <returns></returns>
        private byte[] DerivePassword(string originalPassword, int passwordLength)
        {
#if !CompactFramework
            Rfc2898DeriveBytes derivedBytes = new Rfc2898DeriveBytes(originalPassword, _saltBytes, 5);
            return derivedBytes.GetBytes(passwordLength);
#else
            return System.Text.Encoding.UTF8.GetBytes(originalPassword.Substring(0, passwordLength));
#endif
        }



        #endregion
    }
}

