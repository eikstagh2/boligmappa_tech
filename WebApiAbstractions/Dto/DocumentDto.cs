using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiAbstractions.Enums;

namespace WebApiAbstractions.Dto
{
    public class DocumentDto
    {
        public string Id { get; set; }
        public Guid FkPropertyId { get; set; }
        public string Name { get; set; }
        public DocumentType DocumentType { get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public bool ExtendedExpiration { get; set; }

        [Required]
        public byte[] DocumentContent { get; set; }

    }
}
