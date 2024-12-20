using System;
using System.Collections.Generic;

namespace domain
{

	public class Customer
	{
        public int Number { get; set; }

        public string FirstName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public List<Project> Projects { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Traffic { get; set; }
        public int TIN { get; set; }

        public Customer()
        {
        }

        public Customer(int numder, string name, string phoneNumber, List<Project> projects, 
            string email, string company, string traffic, int tIN)
        {
            Number = numder;
            Name = name;
            PhoneNumber = phoneNumber;
            Projects = projects;
            Email = email;
            Company = company;
            Traffic = traffic;
            TIN = tIN;
        }
    }
}
