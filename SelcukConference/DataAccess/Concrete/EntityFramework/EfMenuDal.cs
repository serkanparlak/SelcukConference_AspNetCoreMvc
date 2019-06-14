using System.Collections.Generic;
using System.Linq;
using DataAccess.Abstract;
using DataAccess.Entities;
using _Core.DataAccess.EntityFramework;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfMenuDal : EfEntityRepository<Menu, DbtechconfContext>, IMenuDal
    {
        // -> "Db" vertabanı nesnesi adı
        public List<Menu> GetOrderedList()
        {
            return Db.Menu.OrderBy(x=>x.ListeSira).ToList();
        }
    }
}