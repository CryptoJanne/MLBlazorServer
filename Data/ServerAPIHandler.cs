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

        public async Task<List<Dictionary<string, float>>> GetFiveMinDataSplitIntoDict()
        {
            List<Dictionary<string, float>> returnList = new List<Dictionary<string, float>>();
            Dictionary<string, float> core1 = new Dictionary<string, float>();
            Dictionary<string, float> core2 = new Dictionary<string, float>();
            Dictionary<string, float> core3 = new Dictionary<string, float>();
            Dictionary<string, float> core4 = new Dictionary<string, float>();
            Dictionary<string, float> core5 = new Dictionary<string, float>();
            Dictionary<string, float> core6 = new Dictionary<string, float>();
            Dictionary<string, float> core7 = new Dictionary<string, float>();
            Dictionary<string, float> core8 = new Dictionary<string, float>();
            var query = await QueryAndCalculateAndSplit5MinInto1MinSegments();
            foreach (var minute in query)
            {
                float cpu1 = 0;
                float cpu2 = 0;
                float cpu3 = 0;
                float cpu4 = 0;
                float cpu5 = 0;
                float cpu6 = 0;
                float cpu7 = 0;
                float cpu8 = 0;
                foreach (var second in minute)
                {
                    cpu1 += second.cpu.core1;
                    cpu2 += second.cpu.core2;
                    cpu3 += second.cpu.core3;
                    cpu4 += second.cpu.core4;
                    cpu5 += second.cpu.core5;
                    cpu6 += second.cpu.core6;
                    cpu7 += second.cpu.core7;
                    cpu8 += second.cpu.core8;
                }

                cpu1 = cpu1 / minute.Count;
                cpu2 = cpu2 / minute.Count;
                cpu3 = cpu3 / minute.Count;
                cpu4 = cpu4 / minute.Count;
                cpu5 = cpu5 / minute.Count;
                cpu6 = cpu6 / minute.Count;
                cpu7 = cpu7 / minute.Count;
                cpu8 = cpu8 / minute.Count;
                core1.Add(minute.First().tid.ToString(), cpu1);
                core2.Add(minute.First().tid.ToString(), cpu2);
                core3.Add(minute.First().tid.ToString(), cpu3);
                core4.Add(minute.First().tid.ToString(), cpu4);
                core5.Add(minute.First().tid.ToString(), cpu5);
                core6.Add(minute.First().tid.ToString(), cpu6);
                core7.Add(minute.First().tid.ToString(), cpu7);
                core8.Add(minute.First().tid.ToString(), cpu8);
            }
            returnList.Add(core1);
            returnList.Add(core2);
            returnList.Add(core3);
            returnList.Add(core4);
            returnList.Add(core5);
            returnList.Add(core6);
            returnList.Add(core7);
            returnList.Add(core8);
            return returnList;
        }
    }
}