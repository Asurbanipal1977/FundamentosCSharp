using FundamentosCSharp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FundamentosCSharp.Service
{ 
    //Solo admite clases generic que implementen la interfaz ISendRequest
    public class SendRequest<T> where T : ISendRequest
    {
        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        private HttpClient http = new HttpClient();


        public async Task<List<T>> Listar()
        {
            string url = "https://jsonplaceholder.typicode.com/posts";
            List<T> posts = new List<T>();

            //Espera hasta que haya respuesta
            var res = await http.GetAsync(url);

            if (res.IsSuccessStatusCode)
            {
                var respuesta = await res.Content.ReadAsStringAsync();
                posts = JsonSerializer.Deserialize<List<T>>(respuesta, options);
            }

            return posts;
        }

        public async Task<T> AddTypicode(T model)
        {
            string retorno = string.Empty;
            T postRetorno = default(T);
            string url = "https://jsonplaceholder.typicode.com/posts";
            
            string postSerializado = JsonSerializer.Serialize(model);
            HttpContent content = new StringContent(postSerializado, System.Text.Encoding.UTF8, "application/json");
            var response = await http.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                retorno = await response.Content.ReadAsStringAsync();
                postRetorno = JsonSerializer.Deserialize<T>(retorno, options);
            }
            return postRetorno;
        }

        public async Task<T> ModifyTypiCode(int id, T model)
        {
            string retorno = string.Empty;
            T putRetorno = default(T);
            string url = $"https://jsonplaceholder.typicode.com/posts/{id}";

            string putSerializado = JsonSerializer.Serialize(model);
            HttpContent content = new StringContent(putSerializado, System.Text.Encoding.UTF8, "application/json");
            var response = await http.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                retorno = await response.Content.ReadAsStringAsync();
                putRetorno = JsonSerializer.Deserialize<T>(retorno, options);
            }
            return putRetorno;
        }

        public async Task<bool> DeleteTypiCode(int id)
        {
            string retorno = string.Empty;
            bool putRetorno = false;
            string url = $"https://jsonplaceholder.typicode.com/posts/{id}";
            var response = await http.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                retorno = await response.Content.ReadAsStringAsync();
                putRetorno = true;
            }
            return putRetorno;
        }
    }
}
