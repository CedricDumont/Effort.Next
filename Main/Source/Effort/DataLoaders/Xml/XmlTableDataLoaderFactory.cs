using Effort.DataLoaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Effort.DataLoaders.Xml
{
    internal class XmlTableDataLoaderFactory : ITableDataLoaderFactory
    {
        private readonly XmlFileSource xmlsource;

        public XmlTableDataLoaderFactory(XmlFileSource source)
        {
            this.xmlsource = source;
        }

      
        public ITableDataLoader CreateTableDataLoader(TableDescription table)
        {
            try
            {
                IFileReference file = this.xmlsource.GetFile(table);

                if (file == null || !file.Exists)
                {
                    return new EmptyTableDataLoader();
                }

                return new CsvTableDataLoader(file, table);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///     Does nothing.
        /// </summary>
        public void Dispose()
        {
            xmlsource.Dispose();
        }
    }
}
