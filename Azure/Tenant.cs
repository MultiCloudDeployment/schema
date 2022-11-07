using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MCD.Azure
{
    public class Tenant
    {
        public Tenant()
        {
            Subscriptions = new List<Subscription>()
            { 
                new Subscription()
            };
        }
        public Tenant(Tenant tenant)
        {
            this.id = tenant.id;
            this.displayName = tenant.displayName;
            this.domains = new List<string>();
            foreach (string domain in tenant.domains)
            {
                this.domains.Add(new string(domain));
            }
            Subscriptions = new List<Subscription>();
            foreach (Subscription subscription in tenant.Subscriptions)
            {
                this.Subscriptions.Add(new Subscription(subscription));
            }
        }
        public string id { get; set; }
        public string displayName { get; set; }
        public List<string> domains { get; set; }
        public List<Subscription> Subscriptions { get; set; }
    }
}