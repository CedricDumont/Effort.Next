using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Effort.DataLoaders;
using Effort.Provider;

namespace Effort
{
    public static class DbContextExtensions
    {

        public static void RefreshContext(this DbContext ctx, IDataLoader loader = null)
        {
            EffortConnection connection = ctx.Database.Connection as EffortConnection;

            if(connection == null)
            {
                throw new Exception("This extension method may only be used by DbContext linked to an EffortConnection");
            }

            
                var entries = ctx.ChangeTracker.Entries();

                var total = entries.Count();

                foreach (var item in entries)
                {
                    item.State = EntityState.Detached;
                }
            

            connection.LoadData(loader);
        }
    }
}
