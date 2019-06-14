using System.Collections.Generic;
using System.Linq;
using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Entities;

namespace Business.Concrete
{
    public class MesajManager : IMesajService
    {
        private readonly IMesajDal _mesajDal;

        public MesajManager(IMesajDal mesajDal)
        {
            _mesajDal = mesajDal;
        }


        public Mesaj GetMesajById(int id)
        {
            return _mesajDal.Get(x => x.Id == id);
        }

        public List<Mesaj> GetAll()
        {
            return _mesajDal.GetList();
        }

        public Mesaj GetWIncById(int id)
        {
            return _mesajDal.Get_WInc(x => x.Id == id);
        }

        public List<Mesaj> GetListBoth(int gonderenId, int aliciId)
        {
            return _mesajDal.GetList_WInc(x => x.GonderenId == gonderenId && x.AliciId == aliciId || x.GonderenId == aliciId && x.AliciId == gonderenId).OrderBy(x => x.IletimTarihi).ToList();
        }

        public Mesaj EkleKaydet(Mesaj msj)
        {
            return _mesajDal.AddWithSave(msj);
        }

        public Mesaj GetNewMessageCheck(int aId, int gId)
        {
            return _mesajDal.Get(x => x.AliciId == gId && x.GonderenId == aId && !x.Okundu);
        }

        public void GuncelleKaydet(Mesaj message)
        {
            _mesajDal.UpdateWithSave(message);
        }

        public void TopluGuncelleKaydet(List<Mesaj> messages)
        {
            _mesajDal.UpdateMultiple(messages);
            _mesajDal.Save();
        }

        public List<MessageNotify> GetForMesajNotify(int id)
        {
            return _mesajDal.GetForMesajNotify(id);
        }

        public List<MessageNotify> GetMyLastMessageList(int gonderenId)
        {
            return _mesajDal.GetMyLastMessageList(gonderenId);
        }

        public int GetSonOkunanMesajId(int aliciId, int gonderenId)
        {
            return _mesajDal.GetMyLastReadedMesageId(aliciId, gonderenId);
        }
    }
}
