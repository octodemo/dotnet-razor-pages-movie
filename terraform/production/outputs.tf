output "container_app_id" {
  description = "The ID of the container app"
  value       = azurerm_container_app.production.id
}

output "container_app_url" {
  description = "The URL of the container app"
  value       = azurerm_container_app.production.ingress[0].fqdn
}