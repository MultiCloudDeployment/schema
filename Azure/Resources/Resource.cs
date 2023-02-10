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
        public Resource(Resource resource)
        {
            if (resource.AutomationAccounts.Count > 0)
            {
                AutomationAccounts = new List<AutomationAccount>();
                foreach (AutomationAccount automationAccount in resource.AutomationAccounts)
                {
                    this.AutomationAccounts.Add(automationAccount);
                }
            }

        }
        public List<AutomationAccount> AutomationAccounts { get; set; }
    }
}