using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DTO
{
    public class DeviceRequestBody(string name, string deviceType, bool isEnabled, JsonElement additionalProperties)
    {
        public string Name { get; set; } = name;

        public string DeviceType { get; set; } = deviceType;

        public bool IsEnabled { get; set; } = isEnabled;

        public JsonElement AdditionalProperties { get; set; } = additionalProperties;
    }
}
