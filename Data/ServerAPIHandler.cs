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
        
        public async Task<(List<SystemStats>, DateTime)> QueryServerForLogDataFiveMinutes()
        {
            DateTime now = DateTime.Now;
            DateTime fiveMinAgo = now.Subtract(TimeSpan.FromMinutes(5));
            using (var asyncSession = DocumentStoreHolder.Store.OpenAsyncSession())
            {
                List<SystemStats> data = await asyncSession
                    .Query<SystemStats>()
                    .Where(x => x.tid > fiveMinAgo)
                    .ToListAsync();
                return (data, now);
            }
        }

        public async Task<List<List<SystemStats>>> QueryAndCalculateAndSplit5MinInto1MinSegments()
        {
            List<SystemStats> minOne = new List<SystemStats>();
            List<SystemStats> minTwo = new List<SystemStats>();
            List<SystemStats> minThree = new List<SystemStats>();
            List<SystemStats> minFour = new List<SystemStats>();
            List<SystemStats> minFive = new List<SystemStats>();
            List<List<SystemStats>> divided = new List<List<SystemStats>>();
            var returnTuple = await QueryServerForLogDataFiveMinutes();
            List<SystemStats> allData = returnTuple.Item1;
            DateTime now = returnTuple.Item2;
            DateTime oneMinAgo = now.Subtract(TimeSpan.FromMinutes(1));
            DateTime twoMinAgo = now.Subtract(TimeSpan.FromMinutes(2));
            DateTime threeMinAgo = now.Subtract(TimeSpan.FromMinutes(3));
            DateTime fourMinAgo = now.Subtract(TimeSpan.FromMinutes(4));
            DateTime fiveMinAgo = now.Subtract(TimeSpan.FromMinutes(5));
            foreach (SystemStats stat in allData)
            {
                if (stat.tid < now && stat.tid > oneMinAgo)
                {
                    minOne.Add(stat);
                }
                else if (stat.tid < oneMinAgo && stat.tid > twoMinAgo)
                {
                    minTwo.Add(stat);
                }
                else if (stat.tid < twoMinAgo && stat.tid > threeMinAgo)
                {
                    minThree.Add(stat);
                }
                else if (stat.tid < threeMinAgo && stat.tid > fourMinAgo)
                {
                    minFour.Add(stat);
                }
                else if(stat.tid < fourMinAgo && stat.tid > fiveMinAgo)
                {
                    minFive.Add(stat);
                }
            }
            divided.Add(minOne);
            divided.Add(minTwo);
            divided.Add(minThree);
            divided.Add(minFour);
            divided.Add(minFive);
            return divided;
        }

        public async Task<Dictionary<string, float>> testdis()
        {
            Dictionary<string, float> returnvalue = new Dictionary<string, float>();
            var query = await QueryAndCalculateAndSplit5MinInto1MinSegments();
            foreach (var minute in query)
            {
                float cpu1 = 0;
                foreach (var second in minute)
                {
                    cpu1 += second.cpu.core1;
                }

                cpu1 = cpu1 / minute.Count;
                returnvalue.Add(minute.First().tid.ToString(), cpu1);
            }

            return returnvalue;
        }
    }
}