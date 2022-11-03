using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MCD.Azure.Resources
{
    public class Resource
    {
        public Resource()
        {
            AutomationAccounts = new List<AutomationAccount>()
            {
                new AutomationAccount()
            };
        }
        public List<AutomationAccount> AutomationAccounts { get; set; }
    }
}