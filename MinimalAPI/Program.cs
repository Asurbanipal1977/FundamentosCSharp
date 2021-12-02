

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/hola", (string name) => $"Hola {name}");
app.MapGet("/holaNew/{name}/{surname}", (string name, string surname) => $"Hola {name} {surname}");

app.MapGet("/peticion", async () =>
{
    string url = "https://jsonplaceholder.typicode.com/posts";
    HttpClient client = new HttpClient();
    var httpResponseMessage = await client.GetAsync(url);
    httpResponseMessage.EnsureSuccessStatusCode();
    var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
    return respuesta;
});

app.Run();
