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
    /// <summary>
    /// AutomationAccount Class
    /// 
    /// This contains all the properties and methods in order to
    /// create an AutomationAccount using any one of the supported
    /// deployment languages.
    /// 
    /// Arm
    /// Bicep
    /// Terraform
    /// </summary>
    public class AutomationAccount
    {
        const Sku DefaultSkuName = Sku.Basic;
        const bool DefaultDisableLocalAuth = false;
        const bool DefaultPublicNetworkAccess = true;
        public AutomationAccount()
        {
            SkuName = Sku.Basic;
            DisableLocalAuth = DefaultDisableLocalAuth;
            PublicNetworkAccess = DefaultPublicNetworkAccess;
            Tags = new List<Tag>();
        }
        /// <summary>
        /// Create an AutomationAccount object from JSON
        /// </summary>
        /// <param name="json"></param>
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
        [DefaultValue(Sku.Basic)]
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
        /// <summary>
        /// ToString() Override
        /// </summary>
        /// <param name="deploymentType"></param>
        /// <param name="deploymentId"></param>
        /// <returns>Returns either an ARM parameterfile, Bicep file, or Terraform module</returns>
        public string ToString(Azure.DeploymentType deploymentType, string deploymentId)
        {
            Azure.DeploymentType TemplateLanguage = deploymentType;
            string Source = "github.com/MultiCloudDeployment/terraform-azurerm-automationaccounts.git?ref=1.3.3";
            string DeploymentId = deploymentId;

            StringBuilder buffer = new();

            switch (TemplateLanguage.Name.ToLower())
            {
                case "arm":
                    buffer.AppendLine("{");
                    buffer.AppendLine(" \"$schema\": \"http://schema.management.azure.com/schemas/2019-04-01/deploymentParameters.json#\",");
                    buffer.AppendLine(" \"contentVersion\": \"1.0.0.4\",");
                    buffer.AppendLine(" \"parameters\": {");
                    buffer.AppendLine("  \"name\": {");
                    buffer.AppendLine($"   \"value\": \"{this.Name}\"");
                    buffer.AppendLine("  },");
                    buffer.AppendLine("  \"location\": {");
                    buffer.AppendLine($"   \"value\": \"{this.Location}\"");
                    buffer.AppendLine("  },");
                    buffer.AppendLine("  \"tags\": {");
                    if (this.Tags.Count > 0)
                    {
                        buffer.AppendLine($"   \"value\": {{{this.Tags}}}");
                    }
                    else
                    {
                        buffer.AppendLine("   \"value\": {}");
                    }
                    buffer.AppendLine("  },");
                    buffer.AppendLine("  \"SkuName\": {");
                    buffer.AppendLine($"   \"value\": \"{this.SkuName}\"");
                    buffer.AppendLine("  \"DisableLocalAuth\": {");
                    buffer.AppendLine($"   \"value\": {this.DisableLocalAuth}");
                    buffer.AppendLine("  },");
                    buffer.AppendLine("  \"PublicNetworkAccess\": {");
                    buffer.AppendLine($"   \"value\": {this.PublicNetworkAccess}");
                    buffer.AppendLine("  }");
                    buffer.AppendLine("  \"ResourceGroupName\": {");
                    buffer.AppendLine($"   \"value\": {this.ResourceGroup}");
                    buffer.AppendLine("  }");
                    buffer.AppendLine(" }");
                    buffer.AppendLine("}");
                    break;
                case "bicep":
                    buffer.AppendLine("module AutomationAccount 'automationaccount.bicep' = {");
                    buffer.AppendLine($"  name: \"{this.Name}-deployment\"");
                    buffer.AppendLine($"  scope: resourceGroup({this.ResourceGroup})");
                    buffer.AppendLine("  params: {");
                    buffer.AppendLine($"    name: \"{this.Name}\"");
                    buffer.AppendLine($"    location: \"{this.Location}\"");
                    if (this.Tags.Count > 0)
                    {
                        buffer.AppendLine($"    tags: {this.Tags}");
                    }
                    buffer.AppendLine($"    DisableLocalAuth: {this.DisableLocalAuth}");
                    buffer.AppendLine($"    PublicNetworkAccess: {this.PublicNetworkAccess}");
                    buffer.AppendLine($"    SkuName: \"{this.SkuName}\"");
                    buffer.AppendLine("  }");
                    buffer.AppendLine("}");
                    break;
                case "terraform":
                    string ModuleName = "aa-" + this.Name + "-" + DeploymentId;

                    buffer.AppendLine($"module \"{ModuleName}\" {{");
                    buffer.AppendLine($"  source = \"{Source}\"");
                    buffer.AppendLine("");
                    buffer.AppendLine($"  name                = \"{this.Name}\"");
                    buffer.AppendLine($"  location            = \"{this.Location}\"");
                    buffer.AppendLine($"  resourceGroup       = azurerm_resource_group.rsg-{this.ResourceGroup}-{DeploymentId}.name");
                    buffer.AppendLine("");
                    buffer.AppendLine($"  skuName             = \"{this.SkuName}\"");
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
            }

            return buffer.ToString();
        }
        /// <summary>
        /// Cast a JSON object to an AutomationAccount
        /// </summary>
        /// <param name="json"></param>
        public static explicit operator AutomationAccount(string json)
        {
            return new AutomationAccount(json);
        }
        public enum Sku
        {
            Free,
            Basic
        }
    }
}