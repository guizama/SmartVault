using SmartVault.DataGeneration.DTOs;
using SmartVault.DataGeneration.Models;
using System;
using System.IO;

namespace SmartVault.DataGeneration.Services
{
    public class FakeDataFactory
    {
        private static readonly Random Random = new();

        private const int TotalAccounts = 100;

        private const int DocumentsPerAccount = 10000;

        public GeneratedDataDto Generate()
        {
            var data = new GeneratedDataDto();

            var documentNumber = 0;

            var basePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\SharedFiles");

            var documentPath = Path.GetFullPath(Path.Combine(basePath, "TestDoc.txt"));

            var documentLength = new FileInfo(documentPath).Length;

            for (int i = 0; i < TotalAccounts; i++)
            {
                var randomDate = RandomDay();

                data.Accounts.Add(new AccountRecord
                {
                    Id = i,
                    Name = $"Account{i}",
                    CreatedOn = DateTime.UtcNow
                });

                data.Users.Add(new UserRecord
                {
                    Id = i,
                    FirstName = $"FName{i}",
                    LastName = $"LName{i}",
                    DateOfBirth = randomDate,
                    AccountId = i,
                    Username = $"UserName-{i}",
                    Password = "e10adc3949ba59abbe56e057f20f883e",
                    CreatedOn = DateTime.UtcNow
                });

                data.OAuthIntegrations.Add(new OAuthIntegrationRecord
                {
                    Id = i,
                    Provider = "Google",
                    ClientId = $"client-id-{i}",
                    ClientSecret = $"secret-{i}",
                    RedirectUrl = "https://localhost/callback",
                    CreatedOn = DateTime.UtcNow
                });

                for (int d = 0; d < DocumentsPerAccount; d++, documentNumber++)
                {
                    data.Documents.Add(new DocumentRecord
                    {
                        Id = documentNumber,
                        Name = $"Document{i}-{d}.txt",
                        FilePath = documentPath,
                        Length = documentLength,
                        AccountId = i,
                        CreatedOn = DateTime.UtcNow
                    });
                }
            }

            return data;
        }

        private static DateTime RandomDay()
        {
            DateTime start = new(1985, 1, 1);

            int range = (DateTime.Today - start).Days;

            return start.AddDays(Random.Next(range));
        }
    }
}