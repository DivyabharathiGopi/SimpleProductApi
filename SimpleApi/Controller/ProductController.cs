using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleApi.Models.Domain;
using System.Linq;
using System.Collections.Generic;


[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private static List<Product>  products=new List<Product>
    {
        new Product
        {
            Id=Guid.NewGuid(),
            Name="Laptop",
            Description="Electronic"
        },

        new Product
        {
            Id=Guid.NewGuid(),
            Name="TV",
            Description="Electronic"
        }
    };

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Product>> Get()
    {
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Product> Get([FromRoute] Guid id)
    {
        var ExsitingProduct = products.FirstOrDefault(x=>x.Id==id);
        if(ExsitingProduct==null)
        {
            return NotFound("Product Not Found!");
        }
        return Ok(ExsitingProduct);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Product> Post([FromBody] Product product)
    {
        if(product==null)
        {
            return BadRequest("Invalid or missing Data");
        }
        product.Id=Guid.NewGuid();
        products.Add(product);
        return CreatedAtAction(nameof(Get), new {id=product.Id},product);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    
    public ActionResult Update([FromRoute] Guid id,[FromBody] Product product)
    {
        if(id!=product.Id)
        {
            return BadRequest();
        }

        var ExsitingProduct = products.FirstOrDefault(x=>x.Id==id);
        if(ExsitingProduct==null)
        {
            return NotFound();
        }

        ExsitingProduct.Name=product.Name;
        ExsitingProduct.Description=product.Description;
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult Delete([FromRoute] Guid id)
    {
        var ExsitingProduct=products.FirstOrDefault(x=>x.Id==id);
        if(ExsitingProduct==null)
        {
            return NotFound();
        }
        products.Remove(ExsitingProduct);
        return NoContent();
    }
}