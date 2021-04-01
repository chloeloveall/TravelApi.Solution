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
  [Route("api/[controller]")]
  [ApiController]
  public class ReviewsController : ControllerBase
  {
    private TravelApiContext _db;
    public ReviewsController(TravelApiContext db)
    {
      _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Review>>> GetAction(string title, string text)
    {
      var query = _db.Reviews.AsQueryable();
      // if (rating != null)
      // {
      //   query = query.Where(entry => entry.Rating == rating);
      // }
      if (title != null)
      {
        query = query.Where(entry => entry.Title == title);
      }
      if (text != null)
      {
        query = query.Where(entry => entry.Text == text);
      }

      return await query.ToListAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Review>> GetReview (int id)
    {
      var review = await _db.Reviews.FindAsync(id);
      if(review == null)
      {
        return NotFound();
      }
      return review;
    }

    [HttpPost("{locationid}/createreview")]
    public async Task<ActionResult<Review>> Post(Review review, int locationid)
    {
      var thisLocation = _db.Locations.Include(entry => entry.Reviews).FirstOrDefault(entry => entry.LocationId == locationid);
      if (thisLocation != null)
      {
        review.LocationId = thisLocation.LocationId;
        thisLocation.Reviews.Add(review);
        _db.Locations.Update(thisLocation);
        await _db.SaveChangesAsync();
      }
      else
      {
        return BadRequest();
      }
      // _db.Reviews.Add(review);
      // await _db.SaveChangesAsync();    
      // return CreatedAtAction("Post", new { id = message.GroupId}, thisGroup);
      return CreatedAtAction(nameof(GetReview), new { id = review.LocationId }, thisLocation );
    }

    [HttpPut ("{id}")]
    public async Task<IActionResult> Put(int id, Review review)
    {
      if(id != review.ReviewId)
      {
        return BadRequest();
      }
      _db.Entry(review).State = EntityState.Modified;
      try
      {
        await _db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if(!ReviewExists(id))
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
    public async Task<IActionResult> DeleteReview(int id)
    {
      var review = await _db.Reviews.FindAsync(id);
      if (review == null)
      {
        return NotFound();
      }
      _db.Reviews.Remove(review);
      await _db.SaveChangesAsync();
      return NoContent();
    }

    private bool ReviewExists(int id)
    {
      return _db.Reviews.Any(e => e.ReviewId == id);
    }
  }
}