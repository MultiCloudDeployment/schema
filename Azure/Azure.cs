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
        public DeploymentType DeploymentLanguage { get; set; }
        public Tenant Tenant { get; set; }
        public class DeploymentType
        {
            private DeploymentType(string value) { Name = value; }
            public string Name { get; private set; }
            public static DeploymentType Arm { get { return new DeploymentType("Arm"); } }
            public static DeploymentType Bicep { get { return new DeploymentType("Bicep"); } }
            public static DeploymentType Terraform { get { return new DeploymentType("Terraform"); } }
        }
    }
}