using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Shop.Models;
using Shop.Data;


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
    public async Task<ActionResult<List<Category>>> Post(
        [FromBody]Category model,
        [FromServices]DataContext context
    )
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            context.Categories.Add(model);
            await context.SaveChangesAsync();
            return Ok(model);
        }
        catch (Exception)
        {
            return BadRequest(new {message = "Não foi possível criar a categoria"});
        }
        
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<List<Category>>> Put(
        int id,
        [FromBody]Category model,
        [FromServices]DataContext context
    )
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
        
        try
        {
            context.Entry<Category>(model).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(model);
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest(new { message = "Esse registro já foi atualizado"});
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Não foi possível atualizar a categoria"});
        }
    }

    [HttpDelete]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Delete()
    {
        return Ok();
    }
} 