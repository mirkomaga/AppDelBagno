using System.ComponentModel.DataAnnotations;

namespace AppDelBagno.Models
{
    public class Coda
    {
        public int Id { get; set; }
        public string Utente { get; set; }

        [DataType(DataType.Date)]
        public DateTime? datetime { get; set; }

    }
}
