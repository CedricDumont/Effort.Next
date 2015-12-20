using Effort.DataLoaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Effort.DataLoaders.Xml
{
    public class XmlFileReference : IFileReference
    {

        private String _content;

        public XmlFileReference(String s)
        {
            _content = s;
        }
        public bool Exists
        {
            get {
                return true;
            }
        }

        public System.IO.Stream Open()
        {
            return _content.AsStream();
        }

       
    }
}
