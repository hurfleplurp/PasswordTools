using System;
using System.Windows.Media;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PGen.Obfuscation
{
    enum HashType { SHA1, SHA256, SHA384, SHA512, MD5 }
    internal interface IHasher
    {
        string HashInput(string salt, HashType hashType);
    }
    class Obfuscator
    {
        // Constructor defaults
        private IHasher hashFamily = null;
        private short desiredLength = 12;
        private Tuple<string, SolidColorBrush> regexStatus = Tuple.Create("No RegEx", Brushes.Orange);

        // Accessors
        public IHasher HashFamily
        {
            get
            {
                return hashFamily;
            }
            set
            {
                hashFamily = value;
            }
        }
        public string Salt { get; set; }
        public string RegEx { get; set; }
        public string ReplacementText { get; set; }
        public short DesiredLength
        {
            get
            {
                return desiredLength;
            }
            set
            {
                desiredLength = value;
            }
        }
        public Tuple<string, SolidColorBrush> RegexStatus
        {
            get
            {
                return regexStatus;
            }
            private set
            {
                regexStatus = value;
            }
        }
        public string Password { get; private set; }

        // Methods
        public void GeneratePassword()
        {
            SHAHashing shaHasher = new SHAHashing();
            HashFamily = shaHasher;
            Hash(HashType.SHA1);
            ApplyRegex();
            Password = Password.Substring(2, DesiredLength);
        }

        private void ApplyRegex()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(RegEx))
                {
                    Regex exp = new Regex(RegEx);
                    Password = exp.Replace(Password, ReplacementText);
                    RegexStatus = Tuple.Create("Valid", Brushes.Green);
                }
            }
            catch (ArgumentException)
            {
                RegexStatus = Tuple.Create("Invalid", Brushes.Red);
            }
        }
        public Tuple<string, SolidColorBrush> ApplyRegex(string hash)
        {
            Tuple<string, SolidColorBrush> regexReturnStatus = Tuple.Create("No RegEx", Brushes.Orange);

            try
            {
                if (!string.IsNullOrWhiteSpace(RegEx))
                {
                    Regex exp = new Regex(RegEx);
                    hash = exp.Replace(hash, ReplacementText);
                    regexReturnStatus = Tuple.Create("Valid", Brushes.Green);
                }
            }
            catch (ArgumentException)
            {
                regexReturnStatus = Tuple.Create("Invalid", Brushes.Red);
            }

            return regexReturnStatus;
        }
        private void Hash(HashType hashType)
        {
            Password = hashFamily.HashInput(Salt, hashType);
        }
        public string Hash(string salt, HashType hashType)
        {
            return hashFamily.HashInput(salt, hashType);
        }
    }

    class SHAHashing : IHasher
    {

        public string HashInput(string salt, HashType hashType)
        {
            string result;

            switch (hashType)
            {
                case HashType.SHA1:
                    using (var cryptoProvider = new SHA1CryptoServiceProvider())
                    {
                        byte[] saltBytes = Encoding.ASCII.GetBytes(salt);
                        result = Convert.ToBase64String(cryptoProvider.ComputeHash(saltBytes));
                        cryptoProvider.Dispose();
                    }
                        break;
                case HashType.SHA256:
                    using (var cryptoProvider = new SHA256CryptoServiceProvider())
                    {
                        byte[] saltBytes = Encoding.ASCII.GetBytes(salt);
                        result = Convert.ToBase64String(cryptoProvider.ComputeHash(saltBytes));
                        cryptoProvider.Dispose();
                    }
                    break;
                case HashType.SHA384:
                    using (var cryptoProvider = new SHA384CryptoServiceProvider())
                    {
                        byte[] saltBytes = Encoding.ASCII.GetBytes(salt);
                        result = Convert.ToBase64String(cryptoProvider.ComputeHash(saltBytes));
                        cryptoProvider.Dispose();
                    }
                    break;
                case HashType.SHA512:
                    using (var cryptoProvider = new SHA512CryptoServiceProvider())
                    {
                        byte[] saltBytes = Encoding.ASCII.GetBytes(salt);
                        result = Convert.ToBase64String(cryptoProvider.ComputeHash(saltBytes));
                        cryptoProvider.Dispose();
                    }
                    break;
                default:
                    throw new UnknownHashTypeException(string.Format("{0} is not a valid SHA hash type.", Enum.GetName(typeof(HashType), hashType)));
            }

            return result;            
        }
    }

    class MDHashing : IHasher
    {
        public string HashInput(string salt, HashType hashType)
        {
            string result;

            switch (hashType)
            {
                case HashType.MD5:
                    using (var cryptoProvider = new MD5CryptoServiceProvider())
                    {
                        Byte[] saltBytes = Encoding.ASCII.GetBytes(salt);
                        result = Convert.ToBase64String(cryptoProvider.ComputeHash(saltBytes));
                        cryptoProvider.Dispose();
                    }
                    break;
                default:
                    throw new UnknownHashTypeException(string.Format("{0} is not a valid MD hash type.", Enum.GetName(typeof(HashType), hashType)));
            }

            return result;
        }
    }

    public class UnknownHashTypeException : Exception, ISerializable
    {
        public UnknownHashTypeException()
        {
            // Add implementation.
        }
        public UnknownHashTypeException(string message)
        {
            // Add implementation.
        }
        public UnknownHashTypeException(string message, Exception inner)
        {
            // Add implementation.
        }

        // This constructor is needed for serialization.
        protected UnknownHashTypeException(SerializationInfo info, StreamingContext context)
        {
            // Add implementation.
        }
    }

}
