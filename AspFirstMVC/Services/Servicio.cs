using AspFirstMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspFirstMVC.Services
{
	public class Servicio : IServicio
	{
		private JsonSerializerOptions _options=
			new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};

		private readonly HttpClient _httpClient;

		public Servicio(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<List<Post>> ListarPost()
		{
			List<Post> lista = new List<Post>();
			string url = "https://jsonplaceholder.typicode.com/posts";

			var response = await _httpClient.GetAsync(url);
			//lanza una excepcion si el estado no es 200
			response.EnsureSuccessStatusCode();
			var json = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<List<Post>>(json, _options);
		}
	}
}
