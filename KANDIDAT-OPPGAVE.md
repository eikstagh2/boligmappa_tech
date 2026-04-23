# Teknisk Intervjuoppgave — Senior Full-Stack .NET-utvikler

**Tidsbegrensning:** 2 timer (hjemmeoppgave)

> **Slik fungerer det:** Dette er en **hjemmeoppgave** som skal gjennomføres **før intervjuet**. Del 1 og 2 gjøres på egen hånd. Under selve intervjuet gjennomgår vi koden din sammen og diskuterer tilnærmingen og de arkitektoniske valgene dine (Del 3). Det kommer ingen overraskelser — diskusjonstemaene er listet nedenfor så du kan forberede deg.

---

## Bakgrunn

Du blir med i et team som vedlikeholder en **plattform for eiendomsdokumentasjon**. Systemet lar eiendomseiere lagre og hente dokumenter (plantegninger, el-sertifikater, tilstandsrapporter osv.) knyttet til eiendommene sine.

Den eksisterende backend-en er et eldre **.NET Framework 4.8 Web API** med en monolittisk SQL Server-database. Teamet har begynt migreringen til **.NET 8+** og ønsker å modernisere arkitekturen steg for steg.

Din oppgave er å bygge en liten, selvstendig funksjonalitet som viser hvordan du ville tilnærmet deg denne moderniseringen.

---

## Oppgaven: Varslingstjeneste for dokumentutløp

Eiendomsdokumenter kan ha en **utløpsdato** (f.eks. et el-tilsynssertifikat utløper etter 5 år). Forretningssiden ønsker en funksjon der:

1. Eiendomseiere kan se hvilke dokumenter som **utløper innen de neste 90 dagene**.
2. Et internt API kan kalles for å hente **alle utløpende dokumenter** på tvers av eiendommer (for en batch-varslingsjobb).
3. Eiendomseiere kan **utsette** en påminnelse for et spesifikt dokument (utsette med 30 dager).

---

## Del 1 — Backend API (.NET 8+) `[~60 min]`

Lag et **.NET 8+ Web API**-prosjekt med følgende:

### Datamodell

Design en EF Core-modell med minst disse entitetene:

- **Property** — `Id`, `Address`, `OwnerId`
- **Document** — `Id`, `PropertyId`, `Name`, `DocumentType`, `ExpiryDate`, `CreatedAt`
- **ReminderSnooze** — `Id`, `DocumentId`, `SnoozedUntil`, `SnoozedAt`

Bruk **EF Core med SQL Server** (LocalDB eller en in-memory-provider for øvelsen).

### API-endepunkter

| Metode | Rute | Beskrivelse |
|--------|------|-------------|
| `GET` | `/api/properties/{propertyId}/documents/expiring` | Hent dokumenter som utløper innen 90 dager for en eiendom. Utsettelsesregler må respekteres (utsatte dokumenter ekskluderes til utsettelsen utløper). |
| `GET` | `/api/documents/expiring` | Hent ALLE utløpende dokumenter på tvers av eiendommer (paginert). Dette er det interne endepunktet for batch-varslingsjobben. |
| `POST` | `/api/documents/{documentId}/snooze` | Utsett en dokumentpåminnelse. Bare eiendomseieren skal kunne utsette. |

### Krav

- **Ren arkitektur / prosjektstruktur** — Hvordan organiserer du lag, tjenester og ansvarsområder?
- **EF Core-bruk** — Effektive spørringer, riktig bruk av navigasjonsegenskaper, migreringstilnærming.
- **Inputvalidering og feilhåndtering** — Hvordan håndterer du ugyldig input, manglende ressurser, autorisasjon?
- **Paginering** — Implementer markør-basert eller offset-paginering på det interne endepunktet.
- **Autorisasjonslogikk** — Utsettelsesendepunktet skal verifisere eierskap. Du kan forenkle autentiseringsmekanismen, men *designet* skal være tydelig.
- **Moderne C#-funksjoner** — Bruk av records, pattern matching, nullable reference types, primary constructors osv. der det er hensiktsmessig.

### Bonus (hvis tiden tillater)

