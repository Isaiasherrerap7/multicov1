using Delab.AccessData.Data;
using Delab.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Delab.Backend.Controllers;

[Route("api/countries")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
[ApiController]
public class CountriesController : ControllerBase
{
    // 2. Inyectamos el contexto de datos medianto campo de context
    private readonly DataContext _context;

    // 1.Constructor para inyectar el contexto de datos
    public CountriesController(DataContext context)
    {
        _context = context;
    }

    // 3. Creamos el metodo GetCountries para obtener todos los paises
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
    {
        try
        {
            // 3.1 Obtenemos los paises de la base de datos
            var listcountry = await _context.Countries.Include(x => x.States)!.ThenInclude(x => x.Cities)
                .OrderBy(x => x.Name)
                .ToListAsync();
            // 3.2 Retornamos los paises
            return Ok(listcountry);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // 4. Creamos el metodo GetCountry para obtener un pais por id
    [HttpGet("{id}")]
    public async Task<ActionResult<Country>> GetCountry(int id)
    {
        // 4.1 Obtenemos el pais de la base de datos dentro de un bloque try-catch
        try
        {
            var Idcountry = await _context.Countries.FindAsync(id);
            return Ok(Idcountry);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // 5. Creamos el metodo post para crear un nuevo pais
    [HttpPost]
    public async Task<IActionResult> PostCountry([FromBody] Country modelo)
    {
        try
        {
            _context.Countries.Add(modelo);

            await _context.SaveChangesAsync();

            return Ok();
        }
        catch (DbUpdateException dbEx)
        {
            if (dbEx.InnerException!.Message.Contains("duplicada"))
            {
                return BadRequest("Ya existe un registro con el mismo nombre");
            }
            else
            {
                return BadRequest(dbEx.InnerException.Message);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // 6. Creamos el metodo put para actualizar un pais
    [HttpPut]
    public async Task<ActionResult<Country>> PutCountry(Country modelo)
    {
        // 6.1 Buscamos el país en la base de datos por su ID dentro de un bloque try-catch
        try
        {
            var UpdateCountry = await _context.Countries.FirstOrDefaultAsync(x => x.CountryId == modelo.CountryId);
            if (UpdateCountry == null)
            {
                return NotFound("El país no fue encontrado.");
            }

            // 6.2 Asignamos los nuevos valores desde el modelo recibido
            UpdateCountry.Name = modelo.Name;
            UpdateCountry.CodPhone = modelo.CodPhone;

            // 6.3 Actualizamos el país en la base de datos
            _context.Countries.Update(UpdateCountry);

            // 6.4 Guardamos los cambios en la base de datos
            await _context.SaveChangesAsync();

            // 6.5 Retornamos el país actualizado
            return Ok(UpdateCountry);
        }
        catch (DbUpdateException dbEx)
        {
            if (dbEx.InnerException!.Message.Contains("duplicada"))
            {
                return BadRequest("Ya existe un registro con el mismo nombre");
            }
            else
            {
                return BadRequest(dbEx.InnerException.Message);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // 7. Creamos el metodo delete para eliminar un pais
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        try
        {
            // 7.1 Buscamos el país en la base de datos por su ID
            var DeleteCountry = await _context.Countries.FindAsync(id);
            // 7.2 Si el país no existe, retornamos un NotFound
            if (DeleteCountry == null)
            {
                return BadRequest("No se encontró el registro para borrar");
            }

            // 7.3 Si el país existe, lo eliminamos de la base de datos
            _context.Countries.Remove(DeleteCountry);
            // 7.4 Guardamos los cambios en la base de datos
            await _context.SaveChangesAsync();
            // 7.5 Retornamos un Ok
            return Ok("Registro eliminado");
        }
        catch (DbUpdateException dbEx)
        {
            if (dbEx.InnerException!.Message.Contains("REFERENCE"))
            {
                return BadRequest("No puede eliminar el registro por que tiene data relacionada");
            }
            else
            {
                return BadRequest(dbEx.InnerException.Message);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}