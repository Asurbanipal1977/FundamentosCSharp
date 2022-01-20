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
- Una expresión lambda es una función o método anónimo con el que podemos crear tipo delegados y árboles de expresiones
- Enviroment.NewLine permite poner una linea en blanco tal y como hace \n.
- Se puede encadenar métodos. También puedes crear métodos que encadenen. Para ello siempre debes responder el objeto actual. 
Ej de concatenación: cadena.toUpper().Trim()

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

#### 2.4. LINQ Y PLINQ
Linq extiende las propiedades de los objetos.
```
var query = from person in people
           join pet in pets on person equals pet.Owner
           select new { OwnerName = person.FirstName, PetName = pet.Name };
```

Hay un ejemplo de group by en:
[GroupBy](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/Concurrencia/Program.cs)

Métodos poco usados:
- lista.All(c => c > 5) : Devolvería true o false en función de si se culpe o no la condición del lambda
- lista.SelectMany(e => e.sublista, (lista,sublista)=> new {lista, sublista}).Select(l => new {Name: l.lista.Name, NameSub: l.sublista}) este permite obtener una lista mezclando lista y sublista.

Se puede también leer archivos xml con c#.Ej:
```
var filename = @"C:\Users\mame1\source\repos\FundamentosCSharp\FundamentosCSharp\Concurrencia\store.xml";
XDocument store = XDocument.Load(filename);
var empleadosXML = store.Root.Elements("Empleado").OrderBy(e=>e.Attribute("Nombre").Value).ToList();
empleadosXML.ForEach(empleado => Console.WriteLine(empleado.Attribute("Nombre").Value));
```

PLinq es un Linq pero usando ejecución paralela. Se puede conseguir con los métodos: AsParallel().WithDegreeOfParallelism(2). El segundo método es para indicar el número de hilos.
Ej: [PLinq](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/Concurrencia/Program.cs)

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
Los closures son funciones que pueden acceder a variables no locales (externas a la función), pero que son útiles a la función. Un closure en C# toma la forma de un método delegado / anónimo en línea. Se adjunta un cierre a su método principal, lo que significa que se puede hacer referencia a las variables definidas en el cuerpo del método principal desde el método anónimo.
Los closure devuelven como respuesta una función y permiten guardar el estado entre ejecuciones. Ej:
[Closure](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/Closure)

Se puede también pasar una función como parámetro.Ej:
[Closure](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/Closure)

#### 2.8. PRUEBAS UNITARIAS
Permiten asegurar la funcionalidad de la aplicación de manera que cualquier cambio no pueda provocar un mal funcionamiento de la aplicación. Un de las maneras es realizar un MStest.
Para lanzar la prueba se tiene que usar el explorador de pruebas. Desde este explorador se puede lanzar todas las pruebas existentes.
Ej:
[Test](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/AuthTestingTests)

#### 2.9. SCAFFOLDING
##### 2.9.1. NET CORE
- Hay que instalar Microsoft.EntityFrameworkCore.SqlServer y Microsoft.EntityFrameworkCore.Tools en Nuget
- Desde la línea de comando lanzar:
scaffold-dbcontext "Server=gigabyte-sabre\sqlexpress;Database=pruebas;integrated security=True;" Microsoft.EntityFrameworkCore.SqlServer -outputdir Models -context EFContext

Para poder usar en .net core la configuración de desarrollo y producción:
Por defecto, si estamos en entorno de desarrollo, tomará lo que hay en el fichero de appsettings.Development. En caso de no encontrarse, se tomará, appsetttings.

Para poder leer del fichero en función de las variables de entorno para .net 5, podemos usar:
1. En program.cs se usará un código como este, de manera que el archivo de configuración tome datos del appsettings que corresponda. De todas formas, esa configuración por defecto ya está en el contsructor estático CreateDefaultBuilder
```
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostContext, config) =>
            {
                var env = hostContext.HostingEnvironment;

                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
```
2. En la clase Startup.cs se debe inyectar la configuración mediante la interfaz.
3. En el método ConfigureServices haremos:
```
services.AddDbContext<ApplicationDbContext>(options =>
         options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
```
4. Dentro del archivo json de cada entorno tendremos que definir la cadena de conexión:
```
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=LeerAppSettings;Trusted_Connection=True;MultipleActiveResultSets=true",
  "OtraBase": "Server=server2;Database=Base;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```
