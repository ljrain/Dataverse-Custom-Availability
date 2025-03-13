# Custom Availabliity Test for Dataverse (Power Platform)

![image](https://github.com/user-attachments/assets/d5de39ff-8ca3-4f02-820b-813053e63fdc)

This project is a C# Console App that will login to Dataverse and check if the service is available and return the status, user id, organization id and business unit id. There are two components to these project:

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


