using Microsoft.EntityFrameworkCore;
using MinimalAPI;
using MinimalAPI.Models;
using System.Text.Json;

string MyCors = "MyCors";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<EFContext>();
builder.Services.AddTransient<Prueba>();
builder.Services.AddCors(opts => opts.AddPolicy(MyCors, builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(MyCors);
app.UseSwagger();
app.UseSwaggerUI();

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

    return JsonSerializer.Deserialize<Models.Post[]>(respuesta, new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
    });
});

app.MapGet("/cervezas", 
    async (EFContext context) => await context.Cervezas.ToListAsync()
);
app.MapGet("/cerveza/{id}",
    async (int id, EFContext context) =>
    {
        var cerveza = await context.Cervezas.FindAsync(id);
        return (cerveza != null ? Results.Ok(cerveza) : Results.NotFound());
    }
);

app.MapGet("/prueba", (Prueba prueba) => prueba.Hola());

app.MapPost("/post", (Data data) => $"{data.Id} {data.Name}");

app.Run();
