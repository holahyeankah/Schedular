using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Customer.Models.DatabaseModels
{
    public class Lga
    {
        [Key]
        public int LgaId { get; set; }

        public int LgaName { get; set; }

        public int StateId { get; set; }


    }
}
