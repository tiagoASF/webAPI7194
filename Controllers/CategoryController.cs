using Microsoft.AspNetCore.Mvc;

//https://localhost:5001/categories
[Route("categories")]
public class CategoryController : ControllerBase
{
    //https://localhost:5001/categories/olamundo
    [Route("olamundo")]
    public string MeuMetodo()
    {
        return "Olá mundo MVC";
    }
}