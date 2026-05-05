using Microsoft.Extensions.Logging;
using System.Xml.XPath;
using WebApiAbstractions.Dto;

namespace WebApiService.Extentions
{
    public class DocumentExtension(
        IMapper mapper,
        ILogger logger
        private readonly IDataBaseLayer _dbLayer;
    )
    {

        public DocumentDto GetDocument(string documentId)
        {

            var doc = dbLayer.GetDocument(documentId);

            var res = new DocumentDto();
            res.Id = id;

            return res;
        }
        public DocumentDto GetDocuments(string companyId)
        {

            var doc = DBLayer.GetDocuments(companyId);

            var res = new DocumentDto();
            res.Id = id;

            return res;
        }
        public DocumentDto DeleteDocuments(string[] ids)
        {

            var doc = DBLayer.GetDocument(id);

            var res = new DocumentDto();
            res.Id = id;

            return res;
        }
    }
}
