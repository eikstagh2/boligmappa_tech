using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace WebApiAbstractions
{
    public class Property : DbContext
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid OwnerId { get; set; }
        [Required]
        public Adresse Adresse { get; set; }
        [Required]
        public Enum DocumentType{ get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public bool ExtendedExpirationDate { get; set; }


    }
}
