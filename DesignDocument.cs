using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MCD
{
    public class DesignDocument
    {
        public DesignDocument()
        {
            Platform = new();
        }
        public DesignDocument(string jsonDocument)
        {
            DesignDocument designDocument = JsonSerializer.Deserialize<DesignDocument>(jsonDocument);
            Platform = new Platform(designDocument.Platform);
        }
        public Platform Platform { get; set; }
    }
}