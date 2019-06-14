using DataAccess.Entities;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface IBildiriService
    {
        Bildiri EkleKaydet(Bildiri yeniBildiri);
        Bildiri GetById(int bId);
        Bildiri GetWIncById(int bId);
        List<Bildiri> GetListByYazarId(int yId, int listId);
        List<Bildiri> GetListByEditorId(int eId, int listId);
        List<Bildiri> GetListByHakemId(int hId, int listId);
        void GuncelleKaydet(Bildiri bildiri);
        void SilKaydet(Bildiri bildiri);
    }
}