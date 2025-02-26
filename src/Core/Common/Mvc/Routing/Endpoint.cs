namespace GamaEdtech.Common.Mvc.Routing
{
    using System.Collections.Generic;

    public struct Endpoint
    {
        public string? EndpointName { get; set; }

        public KeyValuePair<string, string>? Area { get; set; }

        public KeyValuePair<string, string>? Action { get; set; }

        public KeyValuePair<string, string>? Controller { get; set; }
    }
}
