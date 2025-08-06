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
using Microsoft.PowerBI.DataExtension.Contracts.Internal;
using System.IO;
using MsolapWrapper;


namespace Translation
{
    public class DataViewQueryTranslationResult
    {
        // Only here to make python bindings nicer
        public readonly string DaxExpression;
        public readonly System.Collections.Generic.IReadOnlyDictionary<string, string> SelectNameToDaxColumnName;

        public DataViewQueryTranslationResult(Microsoft.InfoNav.Explore.ServiceContracts.Internal.DataViewQueryTranslationResult results)
        {
            this.DaxExpression = results.DaxExpression;
            this.SelectNameToDaxColumnName = results.SelectNameToDaxColumnName;
        }
    }
    public class PrototypeQuery
    {
        static (EngineDataModel, PowerViewHandler) GetEngineDataModel(FeatureSwitches featureSwitches, string dbname, int port)
        {
            string connStr = $"Provider=MSOLAP.8;Persist Security Info=True;Initial Catalog={dbname};Data Source=localhost:{port};MDX Compatibility=1;Safety Options=2;MDX Missing Member Mode=Error;Update Isolation Level=2;";
            //var test = new Connection(connStr);
            //test.Open();
            //var reader = test.CreateCommand("Evaluate DataTable").ExecuteReader();
            //Console.WriteLine(reader.NextResult());
            //System.Environment.Exit(0);
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
                new WebView2VersionUtils(),
                new PowerBIUserSettings(powerBISettings, systemEnvironment)
            );
            //manager.RegisterKnownSwitches();
            var proxyService = new FeatureSwitchProxyService(manager);
            return new FeatureSwitches(proxyService);
        }
        static ExploreClientHandlerContext GetContext(PowerViewHandler powerViewer, FeatureSwitches featureSwitches)
        {
            return new ExploreClientHandlerContext(
                powerViewer,
                DataShapeEngine.Instance,
                featureSwitches,
                new QueryCancellationManager()
            );
        }
        static PowerBIMockTelemetryService getTelemetry()
        {
            var telemetry = new PowerBIMockTelemetryService();
            DependencyInjectionService.Get().RegisterInstance((IPowerBITelemetryService)telemetry);
            return telemetry;
        }
        public static DataViewQueryTranslationResult Translate(string query, string dbName, int port, string workingDirectory = null)
        {
            // expected to be the only entrypoint for Python
            if (workingDirectory != null)
            {
                Directory.SetCurrentDirectory(workingDirectory);

            }
            var telemetry = getTelemetry();
            var featureSwitches = InitializeFeatureSwitches();
            var (engineDataModel, powerViewer) = GetEngineDataModel(featureSwitches, dbName, port);
            var context = GetContext(powerViewer, featureSwitches);

            var queryFlow = new TranslateDataViewQueryFlow(
                context: context,
                databaseID: dbName,
                definition: Query.Convert(query)
            );
            queryFlow.Translate(engineDataModel);
            return new DataViewQueryTranslationResult(queryFlow.Result);
        }
        // static void Main(string[] args)
        // {
        //     // only used for debugging
        //     var dbName = "e1ff5407-9b29-4692-870b-41bacbb9c4f5";
        //     var port = 50025;
        //     var query = File.ReadAllText(@"<file path to your query file>");
        //     var result = Translate(query, dbName, port);
        //     Console.WriteLine(result.DaxExpression);
        //     Console.ReadKey();
        //     Console.ReadKey();
        //     Console.ReadKey();
        // }
    }
}
