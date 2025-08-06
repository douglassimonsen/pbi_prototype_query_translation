using Microsoft.InfoNav.Data.Contracts.Internal;
using Microsoft.InfoNav.Explore.ServiceContracts.Internal;
using Newtonsoft.Json;
using System.IO;



namespace Translation
{
    class Query
    {

        public static DataViewQueryDefinition Convert(string raw)
        {
            var ret = JsonConvert.DeserializeObject<QueryDefinition>(raw);
            return new DataViewQueryDefinition(queryDefinition: ret);
        }
    }
}
