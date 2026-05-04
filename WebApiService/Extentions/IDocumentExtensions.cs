using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiAbstractions.Dto;

namespace WebApiService.Extentions
{
    internal interface IDocumentExtensions
    {
        Task<string> GetDocument(DocumentDto dto);
        Task<IList<DocumentDto>> GetDocuments(string companyId)
        //Task<string> DeleteRetentionPolicyForTenant(string policyId, string tenantId);
    }
}
