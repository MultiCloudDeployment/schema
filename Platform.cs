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
        public Platform(Platform platform)
        {
            Azure = new Azure.Azure(platform.Azure);
        }
        public Azure.Azure Azure { get; set; }
    }
}