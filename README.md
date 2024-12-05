# RazorPagesMovie CI/CD with GitHub Actions

This repository demonstrates how to set up a CI/CD pipeline for a Razor Pages Movie application using GitHub Actions. The pipeline includes building, testing, and deploying the application to Azure Container Apps.

## Overview

The CI/CD pipeline is defined using GitHub Actions workflows and Terraform for infrastructure as code. The main workflows are:

- **CI Workflow**: Builds and tests the application.
- **CD Workflow**: Deploys the application to Azure and runs UI tests.
- **Housekeeping Workflow**: Cleans up resources after testing.

## CI/CD Practices

### Continuous Integration (CI)

Continuous Integration ensures that every change to the codebase is automatically built and tested. This helps catch issues early and maintain code quality. The CI workflow is triggered on push and pull request events and performs the following steps:

1. **Checkout Code**: Checks out the repository code.
2. **Set up .NET**: Sets up the .NET environment.
3. **Restore Dependencies**: Restores the project dependencies.
4. **Build Project**: Builds the project.
5. **Run Tests**: Runs unit tests and uploads test results.

Mermaid diagram:
<details>
  <summary>CI Workflow Steps</summary>

```mermaid
graph TD
    A[Code] --> B(Initialize CodeQL)
    B --> C{Set up .NET}
    C --> D(Cache NuGet Packages)
    D --> E(Restore Dependencies)
    E --> F(Build Project)
    F --> G(Publish Project)
    G --> H(Upload Published App)
    H --> I(Perform CodeQL Analysis)
    I --> J{Split Tests}
    J --> K(Run Unit Tests)
    K --> L(Publish Test Results report)
```
</details>

### Continuous Deployment (CD)

Continuous Deployment automatically deploys the application to Azure Container Apps whenever changes are pushed to the main branch. This ensures that the latest version of the application is always available in the staging and production environments. The CD workflow includes:

1. **Build and Push Docker Image**: Builds the Docker image and pushes it to GitHub Container Registry.
2. **Deploy to Azure Container Apps**: Deploys the image to GitHub Container Apps.
* **Staging**: Using terraform to deploy the infrastructure and run the UI tests in the staging environment.
3. **Run UI Tests**: Executes UI tests using Selenium.
4. **Post Deployment Steps**: Cleans up resources and sends deployment status.### Continuous Integration (CI)

Continuous Integration ensures that every change to the codebase is automatically built and tested. This helps catch issues early and maintain code quality. The CI workflow is triggered on push and pull request events and performs the following steps:

1. **Checkout Code**: Checks out the repository code.
2. **Initialize CodeQL**: Initializes CodeQL for code analysis.
3. **Set up .NET**: Sets up the .NET environment.
4. **Cache NuGet Packages**: Caches the NuGet packages to speed up the build process.
5. **Restore Dependencies**: Restores the project dependencies.
6. **Build Project**: Builds the project.
7. **Publish Project**: Publishes the project.
8. **Upload Published App**: Uploads the published app as an artifact.
9. **Perform CodeQL Analysis**: Performs CodeQL analysis on the codebase.
10. **Split Tests**: Splits the tests for parallel execution.
11. **Run Tests**: Runs unit tests and uploads test results.
12. **Publish Test Results**: Publishes the test results.

### Continuous Deployment (CD)

Continuous Deployment automatically deploys the application to Azure Container Apps whenever changes are pushed to the main branch. This ensures that the latest version of the application is always available in the staging and production environments. The CD workflow includes:

1. **Checkout Code**: Checks out the repository code.
2. **Az CLI Login via OIDC**: Logs in to Azure CLI using OIDC.
3. **Deploy the Azure App - Including DB Migrations [STAGING ENVIRONMENT]**: Deploys the application to the staging environment using Terraform.
4. **Capture Terraform Outputs**: Captures the Terraform outputs.
5. **Generate URL at Commit Hash to IaC Staging Files**: Generates the URL at the commit hash to the IaC staging files.
6. **Run UI Automated Selenium Tests**: Runs UI tests using Selenium.
7. **Generate Workflow Telemetry**: Generates heat map and performance data.
8. **Create QA Ticket**: Creates a QA ticket to notify that the staging environment is ready for testing.
9. **Deploy the Azure App - Including DB Migrations [PRODUCTION ENVIRONMENT]**: Deploys the application to the production environment using Terraform.
10. **Capture Terraform Output**: Captures the Terraform output.
11. **Create GitHub Release**: Creates a GitHub release with the deployment details.

<details>
  <summary>CD Workflow Steps</summary>

```mermaid
graph TD
    A[Code] --> B(Checkout Code)
    B --> C(Az CLI Login via OIDC)
    C --> D(Deploy the Azure App - Including DB Migrations STAGING ENVIRONMENT)
    D --> E(Capture Terraform Outputs)
    E --> F(Generate URL at Commit Hash to IaC Staging Files)
    F --> G(Run UI Automated Selenium Tests)
    G --> H(Generate Workflow Telemetry)
    H --> I(Create QA Ticket)
    I --> J(Deploy the Azure App - Including DB Migrations PRODUCTION ENVIRONMENT)
    J --> K(Capture Terraform Output)
    K --> L{Check if Revision Exists}
    L --> M{If Revision Exists, Deploy New Revision - Canary Deployment}
    M --> N(Create GitHub Release)
```

</details>
