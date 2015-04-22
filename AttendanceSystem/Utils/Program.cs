using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Utils
{
    [Serializable]
    public class Item
    {
        public string Text { get; private set; }

        private Item(string text)
        {
            Text = text;
        }

        public static Item Create() { return new Item("Khoa"); }
    }

    class Program
    {
        

        static void Main(string[] args)
        {
            //using (var dsa = DSA.Create())
            //{
            //    using (var writer = File.CreateText("key.private"))
            //    {
            //        writer.Write(dsa.ToXmlString(true));
            //    }
            //    using (var writer = File.CreateText("key.public"))
            //    {
            //        writer.Write(dsa.ToXmlString(false));
            //    }
            //}

            var formatter = new BinaryFormatter();
            byte[] data;
            using (var ms = new MemoryStream())
            {
               
                formatter.Serialize(ms, Item.Create());
                data = ms.ToArray();
            }

            object obj;
            using (var ms2 = new MemoryStream(data))
            {
                obj = formatter.Deserialize(ms2);
            }
            byte b = 1;

            Console.WriteLine(string.Format("{0:X2}", b));
            Console.Read();
        }
    }
}
