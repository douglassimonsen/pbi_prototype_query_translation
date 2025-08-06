using Microsoft.PowerBI.ReportingServicesHost;
using Microsoft.PowerBI.ExploreHost.Utils;
using Microsoft.DataShaping.Engine;
using Microsoft.PowerBI.ExploreHost.SemanticQuery;
using System;

namespace ConsoleApp3
{
    class Program
    {
        static EngineDataModel GetEngineDataModel(string dbname, int port)
        {
            string connStr = $"Provider=MSOLAP.8;Persist Security Info=True;Initial Catalog={dbname};Data Source=localhost:{port};MDX Compatibility=1;Safety Options=2;MDX Missing Member Mode=Error;Update Isolation Level=2;";
            //var test = new Connection(connStr);
            //test.Open();
            //var reader = test.CreateCommand("Evaluate example").ExecuteReader();
            //Console.WriteLine(reader.NextResult());
            //return;
            var powerViewHandler = new PowerViewHandler();
            var connInfo = ASConnectionInfo.CreateLocalConnectionInfo(
               databaseName: dbname,
               databaseID: dbname,
               connectionString: connStr
            );
            powerViewHandler.CreateNewReportingSession(connInfo, null);
            var engineDataModel = powerViewHandler.GetEngineDataModel(
                connInfo.DatabaseID,
                "2.0",
                null,
                ExploreHostUtils.GetEngineDataModel
            );
            return engineDataModel;
        }
        static void Main(string[] args)
        {
            var dbName = "2e7de914-94c5-4257-bc86-4b07668bb9c5";
            var port = 56595;
            var engineDataModel = GetEngineDataModel(dbName, port);
            var queryFlow = new TranslateDataViewQueryFlow(
                context: null,
                databaseID: dbName,
                definition: Query.GetQuery()
            );
            queryFlow.Translate(engineDataModel);
            Console.WriteLine(queryFlow.Result);
        }
    }
}
