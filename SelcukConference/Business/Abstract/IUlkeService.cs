using System.Collections.Generic;
using DataAccess.Entities;

namespace Business.Abstract
{
    public interface IUlkeService
    {
        Ulke Get(int ulkeId);
        Ulke GetWInc(int id);
        List<Ulke> GetList();
        List<Ulke> GetListWInc();
    }
}
