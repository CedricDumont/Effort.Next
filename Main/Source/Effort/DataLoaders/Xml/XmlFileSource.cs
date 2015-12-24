using Effort.DataLoaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Effort.DataLoaders.Xml
{
    public class XmlFileSource : IDisposable
    {

        private XElement xml ;

        private XDocument xmlDoc;

        public XmlFileSource(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            if (fi.Exists)
            {
                xml = XElement.Load(filePath);
            }
            else
            {
                //TODO : check here because the message is not correctly forwarded to test.
                throw new Exception(filePath + " is not a valid file path");
            }
        }

        public XmlFileSource(Stream stream)
        {
            if (stream != null)
            {
                stream.Position = 0;

                xml = XElement.Load(stream);
               // xml = xmlDoc.
            }
            else
            {
                //TODO : check here because the message is not correctly forwarded to test.
                throw new ArgumentException("stream was null");
            }
        }



        public IFileReference GetFile(TableDescription table)
        {
            try
            {
                Boolean columnNameWritten = false;

                //if (table.Name == "Travailleur" || table.Name == "Prime_Travailleur")
                //{
                StringBuilder sb = new StringBuilder();

                foreach (XElement row in xml.Elements(table.Name))
                {
                    if (!columnNameWritten)
                    {
                        WriteColumnNames(sb, table.Columns);
                        columnNameWritten = true;
                        sb.Append(Environment.NewLine);
                    }

                    WriteValues(sb, table.Columns, row);
                    sb.Append(Environment.NewLine);
                }

                return new XmlFileReference(sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
         


            //}
            //return null;
        }

        public void WriteColumnNames(StringBuilder sb, IEnumerable<ColumnDescription> columnMetaData)
        {
            foreach (ColumnDescription colummn in columnMetaData)
            {
                sb.Append(colummn.Name);
                sb.Append(",");
            }
            sb.RemoveLast();
        }

        public void WriteValues(StringBuilder sb, IEnumerable<ColumnDescription> columnMetaData, XElement row)
        {
            foreach (ColumnDescription colummn in columnMetaData)
            {
                var value = row.Element(colummn.Name, true);
                if (value != null)
                {
                    if (colummn.Type == typeof(string))
                    {
                        sb.Append("\"");
                        sb.Append(value.Value);
                        sb.Append("\"");
                    }
                    else
                    {
                        sb.Append(value.Value);
                    }
                }
                else
                {
                    if(colummn.Type == typeof(decimal))
                    {
                        sb.Append((decimal)0);
                    }
                    //if(colummn.Type == typeof(decimal))
                    //{
                    //    sb.Append((decimal)0);
                    //}
                }
                sb.Append(",");
            }

            sb.RemoveLast();
        }

        public void Dispose()
        {
        }
    }
}
