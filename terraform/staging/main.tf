resource "azurerm_mssql_database" "staging" {
  name     = "Staging_RazorPagesMovieContext"
  server_id = var.sql_server_id
  sku_name = "S0"
}

resource "azurerm_container_app" "staging" {
  name                = "${var.container_app_name}-staging"
  resource_group_name = var.resource_group_name
  container_app_environment_id = var.container_app_environment
  revision_mode       = "Single"

  ingress {
    external_enabled = true
    target_port      = 80
    transport        = "http"

    traffic_weight {
      percentage      = 100
      label           = "default"
      latest_revision = true
    }
  }

  template {
    container {
      name   = "${var.container_app_name}-staging"
      image  = "${var.container_registry}:${var.image_tag}"
      cpu    = "2.0"
      memory = "4Gi"

      env {
        name  = "ConnectionStrings__RazorPagesMovieContext"
        value = "Server=${var.sql_server_name}.database.windows.net,1433;Database=${azurerm_mssql_database.staging.name};User ID=${var.sql_admin_username};Password=${var.sql_admin_password};"
      }

      env {
        name  = "DISABLE_SESSION"
        value = "false"
      }

      env {
        name  = "AZURE_BLOB_KEYRING_CONNECTION_STRING"
        value = "DefaultEndpointsProtocol=https;AccountName=razorpagesmoviestorage;AccountKey=kOXkWNLVhNZk/dTU6bH6ZOcrPrpeC13gms2XdOA/fqAGB+sUNgqnjI4yn07ODMkbacJxlL2oqliF+AStuZxNxw==;EndpointSuffix=core.windows.net"
      }

      env {
        name  = "AZURE_BLOB_KEYRING_CONTAINER"
        value = "dataprotection"
      }
    }
  }
}

# Patch Sticky sessions for UI test consistency
resource "azapi_update_resource" "sticky_session_staging" {
  type        = "Microsoft.App/containerApps@2024-03-01"
  resource_id = azurerm_container_app.staging.id
  body = {
    properties = {
      configuration = {
        ingress = {
          stickySessions = {
            affinity = "sticky"
          }
        }
      }
    }
  }

  depends_on = [azurerm_container_app.staging]
}