using MyShop.Core.Model;
using System.Linq;

namespace MyShop.Core.Contracts

{
    public interface IRepository<T> where T : BaseEntity
    {
        void Commit();
        void Delete(string iD);
        T Find(string iD);
        IQueryable<T> GetAll();
        void Insert(T t);
        void Update(T t);
    }
}