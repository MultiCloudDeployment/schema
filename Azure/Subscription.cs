﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MCD.Azure
{
    public class Subscription
    {
        public Subscription()
        {
            Resources = new();
        }
        public Subscription(Subscription subscription)
        {
            this.id = subscription.id;
            this.displayName = subscription.displayName;
            Resources = new Resources.Resource(subscription.Resources);
        }
        public string id { get; set; }
        public string displayName { get; set; }
        public Resources.Resource Resources { get; set; }
    }
}