Mas detalles en:
[Mas detalles](https://aspnetcoremaster.com/asp.net/core/2019/04/07/aspnetcore-appsettings.html)

Ej: [Uso de bd distintas en desarrollo y producción](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/AspFirstMVC)

Es importante hacer notar que el nombre del entorno saldrá del archivo Properties\launchSettings.json en desarrollo. Para mas detalles:
[Entornos](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-6.0)

#### 2.9.2. NET FRAMEWORK
Desde la versión 4 (Visual studio 2010), se dispone de la posibilidad de tener archivos de configuración por entorno. Habría que hacer algo como esto:
[Varias versiones web.config](http://www.profesional.co.cr/es/2010/11/04/como-usar-diferentes-webconfig-para-cada-ambiente-transformaciones-webconfig-1473/)

Los ficheros de configuración son xml.

#### 2.9.3. ENTITYFRAMEWORK PARA SQLLITE CON FIRST CODE.
1. Crear un proyecto de consola o de otro tipo de .net core.
2. Instalar la librería de entityFramework para sqllite.
3. Crear una clase por cada tabla.
4. Crear una clase que herede de DbContext y sobreescribir lo métodos OnConfiguring y OnModelCreating
5. Generar el acceso a bd y grabar en la tabla.
6. Abrir el archivo con un manejador de BD como Navicat o DB Browser for Lite.
Ej: [SQLLite](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/SqlLiteDoNet)


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
Se puede incluso definir el mismo método implementado en dos interfaces distintas que implemente una clase. La limitación es que debemos indicar que interfaz usamos.Ej:
((IPrueba) oPrueba).Metodo()

Si se ha definido un método igual en la clase, ejecutará el evento de esa clase independientemente del interfaz indicado. 


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
Para usarlo:
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

#### 2.23. GUARDAR Y LEER UN FICHERO
Para guardar un fichero se puede usar varias técnicas:
1) Guardarlo en base de datos. Para ello se usa EntityFrameWork y MemoryStream.
2) Guardarlo en disco.
3) Guardarlo en un S3(Sample Storage Service). Para usar ese servicio, se tiene que importar la librería Minio.
Ej de grabado y lectura de fichero de las tres formas anteriores:
[Guardar y leer fichero en base de datos](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/AspFirstMVC)
    
#### 2.24. COMPARATIVA ENTRE ASÍNCRONO Y SÍNCRONO
Para probar se hace 100 llamadas al servicio de jsonplaceholder. El resultado es que con asíncrono va mas rápido. Para el ejemplo habría que ñlanzarlo comentando la parte de asíncrono y luego la de asíncrono:
[Comparativa](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/Concurrencia/Program.cs)
    
#### 2.25. PROGRAMACIÓN REACTIVA
Se basa en crear un observable y que aquel que se suscriba a dicho observable pueda ejecutar ciertas tareas cada vez que se ejecute el evento asociado al observable. 
- Se usa la libreria System.Reactive que se instala desde Nuget.
- El código sería:
```
    var observable = Observable.FromEventPattern<int>(
                c => MyEvent += c,
                c => MyEvent -= c
   );

   var subscrito1 = observable.Subscribe(c => { Console.WriteLine($"Se ha detectado el cambio por subscrito1: {c.EventArgs}"); });
   var subscrito2= observable.Subscribe(c => { Console.WriteLine($"Se ha detectado el cambio por subscrito2: {c.EventArgs}"); });

   MyEvent(null, 1);
   MyEvent(null, 2);
```
[Programación reactiva](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/Concurrencia/Program.cs)

