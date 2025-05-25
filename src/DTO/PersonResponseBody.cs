using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PersonResponseBody(int id, string passportNumber, string firstName, string? middleName, 
                                    string lastName, string phoneNumber, string email)
    {
        public int Id { get; set; } = id;

        public string PassportNumber { get; set; } = passportNumber;

        public string FirstName { get; set; } = firstName;

        public string? MiddleName { get; set; } = middleName;

        public string LastName { get; set; } = lastName;

        public string PhoneNumber { get; set; } = phoneNumber;

        public string Email { get; set; } = email;
    }
}
