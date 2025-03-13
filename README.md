# Custom Availabliity Test for Power Platform

This project contains custom availability tests for Microsoft Power Platform. The tests are designed to check the availability of various components within the Power Platform ecosystem, including Power Apps, Power Automate, and Power BI. Application Insights provides an out of box availability test that will ping the target URL to verify availability. This project extends that functionality by providing a custom test that can be used to check the availability of Power Platform components. 

The solution contains a Visul Studio project that contains the logic for the custom availability test. The project produces a C# Console Application that can be executed from a GitHub Action. The two GitHub Actions are described below.


- BuildPublishRunTrackAvailablity.yml
	- This GitHub Action builds the C# Console Application and publishes it to a GitHub Release. The action is triggered on a push to the main branch. The action uses the `dotnet` CLI to build and publish the application. The action also creates a GitHub Release and uploads the published application to the release.

[![Build Publish Run Track Availablity](https://github.com/ljrain/DataverseAppInsightsTrackAvailability/actions/workflows/BuildPublishRunTrackAvailablity.yml/badge.svg)](https://github.com/ljrain/DataverseAppInsightsTrackAvailability/actions/workflows/BuildPublishRunTrackAvailablity.yml)

- TrackAvailability.yml
- 	- This GitHub Action runs the C# Console Application and tracks the availability of the Power Platform components. The action is triggered on a schedule (every 5 minutes). The action uses the `dotnet` CLI to run the application. The action also sends the availability data to Application Insights using the `ApplicationInsights` NuGet package. The action uses the `AZURE_APPLICATION_INSIGHTS_INSTRUMENTATION_KEY` secret to authenticate with Application Insights. The action also uses the `AZURE_SUBSCRIPTION_ID`, `AZURE_TENANT_ID`, and `AZURE_CLIENT_ID` secrets to authenticate with Azure. The action uses the `AZURE_CLIENT_SECRET` secret to authenticate with Azure. The action uses the `AZURE_RESOURCE_GROUP_NAME` secret to specify the resource group name for Application Insights.

[![Get Recent Artifact](https://github.com/ljrain/DataverseAppInsightsTrackAvailability/actions/workflows/TrackAvailability.yml/badge.svg)](https://github.com/ljrain/DataverseAppInsightsTrackAvailability/actions/workflows/TrackAvailability.yml)