#### 2.26. EVENTOS
Muy bien explicado en: [Eventos](https://geeks.ms/etomas/2012/01/05/c-bsico-eventos/)
    
Y un ejemplo en: [Eventos](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/Concurrencia/Program.cs)

#### 2.27. TUPLAS
Es una lista finita que contiene una serie de elementos. Ej:
(int,string) valor = (5, "pepe");

Para acceder a los valores:
valor.Item1;
valor.Item2;

También se le puede dar nombre a cada uno de los campos:
(int,string) valor = (entero: 5, cadena: "pepe");

Se puede deconstruir un clase en una tupla. Para ello se tiene que hacer:
public void Deconstruct(out int dato1, out string dato2) => (dato1, dato2) = (Dato1, Dato2);

Para invocarlo se usa:
var (valor1,valor2) = objeto;
    
Para ignorar un valor de la tupla, bastaría con poner _.

Ej:
    [Tuplas](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/Closure/Program.cs)

#### 2.28. CURRIFICACIÓN
Se trata de poder llamar a una serie de funciones de manera secuencial con un solo argumento. Básicamente permite que se pueda llamar a funciones que tienen dos parámetros desde funciones con uno, utilizando expresiones lambda para crear funciones anónimas.
Un ejemplo tenemos en esta dirección:
[Currificacion](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/Closure/Program.cs)
    
#### 2.29. HILOS RECURSOS COMPARTIDOS
Ej: [Hilos](https://github.com/Asurbanipal1977/FundamentosCSharp/blob/main/HilosRecursosCompartidos/Program.cs)

#### 2.30. SUBIR A NUGET
- Dando a lbotón derecho, en Propiedades, Paquetes, se tiene que seleccionar la opción generar paquete Nuget.
- Una vez generado al fichero se puede subir de dos formas:
   - Sitio Nuget: https://www.nuget.org/
   - Comando, sacando la clave del API de la página de nuget:
    
   dotnet nuget push NombreDeTuPaquete.1.0.0.nupkg --api-key tukey --source https://api.nuget.org/v3/index.json
    
#### 2.31. Utilizar Redis en C# .Net
- Se tiene que instalar chocolatey, que es un gestor de paquetes, desde esta url:
[chocolatey](https://chocolatey.org/install)
- Se instala redis: choco install redis-64
- En la aplicación, se instala el paquete StackExchange.Redis
- Después, basta con generar una clase de este tipo:
```
    public class RedisDB
    {
        //Para objetos grandes
        private static Lazy<ConnectionMultiplexer> _lazy;

        public static ConnectionMultiplexer Connection
        {
            get { return _lazy.Value; }
        }
        static RedisDB()
        {
            _lazy = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect("localhost"));
        }
    }
```
    
Ej: [Hilos](https://github.com/Asurbanipal1977/FundamentosCSharp/blob/main/HilosRecursosCompartidos/Program.cs)
    
#### 2.32. PRINCIPIOS SOLID
El principio Solid o principio de responsabilidad única indica que una clase solo debería tener una responsabilidad. Por tanto, tendremos una clase con los datos, otra que, por ejemplo, grabe en base de datos y otra que envie correos.
Para los principios solid ayudan mucho las interfaces, puesto que gracias al poliformismo, podenos invocar al método de una clase que implementa un interfaz 

#### 2.37. MEMOIZATION
Es el proceso de optimización del código para aumentar la velocidad de ejecución. Si tienes una función puro (siempre devuelve lo mismo), no es útil ejecutar la misma función, di no que puedes tener almacenado en una caché los valores de esa ejecución.
Ej: [Memoization](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/HilosRecursosCompartidos)
    
#### 2.38 BLAZOR CON SWAGGER
Podemos usar Blazor con Swagger. Para ello:
- Tomamos el fichero .json del swagger: https://localhost:7177/swagger/v1/swagger.json
- Damos botón derecho en Connected Services y damos a administrar servicios conectados.
- Se llama a los servicios de la API.
Ej: [Blazor](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/BlazorApp)
    
#### 2.39. SIGNALR
Permite recibir la información de un CRUD en tiempo real.
- Se agrega la biblioteca signalr del lado de cliente: @microsoft/signalr@latest
- Se crea una clase que herede de Hub y que realice el envío de los datos.
- Se añade en el startup la inyeccion y se añade el punto de acceso al hub: endpoints.MapHub<PostHub>("/postHub");
- Se añade la libreria js: /lib/microsoft/signalr/dist/browser/signalr.min.js en el fichero dónde queramos que los datos estén en tiempo real.
- Se define con javascript la conexión.
- Se añade la inyección de dependencia del hub a la clase que queramos usar esta librería.
- Se llama a All.SendAsync para que el envío se realice y se muestre en tiempo real los cambios:  
    _hubContext.Clients.All.SendAsync("Receive", post.Id, post.Title);
    
Ej:[SignalR](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/AspFirstMVC)   
    
Se puede también usar signalR para una aplicación de Blazor con parte cliente y servidor. Ej:
[BlazorSignalR](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/BlazorSignalR)   
    
Otro ejemplo con SignalR, EntityFrameWork y IHostedService (Procesos en Segundo plano). En este caso, el SignalR se inyecta en el IHostedService con:
```
    public PopulationHostedService (IHubContext<PopulationHub> hubContext)
    {
       _hubContext = hubContext;
    }
```
Ej:[HostedServiceSignalR](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/HostedServiceSignalR)
    
Mediante signalR se puede crear un chat con varias salas. El ejemplo:
[Chat](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/ChatRoom) 
    
#### 2.40. PROCESOS EN SEGUNDO PLANO
- Se crea una clase que implenta de IHostedService.
- Se implementan los métodos StartAsync y StopAsync.
- Se inyecta en el fichero Startup.cs
Ej:[Segundo Plano](https://github.com/Asurbanipal1977/FundamentosCSharp/tree/main/AutoMapperMVC/IntervalTaskHostedService.cs) 

#### 2.41. GUID
Es un tipo de datos llamado Identificador único global. Se utiliza para poder insertar en una tabla con clave ajena. En ese caso, tenemos que insertar antes en la tabla padre y este tipo de datos nos permite obtener un número único para poder dar de alta en la tabla relacionada.
    
#### 2.42. GENERAR MÉTODOS DE INTERFACES DESDE CLASE
Si se pulsa control + intro encima del método y se da a Extraer a la interfaz, el método se llevará a la interfaz.

#### 2.43. SERVICIOS SOAP CON WCF
WCF es Windows Communication Foundation y permite crear un servicio SOAP (Service Object Action Protocol). Las características de un servicio SOAP:
- Es fuertemente tipado.
- Es mas lento y pesado que un API Rest.
- Se define un fichero wsdl que define el formato de la entrada y salida.
- Se crea mediante una aplicación de servicios WCF.
- Los atributos DataContract y DataMember permite definir la clase de entrada y de salida del servicio.
- Los atributos ServiceContract y OperationContract definen el servicio y los métodos expuestos.
- **Para que los métodos expuestos no den error, hay que definir siempre el get y el set.**

Después se puede crear un cliente que consuma este servicio. Para ello se puede "Agregar referencia a servicio" e indicar este servicio. Si se cambia debe actualizarse el servicio siempre que afecte a los servicios o métodos que se exponen.
    
Ej:[SOAP APi](https://github.com/Asurbanipal1977/WcfPersonas)

Con respecto a los antiguos servicios asmx, estas son las diferencias:
- asmx solo puede ser invocado por http, mientras que wcf puede ser invocado por http, tcp, etc.
- asmx no es flexible, mientras wue en WCF si creas una nueva versión solo debes exponer un nuevo final.
- asmx es:
  - Fácil de montar y configurar
  - No garantiza transmisión de datos, utiliza estructuras rudimentarias para envío de dstos, es lento y es propenso a conflictos entre diversos sistemas operativos.
- WCF:
  - Asegura entrega de mensajes, transacciones seguras, segmentación binaria y reemplaza los xml
  - Requiere mayores conocimientos y tiempo de configuración.

### 3. C#.Net Core
#### 1. Inyección de dependencias
Se trata de un patrón de diseño que se encarga de extraer la responsabilidad de la creación de instancias de un componente para delegarla en otro.
Para inyectar dependencias en .Net Core se puede usar estas sentencias, según el ciclo de vida:
- AddTransient: Los servicios son creados cada vez que hay una petición.Este ciclo de vida funciona mejor para servicios sencillos y sin estado.
- AddScoped: Los servicios se crean una vez por petición dentro del alcance. Por ejemplo, para un misma petición reutiliza la misma instancia.
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
    
  - Se puede acceder a las variables del fichero json mediante la inyección del servicio IConfiguration.
    
  
Se puede usar inyección de dependencia y swagger. Para usar swagger:
1) Se importan las librerías: Swashbuckle.AspNetCore y Swashbuckle.AspNetCore.Swagger
2) Se da permiso para que pueda explorar los métodos que se exponen desde el API: builder.Services.AddEndpointsApiExplorer();
3) Se inyecta el servicio de swagger: builder.Services.AddSwaggerGen();
4) Se usa swagger: app.UseSwagger(); y app.UseSwaggerUI();
Ej: Swagger en Minimal API ->
[Swagger en Minimal API](https://github.com/Asurbanipal1977/FundamentosCSharp/blob/main/MinimalAPI)

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
2. Se tiene que dar permiso para permitir el acceso a ese API  y evitar el error de CORS. El CORS es un mecanisco para evitar que se compartan recursos entre dos dominios.
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
    
#### 8. CÓDIGO JAVASCRIPT EN VISUAL STUDIO CODE
Para poder usar el intellisense de javascript, hay que usar una librería llamada Javascript Snippet Pack. De esta manera tendremos un intellisense muy útil.
    
#### 9. SECURIZAR SERVICIOS CON WEBTOKEN
Se puede securizar servicios mediante la Api JWT. Para securizarlos se debe indicar en el controlador:
[Authorize]

Para realizar el Api nos basamos en: [Construir web api con JWT](https://enmilocalfunciona.io/construyendo-una-web-api-rest-segura-con-json-web-token-en-net-parte-ii/). Se deben usar tres librerías de Nuget: Microsoft.IdentityModel.JsonWebTokens, Microsoft.IdentityModel.Logging, Microsoft.IdentityModel.Tokens

Para almacenar el web token es normal usar el localstorage. Un ejercicio de ejemplo le tenemos en:
[https://github.com/Asurbanipal1977/WebApiSeguro](https://github.com/Asurbanipal1977/WebApiSeguro)
    
#### 10. CAMPOS NULLABLES EN .NET 6
    
Si no queremos que tenga en cuenta y oblige a indicar si acepta o no nulos, se puede poner el campo <nullable> a disable en la definición del proyecto.
```
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

</Project>
```
    
#### 11. RAZOR
Se puede mezclar código cliente y servidor mediante @ y @{}. Las páginas razor aportan un gran dinamismo a una página. 
1. Se puden hacer if, for, foreach, ...
2. Se pueden crear vistas parciales y añadirlas mediante: 
```
<partial name="Nombre de la vista parcial" />
```
También se puede definir el modelo que se va a pasar a esa sección o vista parcial: 
```
<partial name="_SeccionProyectos" model="Model.Proyectos" />
```
3. Para añadir validación javascript:
- En la página se añade un span en el campo que queramos vañlidar:
    <span asp-validation-for="Nombre" class="text-danger"></span>
- Se añade en esa página una sección de esta forma:
```
@section Scripts{
    <partial name="_ValidationScriptsPartial"></partial>
}
```
- En el layout esta este código, que hará que se ejecute la parte de los cripts parciales al final de la carga del layout. 
@await RenderSectionAsync("Scripts", required: false)
    
- Tag Helpers:
Nos permiten añadir ciertas funcionalidades:
  - asp-append-version: permite añadir un string a la url de un css o js para evitar problemas de caché. Ej:
  ```
  <link rel="stylesheet" href="~/PortaFolio.styles.css" asp-append-version="true" />
  ```
  - asp-controller
  - asp-action
  - asp-validation-summary
  - asp-for: Para enlacar con un campo del modelo.
    
- Validaciones personalizas: Pueden ser:
  - Por atributo: 
    - Se debe crear una clase que hereda de ValidationAttribute.
    - Se sobreescribe el método IsValid.
    - En la clase a validar se añade el atributo. Ej:
    [PrimeraLetraMayuscula]
  - Por módelo:
    - Se tiene que implementar la interface IValidatableObject.
    - Se implementa el método Validate con la validación deseada.

#### 12. JENKINS
- Servidor OpenSource con integración continua y automatización para Java, Node.js y .net, entre otros.
- Pasos para configurarlo:
  - [Descarga](https://jenkins.io/download/)
  - Desbloqueamos jenkins. Es para asegurarnos que somos nosotros los que vamos a administrar.
    ![imagen](https://user-images.githubusercontent.com/37666654/149171426-f92607de-5418-45ad-be75-5b99830c125a.png)

    Solo hay que irse a la ruta y coger la clave de esa ruta.
    
    La url de jenkins es: http://localhost:8080 salvo que se cambie en el fichero jenkins.xml.
    
  - A continuación, se selecciona los plugins a descargar. Selecionamos los sugeridos.
  - Después empezamos la configuración de Jenkins y su integración con github:
    1) Cambios el fichero jenkins.xml para cambiar la ruta donde se guardan las compilaciones. Se puede ver en Administrar Jenkins / Configurar Sistema
    2) Se reinicia con el comando: jenkins restart
    3) Se crea una nueva tarea de tipo Folder (yo he creado ProyectosGitHUb).
    4) Se crea un item y se configura el git.
    5) Se tiene que crear un access token en github y se añade la url en el formato:
    https://access_token@github.com/Asurbanipal1977/FundamentosCSharp.git
    6) Se indica que es un repositorio de github y se marca Consultar repositorio (SCM) cada el tiempo que queramos (por ejemplo, cada 15 min):
    H/15 * * * *
    7) se debe cambiar el */master por */main en el apartado: Branch Specifier (blank for 'any')
    8) Se pulsa a "Construir ahora", procediéndose a la descarga del proyecto.
  - Para SVN, tenemos que instalar SVNServer y un cliente como TortoiseSVN
  - Compilar un proyecto ASP.net en Jenkins con MSBuild.
   - Vamos a la url: [https://www.visualsvn.com/visualsvn/download/](https://www.visualsvn.com/visualsvn/download/) para descargar el MSBuild que corresponda.
   - En plugins debemos instalar, si no lo tenemos ya, el plugin MsBuild
   - Se configura en Global Tool Configuration el MsBuild indicando la ruta de ese archivo. En el caso de visual studio 2022 la ruta es:
      D:\Programas\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin
   - Se configura el msbuild. En esa configuración se pueden poner todas las tareas que sean necesarias.                                                        
Por otro lado, se puede indicar la solución o los proyectos. En el ejemplo, he puesto dos tareas:                                                          
![imagen](https://user-images.githubusercontent.com/37666654/149401171-2353a09b-be2b-4ffa-9fd2-63acd215b14e.png)
```
/t:Rebuild
/p:Configuration=Debug
/p:RestorePackagesConfig=true    
```
                                                           
De esta manera, a parte de descargar desde github, se automatiza la compilación.
    - Si faltan los paquetes nuget, se puede incluir la tarea /t:restore
                                                           
   Para mas datos: https://docs.microsoft.com/es-es/nuget/consume-packages/package-restore#restore-using-msbuild
                                                           
Se puede personalizar el log:
- Se instala el Plugin "Collapsing Console Sections"
- En configuración configuramos la sección: "Collapsing Console Sections" mediante expresiones regulares. Las expersiones regulares pueden comprobarse en:
https://regexr.com/
- Una vez guardado, nos saldrá el fichero de log agrupado por las secciones que indicamos.
                                                           
**Sonarqube**:
- Es una plataforma web que se utiliza para analizar y cuantificar la calidad del código fuente
- Se instala en esta dirección: [https://www.sonarqube.org/downloads/](https://www.sonarqube.org/downloads/)
- Vamos al archivo sonar.properties y: Descomentamos las líneas de username y password, y se descomenta sonar.jdbc.url, con la url de la base de datos que queramos usar. En nuestro caso tenemos instalado sql server 2019 Express, edición Developer y Express. Se puede ver las características de la instalación en Windows / Centro de instalación de Sql Server". Además podemos seleccionar el puerto y el contexto:
sonar.web.port=9001
sonar.web.context=/sonarqube
     
- Se crea la base de datos de sonar: Se crea con el collation:Latin1_General_CS_AS(para acentos y case sensitive). Además, se crea un usario que por defecto abra la base de datos sonar y, en Ussser Mapping, se pone que sea propietario de la base de datos (db_owner). Debemos seguir las instrucciones de la página:
[https://docs.sonarqube.org/latest/setup/install-server/](https://docs.sonarqube.org/latest/setup/install-server/)

Entre otras cosas, se nos informa que debemos lanzar:
ALTER DATABASE YourSonarQubeDatabase SET READ_COMMITTED_SNAPSHOT ON WITH ROLLBACK IMMEDIATE;
 
- Para probar que funciona, vamos a D:\Programas\sonarqube-8.9.6.50800\bin\windows-x86-64 y se ejecuta StartSonar.bat.
- Para modificar las propiedades a travcés del Sql Configuration Manager, se tiene que ejecutar el SQLServerManager15.msc.
- Una vez dentro, hay que habilitar el protócolo TCP/IP y Canalizaciones con nombre.
     ![imagen](https://user-images.githubusercontent.com/37666654/149575854-24ad3677-b74e-4c62-ad94-8f4579fe638f.png)
- Se comprueba que las tablas se han creado en la base de datos de sonar.
- Se ejecuta el: http://localhost:9002/sonarqube, que es dónde hemos instalado el sonar. El usuario es admin y la clave, la que pongo siempre.
- Se da de alta como servicio:
sc.exe create SonarQube binPath= "D:\Programas\sonarqube-8.9.6.50800\bin\windows-x86-64\wrapper.exe -s D:\Programas\sonarqube-8.9.6.50800\conf\wrapper.conf"
     
- Integración con Jenkins:
  - Creamos un nuevo usuario en: http://localhost:9002/sonarqube/admin/users
  - Generamos un token
  - Instalamos en Jenkins el plugin: SonarQube Scanner 
  - En "Configuración" vamos a: SonarQube servers
  - En "Global Tool Configuration": http://localhost:8080/configureTools/ indicamos los datos para SonarQube Scanner
  - Se administra la parte de de Configuración "SonarQube servers".
  - Para ejecutarlo, en la parte de configuración del item, en Execute SonarQube Scanner ponemos:
```
#*****************************************************
# Project Identification
#*****************************************************
sonar.projectKey=MinimalAPI
sonar.projectName=MinimalAPI
sonar.projectVersion=1
sonar.dotnet.visualstudio.solution=MinimalApi/MinimalAPI.csproj
sonar.projectDescription=Proyecto de Minimal API
sonar.scm.disabled=true
#*****************************************************
# C# Specific Properties
#*****************************************************
sonar.sources=MinimalApi/.
sonar.language=cs
sonar.sourceEncoding=UTF-8
#*****************************************************
````
     
Sin embargo, para C# para versiones de .Net superiores a la 5, habría que utilizar este código utilizando una instalación de [.Net5+](https://github.com/SonarSource/sonar-scanner-msbuild/releases/download/5.4.1.41282/sonar-scanner-msbuild-5.4.1.41282-net5.0.zip) y podriamos tener una tarea del tipo "Execute windows batch command":
````
dotnet build --no-incremental Models/Models.csproj -c Debug
dotnet restore
dotnet "D:\Programas\SonarScanner\SonarScanner.MSBuild.dll" begin /k:"MinimalAPI"  /d:sonar.login="61f4c10a37b9bc544c857255d621eecfedf616b8" /d:sonar.host.url="http://localhost:9002/sonarqube"
dotnet build --no-incremental MinimalApi/MinimalAPI.csproj -c Debug
dotnet "D:\Programas\SonarScanner\SonarScanner.MSBuild.dll" end /d:sonar.login="61f4c10a37b9bc544c857255d621eecfedf616b8"
````
    
El resultado se puede ver en esta ruta: http://localhost:9002/sonarqube/dashboard?id=MinimalAPI

**Pruebas Unitarias**
- Creamos un proyecto de pruebas en Visual Studio para la minimalAPI. Para ello creamos un poryecto de xUnit que use:
```
using Xunit;
using System.Net.Http.Json;
```
- Para las pruebas, en la MinimalAPI hay que poner:
public partial class Program { }
    
- En Jenkins hay que crear una tarea que ejecute:
```
 dotnet test MinimalApi.Test
```

- Dos páginas que explican estas pruebas:
  - https://stackoverflow.com/questions/69586239/integration-test-for-asp-net-core-6-web-api-throws-system-invalidoperationexcept
  - https://www.hanselman.com/blog/minimal-apis-in-net-6-but-where-are-the-unit-tests

  **Publicar**
 
- Se crea una tarea de este tipo: dotnet publish MinimalApi/MinimalAPI.csproj --configuration Debug --output "C:\inetpub\wwwroot\MinimalAPI"
- Tenemos que instalar un paquete para que se puedan ejecutar aplicaciones de .net core en IIS:
[https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-2.2.5-windows-hosting-bundle-installer](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-2.2.5-windows-hosting-bundle-installer)
- Crear el sitio web en el puerto que queramos.
- Dar permisos totales al usuario IIS_IUSRS.
    
### 13. BASES DE DATOS
1. Si se quiere cambiar de modo de acceso, se tiene que lanzar:
```
ALTER LOGIN sa ENABLE ;  
GO  
ALTER LOGIN sa WITH PASSWORD = 'sa' ;  
GO  
```
    
Y, a continuación, reiniciar el servicio de Sql Server.
    
2. Para ver un diagrama de DD, hay que loguearse con el usaurio sa.
    
#### 14. DAPPER
- Nos permite mapear en clases las tablas.
- Se tiene que instalar: Dapper y Microsoft.Data.SqlClient
- Para hacer la inserción en Dapper:
```    
    var id = connection.QuerySingle<int>($@"INSERT INTO TiposCuentas (Nombre, UsuarioId, Orden) 
                                                    VALUES (@Nombre, @UsuarioId, 0);
                                                    SELECT SCOPE_IDENTITY();",tipoCuenta);
```

### 15. VALIDACIÓN POR JAVASCRIPT USANDO REMOTE.
- En el servicio creamos la validación que deseemos. Ej:
```
    [HttpGet]
        public async Task<IActionResult> ExisteTipoCuenta(string nombre)
        {
            var yaExiste = await _repositoriosTiposCuentas.Existe(new TipoCuenta { UsuarioId = 1, Nombre = nombre });

            return (yaExiste ? Json($"El nombre ya existe") : Json(true));
        }
 ```
- En la clase, añadimos la etiqueta Remote, indicando el controlador y la acción a ejecutar:
[Remote(action: "ExisteTipoCuenta",controller:"TiposCuentas")]
    
### 16. USO DE LA LIBRERIA JQUERYUI
Esta librería, permite, entre otras cosas la ordenación manual de una tabla. Para ello:
1. Se instala la librería. Que se puede desacargar de: https://jqueryui.com/
2. Se copia esta librería en la carpeta libs del proyecto .Net Core.
3. En el tbody de la tabla se añade un id, que desde el ready se referencia para llamar a .sortable().
4. Se añade el estilo cursor:move a la fila de la tabla (tr).
