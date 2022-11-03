using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace MCD.Azure.Resources
{
    public class AutomationAccount
    {
        const bool DefaultDisableLocalAuth = false;
        const bool DefaultPublicNetworkAccess = true;
        public AutomationAccount()
        {
            SkuName = Sku.Basic;
            DisableLocalAuth = DefaultDisableLocalAuth;
            PublicNetworkAccess = DefaultPublicNetworkAccess;
            Tags = new List<Tag>();
        }
        public AutomationAccount(string json)
        {
            AutomationAccount resource = JsonSerializer.Deserialize<AutomationAccount>(json);
            foreach (PropertyInfo r in resource.GetType().GetProperties())
            {
                PropertyInfo p = GetType().GetProperties().Where(p => p.Name.ToLower() == r.Name.ToLower()).FirstOrDefault();
                p.SetValue(this, r.GetValue(resource));
            }
        }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        public List<Tag> Tags { get; set; }
        [Required]
        [DefaultValue("Basic")]
        private Sku _SkuName { get; set; }
        public Sku SkuName
        {
            get => _SkuName;
            set
            {
                if (value == null)
                {
                    _SkuName = Sku.Basic;
                } else
                {
                    _SkuName = value;
                }
            }
        }
        [DefaultValue(DefaultDisableLocalAuth)]
        private bool? _DisableLocalAuth { get; set; }
        public bool? DisableLocalAuth
        {
            get => _DisableLocalAuth;
            set
            {
                if (value == null)
                {
                    _DisableLocalAuth = DefaultDisableLocalAuth;
                } else
                {
                    _DisableLocalAuth = value;
                }
            }
        }
        [DefaultValue(DefaultPublicNetworkAccess)]
        private bool? _PublicNetworkAccess { get; set; }
        public bool? PublicNetworkAccess
        {
            get => _PublicNetworkAccess;
            set
            {
                if (value == null)
                {
                    _PublicNetworkAccess = DefaultPublicNetworkAccess;
                }
                else
                {
                    _PublicNetworkAccess = value;
                }
            }
        }
        [Required]
        public string ResourceGroup { get; set; }
        public string ToString(Azure.DeploymentType deploymentType, string deploymentId)
        {
            Azure.DeploymentType TemplateLanguage = deploymentType;
            string Source = "github.com/MultiCloudDeployment/terraform-azurerm-automationaccounts.git?ref=1.3.3";
            string DeploymentId = deploymentId;

            StringBuilder buffer = new();

            switch (TemplateLanguage.Name.ToLower())
            {
                case "terraform":
                    string ModuleName = "aa-" + this.Name + "-" + DeploymentId;

                    buffer.AppendLine($"module \"{ModuleName}\" {{");
                    buffer.AppendLine($"  source = \"{Source}\"");
                    buffer.AppendLine("");
                    buffer.AppendLine($"  name                = \"{this.Name}\"");
                    buffer.AppendLine($"  location            = \"{this.Location}\"");
                    buffer.AppendLine($"  resourceGroup       = azurerm_resource_group.rsg-{this.ResourceGroup}-{DeploymentId}.name");
                    buffer.AppendLine("");
                    buffer.AppendLine($"  skuName             = \"{this.SkuName.Name}\"");
                    buffer.AppendLine("");
                    buffer.AppendLine($"  disableLocalAuth    = {this.DisableLocalAuth}");
                    buffer.AppendLine($"  publicNetworkAccess = {this.PublicNetworkAccess}");
                    if (this.Tags.Count > 0)
                    {
                        buffer.AppendLine("");
                        buffer.AppendLine($"  tags                = {this.Tags}");
                        buffer.AppendLine("");
                    }
                    buffer.AppendLine("}");
                    break;
                case "bicep":
                    break;
            }

            return buffer.ToString();
        }
        public static explicit operator AutomationAccount(string json)
        {
            return new AutomationAccount(json);
        }
        public class Sku
        {
            private Sku(string value) { Name = value; }
            public string Name { get; private set; }
            public static Sku Basic { get { return new Sku("Basic"); } }
            public static Sku Free { get { return new Sku("Free"); } }
        }
    }
}