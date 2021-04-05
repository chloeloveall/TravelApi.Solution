using System.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelApi.Models;

namespace TravelApi.Controllers
{
  [ApiVersion("2.0")]
  // [Route("api/[controller]")]
  [Route("api/{v:apiVersion}/locations")] 
  [ApiController]
  public class LocationsV2Controller : ControllerBase
  {
    private readonly TravelApiContext _db;
    public LocationsV2Controller(TravelApiContext db)
    {
      _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Location>>> Get(string country, string city)
    {
      var query = _db.Locations.Include(entry => entry.Reviews).AsQueryable();
      if (country != null)
      {
        query = query.Where(entry => entry.Country == country);
      }
      if (city != null)
      {
        query = query.Where(entry => entry.City == city);
      }
      // if (rating != null)
      // {
      //   query = query.Where(entry => entry.Rating == rating);
      // }
      return await query.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Location>> GetLocation(int id)
    {
      // var thisLocation = _db.Locations.FirstOrDefault(entry => entry.City == city).LocationId;
      var location = await _db.Locations.FindAsync(id);
      if (location == null)
      {
        return NotFound();
      }
      return location;
    }

    [HttpPost]
    public async Task<ActionResult<Location>> Post(Location location)
    {
      _db.Locations.Add(location);
      await _db.SaveChangesAsync();
      return CreatedAtAction(nameof(GetLocation), new { id = location.LocationId }, location);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Location location)
    {
      if (id != location.LocationId)
      {
        return BadRequest();
      }
      _db.Entry(location).State = EntityState.Modified;
      try
      {
        await _db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if(!LocationExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }
      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLocation(int id)
    {
      var location = await _db.Locations.FindAsync(id);
      if (location == null)
      {
        return NotFound();
      }
      _db.Locations.Remove(location);
      await _db.SaveChangesAsync();
      return NoContent();
    }

    private bool LocationExists(int id)
    {
      return _db.Locations.Any(e => e.LocationId == id);
    }

  }
}