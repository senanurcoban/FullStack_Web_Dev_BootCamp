using System.ComponentModel.DataAnnotations;
namespace BootcampEf.Data
{
    public class KursKayit
    {
        [Key]
        public int KayitId { get; set; }

        // foreign key->bağlantılı id değerleri için kullanılır.
        public int OgrenciId { get; set; }

        public Ogrenci Ogrenci { get; set; }  = null!;     // Tablolar arası ilişki kurma-(RDBMS)
        public int KursId { get; set; }
        public Kurs Kurs { get; set; } = null!;           // Tablolar arası ilişki kurma-(RDBMS)
        public DateTime KayitTarihi { get; set; }
    }
}