using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MCD.Azure.Resources
{
    public class AutomationAccount
    {
        public AutomationAccount()
        {
            DisableLocalAuth = false;
            PublicNetworkAccess = true;
        }
        [Required]
        [Display(Name = "name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "location")]
        public string Location { get; set; }
        [Display(Name = "tags")]
        public List<Tag> Tags { get; set; }
        [Required]
        [Display(Name = "SkuName")]
        [DefaultValue("Basic")]
        public string SkuName { get; set; }
        [DefaultValue(false)]
        public bool DisableLocalAuth { get; set; }
        [DefaultValue(true)]
        public bool PublicNetworkAccess { get; set; }
        [Required]
        public string ResourceGroup { get; set; }
        public string ToString(params string[] args)
        {
            string TemplateLanguagee = args[0];
            string Source = args[1];
            string DeploymentId = args[2];

            StringBuilder buffer = new StringBuilder();

            switch (TemplateLanguagee.ToLower())
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
                        buffer.AppendLine($"  skuName             = \"{this.SkuName}\"");
                        buffer.AppendLine("");
                        buffer.AppendLine($"  disableLocalAuth    = {this.DisableLocalAuth}");
                        buffer.AppendLine($"  publicNetworkAccess = {this.PublicNetworkAccess}");
                        buffer.AppendLine("");
                        buffer.AppendLine($"  tags                = {this.Tags}");
                        buffer.AppendLine("");
                        buffer.AppendLine("}");
                    break;
                case "arm":
                    break;
            }

            return buffer.ToString();
        }
    }
}