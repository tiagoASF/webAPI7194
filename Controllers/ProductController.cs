using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Shop.Models;
using Shop.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Shop.Controllers
{
    //https://localhost:5001/products
    [Route("products")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
        {
            var products = await context
                .Products
                .Include(x => x.Category)
                .AsNoTracking()
                .ToListAsync();
            return Ok(products);    
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> GetById(
            int id,
            [FromServices]DataContext context
        )
        {
            var product = await context
                .Products
                .Include(x => x.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            
            return Ok(product);
        }

        //Método GET que pega todos os produtos de uma determinada categoria
        //Https://products/categories/1
        [HttpGet]
        [Route("categories/{id:int}")]

        public async Task<ActionResult<List<Product>>> GetByCategory(
            [FromServices] DataContext context,
            int id 
        )
        {
            var products = await context
                .Products
                .Include(x => x.Category)
                .AsNoTracking()
                .Where(x => x.Category.Id == id)
                .ToListAsync();
            
            return Ok(products);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<List<Product>>> Post(
            [FromBody]Product model,
            [FromServices]DataContext context
        )
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                context.Products.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest(new {message = "Não foi possível criar o produto"});
            }
            
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<Product>>> Put(
            int id,
            [FromBody]Product model,
            [FromServices]DataContext context
        )
        {
            //Verifica se o ID é válido
            if (model.Id != id)
            {
                return NotFound(new { message = "Produto não localizado"});
            }

            //Verifica se os dados são validos
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            } 
            
            try
            {
                context.Entry<Product>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Esse registro já foi atualizado"});
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível atualizar o produto"});
            }
        }


        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<Product>>> Delete(
            int id,
            [FromServices]DataContext context
        )
        //Para efetuar o DELETE, primeiro é necessário recuperar a categoria do banco
        {
            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound(new { message = "Produto não encontrado"});
            }

            try
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return Ok(new { message = "Produto removido com sucesso"});
                // ou
                //return Ok(category);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não é possível remover o produto"});
            
            }
        }


    }
}