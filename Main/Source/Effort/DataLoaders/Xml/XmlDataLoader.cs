using Effort.DataLoaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Effort.DataLoaders.Xml
{
    public class XmlDataLoader : IDataLoader
    {
        //TODO : Replace this with my Filerepository pattern, to be sure that stream are correctly closed.
        private static IDictionary<string, Stream> _streamList = new Dictionary<string, Stream>();

        public XmlDataLoader()
        {
        }

        public XmlDataLoader(Stream stream)
        {
            InitStream(stream);
           // this._stream = stream;
        }

        public XmlDataLoader(string path)
        {
            if(!File.Exists(path))
            {
                throw new Exception("file does not exists : " + path);
            }
            //FileStream s = new FileStream()
            Stream s = File.OpenRead(path);
            InitStream(s);
           // this._stream = path.AsStream();
        }

        public void InitStream(Stream s)
        {
            String key = Guid.NewGuid().ToString();
            this.Argument = key;
            _streamList.Add(key, s);
        }

        public string Argument
        {
            get; set;
        }

        /// <summary>
        ///     Creates a <see cref="CsvTableDataLoaderFactory" /> instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="CsvTableDataLoaderFactory" /> instance.
        /// </returns>
        public ITableDataLoaderFactory CreateTableDataLoaderFactory()
        {
            Stream s = _streamList[Argument];
            
            XmlFileSource source = new XmlFileSource(s);

            return new XmlTableDataLoaderFactory(source);
        }
    }
}
