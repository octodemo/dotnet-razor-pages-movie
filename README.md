# 🎬 RazorPagesMovie CI/CD with GitHub Actions

This repository demonstrates how to set up a CI/CD pipeline for a Razor Pages Movie application using GitHub Actions. The pipeline includes building, testing, and deploying the application to Azure Container Apps.

## 🌟 Overview

The Razor Pages Movie application is a simple movie list application that allows users to view, create, edit, and delete movies. The application is built using the following technologies:
- **Frontend**: Razor Pages, HTML, CSS, Bootstrap
- **Backend**: ASP.NET Core Razor Pages, Entity Framework Core
- **Database**: Azure SQL Or SQL Server Database
- **Testing**: xUnit, Selenium
- **Deployment**: Azure Container Apps via OIDC (OpenID Connect)
- **Infrastructure**: Terraform (IaC)
- **CI/CD**: GitHub Actions
- **Monitoring**: Application Insights on Azure Portal
- **GitHub Advanced Security**: CodeQL Analysis, secret scanning, Dependabot alerts, GitHub Copilot Auto-Fix suggestions on PRs

<div style="display: flex; justify-content: space-between;">
  <img src="./assets/app-screenshot1.png" alt="Home Page" width="45%"/>
  <img src="./assets/app-screenshot2.png" alt="Movies Library" width="44%"/>
</div>

## 💻 🚀 Running the app localy
To run the Razor Pages Movie application locally using Docker Compose, follow these steps:

1. Ensure you have Docker and Docker Compose installed on your machine.
2. Navigate to the root directory of your project where the `docker-compose.yml` file is located.
3. Run the following command to start the application:

```sh
docker compose up
```

This command will start both the SQL Server and the web application containers. 

## 🚀 Running the app in GitHub Codespaces
To run the Razor Pages Movie application in GitHub Codespaces with the default .devcontainer setup, follow these steps:

Open the repository in GitHub Codespaces.
The .devcontainer setup will automatically start the application on startup on port 80.

## 🌐 Accessing the Application
The landing page will prompt you to login. By default, there are two main user logins for demonstration purposes:
- **Admin**: Username: `admin`, Password: `password`
- **User**: Username: `user`, Password: `password`


