global using ExtendedProperties.Clases;
using ExtendedProperties.imprimir;


Person person = new Person()
{
    Name = "Juan",
    Address = new Address() { City = "Madrid", State = "España" }
};
Console.WriteLine(Discount.Get(person, 20));
const string cadena1 = "Prueba1";
const string cadena2 = "Prueba2";
const string cadena3 = $"{cadena1} {cadena2}";
Console.WriteLine(cadena3);

MostrarDato.imprimir(person);

