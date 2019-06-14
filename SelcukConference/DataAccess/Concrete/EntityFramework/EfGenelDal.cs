using System.Linq;
using DataAccess.Abstract;
using DataAccess.Entities;
using _Core.DataAccess.EntityFramework;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfGenelDal : EfEntityRepository<Genel, DbtechconfContext>, IGenelDal
    {
        // -> "Db" vertabanı nesnesi adı

        public Genel GetFirstSingle()
        {
            return Db.Genel.FirstOrDefault();
        }
    }
}