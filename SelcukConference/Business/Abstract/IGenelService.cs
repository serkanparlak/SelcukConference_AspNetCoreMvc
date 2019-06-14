using DataAccess.Entities;

namespace Business.Abstract
{
    public interface IGenelService
    {
        Genel GetSingle();
        void GuncelleKaydet(Genel genelData);
    }


}