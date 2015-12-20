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
        private string path;

        public XmlDataLoader()
        {
        }

        public XmlDataLoader(string path)
        {
            if(!File.Exists(path))
            {
                throw new Exception("file does not exists : " + path);
            }
            this.path = path;
        }

        public string ContainerFolderPath
        {
            get
            {
                return this.path;
            }
        }

       
        string IDataLoader.Argument
        {
            get
            {
                return this.path;
            }

            set
            {
                this.path = value;
            }
        }

        /// <summary>
        ///     Creates a <see cref="CsvTableDataLoaderFactory" /> instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="CsvTableDataLoaderFactory" /> instance.
        /// </returns>
        public ITableDataLoaderFactory CreateTableDataLoaderFactory()
        {
            
            XmlFileSource source = new XmlFileSource(this.path);

            return new XmlTableDataLoaderFactory(source);
        }
    }
}
