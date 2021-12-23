using System.ComponentModel.DataAnnotations;

namespace AppDelBagno.Models
{
    public class Bagno
    {
        public int Id { get; set; }
        public string Utente { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Entrata { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Uscita { get; set; }

    }

}
