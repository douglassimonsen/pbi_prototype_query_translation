using Microsoft.PowerBI.ReportingServicesHost;
using Microsoft.PowerBI.ExploreHost.Utils;
using Microsoft.DataShaping.Engine;
using Microsoft.PowerBI.ExploreHost.SemanticQuery;
using Microsoft.PowerBI.ExploreHost;
using System;
using Microsoft.BusinessIntelligence;
using Microsoft.PowerBI.Client.Windows.Services;
using Microsoft.PowerBI.Client.Windows.Telemetry;
using Microsoft.PowerBI.Client.Windows;
using Microsoft.PowerBI.Client.Shared;
using Microsoft.Mashup.Host.Document;
using Microsoft.PowerBI.Client.Windows.Utilities;
using Microsoft.PowerBI.Client.Telemetry;
using Microsoft.ReportingServices.Library;
using System.Threading;
using System.Collections.Generic;
using Microsoft.PowerBI.DataExtension.Contracts.Internal;

namespace ConsoleApp3
{
    class Program
    {
        static (EngineDataModel, PowerViewHandler) GetEngineDataModel(FeatureSwitches featureSwitches, string dbname, int port)
        {
            string connStr = $"Provider=MSOLAP.8;Persist Security Info=True;Initial Catalog={dbname};Data Source=localhost:{port};MDX Compatibility=1;Safety Options=2;MDX Missing Member Mode=Error;Update Isolation Level=2;";
            //var test = new Connection(connStr);
            //test.Open();
            //var reader = test.CreateCommand("Evaluate example").ExecuteReader();
            //Console.WriteLine(reader.NextResult());
            //return;
            var powerViewHandler = new PowerViewHandler(featureSwitchProxy: featureSwitches.FeatureSwitchesProxy);
            var connInfo = ASConnectionInfo.CreateLocalConnectionInfo(
               databaseName: dbname,
               databaseID: dbname,
               connectionString: connStr
            );
            powerViewHandler.CreateNewReportingSession(connInfo, null);
            var engineDataModel = powerViewHandler.GetEngineDataModel(
                connInfo.DatabaseID,
                "2.0",
                TranslationsBehavior.Default,
                ExploreHostUtils.GetEngineDataModel
            );
            return (engineDataModel, powerViewHandler);
        }
        static FeatureSwitches InitializeFeatureSwitches()
        {
            var systemEnvironment = new SystemEnvironment();
            var powerBISettings = PowerBISettingsFactory.GetSettingsFromAppConfig(systemEnvironment);
            var constants = new PowerBIConstants(
                powerBISettings,
                systemEnvironment
            );
            DependencyInjectionService.Get().RegisterInstance((IApplicationPaths)constants);
            DependencyInjectionService.Get().RegisterInstance((IApplicationSettings)powerBISettings);

            var manager = new FeatureSwitchManager(
                new VersionInfo(),
                constants,
                new PowerBIMockTelemetryService(),
                powerBISettings,
                new WebView2VersionUtils()
            );
            manager.RegisterKnownSwitches();
            var proxyService = new FeatureSwitchProxyService(manager);
            return new FeatureSwitches(proxyService);
        }
        static ExploreClientHandlerContext GetContext(PowerViewHandler powerViewer, FeatureSwitches featureSwitches)
        {
            return new ExploreClientHandlerContext(
                powerViewer,
                DataShapeEngine.Instance, // seems good
                featureSwitches,
                new QueryCancellationManager()  // seems good imo
            );
        }
        static PowerBIMockTelemetryService getTelemetry()
        {
            var telemetry = new PowerBIMockTelemetryService();
            DependencyInjectionService.Get().RegisterInstance((IPowerBITelemetryService)telemetry);
            return telemetry;
        }
        static void Main(string[] args)
        {
            var dbName = "628cbc55-9fee-4bff-95dd-6b09256e3ba0";
            var port = 63751;
            var telemetry = getTelemetry();
            var featureSwitches = InitializeFeatureSwitches();
            var (engineDataModel, powerViewer) = GetEngineDataModel(featureSwitches, dbName, port);
            var context = GetContext(powerViewer, featureSwitches);
            var queryFlow = new TranslateDataViewQueryFlow(
                context: context,
                databaseID: dbName,
                definition: Query.GetQuery()
            );
            queryFlow.Translate(engineDataModel);
            foreach (KeyValuePair<string, string> kvp in queryFlow.Result.SelectNameToDaxColumnName)
            {
                Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }

            var expr = queryFlow.Result.DaxExpression;
            Console.WriteLine(expr != null);
            //Console.WriteLine($"Length: {expr.Length}, Query: {expr}");
        }
    }
}
