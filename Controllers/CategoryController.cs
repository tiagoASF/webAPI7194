using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Shop.Models;


//https://localhost:5001/categories
[Route("categories")]
public class CategoryController : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Get()
    {
        return new List<Category>();
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> GetById(int id)
    {
        return new Category();
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Post([FromBody]Category model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok(model);
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<List<Category>>> Put(int id, [FromBody]Category model)
    {
        //Verifica se o ID é válido
        if (model.Id != id)
        {
            return NotFound(new { message = "Categoria não localizada"});
        }

        //Verifica se os dados são validos
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); 
        } 
        
        return Ok();
    }

    [HttpDelete]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Delete()
    {
        return Ok();
    }
} 