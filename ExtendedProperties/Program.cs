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

//Son iguales
Predicate<int> predicate1 = (x) => x > 1;
Console.WriteLine(predicate1(2));

var predicate2 = (int x) => x > 1;
Console.WriteLine(predicate2(1));

//Devoluciones híbridas
var p = object (bool b) => b ? 1 : "1";
Console.WriteLine(p(true).GetType());
