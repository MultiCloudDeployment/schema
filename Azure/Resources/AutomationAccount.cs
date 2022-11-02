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
        public string SkuName { get; set; }
        [DefaultValue(false)]
        public bool DisableLocalAuth { get; set; }
        [DefaultValue(true)]
        public bool PublicNetworkAccess { get; set; }
        public string ToHcl(string ResourceGroup, string Source, string BuildId, string DeviceId)
        {
            var buffer = new StringBuilder();
            string ModuleName = "aa-" + DeviceId + this.Name + "-" + BuildId;

            buffer.AppendLine($"module \"{ModuleName}\" {{");
            buffer.AppendLine($"  source          = \"{Source}\"");
            buffer.AppendLine("");
            buffer.AppendLine($"  rsg             = azurerm_resource_group.rsg-{ResourceGroup}-{BuildId}.name");
            buffer.AppendLine($"  coreDevice      = \"{DeviceId}\"");
            buffer.AppendLine($"  customSuffix    = \"{this.Name}\"");
            buffer.AppendLine("");
            buffer.AppendLine($"  location        = \"{this.Location}\"");
            buffer.AppendLine("");
            buffer.AppendLine("  environment     = var.environment");
            buffer.AppendLine("  tag_buildby     = var.buildby");
            buffer.AppendLine("  tag_buildticket = var.buildticket");
            buffer.AppendLine("  tag_builddate   = var.builddate");
            buffer.AppendLine("}}");

            return buffer.ToString();
        }
    }
}