using AttendanceSystem.Infrastructure.Security;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace AttendanceSystem.Models
{
    public partial class Ticket
    {
        private Encoding _encoding = Encoding.ASCII;

        public void GenerateQrCode()
        {
            var encoder = new QrEncoder();
            QrCode qrCode = null;
            var datatoEncoded = ComposeSignedDataString();
            encoder.TryEncode(datatoEncoded, out qrCode);

            using (var ms = new MemoryStream())
            {
                var render = new GraphicsRenderer(new FixedModuleSize(2, QuietZoneModules.Two),
                                                  Brushes.Black, Brushes.White);
                render.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);
                this.QrCode = ms.ToArray();
            }            
        }

        public bool Verify(string incoming)
        {
            var data = Convert.FromBase64String(incoming);
            var json = _encoding.GetString(data);
            var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            var rgb = Convert.FromBase64String(dic["rgb"]);
            var signature = Convert.FromBase64String(dic["signature"]);
            var ownedRgb = ComposeData();

            if (!IsArraySame(rgb, ownedRgb)) return false;
            
            using (var dsa = DSAHelper.GetPublicDsa())
            {
                return dsa.VerifySignature(rgb, signature);
            }
        }

        public static int ExtractStudentId(string incoming)
        {
            var data = Convert.FromBase64String(incoming);
            var json = Encoding.ASCII.GetString(data);
            var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return dic.ContainsKey("id") ? 0 : int.Parse(dic["id"]);
        }

        public byte[] ComposeSignedData()
        {
            var rgbHash = ComposeData();
            var signature = SignData(rgbHash);
            var dic = new Dictionary<string, string>();
            dic["id"] = this.StudentId.ToString();
            dic["rgb"] = Convert.ToBase64String(rgbHash);
            dic["signature"] = Convert.ToBase64String(signature);
            var strData = JsonConvert.SerializeObject(dic);
            return _encoding.GetBytes(strData);
        }

        public string ComposeSignedDataString()
        {
            return Convert.ToBase64String(ComposeSignedData());
        }

        public byte[] ComposeData()
        {
            using (var sha1 = System.Security.Cryptography.SHA1.Create())
            {
                var data = string.Format("Student:{0}_Class:{1}", this.StudentId, this.QrCode);
                var rgb = _encoding.GetBytes(data);
                return sha1.ComputeHash(rgb);
            }
        }
        
        private byte[] SignData(byte[] data)
        {
            using (var dsa = DSAHelper.GetPrivateDsa())
            {
                var result = dsa.CreateSignature(data);
                return result;
            }
        }

        private bool IsArraySame(byte[] arr1, byte[] arr2)
        {
            if (arr1 == null && arr2 == null) return true;
            else if ((arr1 == null || arr2 == null) ||
                     (arr1.Length != arr2.Length)) return false;

            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != arr2[i]) return false;
            }

            return true;
        }
    }
}