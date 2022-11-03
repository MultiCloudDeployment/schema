﻿using System;
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
            SkuName = SkuName.Basic;
            DisableLocalAuth = DefaultDisableLocalAuth;
            PublicNetworkAccess = DefaultPublicNetworkAccess;
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
        private SkuName _SkuName { get; set; }
        public SkuName SkuName
        {
            get => _SkuName;
            set
            {
                if (value == null)
                {
                    _SkuName = SkuName.Basic;
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
                        buffer.AppendLine($"  skuName             = \"{this.SkuName.Value}\"");
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
        public static explicit operator AutomationAccount(string json)
        {
            return new AutomationAccount(json);
        }
    }
    public class SkuName
    {
        private SkuName(string value) { Value = value; }
        public string Value { get; private set; }
        public static SkuName Basic { get { return new SkuName("Basic"); } }
        public static SkuName Free { get { return new SkuName("Free"); } }
    }
}