- Legg til en enkel **enhetstest** for utsettelseslogikken (verifiser at utsatte dokumenter ekskluderes korrekt).
- Legg til et **health check**-endepunkt.

---

## Del 2 — Frontend `[~40 min]`

Lag en **liten React- (eller Angular-) applikasjon** som:

1. Viser en liste over utløpende dokumenter for en gitt eiendom (kall det første endepunktet).
2. Hver dokumentrad viser: **Dokumentnavn**, **Type**, **Utløpsdato** og en **«Utsett 30 dager»**-knapp.
3. Når utsettelsesknappen klikkes, kall utsettelse-API-et og fjern dokumentet fra listen (optimistisk eller etter bekreftelse).
4. Vis en lastetilstand og håndter API-feil på en god måte.

### Krav

- **Komponentstruktur** — Hvordan bryter du ned brukergrensesnittet?
- **Tilstandshåndtering** — Hvordan hentes, caches og oppdateres data?
- **TypeScript-bruk** — Riktig typing, interfaces for API-responser.
- **UX-hensyn** — Lasteindikatorer, feilmeldinger, bekreftelse ved utsettelse.
- **CSS/styling** — Trenger ikke å være vakkert, men bør vise bevissthet rundt layout og responsivitet. Bruk den tilnærmingen du foretrekker (CSS-moduler, Tailwind, styled-components osv.).

---

## Del 3 — Arkitektur og diskusjon (under intervjuet) `[~20 min]`

Denne delen skjer **under selve intervjuet**, ikke som en del av hjemmeoppgaven. Vi gjennomgår koden din sammen og diskuterer valgene dine. Vær forberedt på å diskutere:

1. **Migrering fra legacy** — Det eksisterende systemet bruker .NET Framework 4.8 med ADO.NET og stored procedures. Hvordan ville du tilnærmet deg en migrering til .NET 8+ / EF Core stegvis? Hva er risikoene?

2. **Databasedesign** — Hvis systemet har 10 millioner dokumenter, hvordan ville du sikret at «utløpende dokumenter»-spørringen yter godt? Hvilke indekser ville du lagt til?

3. **Sky og containere** — Hvis denne tjenesten skulle deployes til Azure (eller AWS):
   - Hvordan ville du containerisert den?
   - Hvilke Azure-tjenester ville du brukt (App Service, AKS, Azure SQL osv.)?
   - Hvordan ville du håndtert batch-varslingsjobben (Azure Functions, Hangfire, hosted service)?

4. **CI/CD** — Beskriv hvordan du ville satt opp en CI/CD-pipeline for dette prosjektet. Hvilke steg? Hvilke kvalitetsporter?

5. **Kodegjennomgang** — Se på kodesnutten nedenfor og gi en gjennomgang. Hvilke problemer ser du? Hvordan ville du refaktorert den?

```csharp
// Legacy .NET Framework 4-kode — gjennomgå denne
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

## Kom i gang

1. **Fork** repositoriet på [https://github.com/SpirGroupPublic/boligmappa_tech](https://github.com/SpirGroupPublic/boligmappa_tech) til din egen GitHub-konto.
2. **Klon** din fork lokalt og opprett en ny branch for løsningen din.
3. Implementer løsningen — du kan bruke `dotnet new`, `npx create-react-app`, `ng new` eller andre verktøy du foretrekker.
4. **Commit** arbeidet ditt jevnlig med meningsfulle commit-meldinger — vi ønsker å se hvordan du jobber steg for steg.
5. **Push** branchen din og opprett en **Pull Request** tilbake til din egen forks `main`-branch.
6. Del lenken til din fork (eller PR) med oss.

> Du har tilgang til dokumentasjon og internett — dette er ikke en hukommelsestest.

## Hva du skal levere

- En lenke til ditt **forkede repository** med løsningen din på en feature-branch (med en åpen PR).
- Vi verdsetter **fremgang og resonering over perfeksjon** — det er helt greit om du ikke fullfører alt. Fortell oss hva du ville gjort videre.

---

*Lykke til! Vi ser etter pragmatisk, produksjonskvalitets-tenkning — ikke overengineering.*
