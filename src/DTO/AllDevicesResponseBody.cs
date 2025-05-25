using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class AllDevicesResponseBody(int id, string name)
    {
        public int Id { get; set; } = id;

        public string Name { get; set; } = name;
    }
}
