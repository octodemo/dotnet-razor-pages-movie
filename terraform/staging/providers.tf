terraform {

  required_version = ">=1.2"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
    }
    azapi = {
      source  = "Azure/azapi"
      version = "~>2.0"
    }
  }
  backend "azurerm" {
    resource_group_name  = "tsvi-rg"
    storage_account_name = "razorpagesmoviestorage"
    container_name       = "tfstatestage"
    key                  = "terraform.tfstate"
  }
}

provider "azurerm" {
  subscription_id = var.subscription_id
  features {}
}