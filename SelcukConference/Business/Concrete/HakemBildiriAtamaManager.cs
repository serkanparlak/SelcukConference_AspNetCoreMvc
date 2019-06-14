using System.Collections.Generic;
using System.Linq;
using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Entities;

namespace Business.Concrete
{
    public class HakemBildiriAtamaManager : IHakemBildiriAtamaService
    {
        private readonly IHakemBildiriAtamaDal _hakemBildiriAtamaDal;
        private readonly IPaperNotifyService _paperNotifyService;

        public HakemBildiriAtamaManager(IHakemBildiriAtamaDal hakemBildiriAtamaDal, IPaperNotifyService paperNotifyService)
        {
            _hakemBildiriAtamaDal = hakemBildiriAtamaDal;
            _paperNotifyService = paperNotifyService;
        }

        public void EkleSilKaydet(int bildiriId, int[] seciliHakemIds, int editorId)
        {
            var silinecekHakemler = _hakemBildiriAtamaDal.GetList(x => x.BildiriId == bildiriId && !seciliHakemIds.Contains(x.HakemId));
            _hakemBildiriAtamaDal.TopluSil(silinecekHakemler);
            var silinecekBildirimler = _paperNotifyService.GetList(x => x.PaperId == bildiriId && !seciliHakemIds.Contains(x.UyeId) && x.UyeId != editorId);
            _paperNotifyService.RemovePaperNotifyMultiple(silinecekBildirimler);
            foreach (var hId in seciliHakemIds)
            {
                if (!_hakemBildiriAtamaDal.Control(x => x.BildiriId == bildiriId && x.HakemId == hId))
                {
                    _hakemBildiriAtamaDal.Add(new HakemBildiriAtama
                    {
                        BildiriId = bildiriId,
                        HakemId = hId,
                        Sonuc = 2
                    });
                    _paperNotifyService.AddPaperNotify(new PaperNotify // bildiri oluşturuldu
                    {
                        PaperId = bildiriId,
                        UyeId = hId,
                        BildiriMesaj = "Bir bildiride hakem olarak atandınız",
                        OkunduMu = false
                    });
                }
            }
            _hakemBildiriAtamaDal.Save();
            _paperNotifyService.Kaydet();
        }

        public void DeleteByBildiriId(int bId)
        {
            var list = _hakemBildiriAtamaDal.GetList(x => x.BildiriId == bId);
            foreach (var item in list)
            {
                _hakemBildiriAtamaDal.Delete(item);
            }
        }

        public HakemBildiriAtama Ekle(HakemBildiriAtama hakemBildiriAtama)
        {
            return _hakemBildiriAtamaDal.Add(hakemBildiriAtama);
        }

        public void Kaydet()
        {
            _hakemBildiriAtamaDal.Save();
        }

        public HakemBildiriAtama GetById(int hbId)
        {
            return _hakemBildiriAtamaDal.Get(x => x.Id == hbId);
        }

        public HakemBildiriAtama GetWIncById(int hbId)
        {
            return _hakemBildiriAtamaDal.Get_WInc(x => x.Id == hbId);
        }

        public void GuncelleKaydet(HakemBildiriAtama hakemBildiri)
        {
            _hakemBildiriAtamaDal.UpdateWithSave(hakemBildiri);
        }

        public void TopluGuncelleKaydet(List<HakemBildiriAtama> hakemler)
        {
            _hakemBildiriAtamaDal.UpdateMultiple(hakemler);
        }

        public List<HakemBildiriAtama> GetByBildiriId(int bId)
        {
            return _hakemBildiriAtamaDal.GetList(x => x.BildiriId == bId);
        }

        public bool Kontrol(int bId)
        {
            return _hakemBildiriAtamaDal.Control(x => x.BildiriId == bId);
        }
    }
}