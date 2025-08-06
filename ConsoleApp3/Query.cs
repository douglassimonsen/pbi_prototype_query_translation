using Microsoft.InfoNav.Data.Contracts.Internal;
using Microsoft.InfoNav.Explore.ServiceContracts.Internal;
using Newtonsoft.Json;
using System.IO;



namespace ConsoleApp3
{
    class Query
    {
        public static DataViewQueryDefinition GetSource()
        {
            var raw = File.ReadAllText(@"C:\Users\USER\Documents\repos\pbyx\data.json");
            var ret = JsonConvert.DeserializeObject<QueryDefinition>(raw);
            return new DataViewQueryDefinition(queryDefinition: ret);

        }
    }
}
