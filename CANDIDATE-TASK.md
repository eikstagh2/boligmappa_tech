# Technical Interview Task — Senior Full-Stack .NET Developer

**Time limit:** 2 hours

---

## Background

You are joining a team that maintains a **property documentation platform**. The system allows property owners to store and retrieve documents (floor plans, electrical certificates, inspection reports, etc.) related to their properties.

The existing backend is a legacy **.NET Framework 4.8 Web API** with a monolithic SQL Server database. The team has started migrating to **.NET 8** and wants to modernize the architecture incrementally.

Your task is to build a small, self-contained feature that demonstrates how you would approach this modernization.

---

## The Task: Document Expiry Notification Service

Property documents can have an **expiry date** (e.g., an electrical inspection certificate expires after 5 years). The business wants a feature where:

1. Property owners can see which of their documents are **expiring within the next 90 days**.
2. An internal API can be called to retrieve **all expiring documents** across all properties (for a batch notification job).
3. Property owners can **snooze** a reminder for a specific document (defer it by 30 days).

---

## Part 1 — Backend API (.NET 8) `[~60 min]`

Create a **.NET 8 Web API** project with the following:

### Data Model

Design an EF Core model with at least these entities:

- **Property** — `Id`, `Address`, `OwnerId`
- **Document** — `Id`, `PropertyId`, `Name`, `DocumentType`, `ExpiryDate`, `CreatedAt`
- **ReminderSnooze** — `Id`, `DocumentId`, `SnoozedUntil`, `SnoozedAt`

Use **EF Core with SQL Server** (LocalDB or an in-memory provider for the exercise).

### API Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/properties/{propertyId}/documents/expiring` | Get documents expiring within 90 days for a property. Snooze rules must be respected (snoozed documents are excluded until their snooze expires). |
| `GET` | `/api/documents/expiring` | Get ALL expiring documents across all properties (paginated). This is the internal endpoint for the notification batch job. |
| `POST` | `/api/documents/{documentId}/snooze` | Snooze a document reminder. Only the property owner should be able to snooze. |

### Requirements

- **Clean architecture / project structure** — How do you organize layers, services, and concerns?
- **EF Core usage** — Efficient queries, proper use of navigation properties, migrations approach.
- **Input validation & error handling** — How do you handle invalid input, missing resources, authorization?
- **Pagination** — Implement cursor-based or offset pagination on the internal endpoint.
- **Authorization logic** — The snooze endpoint should verify ownership. You can mock/simplify the auth mechanism, but the *design* should be clear.
- **Modern C# features** — Use of records, pattern matching, nullable reference types, primary constructors, etc. where appropriate.

### Bonus (if time permits)

- Add a simple **unit test** for the snooze logic (verify that snoozed documents are correctly excluded).
- Add a **health check** endpoint.

---

## Part 2 — Frontend `[~40 min]`

Create a **small React (or Angular) application** that:

1. Displays a list of expiring documents for a given property (call the first endpoint).
2. Each document row shows: **Document name**, **Type**, **Expiry date**, and a **"Snooze 30 days"** button.
3. When the snooze button is clicked, call the snooze API and remove the document from the list (optimistically or after confirmation).
4. Show a loading state and handle API errors gracefully.

### Requirements

- **Component structure** — How do you break down the UI?
- **State management** — How is data fetched, cached, and updated?
- **TypeScript usage** — Proper typing, interfaces for API responses.
- **UX considerations** — Loading indicators, error messages, confirmation on snooze.
- **CSS/styling** — Doesn't need to be beautiful, but should demonstrate awareness of layout and responsiveness. Use whatever approach you prefer (CSS modules, Tailwind, styled-components, etc.).

---

## Part 3 — Architecture & Discussion `[~20 min]`

After coding, we will have a conversation about your choices. Be prepared to discuss:

1. **Legacy migration** — The existing system uses .NET Framework 4.8 with ADO.NET and stored procedures. How would you approach migrating to .NET 8 + EF Core incrementally? What are the risks?

2. **Database design** — If the system has 10 million documents, how would you ensure the "expiring documents" query performs well? What indexes would you add?

3. **Cloud & containers** — If this service were deployed to Azure (or AWS):
   - How would you containerize it?
   - What Azure services would you use (App Service, AKS, Azure SQL, etc.)?
   - How would you handle the batch notification job (Azure Functions, Hangfire, hosted service)?

4. **CI/CD** — Describe how you would set up a CI/CD pipeline for this project. What stages? What quality gates?

5. **Code review** — Look at the legacy code snippet below and provide a review. What issues do you see? How would you refactor it?

```csharp
// Legacy .NET Framework 4 code — review this
public class DocumentService
{
    public List<DocumentDto> GetExpiringDocuments(int propertyId)
    {
        var documents = new List<DocumentDto>();
        var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var query = "SELECT * FROM Documents WHERE PropertyId = " + propertyId + 
                        " AND ExpiryDate < DATEADD(day, 90, GETDATE()) AND ExpiryDate > GETDATE()";
            
            using (var command = new SqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var doc = new DocumentDto();
                        doc.Id = (int)reader["Id"];
                        doc.Name = (string)reader["Name"];
                        doc.ExpiryDate = (DateTime)reader["ExpiryDate"];
                        doc.PropertyId = (int)reader["Id"];
                        documents.Add(doc);
                    }
                }
            }
        }
        return documents;
    }
}
```

---

## Getting Started

1. **Fork** the repository at [https://github.com/SpirGroupPublic/boligmappa_tech](https://github.com/SpirGroupPublic/boligmappa_tech) to your own GitHub account.
2. **Clone** your fork locally and create a new branch for your solution.
3. Implement your solution — you may scaffold projects using `dotnet new`, `npx create-react-app`, `ng new`, or any tooling you prefer.
4. **Commit** your work regularly with meaningful commit messages — we want to see how you work incrementally.
5. **Push** your branch and open a **Pull Request** back to your own fork's `main` branch.
6. Share the link to your fork (or PR) with us.

> You have access to documentation and the internet — this is not a memory test.

## What to Submit

- A link to your **forked repository** with your solution on a feature branch (with a PR open).
- We value **progress and reasoning over perfection** — it's fine if you don't finish everything. Tell us what you would do next.

---

*Good luck! We're looking for pragmatic, production-quality thinking — not gold-plating.*
