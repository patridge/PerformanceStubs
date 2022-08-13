namespace PerformanceStubs.Candidates.ConvertByteArrayToHexString
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    public unsafe class Implementations
    {
        protected string Title
        {
            get
            {
                return "Converting array of bytes into hexadecimal string representation";
            }
        }
        protected string Caption
        {
            get
            {
                return string.Format("Text (n={0:0,0} bytes), {1} iterations.", Input1.Count(), Iterations);
            }
        }
        protected List<Func<byte[], string>> TestCandidates
        {
            get
            {
                return (new Func<byte[], string>[] {
                ByteArrayToHexStringViaStringJoinArrayConvertAll,
                ByteArrayToHexStringViaStringConcatArrayConvertAll,
                ByteArrayToHexStringViaBitConverter,
                ByteArrayToHexStringViaStringBuilderAggregateByteToString,
                ByteArrayToHexStringViaStringBuilderAggregateAppendFormat,
                ByteArrayToHexStringViaStringBuilderForEachByteToString,
                ByteArrayToHexStringViaStringBuilderForEachAppendFormat,
                ByteArrayToHexViaByteManipulation,
                ByteArrayToHexViaByteManipulation2,
                ByteArrayToHexViaLookupAndShift,
                ByteArrayToHexViaLookup,
                ByteArrayToHexViaLookupPerByte,
                ByteArrayToHexViaLookup32UnsafeDirect,
            }).ToList();
            }
        }
        public static string ByteArrayToHexStringViaStringJoinArrayConvertAll(byte[] bytes)
        {
            return string.Join(string.Empty, Array.ConvertAll(bytes, b => b.ToString("X2")));
        }
        public static string ByteArrayToHexStringViaStringConcatArrayConvertAll(byte[] bytes)
        {
            return string.Concat(Array.ConvertAll(bytes, b => b.ToString("X2")));
        }
        public static string ByteArrayToHexStringViaBitConverter(byte[] bytes)
        {
            string hex = BitConverter.ToString(bytes);
            return hex.Replace("-", "");
        }
        public static string ByteArrayToHexStringViaStringBuilderAggregateByteToString(byte[] bytes)
        {
            return bytes.Aggregate(new StringBuilder(bytes.Length * 2), (sb, b) => sb.Append(b.ToString("X2"))).ToString();
        }
        public static string ByteArrayToHexStringViaStringBuilderForEachByteToString(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.Append(b.ToString("X2"));
            return hex.ToString();
        }
        public static string ByteArrayToHexStringViaStringBuilderAggregateAppendFormat(byte[] bytes)
        {
            return bytes.Aggregate(new StringBuilder(bytes.Length * 2), (sb, b) => sb.AppendFormat("{0:X2}", b)).ToString();
        }
        public static string ByteArrayToHexStringViaStringBuilderForEachAppendFormat(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:X2}", b);
            return hex.ToString();
        }
        public static string ByteArrayToHexViaByteManipulation(byte[] bytes)
        {
            char[] c = new char[bytes.Length * 2];
            byte b;
            for (int i = 0; i < bytes.Length; i++)
            {
                b = ((byte)(bytes[i] >> 4));
                c[i * 2] = (char)(b > 9 ? b + 0x37 : b + 0x30);
                b = ((byte)(bytes[i] & 0xF));
                c[i * 2 + 1] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            }
            return new string(c);
        }
        /// <summary>
        /// Derived from http://stackoverflow.com/a/14333437/48700
        /// </summary>
        public static string ByteArrayToHexViaByteManipulation2(byte[] bytes)
        {
            char[] c = new char[bytes.Length * 2];
            int b;
            for (int i = 0; i < bytes.Length; i++)
            {
                b = bytes[i] >> 4;
                c[i * 2] = (char)(55 + b + (((b - 10) >> 31) & -7));
                b = bytes[i] & 0xF;
                c[i * 2 + 1] = (char)(55 + b + (((b - 10) >> 31) & -7));
            }
            return new string(c);
        }

        // using System.Runtime.Remoting.Metadata.W3cXsd2001;
        // Deprecated: No longer supported.
        // For what it's worth, this method was 8th in the last test run. So, alternatives may be better choices.
        //public static string ByteArrayToHexViaSoapHexBinary(byte[] bytes)
        //{
        //    SoapHexBinary soapHexBinary = new SoapHexBinary(bytes);
        //    return soapHexBinary.ToString();
        //}

        const string hexAlphabet = "0123456789ABCDEF";
        public static string ByteArrayToHexViaLookupAndShift(byte[] bytes)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                result.Append(hexAlphabet[(int)(b >> 4)]);
                result.Append(hexAlphabet[(int)(b & 0xF)]);
            }
            return result.ToString();
        }
        static uint[] _Lookup32 = Enumerable.Range(0, 256).Select(i => {
            string s = i.ToString("X2");
            return ((uint)s[0]) + ((uint)s[1] << 16);
        }).ToArray();
        /// <summary>
        /// Derived from http://stackoverflow.com/a/24343727/48700
        /// </summary>
        public static string ByteArrayToHexViaLookupPerByte(byte[] bytes)
        {
            var result = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                var val = _Lookup32[bytes[i]];
                result[2 * i] = (char)val;
                result[2 * i + 1] = (char)(val >> 16);
            }
            return new string(result);
        }

        // { "00", "01", ..., "0E", "0F", "10", "11", ..., "FE", "FF" }
        static readonly string[] hexStringTable = hexAlphabet.SelectMany(n1 => hexAlphabet.Select(n2 => new string(new[] { n1, n2 }))).ToArray();
        public static string ByteArrayToHexViaLookup(byte[] bytes)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                result.Append(hexStringTable[b]);
            }
            return result.ToString();
        }

        static readonly uint* _lookup32UnsafeP = (uint*)GCHandle.Alloc(_Lookup32, GCHandleType.Pinned).AddrOfPinnedObject();
        public static string ByteArrayToHexViaLookup32UnsafeDirect(byte[] bytes)
        {
            var lookupP = _lookup32UnsafeP;
            var result = new string((char)0, bytes.Length * 2);
            fixed (byte* bytesP = bytes)
            fixed (char* resultP = result)
            {
                uint* resultP2 = (uint*)resultP;
                for (int i = 0; i < bytes.Length; i++)
                {
                    resultP2[i] = lookupP[bytesP[i]];
                }
            }

            return result;
        }

        static byte[] GetSource(string fileName)
        {
            FileInfo testSubjectFile = new FileInfo(fileName);
            using (FileStream testSubjectStream = testSubjectFile.OpenRead())
            {
                using (StreamReader testSubjectReader = new StreamReader(testSubjectStream))
                {
                    return Encoding.ASCII.GetBytes(testSubjectReader.ReadToEnd());
                }
            }
        }
        private static byte[] GenerateTestInput()
        {
            //return GetSource(@"Tests\ConvertByteArrayToHexString\SmallText.txt");
            //return GetSource(@"Tests\ConvertByteArrayToHexString\LargeText.txt");
            return Encoding.ASCII.GetBytes("put any sample string you want to test here instead of a file");
        }

        private byte[] _Intput1 = null;
        protected byte[] Input1
        {
            get
            {
                return _Intput1 ?? (_Intput1 = GenerateTestInput());
            }
        }
        protected long Iterations
        {
            get
            {
                return 1000;
            }
        }
        protected bool OutputComparer(string left, string right)
        {
            return left == right;
        }
    }
}