using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Shop.Models;
using Shop.Data;
using Microsoft.AspNetCore.Authorization;


//https://localhost:5001/categories
[Route("categories")]
public class CategoryController : ControllerBase
{
    [HttpGet]
    [Route("")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Category>>> Get(
        [FromServices]DataContext context
    )
    {
        //Todas as ações da queries devem ser feitas antes do ToListAsync(), ele 
        //encerrará a query. O que for colocado após irá para a memória, podendo
        //gerar um efeito indesejado de leak
        var categories = await context.Categories.AsNoTracking().ToListAsync();
        return Ok(categories);
    }

    [HttpGet]
    [Route("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<Category>> GetById(
        int id,
        [FromServices]DataContext context
    )
    {
        var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return Ok(category);
    }

    [HttpPost]
    [Route("")]
    [Authorize(Roles = "employee")]
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
    [Authorize(Roles = "employee")]
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
    [Route("{id:int}")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<List<Category>>> Delete(
        int id,
        [FromServices]DataContext context
    )
    //Para efetuar o DELETE, primeiro é necessário recuperar a categoria do banco
    {
        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (category == null)
        {
            return NotFound(new { message = "Categoria não localizada"});
        }

        try
        {
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return Ok(new { message = "Categoria removida com sucesso"});
            // ou
            //return Ok(category);
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Não é possível remover a categoria"});
        }
    }
} 