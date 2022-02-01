using Microsoft.EntityFrameworkCore;
using MinimalAPI;
using MinimalAPI.Models;
using System.Text.Json;

string MyCors = "MyCors";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<EFContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), builder =>
    {
        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
    });
});

builder.Services.AddTransient<Prueba>();
builder.Services.AddCors(opts => opts.AddPolicy(MyCors, builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(MyCors);
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello World!");
app.MapGet("/hola", (string name) => $"Hola {name}");
app.MapGet("/holaNew/{name}/{surname}", (string name, string surname) => $"Hola, tu nombre es {name} {surname}");

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
    async (EFContext context) => {
        try
        {
            return Results.Ok(await context.Cervezas.ToListAsync());
        }
        catch(Exception e)
        {
            app.Logger.LogError(e.Message);
            return Results.BadRequest(new
            {
                Message = $"{e.Message} - {e.InnerException?.Message}"
            });
        }
    }
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


// Make the implicit Program class public so test projects can access it

namespace MinimalAPI
{
    public partial class Program { }
}
