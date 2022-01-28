using System;
using System.Text.Json.Serialization;

namespace MCD
{
    public class DesignDocument
    {
        public DesignDocument()
        {
            Platform = new();
        }
        public Platform Platform { get; set; }
    }
}