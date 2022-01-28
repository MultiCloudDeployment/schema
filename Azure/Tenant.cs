using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MCD.Azure
{
    public class Tenant
    {
        public Tenant()
        {
            Subscriptions = new List<Subscription>();
        }
        public string id { get; set; }
        public string displayName { get; set; }
        public string[] domains { get; set; }
        public List<Subscription> Subscriptions { get; set; }
    }
}