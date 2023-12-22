using System.ComponentModel.DataAnnotations;
namespace BootcampEf.Data
{
    public class Ogrenci 
    {
        // id->primary key
        [Key]
        public int OgrenciId { get; set; }
        public string? OgrenciAd { get; set; }
        public string? OgrenciSoyad { get; set; }

         // Birleştirme işlemi
         public string AdSoyad { 
            get 
            {
                return this.OgrenciAd + " " + this.OgrenciSoyad;
            } 
        }
        public string? Eposta { get; set; }
        public string? Telefon { get; set; }

        public ICollection<KursKayit> KursKayitlari { get; set; } = new List<KursKayit>();

    }
}