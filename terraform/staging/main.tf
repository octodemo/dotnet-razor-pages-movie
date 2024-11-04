terraform {
  backend "azurerm" {
    resource_group_name  =  "tsvi-rg"
    storage_account_name = "razorpagesmoviestorage"
    container_name       = "tfstatestage"
    key                  = "terraform.tfstate"
  }
}

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
      cpu    = 0.5
      memory = "1.0Gi"

      env {
        name  = "ConnectionStrings__RazorPagesMovieContext"
        value = "Server=${var.sql_server_name}.database.windows.net,1433;Database=${azurerm_mssql_database.staging.name};User ID=${var.sql_admin_username};Password=${var.sql_admin_password};"
      }
    }
  }
}