using Microsoft.AspNetCore.Mvc;
using OrderApi.Repository;

namespace OrderApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class OrderController : Controller
{
    private readonly IRepository _repository;
    public OrderController(IRepository repository)
    {
        _repository = repository;
    }
}
