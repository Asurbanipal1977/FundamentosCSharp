using AutoMapper;
using AutoMapperMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoMapperMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IMapper mapper;

        public ClienteController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        [HttpPost]
        public IActionResult Add(ClienteRequest clienteRequest)
        {
            Cliente cliente = mapper.Map<Cliente>(clienteRequest);
            return Ok(cliente);
        }
    }
}
