using System;
using System.Text.Json.Serialization;

namespace MCD
{
    public class Platform
    {
        public Platform()
        {
            Azure = new();
        }
        public Azure.Azure Azure { get; set; }
    }
}