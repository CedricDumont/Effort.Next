using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Effort.DataLoaders;
using Effort.DataLoaders.Xml;

namespace Effort
{
    public static class DbContextFactory
    {
        public static T CreateFromPersistent<T>(string connectionName, string xmlfileName) where T : DbContext
        {
            IDataLoader loader = xmlfileName == null ? null : new CachingDataLoader(new XmlDataLoader(xmlfileName));

            return CreateFromPersistent<T>(connectionName, loader);
        }

        public static T CreateFromPersistent<T>(string connectionName, Stream xmlStream) where T : DbContext
        {
            IDataLoader loader = xmlStream == null ? null : new CachingDataLoader(new XmlDataLoader(xmlStream));

            return CreateFromPersistent<T>(connectionName, loader);
        }

        public static T CreateFromPersistent<T>(string connectionName, IDataLoader loader = null) where T : DbContext
        {
            DbConnection conn = null;

            if (loader == null)
            {
                conn = DbConnectionFactory.CreatePersistent(connectionName, new EmptyDataLoader());
            }
            else
            {
                conn = DbConnectionFactory.CreatePersistent(connectionName);
            }

            T instance = (T)Activator.CreateInstance(typeof(T), conn);

            if (!instance.Database.Exists())
            {
                instance.Database.Create();
            }

            if (loader != null)
            {
                instance.RefreshContext(loader);
            }

            return instance;
        }

      
    }
}
