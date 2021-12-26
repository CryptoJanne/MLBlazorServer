using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using Raven.Client;
using Raven.Client.Documents;
using System.Security.Cryptography.X509Certificates;



namespace APIHandler
{
    public class ServerConfigJson
    {
        public string url { get; set; }
        public string certificatePath { get; set; }
        public string password { get; set; }
    }
    public static class JsonFileReader
    {
        public static async Task<T> ReadAsync<T>(string filePath)
        {
            using FileStream stream = File.OpenRead(filePath);
            return await JsonSerializer.DeserializeAsync<T>(stream);
        }
    }
    public class DocumentStoreHolder
    {
        //ServerConfigJson asdf = await 
        private static readonly Lazy<IDocumentStore> LazyStore =
            new Lazy<IDocumentStore>(() =>
            {
                var jsonString = File.ReadAllText("serverconf.json");
                var configParsed = JsonSerializer.Deserialize<ServerConfigJson>(jsonString);
                X509Certificate2 clientCertificate = new X509Certificate2(configParsed.certificatePath, configParsed.password);
                var store = new DocumentStore
                {
                    Certificate = clientCertificate,
                    Urls = new[] { configParsed.url },
                    Database = "ServerHealth"
                };

            return store.Initialize();
            });

        public static IDocumentStore Store =>
            LazyStore.Value;
    }
    public class ServerAPIHandler
    {
        
        public string jesus = "asdf";
        
        public async Task<List<SystemStats>> QueryTest()
        {
            DateTime now = DateTime.Now;
            DateTime tenMinAgo = now.Subtract(TimeSpan.FromMinutes(10));
            using (var asyncSession = DocumentStoreHolder.Store.OpenAsyncSession())
            {

                List<SystemStats> data = await asyncSession
                    .Query<SystemStats>()
                    .Where(x => x.tid > tenMinAgo)
                    .ToListAsync();
                return data;
            }
        }
        public async Task<SystemStats> LoadTest()
        {
            using (var asyncSession = DocumentStoreHolder.Store.OpenAsyncSession())
            {
                var bajs = await asyncSession.LoadAsync<SystemStats>("ServerDatas/1638721-A");
                return bajs;
            }
        }
    }
}