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
                    ByteArrayToHexViaSoapHexBinary
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
