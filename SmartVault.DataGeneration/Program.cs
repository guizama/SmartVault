using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SmartVault.DataGeneration.Services;
using SmartVault.Shared;
using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace SmartVault.DataGeneration
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();

            var configuration = BuildConfiguration();

            var sharedDataPath = PathHelper.GetSharedFolder();

            Directory.CreateDirectory(sharedDataPath);

            var databasePath = Path.Combine(sharedDataPath, configuration["DatabaseFileName"]);

            SQLiteConnection.CreateFile(databasePath);

            var documentGenerator = new DocumentGeneratorService();

            var documentPath = Path.Combine(sharedDataPath, "TestDoc.txt");

            File.WriteAllText(documentPath, documentGenerator.Generate());

            var fakeDataFactory = new FakeDataFactory();

            var data = fakeDataFactory.Generate();

            using var connection = new SQLiteConnection(string.Format(configuration["ConnectionStrings:DefaultConnection"],databasePath));

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var schemaService = new DatabaseSchemaService();

            schemaService.CreateTables(connection);

            var bulkInsertService = new BulkInsertService();

            bulkInsertService.Insert(
                connection,
                transaction,
                data);

            transaction.Commit();

            PrintStatistics(connection);

            stopwatch.Stop();

            Console.WriteLine(
                $"Generation completed in {stopwatch.ElapsedMilliseconds} ms");
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        private static void PrintStatistics(SQLiteConnection connection)
        {
            var accountData = connection.Query("SELECT COUNT(*) FROM Account;");

            Console.WriteLine(
                $"AccountCount: {JsonConvert.SerializeObject(accountData)}");

            var documentData = connection.Query("SELECT COUNT(*) FROM Document;");

            Console.WriteLine(
                $"DocumentCount: {JsonConvert.SerializeObject(documentData)}");

            var userData = connection.Query("SELECT COUNT(*) FROM User;");

            Console.WriteLine(
                $"UserCount: {JsonConvert.SerializeObject(userData)}");
        }
    }
}