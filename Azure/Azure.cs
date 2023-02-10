using System;
using System.Text.Json.Serialization;

namespace MCD.Azure
{
    public class Azure
    {
        public Azure()
        {
            DeploymentLanguage = DeploymentType.Terraform;
            Tenant = new();
        }
        public Azure(Azure azure)
        {
            DeploymentLanguage = azure.DeploymentLanguage;
            Tenant = new Tenant(azure.Tenant);
        }
        public DeploymentType DeploymentLanguage { get; set; }
        public Tenant Tenant { get; set; }
        public enum DeploymentType
        {
            Arm,
            Bicep,
            Terraform
        }
    }
}