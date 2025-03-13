using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.DataContracts;
using TrackAvailability.Properties;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.IdentityModel.Abstractions;

namespace TrackAvailability
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Please provide the connection strings for Dataverse and Application Insights as an argument. (arg 1 = Dataverse, arg 2 App Insights.");
                return;
            }
            
            string dvConnectionString = args[0];
            string aiConnectionString = args[1];

            #region "Custom Tests"

            // ********* TEST 1
            DateTime startTime = DateTime.Now;
            AvailabilityTelemetry data = Program.WhoAmI(dvConnectionString);
            data.Duration = DateTime.Now.Subtract(startTime);

            Program.TrackCustomAvailability(aiConnectionString, data);

            // ********* TEST 2
            startTime = DateTime.Now;
            data = Program.CreateRemoveSampleAccount(dvConnectionString);
            data.Duration = DateTime.Now.Subtract(startTime);
            Program.TrackCustomAvailability(aiConnectionString, data);

            #endregion

            Console.WriteLine("Availability tracked successfully.");
        }

        private static AvailabilityTelemetry CreateRemoveSampleAccount(string connectionString)
        {
            AvailabilityTelemetry availabilityTelemetry = new AvailabilityTelemetry();
            availabilityTelemetry.RunLocation = "GitHub";
            availabilityTelemetry.Name = "New Account";

            using (ServiceClient serviceClient = new ServiceClient(connectionString))
            {
                if (serviceClient.IsReady)
                {
                    // Create New Account request
                    Entity account = new Entity("account");
                    account["name"] = "Test Account " + DateTime.Now.ToString("yyyyMMdd-HH");
                    account["accountnumber"] = DateTime.Now.ToString("yyyyMMdd-HH");
                    account["telephone1"] = "123-456-7890";

                    // Execute the request
                    Guid accountId = serviceClient.Create(account);
                    availabilityTelemetry.Success = accountId != Guid.Empty;
                    availabilityTelemetry.Message = String.Format("Created / Removed {0} on {1}", account["name"], serviceClient.ConnectedOrgFriendlyName);

                    // Output the results
                    Console.WriteLine(availabilityTelemetry.Message);
                }
                else
                {
                    Console.WriteLine("Failed to connect to Dataverse.{0}",serviceClient.LastError);
                    availabilityTelemetry.Success = false;
                }
            }
            return (availabilityTelemetry);
        }

        private static AvailabilityTelemetry WhoAmI(string connectionString)
        {
            AvailabilityTelemetry availabilityTelemetry = new AvailabilityTelemetry();
            availabilityTelemetry.RunLocation = "GitHub";
            availabilityTelemetry.Name = "WhoAmI";

            using (ServiceClient serviceClient = new ServiceClient(connectionString))
            {
                if (serviceClient.IsReady)
                {
                    // Create the WhoAmI request
                    WhoAmIRequest whoAmIRequest = new WhoAmIRequest();

                    // Execute the request
                    WhoAmIResponse whoAmIResponse = (WhoAmIResponse)serviceClient.Execute(whoAmIRequest);
                    availabilityTelemetry.Success = serviceClient.IsReady;
                    availabilityTelemetry.Message = serviceClient.ConnectedOrgFriendlyName;

                    // Output the results
                    Console.WriteLine("Connected to {0} {1}", serviceClient.ConnectedOrgFriendlyName, serviceClient.IsReady);
                }
                else
                {
                    Console.WriteLine("Failed to connect to Dataverse.{0}", serviceClient.LastError);
                    availabilityTelemetry.Success = false;
                }
            }
            return (availabilityTelemetry);
        }

        private static bool TrackCustomAvailability(string aiConnectionString, AvailabilityTelemetry data)
        {
            bool bResult = false;

            TelemetryConfiguration config = new TelemetryConfiguration();
            config.ConnectionString = aiConnectionString;

            var telemetryClient = new TelemetryClient(config);
            telemetryClient.TrackAvailability(data);
            telemetryClient.Flush();

            return (bResult);
        }


    }

    

}
