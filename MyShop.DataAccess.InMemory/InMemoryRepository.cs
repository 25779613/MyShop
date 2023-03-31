using MyShop.Core.Contracts;
using MyShop.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity // only accept T if it is a base entity , Base entity is a base class that contains an Id , other models must extend base Entity
    {
        ObjectCache cache = MemoryCache.Default;
        List<T> items;
        string className;

        public InMemoryRepository()
        {
            className = typeof(T).Name;
            items = cache[className] as List<T>;

            if (items == null)
            {
                items = new List<T>();
            }
        }

        public void Commit()
        {
            cache[className] = items;
        }

        public void Insert(T t)
        {
            if (String.IsNullOrEmpty(t.Id))
            {
                t.Id = Guid.NewGuid().ToString();
            }
            items.Add(t);
        }
        public void Update(T t)
        {
            T tToUpdate = items.Find(i => i.Id == t.Id);

            if (tToUpdate != null)
            {
                tToUpdate = t;
            }
            else
            {
                throw new Exception(className + " Not found");
            }
        }

        public T Find(string iD)
        {
            T t = items.Find(i => i.Id == iD);
            if (t != null)
            {
                return t;
            }
            else
            {
                throw new Exception(className + " not found");
            }
        }

        public IQueryable<T> GetAll()
        {
            return items.AsQueryable();
        }

        public void Delete(string iD)
        {
            T tToDelete = items.Find(p => p.Id == iD);

            if (tToDelete != null)
            {
                items.Remove(tToDelete);
            }
            else
            {
                throw new Exception(className + " not found");
            }
        }

        public static implicit operator InMemoryRepository<T>(ProductRepository v)
        {
            throw new NotImplementedException();
        }
    }
}
