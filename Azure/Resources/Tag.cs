using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MCD.Azure.Resources
{
    public class Tag
    {
        public Tag()
        {

        }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}