using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiAbstractions.Enums;


namespace WebApiAbstractions
{
    public class Document
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid PropertyId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DocumentType DocumentType{ get; set; }
        [Required]
        public DateOnly CreatedAt { get; set; }
        [Required]
        public DateOnly ExpiryDate { get; set; }
        public bool ExtendedExpiration { get; set; }
        //Use a path to directory
        [Required]
        public string PathToDocument { get; set; }
        //Or store it in the DB
        [Required]
        public byte[] DocumentContent { get; set; }


    }
}
