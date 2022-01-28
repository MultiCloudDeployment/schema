using System;
using System.Text.Json.Serialization;

namespace MCD.Azure
{
    public class Azure
    {
        public Azure()
        {
            Tenant = new();
        }
        public string DeploymentLanguage { get; set; }
        public Tenant Tenant { get; set; }
    }
}