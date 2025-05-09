resource "azurerm_mssql_database" "production" {
  name      = "RazorPagesMovieContext"
  server_id = var.sql_server_id
  sku_name  = "S0"
}

resource "azurerm_container_app" "production" {
  name                = var.container_app_name
  resource_group_name = var.resource_group_name
  container_app_environment_id = var.container_app_environment
  revision_mode       = "Multiple"

  ingress {
    external_enabled = true
    target_port      = 80

    traffic_weight {
      percentage      = var.traffic_percentage
      label           = var.label
      latest_revision = true
    }
  }

  template {
    revision_suffix = "${var.revision_suffix}-${var.image_tag}"

    container {
      name   = var.container_app_name
      image  = "${var.container_registry}:${var.image_tag}"
      cpu    = 2.0
      memory = "4Gi"

      env {
        name  = "ConnectionStrings__RazorPagesMovieContext"
        value = "Server=${var.sql_server_name}.database.windows.net,1433;Database=${resource.azurerm_mssql_database.production.name};User ID=${var.sql_admin_username};Password=${var.sql_admin_password};"
      }
    }
  }
}