using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Globalization;
using System.Security.Cryptography;

namespace CertificateScanner.Utils
{
    /// <summary>
    /// Class with crypto algorytms for log
    /// </summary>
    public class Crypto
    {
        #region constructors

        /// <summary>
        /// Create instance of encrypting class(Rijndael symetryc algorytm) with user key
        /// </summary>
        /// <param name="IVValue">Vector value 128, 192 or 256 bit.</param>
        /// <param name="KeyValue">Key value 128, 192 or 256 bit.</param>
        public Crypto(string KeyValue, string IVValue)
        {
            //Normalise first and second key(it must have 128 bits or 16 byte or 16 symbols)

            if (String.IsNullOrWhiteSpace(KeyValue))
                Key = Encoding.UTF8.GetBytes(defaultstring);
            else
            {

                if (KeyValue.Length <= 16) //Add symbols from etalon
                    KeyValue += defaultstring.Substring(0, 16 - KeyValue.Length);
                else
                    KeyValue = KeyValue.Substring(0, 16); //Delete symbol after 16

                Key = Encoding.UTF8.GetBytes(KeyValue);
            }

            if (String.IsNullOrWhiteSpace(IVValue))
                IV = Encoding.UTF8.GetBytes(defaultstring);
            else
            {

                if (IVValue.Length <= 16) //Add symbols from etalon
                    IVValue += defaultstring.Substring(0, 16 - IVValue.Length);
                else
                    IVValue = IVValue.Substring(0, 16); //Delete symbol after 16

                IV = Encoding.UTF8.GetBytes(IVValue);
            }
        }

        /// <summary>
        /// Create instance of encrypting class(Rijndael symetryc algorytm) with default key and vector
        /// </summary>
        public Crypto()
        {
            Key = Encoding.UTF8.GetBytes(defaultstring);
            IV = Encoding.UTF8.GetBytes(defaultstring);
        }

        #endregion

        #region private Variable

        private const string defaultstring = "wQvyxoWe9J|EI4ro";

        private byte[] Key;
        private byte[] IV;

        #endregion

        #region Methods

        /// <summary>
        /// Encrypt string to string in format (153;130;225;171;72;)
        /// </summary>
        /// <param name="plainText">input string</param>
        public string encryptStringToString_AES(string plaintext)
        {
            string tempPsw = String.Empty; //Encrypt after first step

            string result = String.Empty; //array of byte

            //First Step

            try
            {
                // Encrypt the string to an array of bytes.
                byte[] encrypted = encryptStringToBytes_AES(plaintext);

                foreach (byte enc in encrypted)
                    tempPsw += Convert.ToChar(enc);

                foreach (byte enc in encrypted)
                    result += enc.ToString(CultureInfo.InvariantCulture) + ";";
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException((ex.InnerException as ArgumentNullException).ParamName);
            }


            result = String.Empty;

            //Second Step
            try
            {
                // Encrypt the string to an array of bytes.
                byte[] encrypted = encryptStringToBytes_AES(tempPsw);

                foreach (byte enc in encrypted)
                    result += enc.ToString(CultureInfo.InvariantCulture) + ";";
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException((ex.InnerException as ArgumentNullException).ParamName);
            }

            return result;
        }

        /// <summary>
        /// Decrypt string to string from format (153;130;225;171;72;)
        /// </summary>
        /// <param name="cipherText">input string</param>
        public string decryptStringToString_AES(string cipherText)
        {
            string result = String.Empty;

            byte[] encrypted = null;

            if (String.IsNullOrWhiteSpace(cipherText))
                throw new ArgumentNullException("cipherText");

            //Chr symbols
            string[] split = cipherText.Split(new Char[] { ';' });

            int i = 0;

            encrypted = new byte[split.Length - 1]; //becose last string is empty(last char is ';')

            foreach (string s in split)
            {
                if (!String.IsNullOrEmpty(s.Trim()))
                {
                    encrypted[i] = byte.Parse(s, CultureInfo.InvariantCulture);
                    ++i;
                }
            }

            byte[] tempPsw = null; //Decrypt after first step

            //First Step

            try
            {
                // Decrypt the bytes to a string.
                string roundtrip = decryptStringFromBytes_AES(encrypted);

                result = roundtrip;

                tempPsw = new byte[roundtrip.Length];

                for (int j = 0; j < roundtrip.Length; j++)
                    tempPsw[j] = Convert.ToByte(roundtrip[j]);

            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException((ex.InnerException as ArgumentNullException).ParamName);
            }

            //Second Step
            try
            {
                string roundtrip = decryptStringFromBytes_AES(tempPsw);
                result = roundtrip;
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException((ex.InnerException as ArgumentNullException).ParamName);
            }
            return result;
        }

        /// <summary>
        /// Encrypt string to byte[]
        /// </summary>
        /// <param name="plainText">input string</param>
        public byte[] encryptStringToBytes_AES(string plaintext)
        {
            // Check arguments.
            if (plaintext == null || plaintext.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the stream used to encrypt to an in memory
            // array of bytes.
            MemoryStream msEncrypt = null;

            // Declare the RijndaelManaged object
            // used to encrypt the data.
            RijndaelManaged aesAlg = null;

            try
            {
                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged();
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                msEncrypt = new MemoryStream();
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {

                        //Write all data to the stream.
                        swEncrypt.Write(plaintext);
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return msEncrypt.ToArray();
        }

        /// <summary>
        /// Decrypt byte[] to string
        /// </summary>
        /// <param name="cipherText">input</param>
        public string decryptStringFromBytes_AES(byte[] cipherText)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            try
            {
                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged();
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }

        #endregion

    }
}
