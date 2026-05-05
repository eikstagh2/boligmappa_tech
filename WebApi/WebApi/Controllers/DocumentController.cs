using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace WebApi.Controllers
{
    [Route("api/document/")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentExtension _documentExtension;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public DocumentController(IMapper mapper,
            ILogger logger,
            IDocumentExtentions documentExtentions
            ) 
        { 
            _mapper = mapper;
            _logger = logger;
            _documentExtension = documentExtentions;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetDocument(string documentId) {
            var result = await _documentExtension.GetDocument(documentId);
        }

        [HttpGet("expiring")]
        public async Task<ActionResult> Expiring(string documentId)
        {
            var result = await _documentExtension.GetExpiringDocuments(documentId);
        }

        [HttpGet("{companyID}")]
        public async Task<ActionResult> GetDocuments(string companyId)
        {
            var result = await _documentExtension.GetCompanyDocuments(companyId);
        }

        //[HttpDelete("{companyID}")]
        //public async Task<ActionResult> DeleteDocuments(string companyId, string[] documentIds)
        //{
        //    var result = await _documentExtension.DeleteDocuments(documentIds);
        //}
    }
}
