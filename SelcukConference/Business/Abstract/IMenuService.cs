using System.Collections.Generic;
using DataAccess.Entities;

namespace Business.Abstract
{
    public interface IMenuService
    {
        Menu GetMenu(int id);
        List<Menu> SiraliListe();
        int Ekle(Menu m);
        bool MenuKontrol(int id, string ad, string adEng);
        void Guncelle(Menu m);
        void Sil(Menu m);
        Menu Anasayfa();
        List<Menu> GetMenuListWithSubId(int subId, int menuId);
    }


}