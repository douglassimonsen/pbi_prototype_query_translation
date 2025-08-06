using Microsoft.PowerBI.ReportingServicesHost;
using Microsoft.PowerBI.ExploreHost.Utils;
using MsolapWrapper;
using Microsoft.PowerBI.DataExtension.Msolap;
using System;
using System.IO;
using System.Reflection;
using Microsoft.DataShaping.Engine;


namespace ConsoleApp3
{
    class Program
    {
        static EngineDataModel GetEngineDataModel()
        {
            var DbName = "2e7de914-94c5-4257-bc86-4b07668bb9c5";
            string connStr = $"Provider=MSOLAP.8;Persist Security Info=True;Initial Catalog={DbName};Data Source=localhost:56595;MDX Compatibility=1;Safety Options=2;MDX Missing Member Mode=Error;Update Isolation Level=2;";
            //var test = new Connection(connStr);
            //test.Open();
            //var reader = test.CreateCommand("Evaluate example").ExecuteReader();
            //Console.WriteLine(reader.NextResult());
            //return;
            var powerViewHandler = new PowerViewHandler();
            var connInfo = ASConnectionInfo.CreateLocalConnectionInfo(
               databaseName: DbName,
               databaseID: DbName,
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
            GetEngineDataModel();
            //var PowerViewer = GetPowerViewer();
            //PowerViewer.GetEngineDataModel();
        }
    }
}
