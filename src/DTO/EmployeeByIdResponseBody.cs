using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class EmployeeByIdResponseBody(PersonResponseBody person, decimal salary, PositionResponseBody position, DateTime hireDate)
    {
        public PersonResponseBody Person { get; set; } = person;

        public decimal Salary { get; set; } = salary;

        public PositionResponseBody Position { get; set; } = position;

        public DateTime HireDate { get; set; } = hireDate;
    }
}