The web application will be accessible at: [http://localhost](http://localhost)

<details>
  <summary>Disclaimer - expand to read</summary> 

The application is not yet fully functional and is still under development.
The default behavior is that the application will run on http://localhost with SSL/TLS certificate validation disabled when connecting to the SQL Server. This is achieved by adding the `TrustServerCertificate` parameter to the connection string in the `docker-compose.yml` file. This allows the application to connect to the SQL Server without validating the SSL/TLS certificate.

</details>

## 🚀 Deployment Pipline in CI/CD
The CI/CD pipeline is defined using GitHub Actions workflows and Terraform for infrastructure as code. The main workflows are:

- **🔄 CI Workflow**: Builds and tests the application.
- **🚀 CD Workflow**: Deploys the application to Azure and runs UI tests.
- **🧹 Housekeeping Workflow**: Cleans up resources after testing.

## 🛠️ CI/CD Practices

### 🔄 Continuous Integration (CI)

Continuous Integration ensures that every change to the codebase is automatically built and tested. This helps catch issues early and maintain code quality. The CI workflow is triggered on push and pull request events and performs the following steps:

1. **📥 Checkout Code**: Checks out the repository code.
2. **⚙️ Set up .NET**: Sets up the .NET environment.
3. **📦 Restore Dependencies**: Restores the project dependencies.
4. **🏗️ Build Project**: Builds the project.
5. **🔍 CodeQL Analysis**: Performs CodeQL analysis on the codebase to identify potential security vulnerabilities.
6. **🧪 Run Tests**: Runs unit tests and uploads test results.


✨ **Interactive Diagram Below!** ✨

<details>
  <summary style="font-size: 1.2em; font-weight: bold; color: #0073e6; cursor: pointer;">🔍 Click to expand and view the CI Workflow Steps chart</summary>

```mermaid
graph TD
    A[📥 Checkout Code<br>Clone the repository to the runner environment] --> B[🔍 Initialize CodeQL<br>Set up CodeQL for security analysis]
    B --> C[⚙️ Set up .NET<br>Install .NET SDK and runtime]
    C --> D[📦 Cache NuGet Packages<br>Cache dependencies to speed up the build process]
    D --> E[📦 Restore Dependencies<br>Restore NuGet packages required for the project]
    E --> F[🏗️ Build Project<br>Compile the project and generate binaries]
    F --> G[🚀 Publish Project<br>Prepare the project for deployment]
    G --> H[⬆️ Upload Published App<br>Upload the compiled project for further steps]
    H --> I[🔍 Perform CodeQL Analysis<br>Analyze the codebase for security vulnerabilities]
    I --> J[🔄 Split Tests<br>Divide tests into smaller groups for parallel execution]
    J --> K1[🧪 Run Unit Tests - Group 1<br>Run unit tests for the first group]
    J --> K2[🧪 Run Unit Tests - Group 2<br>Run unit tests for the second group]
    J --> K3[🧪 Run Unit Tests - Group 3<br>Run unit tests for the third group]
    K1 --> L[📊 Publish Test Results<br>Publish the results of all unit tests]
    K2 --> L
    K3 --> L
    L --> M[📈 Upload Code Coverage Report<br>Generate and upload the code coverage report]

    subgraph Pull Request Process
        N[Create Pull Request<br>Developer creates a pull request] --> O[Run CI Workflow<br>CI workflow is triggered]
        O --> P[CodeQL Analysis<br>Analyze the codebase for security vulnerabilities]
        O --> Q[Build Project<br>Compile the project and generate binaries]
        O --> R[Run Unit Tests<br>Run all unit tests]
        P --> S{CodeQL Analysis Passes?}
        S -- Yes --> T[Proceed to Unit Tests]
        S -- No --> U[Fail PR<br>CodeQL analysis failed]
        Q --> V{Build Passes?}
        V -- Yes --> W[Proceed to Unit Tests]
        V -- No --> X[Fail PR<br>Build failed]
        R --> Y{Unit Tests Pass?}
        Y -- Yes --> Z[All Checks Passed<br>Ready for review and merge]
        Y -- No --> AA[Fail PR<br>Unit tests failed]
    end

    subgraph Merge Process
        AB[Review PR<br>Reviewers review the pull request] --> AC{All Reviews Approved?}
        AC -- Yes --> AD[Merge PR<br>Merge the pull request into the main branch]
        AC -- No --> AE[Request Changes<br>Developer makes changes and updates the PR]
    end

    subgraph Repository Rulesets
        AF[Status Checks<br>Ensure all status checks pass before merging]
        AG[Branch Protection<br>Enforce Repository Branch Rulesets]
        AH[Require Reviews<br>Require at least one review before merging]
        AI[Restrict Merge<br>Restrict who can merge pull requests]
    end

    Z --> AB
    AD --> AF
    AD --> AG
    AD --> AH
    AD --> AI
```
</details>

### 🚀 Continuous Delivery (CD)

Continuous Delivery automatically deploys the application to Azure Container Apps whenever changes are pushed to the main branch. This ensures that the latest version of the application is always available in the staging and production environments. The CD workflow includes:
1. **📦 Build and Deploy Container Image**: Builds the container image and deploys it to a container registry.
2. **📥 Checkout Code**: Checks out the repository code.
3. **🔑 Az CLI Login via OIDC**: Logs in to Azure CLI using OIDC.
4. **🚀 Deploy the Azure App - Including DB Migrations [STAGING ENVIRONMENT]**: Deploys the application to the staging environment using Terraform.
5. **🧪 Run UI Automated Selenium Tests**: Runs UI tests using Selenium.
6. **📊 Generate Workflow Telemetry**: Generates heat map and performance data.
7. **📝 Create QA Ticket**: Creates a QA ticket to notify that the staging environment is ready for testing.
8. **🚀 Deploy the Azure App - Including DB Migrations [PRODUCTION ENVIRONMENT]**: Deploys the application to the production environment using Terraform.
9. **📄 Capture Terraform Output**: Captures the Terraform output.
10. **🏷️ Create GitHub Release**: Creates a GitHub release with the deployment details.

✨ **Interactive Diagram Below!** ✨
<details>
  <summary style="font-size: 1.2em; font-weight: bold; color: #0073e6; cursor: pointer;">🔍 Click to expand and view the CD Workflow Steps chart</summary>

```mermaid

graph TD
    subgraph Build and Push Docker Image
        A[🐳 Build Docker Image<br>Build the Docker image]
        A --> B[📤 Push Docker Image<br>Push the Docker image to GHCR]
    end

    subgraph Deploy to Staging
        D[📥 Checkout Code<br>Clone the repository to the runner environment] --> E[🔑 Az CLI Login via OIDC<br>Authenticate with Azure]
        E --> F[🚀 Deploy the Azure App - Including DB Migrations]
        F --> G[📊 Capture deployment outputs]
        G --> H[🔗 Generate URL at Commit Hash to IaC Staging Files]
    end

    subgraph Functional UI Tests
        H --> I[🧪 Run UI Tests<br>Run UI Automated Selenium Tests]
        I --> Q1[🌐 Run functional UI tests on Chrome]
        I --> Q2[🌐 Run functional UI tests on Firefox]
        I --> Q3[🌐 Run functional UI tests on Edge]
        I --> Q4[🌐 Run functional UI tests on Chromium]
    end

    subgraph Post-Functional tests Steps
        I --> J[📈 Generate Telemetry<br>* Runner Utilization Metrics<br>* CPU heat map<br>* Memory usage]
        J --> K[📝 Create QA Ticket<br>Create QA Ticket for testing]
    end

    subgraph Deploy to Production
        K --> L[🚀 Deploy to Production Azure App]
        L --> M[📊 Capture Terraform Outputs]
        M --> N{🔍 Check if Revision Exists}
        N --> O{🚀 Deploy new revision with smaller traffic <=30%}
        O --> P[🏷️ Create a GitHub release for the new deployment]
    end

    B --> F
    B --> L

```

</details>

## 🔄 Deployment Strategies

### Staging vs Production Deployment Flow - Canary Deployment
This deployment strategy is orchestrated with the Terraform scripts located in the `terraform` folder.

```mermaid
flowchart TD
    subgraph Staging Deployment
        A1[🚀 Start] --> B1[🗄️ Create Staging Database]
        B1 --> C1[📦 Deploy Staging Container App with Single Revision]
        C1 --> D1[🔄 Set Traffic to 100% for Latest Revision]
        D1 --> E1[🏁 End]
        C1 -->|Failure| F1[↩️ Rollback to Previous State]
    end

    subgraph Production Deployment
        A2[🚀 Start] --> B2[🔍 Check Existing Container App]
        B2 --> C2{🔄 Existing Revision?}
        C2 -->|✔️ Yes| D2[🆕 Create New Revision with Canary Deployment]
        C2 -->|❌ No| E2[📦 Create Production Container App]
        D2 --> F2[🔄 Set Traffic Split for Canary Deployment]
        F2 --> G2[📊 Monitor and Validate New Revision]
        G2 --> H2{✅ Valid?}
        H2 -->|✔️ Yes| I2[🚀 Promote New Revision to Production]
        H2 -->|❌ No| J2[↩️ Rollback to Previous Revision]
        E2 --> I2
        I2 --> K2[🏁 End]
    end
```

### Database Deployments Rollout

Database deployments are handled as part of the application deployment process. During the deployment, database migrations are applied to ensure the database schema is up-to-date. If the deployment fails, the changes are rolled back to the previous state to maintain database integrity.

```mermaid

flowchart TD
    subgraph Database Deployment
        A[🚀 Start] --> B[🔍 Check Database State]
        B --> C{🔄 Migrations Pending?}
        C -->|✔️ Yes| D[🔄 Apply Pending Migrations]
        C -->|❌ No| E[🏁 End]
        D --> F[📊 Monitor Migration Progress]
        F --> G{✅ Successful?}
        G -->|✔️ Yes| H[🏁 End]
        G -->|❌ No| I[↩️ Rollback Migration]
        I --> E
        E --> H
    end

```