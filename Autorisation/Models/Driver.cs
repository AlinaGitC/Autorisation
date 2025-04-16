using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autorisation.Models
{
    public class Driver
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PassportSerial { get; set; }
        public string PassportNumber { get; set; }
        public double PostCode { get; set; }
        public string Address { get; set; }
        public string AddressLife { get; set; }
        public string Company { get; set; }
        public string JobName { get; set; }
        public double Phone { get; set; }
        public string Email { get; set; }
        public string PhotoDescription { get; set; }
        public DateTime LicenceDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Categories { get; set; }
        public string LicenceSeries { get; set; }
        public string LicenceNumber { get; set; }
        public string VIN { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public double Year { get; set; }
        public double Weight { get; set; }
        public string Color { get; set; }
        public double EngineType { get; set; }
        public string TypeDrive { get; set; }
    }
}
