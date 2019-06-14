using System.Collections.Generic;
using DataAccess.Entities;

namespace Business.Abstract
{
    public interface IMesajService
    {
        Mesaj GetMesajById(int id);
        List<Mesaj> GetAll();
        Mesaj GetWIncById(int id);
        List<Mesaj> GetListBoth(int gonderenId, int aliciId);
        Mesaj EkleKaydet(Mesaj msj);
        Mesaj GetNewMessageCheck(int aId, int gId);
        void GuncelleKaydet(Mesaj message);
        void TopluGuncelleKaydet(List<Mesaj> messages);
        List<MessageNotify> GetForMesajNotify(int id);
        List<MessageNotify> GetMyLastMessageList(int gonderenId);
        int GetSonOkunanMesajId(int aliciId, int gonderenId);
    }


}