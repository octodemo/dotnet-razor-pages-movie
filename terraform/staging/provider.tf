provider "azurerm" {
  features {}

  subscription_id = var.subscription_id
  use_oidc = true
  use_msi = false
}