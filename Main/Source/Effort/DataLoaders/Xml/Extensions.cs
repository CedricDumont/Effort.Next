using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace Effort.DataLoaders.Xml
{
    public static class Extensions
    {
        public static XElement Element(this XElement element, XName name, bool ignoreCase)
        {
            var el = element.Element(name);
            if (el != null)
                return el;

            if (!ignoreCase)
                return null;

            var elements = element.Elements().Where(e => e.Name.LocalName.ToString().ToLowerInvariant() == name.ToString().ToLowerInvariant());
            return elements.Count() == 0 ? null : elements.First();
        }

        public static Stream AsStream(this string content)
        {
            if (content == null)
            {
                return null;
            }
            using (MemoryStream stream = new MemoryStream())
            {
                MemoryStream destination = new MemoryStream();
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(content);
                    writer.Flush();
                    stream.Position = 0;
                    stream.CopyTo(destination);
                }
                destination.Position = 0;
                return destination;
            }

        }

        public static void RemoveLast(this StringBuilder sb)
        {
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
        }

        public static string EnclosedInXml(this object obj, string tag)
        {
            if (obj == null) { return ""; }
            return "<" + tag + ">" + obj.ToString() + "</" + tag + ">";
        }

        public static string ReadFromStart(this Stream stream)
        {
            if (stream == null)
            {
                return null;
            }
            if (stream.CanRead && stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }




    }

}
