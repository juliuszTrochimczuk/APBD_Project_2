using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Others
{
    public class JwtOptions
    {
        public required string Issuer { get; set; }
        public required string Audiance { get; set; }
        public required string Key { get; set; }
        public required int ValidInMinutes { get; set; }
    }
}
