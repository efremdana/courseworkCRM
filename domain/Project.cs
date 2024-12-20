using System;


namespace domain
{
	public class Project
	{
        public string ProjectNumber { get; set; }
        public string ProjectName { get; set; }
        public Customer Client { get; set; }
        public decimal Cost { get; set; }
        public string DocumentationType { get; set; }
        public string Inginner { get; set; }
        public string City { get; set; }
        public string Contract { get; set; }
        public string DateStartWork { get; set; }
        public string DateEndWork { get; set; }



        public Project(string projectNumber, string projectName, Customer client, 
            decimal cost, string documentationType, string inginner, string city,
            string contract, string dateStartWork, string dateEndWork)
        {
            ProjectNumber = projectNumber;
            ProjectName = projectName;
            Client = client;
            Cost = cost;
            DocumentationType = documentationType;
            Inginner = inginner;
            City = city;
            Contract = contract;
            DateStartWork = dateStartWork;
            DateEndWork = dateEndWork;
        }

        public Project()
        {
        }
    }
}
