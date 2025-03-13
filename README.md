# Custom Availabliity Test for Dataverse (Power Platform)

![image](https://github.com/user-attachments/assets/d5de39ff-8ca3-4f02-820b-813053e63fdc)

This project contains custom availability tests for Dataverse (Power Platform) components. The tests are implemented as a C# Console Application that uses the `Microsoft.PowerPlatform.Dataverse.Client` NuGet package to connect to Dataverse and check the availability of various components. The application is designed to be run as a GitHub Action on a schedule, and it tracks the availability of the components in Azure Application Insights.

The project cotnains the following components and can be executed locally or on GitHub as an Action. The console application "TrackAvailabilty" performs the tests and records the results to Application Insights. 

1. Console App
2. GitHub Actions
2.1 Build Publish Run Track Availablity (BuildPublishRunTrackAvailablity.yml)
2.2  Track Dataverse Availabilty (trackavailablity.yml)


- BuildPublishRunTrackAvailablity.yml
	- This GitHub Action builds the C# Console Application and publishes it to a GitHub Release. The action is triggered on a push to the main branch. The action uses the `dotnet` CLI to build and publish the application. The action also creates a GitHub Release and uploads the published application to the release.

[![Build Publish Run Track Availablity](https://github.com/ljrain/DataverseAppInsightsTrackAvailability/actions/workflows/BuildPublishRunTrackAvailablity.yml/badge.svg)](https://github.com/ljrain/DataverseAppInsightsTrackAvailability/actions/workflows/BuildPublishRunTrackAvailablity.yml)

- TrackAvailability.yml
- 	- This GitHub Action runs the C# Console Application and tracks the availability of the Power Platform components. The action is triggered on a schedule (every 5 minutes). The action uses the `dotnet` CLI to run the application. The action also sends the availability data to Application Insights using the `ApplicationInsights` NuGet package. The action uses the `AZURE_APPLICATION_INSIGHTS_INSTRUMENTATION_KEY` secret to authenticate with Application Insights. The action also uses the `AZURE_SUBSCRIPTION_ID`, `AZURE_TENANT_ID`, and `AZURE_CLIENT_ID` secrets to authenticate with Azure. The action uses the `AZURE_CLIENT_SECRET` secret to authenticate with Azure. The action uses the `AZURE_RESOURCE_GROUP_NAME` secret to specify the resource group name for Application Insights.

[![Get Recent Artifact](https://github.com/ljrain/DataverseAppInsightsTrackAvailability/actions/workflows/TrackAvailability.yml/badge.svg)](https://github.com/ljrain/DataverseAppInsightsTrackAvailability/actions/workflows/TrackAvailability.yml)

# How to Add Tests
To add tests to the console application, follow these steps:
1. Open the `Program.cs` file in the console application.
2. Add a new method for the test you want to implement. The method must return a `AvailabilityTelemetry` with the following information:
   - Name: The name of the test.
   - Success: A boolean indicating whether the test was successful or not.
   - Message: A message describing the result of the test.
   - Duration: The duration of the test in milliseconds.
   - Timestamp: The timestamp of the test result.
3. Call the method in the `Main` method of the console application.
4. Execute 'TrackCustomAvailability' after the test routine is called.
5. Build and test from local to verify the custom availablity is logging
6. Commit and push the changes to the GitHub repository.

NOTE: The following secrets are required to be in place on GitHub.
- AICONNECTIONSTRING: Contains the connection string to the Application Insights resource.
- DVCONNECTIONSTRING: Contains the connection string to the Dataverse environment.
