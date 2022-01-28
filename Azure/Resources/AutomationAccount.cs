using System;
using System.Collections.Generic;
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
        public string name { get; set; }
        [Required]
        public string location { get; set; }
        public List<Tag> tags { get; set; }
        [Required]
        public string SkuName { get; set; }
        [DefaultValue(false)]
        public bool DisableLocalAuth { get; set; }
        [DefaultValue(true)]
        public bool PublicNetworkAccess { get; set; }
    }
}