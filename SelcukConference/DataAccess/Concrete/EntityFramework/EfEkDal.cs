using DataAccess.Abstract;
using DataAccess.Entities;
using _Core.DataAccess.EntityFramework;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfEkDal : EfEntityRepository<Ek, DbtechconfContext>, IEkDal
    {

    }
}