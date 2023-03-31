using MyShop.Core.Contracts;
using MyShop.Core.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.SQL
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
    {
        internal DataContext context;
        internal DbSet<T> dbSet;

        public SQLRepository(DataContext context) {
            this.context = context;
            this.dbSet = context.Set<T>();
                
        }
        public void Commit()
        {
            context.SaveChanges();
        }

        public void Delete(string iD)
        {
            var t = Find(iD);
            if(context.Entry(t).State == EntityState.Detached) // check if the object is linked to the db
            {
                dbSet.Attach(t);//connects the object to the db
            }
            dbSet.Remove(t); // delete object
        }

        public T Find(string iD)
        {
            return dbSet.Find(iD);
        }

        public IQueryable<T> GetAll()
        {
            return dbSet;
        }

        public void Insert(T t)
        {
            dbSet.Add(t);
        }

        public void Update(T t)
        {
            dbSet.Attach(t);
            //updates the object 
            context.Entry(t).State = EntityState.Modified;
        }
    }
}
