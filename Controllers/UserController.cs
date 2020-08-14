using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Shop.Data;
using Shop.Models;
using Shop.Services;

namespace Shop.Controllers
{
    [Route("/users")]
    public class UserController : Controller
    {
        
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context)
        {
            var users = await context
                .Users
                .AsNoTracking()
                .ToListAsync();

            return users;    
        }

        
        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        //[Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Post(
            [FromServices]DataContext context,
            [FromBody]User model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                
                context.Users.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest(new {message = "Não foi possível criar o usuário"});
            }  
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate(
            [FromServices] DataContext context,
            [FromBody] User model)
        {
            var user = await context.Users
                .AsNoTracking()
                .Where(x => x.Username == model.Username && x.Password == model.Password)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { message = "Usuário ou senha inválidos" });
            }

            var token = TokenService.GenerateToken(user);
            return new
            {
                user = user,
                token = token
            };    

        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Put(
            int id,
            [FromBody]User model,
            [FromServices]DataContext context
        )
        {
            //Verifica se o ID é válido
            if (model.Id != id)
            {
                return NotFound(new { message = "Usuário não localizado"});
            }

            //Verifica se os dados são validos
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            } 
            
            try
            {
                context.Entry<User>(model).State = EntityState.Modified;
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

        //Implementar o DELETE

        


       



    }
}