using DataAccess.Entities;
using _Core.DataAccess;

namespace DataAccess.Abstract
{
    public interface IGenelDal : IEntityRepository<Genel>
    {
        //Custom operations
        Genel GetFirstSingle();
    }
}