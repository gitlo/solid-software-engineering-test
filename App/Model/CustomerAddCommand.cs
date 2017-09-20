using System;

namespace App
{
    public class CustomerAddCommand
    {
        public int Id { get; set; }

        public string Firstname { get; set; }

        public string Surname { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string EmailAddress { get; set; }        

        public int CompanyId { get; set; }
    }
}