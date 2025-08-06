using Microsoft.InfoNav.Data.Contracts.Internal;
using Microsoft.InfoNav.Explore.ServiceContracts.Internal;
using Newtonsoft.Json;
using System.IO;



namespace Translation
{
    class Query
    {
        public static string GetSource()
        {
            // used for debugging
            return File.ReadAllText(@"C:\Users\USER\Documents\repos\prototype_parsing\data.json");
        }
        public static DataViewQueryDefinition Convert(string raw)
        {
            var ret = JsonConvert.DeserializeObject<QueryDefinition>(raw);
            return new DataViewQueryDefinition(queryDefinition: ret);
        }
    }
}
