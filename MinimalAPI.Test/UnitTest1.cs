using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Threading.Tasks;
using Models;
using System.Net.Http.Json;

namespace MinimalAPI.Test
{
    public class UnitTest1
    {
        [Fact]
        public async Task TestPost()
        {
            var app = new WebApplicationFactory<Program>();
            using var client = app.CreateClient();
            var posts = await client.GetFromJsonAsync<Post[]>("/peticion");
            Assert.NotEmpty(posts);
        }

        [Fact]
        public async Task TestPrueba()
        {
            var app = new WebApplicationFactory<Program>();
            using var client = app.CreateClient();
            var prueba = await client.GetStringAsync("/prueba");
            Assert.Equal("Es una Prueba falsa",prueba);
        }
    }
}