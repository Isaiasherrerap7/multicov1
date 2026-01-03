using Delab.AccessData.Data;
using Delab.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Delab.Backend.Controllers;

[Route("api/sities")]
[ApiController]
public class CitiesController : ControllerBase
{
    private readonly DataContext _context;

    public CitiesController(DataContext context)
    {
        _context = context;
    }

    // 3. Creamos el metodo para obtener la lista
    [HttpGet]
    public async Task<ActionResult<IEnumerable<City>>> GetListAsync()
    {
        try
        {
            // 3.1 Obtenemos los datos de la base de datos
            var listItem = await _context.Cities
                .OrderBy(x => x.Name)
                .ToListAsync();
            // 3.2 Retornamos los datos
            return Ok(listItem);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // 4. Creamos el metodo para obtener un dato por id
    [HttpGet("{id}")]
    public async Task<ActionResult<City>> GetAsync(int id)
    {
        // 4.1 Obtenemos el dato de la base de datos dentro de un bloque try-catch
        try
        {
            var IdModel = await _context.Cities.FindAsync(id);
            return Ok(IdModel);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // 5. Creamos el metodo post para crear un nuevo dato
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] City modelo)
    {
        try
        {
            _context.Cities.Add(modelo);

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

    // 6. Creamos el metodo put para actualizar un dato
    [HttpPut]
    public async Task<ActionResult<City>> PutAsync(City modelo)
    {
        // 6.1 Buscamos el dato en la base de datos por su ID dentro de un bloque try-catch
        try
        {
            var UpdateModel = await _context.Cities.FirstOrDefaultAsync(x => x.CityId == modelo.CityId);
            // 6.2 Asignamos los nuevos valores desde el modelo recibido
            UpdateModel!.Name = modelo.Name;
            UpdateModel.StateId = modelo.StateId;

            // 6.3 Actualizamos dato en la base de datos
            _context.Cities.Update(UpdateModel);

            // 6.4 Guardamos los cambios en la base de datos
            await _context.SaveChangesAsync();

            // 6.5 Retornamos el dato actualizado
            return Ok(UpdateModel);
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

    // 7. Creamos el metodo delete para eliminar un dato
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            // 7.1 Buscamos el dato en la base de datos por su ID
            var DataRemove = await _context.Cities.FindAsync(id);
            // 7.2 Si el dato no existe, retornamos un NotFound
            if (DataRemove == null)
            {
                return BadRequest("No se encontró el registro para borrar");
            }

            // 7.3 Si el dato existe, lo eliminamos de la base de datos
            _context.Cities.Remove(DataRemove);
            // 7.4 Guardamos los cambios en la base de datos
            await _context.SaveChangesAsync();
            // 7.5 Retornamos un Ok
            return Ok("Registro eliminado");
        }
        catch (DbUpdateException dbEx)
        {
            if (dbEx.InnerException!.Message.Contains("Referencia"))
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