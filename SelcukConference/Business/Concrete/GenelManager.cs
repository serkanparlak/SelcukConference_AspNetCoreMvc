using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Entities;

namespace Business.Concrete
{
    public class GenelManager : IGenelService
    {
        private readonly IGenelDal _genelDal;

        public GenelManager(IGenelDal genelDal)
        {
            _genelDal = genelDal;
        }

        public Genel GetSingle()
        {
            var geneldata = _genelDal.GetFirstSingle();
            if (geneldata == null)
            {
                geneldata = new Genel();
                _genelDal.AddWithSave(geneldata);
            }
            return geneldata;
        }

        public void GuncelleKaydet(Genel genelData)
        {
            _genelDal.UpdateWithSave(genelData);
        }
    }
}
