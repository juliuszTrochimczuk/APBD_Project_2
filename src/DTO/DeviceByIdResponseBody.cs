using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DTO
{
    public class DeviceByIdResponseBody(string name, string deviceTypeName, bool isEnabled, 
                                            JsonElement additionalProperties, EmployeesShortDataResponseBody? currentUser)
    {
        public string Name { get; set; } = name;

        public string DeviceTypeName { get; set; } = deviceTypeName;

        public bool IsEnabled { get; set; } = isEnabled;

        public JsonElement AdditionalProperties { get; set; } = additionalProperties;

        public EmployeesShortDataResponseBody? CurrentUser { get; set; } = currentUser;
    }
}
