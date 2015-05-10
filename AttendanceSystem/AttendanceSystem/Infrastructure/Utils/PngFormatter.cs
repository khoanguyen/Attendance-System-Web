using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;

namespace AttendanceSystem.Infrastructure.Utils
{
    public class PngFormatter : BufferedMediaTypeFormatter
    {
        public override bool CanReadType(Type type)
        {
            return type == typeof(byte[]);
        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof(byte[]);
        }

        public override void WriteToStream(Type type, object value, System.IO.Stream writeStream, System.Net.Http.HttpContent content)
        {
            var data = value as byte[];
            writeStream.Write(data, 0, data.Length);
        }
    }
}