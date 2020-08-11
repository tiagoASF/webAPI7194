using Microsoft.AspNetCore.Mvc;
using Shop.Models;

//https://localhost:5001/categories
[Route("categories")]
public class CategoryController : ControllerBase
{
    [HttpGet]
    [Route("")]
    public string Get()
    {
        return "GET";
    }

    [HttpGet]
    [Route("{id:int}")]
    public string GetById(int id)
    {
        return $"GET item #{id}";
    }

    [HttpPost]
    [Route("")]
    public Category Post([FromBody]Category model)
    {
        return model;
    }

    [HttpPut]
    [Route("{id:int}")]
    public Category Put(int id, [FromBody]Category model)
    {
        if (model.Id == id)
        {
            return model;
        } 
        return null;
    }

    [HttpDelete]
    [Route("")]
    public string Delete()
    {
        return "DELETE";
    }
} 