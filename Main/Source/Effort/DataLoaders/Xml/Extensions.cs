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
    internal static class Extensions
    {
        internal static XElement Element(this XElement element, XName name, bool ignoreCase)
        {
            var el = element.Element(name);
            if (el != null)
                return el;

            if (!ignoreCase)
                return null;

            var elements = element.Elements().Where(e => e.Name.LocalName.ToString().ToLowerInvariant() == name.ToString().ToLowerInvariant());
            return elements.Count() == 0 ? null : elements.First();
        }

        internal static Stream AsStream(this string content)
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

        internal static void RemoveLast(this StringBuilder sb)
        {
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
        }





    }

}
