### 1. Diferencia entre var, dynamic y object
- var: Su tipo se infiere de la parte derecha de la expresión.
Una variable var no puede ser igual a null ni no tener asignación.
Su función es dar soporte a los tipos anónimos.
- dynamic: Es un tipo dinámico, que puede cambiar entre tipos. Obviamente no siempre funcionara y dará error de ejecución si no es posible la conversión.
- object: Funciona igual que el anterior pero no desactiva la comprobación de tipos como si hace dynamic.
```
// Compila
dynamic d = "eiximenis";
string str = d.ToUpper();
// NO compila
object d2 = "eiximenis";        // Ok
string str2 = d2.ToUpper();
```

### 2. CURIOSIDADES C#
- Una constante no es igual que un campo readonly. Mientras que la constante puede asignarse solo en la declaración, el campo readonly puede asignarse también en un constructor. Además, las constantes son constantes en tiempo de ejecución, mientras los campos readonly lo son en tiempo de ejecución.
- El tipo sbyte admite negativos y, va de -127 a 127.
- El tipo uint solo admite valores positivos.
- 127.3f es float, 127.3d es double y 127.3m es un decimal.
- int? Permite los nulos en ese tipo.

#### 2.1. TIPOS ANÓNIMOS Y REFLECTION
- Los tipos anónimos tiene estas características:
  - Son de solo lectura.
  - Pueden tener métodos pero el método no puede especificarse con una expresión lambda.
  - Funciona el reflection, es decir, la capacidad de poder obtener las propiedades de un objeto 
  ```
  foreach (var elem in pruebaAnonimo.GetType().GetProperties())
  {
     Console.WriteLine($"{elem.Name} {elem.GetValue(pruebaAnonimo)} {pruebaAnonimo.GetType().GetProperty(elem.Name)}");
  }
  ```
- Para llamar al constructor padre se usa:
```
:base(parametros)

```
#### 2.2. SERIALIZAR
Para serializar y deserializar se utiliza JSONSerializer de la librería de System.Text.Json.
Anteriormente se usaba Newtonsoft.Json, pero consume mas y es mas lento.

#### 2.3 GENERIC
Los generic permiten hacer clases que pueden recibir distintas clases. Con la clausula where se restringe el acceso a solo aquellas clases que implementen el interfaz. Ej:
```
public class SendRequest<T> where T : ISendRequest
```

#### 2.4. LINQ
Linq extiende las propiedades de los objetos.
```
var query = from person in people
           join pet in pets on person equals pet.Owner
           select new { OwnerName = person.FirstName, PetName = pet.Name };
```

