variable "subscription_id" {
  description = "The subscription ID for Azure"
  type        = string
}

variable "container_app_name" {
  description = "The name of the container app"
  type        = string
}

variable "resource_group_name" {
  description = "The name of the resource group"
  type        = string
}

variable "container_app_environment" {
  description = "The ID of the container app environment"
  type        = string
}

variable "revision_suffix" {
  description = "The revision suffix for the container app"
  type        = string
}

variable "container_registry" {
  description = "The container registry URL"
  type        = string
}

variable "image_tag" {
  description = "The image tag"
  type        = string
}

variable "sql_server_id" {
  description = "The ID of the existing SQL Server"
  type        = string
}

variable "sql_server_name" {
  description = "The name of the existing SQL Server"
  type        = string
}

variable "sql_admin_username" {
  description = "The admin username for the SQL Server"
  type        = string
}

variable "sql_admin_password" {
  description = "The admin password for the SQL Server"
  type        = string
  sensitive   = true
}

variable "label" {
  description = "Label"
  type        = string
  default     = "default"
}

variable "traffic_percentage" {
  description = "The percentage of traffic to route to this revision."
  type        = number
  default     = 100
}