using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiAbstractions.Dto;

namespace DatabaseLayer
{
    internal interface IDatabaseLayer
    {
        public DocumentDto GetDocument(string documentId);
        public IList<DocumentDto> GetDocuments(string companyId);
        //public void DeleteDocuments(string[] documentIds);

    }
}