#### 2.5. DELEGADOS, FUNCTION, ACTION, PREDICADOS
- Los delegados permiten enviar funciones por parámetro.
- Func es una versión mejorada de los delegados. Por ejemplo, en el siguiente caso se indica que la función tiene un parámetro string y devuelve un int de salida. El número máximo de parámetros es de 16.
```
Func<string,int> mostrar = Show;
```
- Action es igual que los delegados pero no devuelve nada, solo ejecuta.
- Predicado es una implementación de un delegado. En este caso, sólo devuelve un true o un false.
- Para extender se utiliza la palabra this antes del parámetro.
- Para recorrerse una lista de forma paralela se usa la clase Parallel. Ej:
```
List<int> listaNumeros = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
Parallel.ForEach(listaNumeros, c => Console.WriteLine(c));
```
- Un Func puede llamar a otro Func. Ej:
```
Func<int,Func<int,int>,int> fnHigher = (number, fn) =>
  {
    if(number > 10) return fn(number);
    else return number;
  };

int resultado = fnHigher(300, (number) => number * 2);
Console.WriteLine(resultado);

```
- Con Predicate y expresiones lambda se puede hacer un validador de objetos. Ej:
[Validador](https://github.com/Asurbanipal1977/FundamentosCSharp/blob/main/Closure/Program.cs)

#### 2.6 TASK
  - Task se utiliza para tareas asíncronas. 
  - Si se ejecuta un array de Task, no se puede determinar que tarea acabará antes.
  - Para cancelar una tarea hay que usar un token. Ese token se pasará por parámetro en el Task.Delay para que, si antes de ese tiempo se cancela,  deje cancelar. Ej:
  ```
  var cancel = new CancellationTokenSource();
  var token = cancel.Token;

  Task[] tasks = new Task[3];
  tasks[0] = Task.Run(async delegate
  {
      await Task.Delay(2000, token);
      Console.WriteLine("Tarea 1");
  });
  tasks[1] = Task.Run(async delegate
  {
      await Task.Delay(2000);
      Console.WriteLine("Tarea 2");
  });
  tasks[2] = Task.Run(async delegate
  {
      await Task.Delay(2000, token);
      Console.WriteLine("Tarea 3");
  });

  await Task.Delay(100);
  cancel.Cancel();

  try { Task.WaitAll(tasks); } catch { }

  foreach (var item in tasks)
  {
      Console.WriteLine($"{item.Id} {item.Status}");
  }
    
 ````

#### 2.7. CLOSURES
- Los closures son funciones que pueden acceder a variables no locales (externas a la función), pero que son útiles a la función. Un closure en C# toma la forma de un método delegado / anónimo en línea. Se adjunta un cierre a su método principal, lo que significa que se puede hacer referencia a las variables definidas en el cuerpo del método principal desde el método anónimo.
Los closure devuelven como respuesta una función y permiten guardar el estado entre ejecuciones. Ej:
[Closure](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/Closure)

#### 2.8. PRUEBAS UNITARIAS
Permiten asegurar la funcionalidad de la aplicación de manera que cualquier cambio no pueda provocar un mal funcionamiento de la aplicación. Un de las maneras es realizar un MStest.
Para lanzar la prueba se tiene que usar el explorador de pruebas. Desde este explorador se puede lanzar todas las pruebas existentes.
Ej:
[Test](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/AuthTestingTests)

#### 2.9. SCAFFOLDING PARA FRAMEWORK
- Hay que instalar Microsoft.EntityFrameworkCore.SqlServer y Microsoft.EntityFrameworkCore.Tools en Nuget
- Desde la línea de comando lanzar:
scaffold-dbcontext "Server=gigabyte-sabre\sqlexpress;Database=pruebas;integrated security=True;" Microsoft.EntityFrameworkCore.SqlServer -outputdir Models -context EFContext

#### 2.10. PETICIONES HTTPCLIENT
Nos permite llamar a servicios externos. Se tiene que crear el objeto httpClient, llamar a GetAsync y, después, leer la respuesta con httpResponse.Content.ReadAsStringAsync o ReadAsAync<T>

#### 2.11. PATTERN-MATCHING
Es la capacidad para hacer una comparación de una manera muy reducida con un switch
```
public static string Level (beer Beer) => beer.Alcohol switch
  <=0 => "water",
  >0 and <=6 => "medium",
  _ => "high"  

```
    
#### 2.12. INTERFAZ DEFAULT METHOD
Desde C# 6 se puede poner un método por defecto en una interfaz, cosa que antes no se podía

#### 2.13. AUTOMAPPERS
Nos permite pasar datos de un objeto a otro de manera automática. Para poderlo usar hay que:
- Instalar la librería AutoMapper.
- Crear el MapperConfiguration e inyectar la dependencia así:
```
IMapper mapper = mapperConfig.CreateMapper();
services.AddSingleton(mapper);
services.AddMvc();
```
- Crear una clase que herede de Profile com oesta:
```
public class MappingProfile : Profile
{
    public MappingProfile()
    {
       //Nos permite mapear aunque el nombre de los campos cambie
       CreateMap<ClienteRequest, Cliente>()
                .ForMember(d => d.Name, o=>o.MapFrom(s=>s.Nombre))
                .ForMember(d => d.Surname, o => o.MapFrom(s => s.Apellido));
    }
}
```
- En un controller usar ese mapeo
Cliente cliente = mapper.Map<Cliente>(clienteRequest);

#### 2.14. CONSTRUCTOR ESTÁTICO
Se puede tener un constructor estático que solo se va a ejecutar la primera vez que haces referencia a esa clase. Es decir, si creas dos instancias de un clase, solo se va a ejecutar la primera vez.

#### 2.15. YIELD
Permite ejecutar un método devolviendo resultados sin que acabe el método. Ej:
[Yield](https://github.com/Asurbanipal1977/FundamentosCSharp/blob/main/Closure/Program.cs)    

#### 2.16. COALESCE NULL
Son las ??. Por ejemplo: Console.WriteLine(a ?? b), imprime b si a es null y a si es not null.
a ??= b asignará el valor de b si a es null.
    
Ejemplo: [AutoMapperMVC](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/AutoMapperMVC)

#### 2.17. SOBRECARGA DE OPERADORES
Permite poder sobrecargar los métodos +, -, *, =, /
Ejemplo de concatenación de listas:
```
public class Persona<T> : List<T>
{
  public static Persona<T> operator + (Persona<T> p1, Persona<T> p2)
  {
    Persona<T> result = new Persona<T>();
    p1.ForEach(elem => result.Add(elem));
    p2.ForEach(elem => result.Add(elem));
    return result;
  }
}
```
    
#### 2.18. FLUENTVALIDATION
- Se instala la librería FluentValidation.
- Se crea una clase que herede de AbstractValidator<T>
- Se crea un constructor como:
```
    public PostValidation(List<Post> posts)
    {
       RuleFor(r => r.Id).NotNull().NotEmpty().GreaterThan(0);
       RuleFor(r => r.UserId).NotNull().NotEmpty().GreaterThan(0);
       RuleFor(r => r.Title).NotNull().NotEmpty().MaximumLength(20).MinimumLength(1);
       RuleFor(r => r.Id).Must(NoExistPost).WithMessage("Ya existe un post igual");
       this.posts = posts;
     }
```
Un ejemplo en: [FluentValidation](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/Concurrencia)

Para formularios hay que usar la librería FluentValidation.ASpNetCore

Un ejemplo en: [FluentValidation](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/AspFirstMVC)    

#### 2.19. Uso de goto
El goto se puede usar en switch case. Bastaría con poner 'goto case valor'

#### 2.20. ATRIBUTOS OBSOLETOS
Se puede indicar poniendo antes del atributo de la clase [ObsoleteAttribute("El atributo ya no es válido")]. Si se pasa como parámetro un true, ya no compilaría la clase con ese atributo.

#### 2.21. TRANSFORMACIONES EXPLICITAS E IMPLICITAS
Se puede transformar un objeto en otro mediante:
- public static explicit operator objetoAlQueTransformas(ObjetoDesdeElqueTransformas o)
  {      
  }

public static implicit operator objetoAlQueTransformas(ObjetoDesdeElqueTransformas o)
  {      
  }

La diferencia entre implicit y explicit radica en si al hacer el cast de un objeto a otro tenemos o no que pasar el tipo del objeto al que transformamos.
    
#### 2.22. SECRETOS
Permiten almacenar información confidencial que no queremos que se suba. Esos datos se guardarán en un archivo en una ruta local de nuestra máquina como un json.
Pa usarlo:
    - Botón derecho en el proyecto y "Administrar secretos de usuario".
    - Es un archivo json dónde pondremos la información confidencial.
    - Para usarlo, se inyecta la depencia en el startup: services.AddSingleton<IConfiguration>(Configuration);
    - En el fichero donde se use hay que hacer algo como esto:
    ```
        private IConfiguration _configuration;
        public PostController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Create()
        {
            ViewBag.Message = TempData["Message"];
            var clave = _configuration["clave"];
            return View();
        }
    ```
Un ejemplo en: [Secretos](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/AspFirstMVC)    


### 3. C#.Net Core
#### 1. Inyección de dependencias
Para inyectar dependencias en .Net Core se puede usar estas sentencias, según el ciclo de vida:
- AddTransient: Los servicios son creados cada vez que hay una petición.Este ciclo de vida funciona mejor para servicios sencillos y sin estado.
- AddScope: Los servicios se crean una vez por petición dentro del alcance. Por ejemplo, para un misma petición reutiliza la misma instancia.
- AddSingleton: Los servicios solo se crean una vez, en la parte de configuración del servicio y todas las siguientes veces se usa la misma instancia
- AddHttpClient: Es un singleton que añade la funcionalidad de HttpClient.

Estos métodos se usan en la clase startup, que es donde se encuentra la inversión del control. Ej:
[AspFirstMVC](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/AspFirstMVC)

En este caso, también hay un control de errores, que se realiza con:
- ModelState.AddModelError("ErrorMessage", $"Es un error: {e.Message}"); : En el controlador
- @Html.ValidationSummary(false, "", new { @class = "text-danger" }) : En la vista


### 4. NOVEDADES EN C# 9

- **Top Level Statement**: Permite prescindir de la declaración de clase y de spacename. Se puede instalar en ILSpy para ver el ensamblado resultante.

No se puede usar si utilizas una definición de clase y creas el objeto después.
Solo puede haber un archivo con top level statement.

- **Init only setters**: Se trata de valores setters que no pueden cambiar. Se definiria de la siguiente manera:
```
public int parametro {get; init;}
```

- **target typed new expression**: Se infiere el tipo del dato instanciado con el tipo de dato indicado a la izquierda:
```
Customer customer = new();
```
- **Patrones relacionales**: Se puede usar un switch de la siguiente forma:
```
switch(edad)
{
  case > 0 and <= 3: 
    Console.WriteLine("Infant");
    break;
  case >= 4 and <= 12: 
    Console.WriteLine("Niño");
    break;
  _: 
   Console.WriteLine("Adulto");
   break;
}
```
- **records**: Se usa record en lugar de class al definir la clase.
```
public record Person (string name);
```
  - Crearía una propiedad con get e init, es decir, inmutable.
  - Es importante hacer notar que este tipo de datos se comporta como un tipo de datos por valor pero, realmente es un tipo de datos por referencia.
  - Si todas las propiedades son iguales entre dos objetos record, arrojará un true en la comparación. Si usas el método ReferenceEquals(record1,record2), arrojará un false.
  - Al hacer el Console.WriteLine de un record, mostrará el contenido del objeto, como si fuera un Json.
  - Es muy fácil hacer una copia.
  ```
  Person p = new ("Juan");
  Person p2 = p with {
    name = "Pedro"
  };
  ```
  - Un record puede heredar de otro record.
  ```
  public record Person2(string name):Person(name);
  ```

### 5. NOVEDADES EN C# 10 EN .NET 6
El objetivo era unificar las plataformas .net core, .net framework y Xamarin. Es el puente para llegar a esta solución que se espera en .net 7.
- Unificado y extendido.
- Extienden las capacidades de Blazor para aplicaciones híbridas y aplicaciones de escritorio en Blazor
- Para crear una aplicación android vasta con poner en la consola: 
  - donet new android
  - donet run: para ejecutar
- Código abierto.
- Se da soporte a Android, IOs, Mac y Windows ARM64.
- El sistema de contenedores de .net 6 está basado en Debian 11.
- .net MultiPlatform App UI (MAUI): Es una unificación y extensión de lo que ya tenía Xamarin.
- Minimal API: En .net 6, si creas un proyecto de .net core vacío, se crea un minimal API. La principal característica es que genera un API con código mínimo. Ej:
[MinimalAPI](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/MinimalAPI)

- Se puede crear estructuras (struct) con constructores sin parámetros.
- Extended properties patterns. En clases que se asocian a otras clases, ya se puede usar el punto para acceder a las propiedades de la otra clase. Ej:
[ExtendedProperties](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/ExtendedProperties)

- Se puede usar constantes en constantes de cadenas interpoladas. Todas deben ser constantes interpoladas en este caso.
- Sellado de sobreescritura de método ToString() en records. Para ello se usa sealed.
- global using en namespace. Permite que todos los archivos de una jerarquía menor hereden ese using. Ej:
[global using](https://github.com/Asurbanipal1977/FundamentosCSharp/blob/main/ExtendedProperties/Program.cs)
- Cambios en las expresiones lambda. 

  - Son equivalentes:
  ```
  Predicate<int> predicate1 = (x) => x > 1;
  var predicate1 = (int x) => x > 1;
  Console.WriteLine(predicate1(2));

  Func<int,int,int> suma = (x,y) => x+y;
  var suma = (int x, int y) => x+y;
  ```

  - Devoluciones híbridas
  ```
  var p = object (bool b) => b ? 1 : "1";
  Console.WriteLine(p(true).GetType());
  ```

  - Cambios en Linq:
    - Index y Range: 
    Se puede obtener fácilmente el penúltimo elemento de una lista: list.elementAt(^2).Name
    Se puede tomar los primeros elementos de una lista: lista.Take(..3) y para recorrerse los tres últimos, lista.Take(^3..)
    - DistinctBy y UnionBy: list.DistinctBy(c=>c.property) permitirá obtener todos los elementos que tengan el valor de esa propiedad distinto.
    - MaxBy y MinBy
    - Chunk. Se utiliza para empaquetar listas. list.Chunk(2) crea un arreglo de arreglos.
    - FirstOrDefault(new Clase {propiedad=""}).propiedad. Nos permite devolver un valor para el campo si no existe un elemento que cumpla la condición.
    - Zip puede combinar tres colecciones: 
     list1.Zip(list2, (a,b) => a.Name + b)

    Ahora:
    list1.Zip(list2,list3)

    La lista resultante tendrá un número se elementos igual al menor de las tres listas y se puede obtener los elementos con: a.First, a.Second, a.Third
  
Se puede usar inyección de dependencia y swagger. Para usar swagger:
1) Se importan las librerías: Swashbuckle.AspNetCore y Swashbuckle.AspNetCore.Swagger
2) Se da permiso para que pueda explorar los métodos que se exponen desde el API: builder.Services.AddEndpointsApiExplorer();
3) Se inyecta el servicio de swagger: builder.Services.AddSwaggerGen();
4) Se usa swagger: app.UseSwagger(); y app.UseSwaggerUI();

### 6. CREAR Y USAR VISUAL STUDIO CODE CON PROYECTO NET 5
- [Descarga de SDK](https://dotnet.microsoft.com/download)
- Configurar vsCode para net 5: Para instalar la extensión de C# ve al panel del lado izquierdo y presiona sobre el ícono cuadrado > Busca C# > Presiona “Instalar” y luego aparecerá la opción de “Recargar para activar”.
- Para crear proyectos se usa el comando dotnet.
  - dotnet new [console | web | mvc | angular | blazorserver] [nombreproyecto]
  - Se ejecuta con dotnet run
 
- Github copilot: La extensión de vscode que nos ayuda a programar mediante inteligencia artificial.

### 7. SWAGGER Y ANGULAR
Este es un proyecto con swagger junto con un proyecto de Angular que usa el api de swagger.
1. Creamos un proyecto API o minimal API.
2. Se tiene que dar permiso para permitir el acceso a ese API  y evitar el error de CORS.
3. Se genera el proyecto de angular.
4. Se copia el json del swagger, que tiene la definición del servicio, en una carpeta del proyecto angular.
5. Añadir en ese json el campo servers en el que indicaremos la ruta del back.
6. Se instala una librería que permita leer el json que utiliza swagger. Esa librería puede ser: 
[ng-openapi-gen](https://www.npmjs.com/package/ng-openapi-gen)
En esta ruta vienen las instrucciones de instalación
7. Esto te genera el módelo y te crea el servicio que hay que inyectar.
8. Para el correcto funcionamiento hay que importar el modulo HttpClientModule.
  
Ejemplo:
[angularswagger](https://github.com/Asurbanipal1977/FundamentosCSharp/blob/main/angularswagger/app/src/app/app.component.ts)
