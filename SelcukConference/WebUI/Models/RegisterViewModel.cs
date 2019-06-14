using System.Collections.Generic;
using DataAccess.Entities;

namespace WebUI.Models
{
    public class RegisterViewModel
    {
        public List<Ulke> Ulkeler { get; set; }
        public ICollection<Sehir> Turkiye { get; set; }
    }
}
