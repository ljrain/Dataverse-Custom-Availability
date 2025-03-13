using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.DataContracts;
using TrackAvailability.Properties;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;

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

            // TEST 1
            AvailabilityTelemetry results = Program.WhoAmI(dvConnectionString);
            results.Duration = DateTime.Now.Subtract(startTime);

            TelemetryConfiguration config = new TelemetryConfiguration();
            config.ConnectionString = aiConnectionString;
            
            var telemetryClient = new TelemetryClient(config);

            var data = new AvailabilityTelemetry
            {
                Duration = results.Duration,
                Success = results.Success,
                Message = results.Message,
                Name = results.Name,
                RunLocation = results.RunLocation,
                Timestamp = DateTimeOffset.UtcNow
            };

            telemetryClient.TrackAvailability(data);
            telemetryClient.Flush();

            // TEST 2
            startTime = DateTime.Now;
            AvailabilityTelemetry results2 = Program.CreateSampleAccount(dvConnectionString);
            results2.Duration = DateTime.Now.Subtract(startTime);

            telemetryClient.TrackAvailability(results2);
            telemetryClient.Flush();

            #endregion

            Console.WriteLine("Availability tracked successfully.");
        }

        private static AvailabilityTelemetry CreateSampleAccount(string connectionString)
        {
            AvailabilityTelemetry availabilityTelemetry = new AvailabilityTelemetry();
            using (ServiceClient serviceClient = new ServiceClient(connectionString))
            {
                if (serviceClient.IsReady)
                {
                    // Create the WhoAmI request
                    Entity accountRec = new Entity("account");
                    accountRec["name"] = "Test Account";
                    accountRec["telephone1"] = "1234567890";

                    try
                    {
                        Guid accountId = serviceClient.Create(accountRec);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(String.Format("{0} creating account: {1}",ex.GetType().Name, ex.Message));
                        availabilityTelemetry.Success = false;
                        availabilityTelemetry.RunLocation = "GitHub";
                        availabilityTelemetry.Message = "New Account";
                        availabilityTelemetry.Name = "NewAccount";
                    }
                    

                    availabilityTelemetry.Success = true;
                    availabilityTelemetry.RunLocation = "GitHub";
                    availabilityTelemetry.Message = "New Account";
                    availabilityTelemetry.Name = "NewAccount";

                    //// Output the results
                    //Console.WriteLine("User ID: " + whoAmIResponse.UserId);
                    //Console.WriteLine("Business Unit ID: " + whoAmIResponse.BusinessUnitId);
                    //Console.WriteLine("Organization ID: " + whoAmIResponse.OrganizationId);
                }
                else
                {
                    Console.WriteLine("Failed to connect to Dataverse.");
                    availabilityTelemetry.Success = false;
                }
            }
            return (availabilityTelemetry);
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
