using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Effort.DataLoaders;
using Effort.DataLoaders.Xml;

namespace Effort
{
    public static class DbContextFactory
    {
        //TODO  instead of filename, give  a kind of data content type.... application/xml or application/csv...
        public static T CreateFromPersistent<T>(string connectionName, string xmlfileName = null) where T : DbContext
        {
            IDataLoader loader = null;

            if (xmlfileName != null)
            {
                loader = new CachingDataLoader(new XmlDataLoader(xmlfileName));
            }
            else
            {
                loader = new EmptyDataLoader();
            }

            DbConnection conn = DbConnectionFactory.CreatePersistent(connectionName, loader);

            T instance = (T)Activator.CreateInstance(typeof(T), conn);

            return instance;
        }
    }
}
