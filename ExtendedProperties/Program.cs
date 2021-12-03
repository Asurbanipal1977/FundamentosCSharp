using ExtendedProperties;

Person person = new Person()
{
    Name = "Juan",
    Address = new Address() { City = "Madrid", State = "España" }
};
Console.WriteLine(Discount.Get(person, 20));
