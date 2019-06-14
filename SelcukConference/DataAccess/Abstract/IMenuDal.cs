using System.Collections.Generic;
using DataAccess.Entities;
using _Core.DataAccess;

namespace DataAccess.Abstract
{
    public interface IMenuDal : IEntityRepository<Menu>
    {
        //Custom operations
        List<Menu> GetOrderedList();
    }
}