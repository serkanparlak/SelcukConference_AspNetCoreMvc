using System.Collections.Generic;
using DataAccess.Entities;

namespace Business.Abstract
{
    public interface ISehirService
    {
        List<Sehir> GetListByUlkeId(int ulkeId);
        Sehir GetById(int id);
        Sehir GetWIncById(int id);
        List<Sehir> All();

    }
}