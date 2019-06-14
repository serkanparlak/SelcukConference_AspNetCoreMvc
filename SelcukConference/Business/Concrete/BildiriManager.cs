using System.Collections.Generic;
using System.Linq;
using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Entities;

namespace Business.Concrete
{
    public class BildiriManager : IBildiriService
    {
        private readonly IBildiriDal _bildiriDal;
        private readonly IHakemBildiriAtamaDal _hakemBildiriAtamaDal;

        public BildiriManager(IBildiriDal bildiriDal, IHakemBildiriAtamaDal hakemBildiriAtamaDal)
        {
            _bildiriDal = bildiriDal;
            _hakemBildiriAtamaDal = hakemBildiriAtamaDal;
        }
        
        public Bildiri EkleKaydet(Bildiri yeniBildiri)
        {
            return _bildiriDal.AddWithSave(yeniBildiri);
        }

        public Bildiri GetById(int bId)
        {
            return _bildiriDal.Get(x => x.Id == bId);
        }

        public Bildiri GetWIncById(int bId)
        {
            return _bildiriDal.Get_WInc(x => x.Id == bId);
        }

        public List<Bildiri> GetListByYazarId(int yId, int listId)
        {
            return listId == 1
                ? _bildiriDal.GetList_WInc(x => x.YazarId == yId)
                : _bildiriDal.GetList_WInc(x => x.YazarId == yId && x.Sonuc == listId);
        }

        public List<Bildiri> GetListByEditorId(int eId, int listId)
        {
            return listId == 1
                ? _bildiriDal.GetList_WInc(x => x.EditorId == eId)
                : _bildiriDal.GetList_WInc(x => x.EditorId == eId && x.Sonuc == listId);
        }

        public List<Bildiri> GetListByHakemId(int hId, int listId)
        {
            return listId == 1
                ? _hakemBildiriAtamaDal.GetList_WInc(x => x.HakemId == hId).Select(x => x.Bildiri).ToList()
                : _hakemBildiriAtamaDal.GetList_WInc(x => x.HakemId == hId && x.Sonuc == listId).Select(x => x.Bildiri).ToList();
        }

        public void GuncelleKaydet(Bildiri bildiri)
        {
            _bildiriDal.UpdateWithSave(bildiri);
        }

        public void SilKaydet(Bildiri bildiri)
        {
            _bildiriDal.DeleteWithSave(bildiri);
        }
       
    }
}