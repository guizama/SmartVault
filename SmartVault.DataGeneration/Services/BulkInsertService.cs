using Dapper;
using SmartVault.DataGeneration.DTOs;
using System.Data.SQLite;

namespace SmartVault.DataGeneration.Services
{
    public class BulkInsertService
    {
        public void Insert(
            SQLiteConnection connection,
            SQLiteTransaction transaction,
            GeneratedDataDto data)
        {
            connection.Execute(@"INSERT INTO Account (Id, Name, CreatedOn) VALUES (@Id, @Name, @CreatedOn)", data.Accounts, transaction: transaction);
            connection.Execute(@"INSERT INTO User (Id, FirstName, LastName, DateOfBirth, AccountId, Username, Password, CreatedOn) VALUES (@Id, @FirstName, @LastName, @DateOfBirth, @AccountId, @Username, @Password, @CreatedOn)", data.Users, transaction: transaction);
            connection.Execute(@"INSERT INTO Document (Id, Name, FilePath, Length, AccountId, CreatedOn) VALUES (@Id, @Name, @FilePath, @Length, @AccountId, @CreatedOn)", data.Documents, transaction: transaction);
            connection.Execute(@"INSERT INTO OAuthIntegration (Id, Provider, ClientId, ClientSecret, RedirectUrl, CreatedOn) VALUES (@Id, @Provider, @ClientId, @ClientSecret, @RedirectUrl, @CreatedOn)", data.OAuthIntegrations, transaction: transaction);
        }
    }
}