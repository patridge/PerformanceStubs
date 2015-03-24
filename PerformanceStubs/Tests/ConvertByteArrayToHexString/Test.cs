namespace PerformanceStubs.Tests.ConvertByteArrayToHexString {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Remoting.Metadata.W3cXsd2001;
    using System.Text;
    using PerformanceStubs.Core;

    public class Test: PerformanceTestOneInOneOut<byte[], string> {
        protected override string Title {
            get {
                return "Converting array of bytes into hexadecimal string representation";
            }
        }
        protected override string Caption {
            get {
                return string.Format("Text (n={0:0,0} bytes), {1} iterations.", Input1.Count(), Iterations);
            }
        }
        protected override List<Func<byte[], string>> TestCandidates {
            get {
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
                    ByteArrayToHexViaSoapHexBinary,
                    ByteArrayToHexViaLookupAndShift,
                    ByteArrayToHexViaLookup,
                    ByteArrayToHexViaLookupPerByte,
                }).ToList();
            }
        }
        static string ByteArrayToHexStringViaStringJoinArrayConvertAll(byte[] bytes) {
            return string.Join(string.Empty, Array.ConvertAll(bytes, b => b.ToString("X2")));
        }
        static string ByteArrayToHexStringViaStringConcatArrayConvertAll(byte[] bytes) {
            return string.Concat(Array.ConvertAll(bytes, b => b.ToString("X2")));
        }
        static string ByteArrayToHexStringViaBitConverter(byte[] bytes) {
            string hex = BitConverter.ToString(bytes);
            return hex.Replace("-", "");
        }
        static string ByteArrayToHexStringViaStringBuilderAggregateByteToString(byte[] bytes) {
            return bytes.Aggregate(new StringBuilder(bytes.Length * 2), (sb, b) => sb.Append(b.ToString("X2"))).ToString();
        }
        static string ByteArrayToHexStringViaStringBuilderForEachByteToString(byte[] bytes) {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.Append(b.ToString("X2"));
            return hex.ToString();
        }
        static string ByteArrayToHexStringViaStringBuilderAggregateAppendFormat(byte[] bytes) {
            return bytes.Aggregate(new StringBuilder(bytes.Length * 2), (sb, b) => sb.AppendFormat("{0:X2}", b)).ToString();
        }
        static string ByteArrayToHexStringViaStringBuilderForEachAppendFormat(byte[] bytes) {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:X2}", b);
            return hex.ToString();
        }
        static string ByteArrayToHexViaByteManipulation(byte[] bytes) {
            char[] c = new char[bytes.Length * 2];
            byte b;
            for (int i = 0; i < bytes.Length; i++) {
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
        static string ByteArrayToHexViaByteManipulation2(byte[] bytes) {
            char[] c = new char[bytes.Length * 2];
            int b;
            for (int i = 0; i < bytes.Length; i++) {
                b = bytes[i] >> 4;
                c[i * 2] = (char)(55 + b + (((b - 10) >> 31) & -7));
                b = bytes[i] & 0xF;
                c[i * 2 + 1] = (char)(55 + b + (((b - 10) >> 31) & -7));
            }
            return new string(c);
        }
        static string ByteArrayToHexViaSoapHexBinary(byte[] bytes) {
            SoapHexBinary soapHexBinary = new SoapHexBinary(bytes);
            return soapHexBinary.ToString();
        }
        static string ByteArrayToHexViaLookupAndShift(byte[] bytes) {
            StringBuilder result = new StringBuilder(bytes.Length * 2);
            string hexAlphabet = "0123456789ABCDEF";
            foreach (byte b in bytes) {
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
        static string ByteArrayToHexViaLookupPerByte(byte[] bytes) {
            var result = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                var val = _Lookup32[bytes[i]];
                result[2*i] = (char)val;
                result[2*i + 1] = (char) (val >> 16);
            }
            return new string(result);
        }
        static string ByteArrayToHexViaLookup(byte[] bytes) {
            string[] hexStringTable = new string[] {
                "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "0A", "0B", "0C", "0D", "0E", "0F",
                "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "1A", "1B", "1C", "1D", "1E", "1F",
                "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "2A", "2B", "2C", "2D", "2E", "2F",
                "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "3A", "3B", "3C", "3D", "3E", "3F",
                "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "4A", "4B", "4C", "4D", "4E", "4F",
                "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "5A", "5B", "5C", "5D", "5E", "5F",
                "60", "61", "62", "63", "64", "65", "66", "67", "68", "69", "6A", "6B", "6C", "6D", "6E", "6F",
                "70", "71", "72", "73", "74", "75", "76", "77", "78", "79", "7A", "7B", "7C", "7D", "7E", "7F",
                "80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "8A", "8B", "8C", "8D", "8E", "8F",
                "90", "91", "92", "93", "94", "95", "96", "97", "98", "99", "9A", "9B", "9C", "9D", "9E", "9F",
                "A0", "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "A9", "AA", "AB", "AC", "AD", "AE", "AF",
                "B0", "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8", "B9", "BA", "BB", "BC", "BD", "BE", "BF",
                "C0", "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "CA", "CB", "CC", "CD", "CE", "CF",
                "D0", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "DA", "DB", "DC", "DD", "DE", "DF",
                "E0", "E1", "E2", "E3", "E4", "E5", "E6", "E7", "E8", "E9", "EA", "EB", "EC", "ED", "EE", "EF",
                "F0", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "FA", "FB", "FC", "FD", "FE", "FF",
            };
            StringBuilder result = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes) {
                result.Append(hexStringTable[b]);
            }
            return result.ToString();
        }

        static byte[] GetSource(string fileName) {
            FileInfo testSubjectFile = new System.IO.FileInfo(fileName);
            string testFileContents = null;
            using (FileStream testSubjectStream = testSubjectFile.OpenRead()) {
                using (StreamReader testSubjectReader = new StreamReader(testSubjectStream)) {
                    testFileContents = testSubjectReader.ReadToEnd();
                }
            }
            return System.Text.ASCIIEncoding.ASCII.GetBytes(testFileContents);
        }
        private static byte[] GenerateTestInput() {
            //return GetSource(@"Tests\ConvertByteArrayToHexString\SmallText.txt");
            //return GetSource(@"Tests\ConvertByteArrayToHexString\LargeText.txt");
            return System.Text.ASCIIEncoding.ASCII.GetBytes("put any sample string you want to test here instead of a file");
        }
        private byte[] _Intput1 = null;
        protected override byte[] Input1 {
            get {
                return _Intput1 ?? (_Intput1 = GenerateTestInput());
            }
        }
        protected override long Iterations {
            get {
                return 1000;
            }
        }
        protected override bool OutputComparer(string left, string right) {
            return left == right;
        }
    }
}
