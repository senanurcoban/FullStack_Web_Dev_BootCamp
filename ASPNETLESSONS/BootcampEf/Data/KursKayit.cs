using System.ComponentModel.DataAnnotations;
namespace BootcampEf.Data
{
    public class KursKayit
    {
        [Key]
        public int KayitId { get; set; }

        // foreign key->bağlantılı id değerleri için kullanılır.
        public int OgrenciId { get; set; }
        public int KursId { get; set; }
        public DateTime KayitTarihi { get; set; }
    }
}