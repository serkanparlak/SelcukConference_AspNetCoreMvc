using System.Collections.Generic;
using System.Linq;
using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Entities;

namespace Business.Concrete
{
    public class MenuManager : IMenuService
    {
        private readonly IMenuDal _menuDal;

        public MenuManager(IMenuDal menuDal)
        {
            _menuDal = menuDal;
        }

        private void ListeSiraAyarla(Menu m)
        {
            var menus = GetMenuListWithSubId(m.AltMenuId, m.Id);
            if (m.ListeSira == -1)
            {
                m.ListeSira = menus.Count > 0 ? menus.Max(max => max.ListeSira) + 1 : 1;
            }
            else
            {
                var menuId = m.ListeSira;
                m.ListeSira = _menuDal.Get(x => x.Id == menuId).ListeSira;
                var sira = m.ListeSira;

                foreach (var menu in menus)
                {
                    if (menu.ListeSira == sira)
                    {
                        menu.ListeSira = ++sira;
                        _menuDal.Update(menu);
                    }
                }
            }
        }

        public int Ekle(Menu m) //m.ListSira ile üst sırasına geçilecek menünün id bilgisi geliyor.
        {
            ListeSiraAyarla(m);
            return _menuDal.AddWithSave(m).Id;
        }

        public bool MenuKontrol(int id, string ad, string adEng)
        {
            return _menuDal.Control(x => x.Id != id && (x.MenuAdi == ad || x.MenuAdiEng == ad || x.MenuAdi == adEng || x.MenuAdiEng == adEng));
        }

        public void Guncelle(Menu m)
        {
            if (m.ListeSira == -2) // Menü adı değişikliği liste sırası değişmeyecek
            {
                var updMenu = _menuDal.Get(x => x.Id == m.Id);
                updMenu.MenuAdi = m.MenuAdi;
                updMenu.MenuAdiEng = m.MenuAdiEng;
                _menuDal.UpdateWithSave(updMenu);
            }
            else if (m.ListeSira == -3) // Menü içerik değişikliği
            {
                var updMenu = _menuDal.Get(x => x.Id == m.Id);
                updMenu.SayfaIcerik = m.SayfaIcerik;
                updMenu.SayfaIcerikEng = m.SayfaIcerikEng;
                _menuDal.UpdateWithSave(updMenu);
            }
            else // Menü adı değişikliği liste sırası değişecek
            {
                ListeSiraAyarla(m);
                _menuDal.UpdateWithSave(m);
            }

        }

        public void Sil(Menu m)
        {
            _menuDal.DeleteWithSave(m);
        }

        public Menu Anasayfa()
        {
            return _menuDal.Get(x => x.MenuAdi.ToUpper() == "ANASAYFA" || x.MenuAdiEng.ToUpper() == "HOME") ??
                   _menuDal.AddWithSave(new Menu { ListeSira = 0, MenuAdi = "Anasayfa", MenuAdiEng = "Home" });


        }

        public List<Menu> GetMenuListWithSubId(int subId, int menuId)
        {
            return _menuDal.GetList(x => x.AltMenuId == subId && x.Id != menuId).OrderBy(x => x.ListeSira).ToList();
        }

        public Menu GetMenu(int id)
        {
            return _menuDal.Get(x => x.Id == id);
        }

        public List<Menu> SiraliListe()
        {
            return _menuDal.GetOrderedList();
        }
    }
}
