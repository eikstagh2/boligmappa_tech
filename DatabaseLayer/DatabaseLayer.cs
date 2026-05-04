using WebApiAbstractions.Dto;

namespace DatabaseLayer
{
    public class DatabaseLayer : IDatabaseLayer
    {
        public DocumentDto GetDocument(string documentId)
        {
            throw new NotImplementedException();
        }

        public IList<DocumentDto> GetDocuments(string companyId)
        {
            throw new NotImplementedException();
        }
    }
}
