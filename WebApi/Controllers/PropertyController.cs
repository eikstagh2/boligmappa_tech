using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/properties/")]
    [ApiController]
    public class PopertiesController : ControllerBase
    {

        public PopertiesController(
            ) { }

        [HttpGet("{propertyid}/documents/expiring")]
        public class GetDocument(string documentId) { 
        }

    }
}
