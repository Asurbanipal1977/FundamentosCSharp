using AspFirstMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspFirstMVC.Services
{
	public interface IServicio
	{
		Task<List<Post>> ListarPost();
	}
}
