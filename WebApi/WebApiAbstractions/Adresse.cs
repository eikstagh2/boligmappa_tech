using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace WebApiAbstractions
{
    public class Adresse
    {
        [Required]
        public string Address { get; set; }
        public string Address2 { get; set; }

        [Required]
        public string PostCode{ get; set; }
        [Required]
        public string City{ get; set; }


    }
}
