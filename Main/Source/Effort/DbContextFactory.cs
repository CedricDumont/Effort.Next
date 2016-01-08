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
    public enum ConnectionBehaviour
    {
        Persistent,
        Transient
    }

    public static class DbContextFactory<T> where T : DbContext
    {
        /// <summary>
        /// Creates a DbContext of type T with a Transient connection behaviour
        /// </summary>
        /// <returns></returns>
        public static T Create()
        {
            return CreateInternal();
        }

        public static T Create(string xmlfileName, ConnectionBehaviour connectionBehaviour = ConnectionBehaviour.Transient, string connectionId = null)
        {
            IDataLoader loader = xmlfileName == null ? null : new CachingDataLoader(new XmlDataLoader(xmlfileName));

            return CreateInternal(loader, connectionBehaviour, connectionId);
        }

        public static T Create(Stream xmlStream, ConnectionBehaviour connectionBehaviour = ConnectionBehaviour.Transient, string connectionId = null)
        {
            IDataLoader loader = xmlStream == null ? null : new CachingDataLoader(new XmlDataLoader(xmlStream));

            return CreateInternal(loader, connectionBehaviour, connectionId);
        }

        public static T Create(IDataLoader loader, ConnectionBehaviour connectionBehaviour = ConnectionBehaviour.Transient, string connectionId = null)
        {
            return CreateInternal(loader, connectionBehaviour, connectionId);
        }

        private static T CreateInternal(
                    IDataLoader loader = null,
                    ConnectionBehaviour connectionBehaviour = ConnectionBehaviour.Transient,
                    string connectionId = null)
        {
            //preconditions
            if (connectionBehaviour == ConnectionBehaviour.Persistent && connectionId == null)
            {
                throw new ArgumentException($"USAGE : A {nameof(connectionId)} must be provided if {ConnectionBehaviour.Transient.ToString()} is used ");
            }

            DbConnection conn = null;

            if (connectionBehaviour == ConnectionBehaviour.Persistent)
            {
                conn = DbConnectionFactory.CreatePersistent(connectionId);
            }
            else
            {
                conn = DbConnectionFactory.CreateTransient(loader);
            }

            T instance = (T)Activator.CreateInstance(typeof(T), conn);

            if (!instance.Database.Exists())
            {
                instance.Database.Create();
            }

            if (loader != null && connectionBehaviour == ConnectionBehaviour.Persistent)
            {
                instance.RefreshContext(loader);
            }

            return instance;
        }
    }
}
