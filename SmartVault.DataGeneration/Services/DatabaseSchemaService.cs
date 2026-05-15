using Dapper;
using SmartVault.Library;
using System.Data.SQLite;
using System.IO;
using System.Xml.Serialization;

namespace SmartVault.DataGeneration.Services
{
    public class DatabaseSchemaService
    {
        public void CreateTables(SQLiteConnection connection)
        {
            var files = Directory.GetFiles(@"..\..\..\..\BusinessObjectSchema");

            foreach (var file in files)
            {
                var serializer = new XmlSerializer(typeof(BusinessObject));

                using var streamReader = new StreamReader(file);

                var businessObject =
                    serializer.Deserialize(streamReader) as BusinessObject;

                connection.Execute(businessObject?.Script);
            }
        }
    }
}