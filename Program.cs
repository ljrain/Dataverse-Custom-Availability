using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.DataContracts;
using System.Runtime.CompilerServices;
using TrackAvailability.Properties;
using System.Configuration;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Crm.Sdk.Messages;

namespace TrackAvailability
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DateTime startTime = DateTime.Now;
            Settings setting = new Settings();
            string aiConnectionString = setting.AppInsightsConnectionString;
            string dvConnectionString = setting.DvConnectionString;

            #region "Custom Tests"
            AvailabilityTelemetry results = Program.WhoAmI(dvConnectionString);

            TelemetryConfiguration config = new TelemetryConfiguration();
            config.ConnectionString = aiConnectionString;
            
            var telemetryClient = new TelemetryClient(config);

            var data = new AvailabilityTelemetry
            {
                Duration = results.Duration,
                Success = results.Success,
                Message = results.Message,
                Name = results.Name,
                Timestamp = DateTimeOffset.UtcNow
            };

            telemetryClient.TrackAvailability(data);
            telemetryClient.Flush();

            #endregion

            Console.WriteLine("Availability tracked successfully.");

        }


        private static AvailabilityTelemetry WhoAmI(string connectionString)
        {
            AvailabilityTelemetry availabilityTelemetry = new AvailabilityTelemetry();

            using (ServiceClient serviceClient = new ServiceClient(connectionString))
            {
                if (serviceClient.IsReady)
                {
                    // Create the WhoAmI request
                    WhoAmIRequest whoAmIRequest = new WhoAmIRequest();

                    // Execute the request
                    WhoAmIResponse whoAmIResponse = (WhoAmIResponse)serviceClient.Execute(whoAmIRequest);

                    availabilityTelemetry.Success = true;
                    availabilityTelemetry.RunLocation = "GitHub";
                    availabilityTelemetry.Message = "Connected";
                    availabilityTelemetry.Name = "WhoAmI";

                    // Output the results
                    Console.WriteLine("User ID: " + whoAmIResponse.UserId);
                    Console.WriteLine("Business Unit ID: " + whoAmIResponse.BusinessUnitId);
                    Console.WriteLine("Organization ID: " + whoAmIResponse.OrganizationId);
                }
                else
                {
                    Console.WriteLine("Failed to connect to Dataverse.");
                    availabilityTelemetry.Success = false;
                }
            }
            return (availabilityTelemetry);
        }


    }

    

}
