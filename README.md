
# 🚀 SmartVault Challenge (.NET 8)

This solution focuses on extending and improving an existing codebase, designing simple/clean solutions, and meeting functional requirements with attention to performance, memory usage, and testability.

The system generates a test SQLite database and executes application use cases for account document processing, including conditional document export and real file size computation using the filesystem as the source of truth.

## 🧰 Prerequisites

To run this solution you need:

- Visual Studio 2022+
- .NET 8 SDK
- (Optional) `dotnet` CLI

No local SQLite installation is required (the solution uses `System.Data.SQLite` via NuGet).

## 📌 Overview (Challenge requirements)

`Challenge.md` defines these requirements:

1. Speed up `SmartVault.DataGeneration` (developers complained it takes too long to create the test database).
2. All business objects must have a `CreatedOn` date.
3. Output the contents of every third file for an account into a single output file, if the file contains the text `"Smith Property"`.
4. Get the total file size of all files by reading the actual file size from disk (the database may be out of sync).
5. Add a new business object to support OAuth integrations (boilerplate only).
6. Commit your code.

> Requirements (2), (3), (4), and (5) were implemented directly. Requirement (1) was analyzed with optimization opportunities documented below.

## 🛠️ Technologies

- .NET 8 (apps/use-cases/tests)
- .NET Standard 2.0 (source generator)
- SQLite (`System.Data.SQLite`)
- Dapper
- Source Generators (Roslyn)
- xUnit

## 🏗️ Architecture / Projects

Projects and responsibilities:

- `SmartVault.CodeGeneration` → Source Generator that emits Business Object classes from additional XML files and injects `CreatedOn` when missing.
- `SmartVault.DataGeneration` → Console tool that creates the `.sqlite` file, creates the schema, and bulk-inserts fake data.
- `SmartVault.Program` → Console app that executes outputs required by the challenge (export + total size).
- `SmartVault.Application` → Use cases and a handler pipeline (Chain of Responsibility style).
- `SmartVault.Infrastructure` → Repositories (SQLite access via Dapper).
- `SmartVault.Shared` → Shared helpers (for example `PathHelper`).
- `SmartVault.Tests` → Automated tests (xUnit) for use cases/handlers.

## 🔄 Use Cases (Application)

### 1) Export “Smith Property” (every 3rd document)

Use case: `SmartVault.Application.UseCases.ExportSmithPropertyDocuments.ExportSmithPropertyDocumentsUseCase`

Pipeline (handlers):

- `GetDocumentsHandler` → loads documents by `AccountId` via repository.
- `FilterThirdDocumentsHandler` → keeps every third document.
- `ReadSmithPropertyHandler` → checks whether file content contains `"Smith Property"`.
- `ExportDocumentsHandler` → writes matching contents into a single output file.

The console app (`SmartVault.Program`) writes the output to `SharedFiles\SmithProperty.txt` by default.

### 2) Total size using real file sizes

Use case: `SmartVault.Application.UseCases.GetAllFileSizes.GetAllFileSizesUseCase`

- `GetFileSizesHandler` → reads document paths from the database and sums the sizes from disk (instead of trusting the `Length` stored in the DB).

## 🧠 Design Decisions

- **Handler pipeline (Chain of Responsibility)**: Use cases in `SmartVault.Application` are composed as a sequence of small handlers, each with a single responsibility (fetch → filter → validate → export). This keeps steps isolated and easy to test.
- **Repository abstraction**: Database access is isolated behind `IDocumentRepository` with a Dapper-based implementation in `SmartVault.Infrastructure`. This reduces coupling to SQLite and makes handlers simpler.
- **Filesystem as the source of truth for file sizes**: Total size is computed from the actual files on disk to avoid inconsistencies when the database `Length` column is stale.
- **Source Generator for business objects**: `SmartVault.CodeGeneration` emits business object classes and ensures a consistent `CreatedOn` property even when not present in the XML model.

## ▶️ How to Run (Important order)

You must generate the database first, then run the application.

### Step 1 — Generate the test database

Visual Studio:

1. Set `SmartVault.DataGeneration` as the Startup Project.
2. Run.

CLI:

- `dotnet run --project SmartVault.DataGeneration`

This creates the SQLite database file and a base test document file (for example `TestDoc.txt`) under `SharedFiles`.

### Step 2 — Run the application

Visual Studio:

1. Set `SmartVault.Program` as the Startup Project.
2. Run.

CLI:

- `dotnet run --project SmartVault.Program`

Expected output:

- Prints `Total file size: ... bytes`
- Generates/updates `SharedFiles\SmithProperty.txt` (export result)

## 🧪 Testing

Project: `SmartVault.Tests`

Run:

- `dotnet test`

## ⚠️ Notes / Assumptions

- The repository uses a shared folder (`SharedFiles`) for the database and document files.
- `Document.Length` exists in the database, but the requirement is to use actual file sizes from disk.
- The generator creates many documents per account (e.g. 100 accounts × 10,000 documents), which explains why `SmartVault.DataGeneration` can be slow.

## 🚀 Performance Considerations

### Implemented improvements

`SmartVault.DataGeneration` was refactored to reduce generation time compared to the original implementation that executed one SQL statement per row and recalculated file metadata repeatedly.

Key improvements:

- **Single transaction for all inserts** to minimize commit overhead.
- **Bulk-style inserts with Dapper** by passing collections directly into `Execute(...)` instead of executing one insert statement per entity.
- **Avoid repeated filesystem IO** by generating `TestDoc.txt` once and caching its full path and length (instead of creating `FileInfo` and reading `Length` for every document).
- **Centralized schema creation** via `DatabaseSchemaService` (instead of deserializing schemas and executing scripts inside the main loop).

### Next optimization steps (not implemented yet)

The following additional optimization opportunities were identified for `SmartVault.DataGeneration`:

- Reduce memory overhead (avoid building huge lists; generate and insert in batches)
- Use SQLite PRAGMAs / bulk strategies during generation
- Avoid repeated `DateTime.UtcNow` calls inside large loops

## ✅ Summary

This solution focuses on maintainability, separation of concerns, testability, and performance awareness while keeping the implementation simple and aligned with the challenge requirements.

Key aspects demonstrated in this implementation:

- Source Generator-based business object standardization (`CreatedOn`)
- SQLite test database generation tooling
- Handler pipeline for use cases (conditional export + real file size total)
- Automated tests with xUnit
