using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtendedProperties.Clases
{
    public class Person
    {
        public string Name { get; set; }
        public Address Address { get; set; }
    }

    public class Address
    {
        public string State { get; set; }
        public string City { get; set; }
    }

    public static class Discount
    {
        public static double Get(Person person, double amount) =>
            person switch
            {
                { Address.City: "Madrid" } => amount - amount * 0.04,
                { Address.City: "Barcelona" } => amount - amount * 0.02,
                { Address.City: "Toledo" } => amount - amount * 0.01,
                _ => amount - amount * 0.005
            };
    }
}
