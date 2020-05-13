using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [ApiController]
    [Route("")]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext applicationDbContext)
        {
            this._context = applicationDbContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var objects = _context.CelestialObjects.ToList();

            foreach(var o in objects)
            {
                o.Satellites = new List<CelestialObject>();
                o.Satellites.AddRange(_context.CelestialObjects.Where(co => co.OrbitedObjectId == o.Id));
            }

            return Ok(objects);
        }

        [HttpGet("{id:int}",Name ="GetById")]
        public IActionResult GetById(int id)
        {
            var o = _context.CelestialObjects.FirstOrDefault(co => co.Id == id);
            
            if (o != null)
            {
                if (o.Satellites == null)
                {
                    o.Satellites = new List<CelestialObject>();
                    o.Satellites.AddRange(_context.CelestialObjects.Where(co => co.OrbitedObjectId == o.Id));
                }
                return Ok(o);
            }

            return NotFound();
        }

        [HttpGet("{name}",Name ="GetByName")]
        public IActionResult GetByName(string name)
        {
            var objects = _context.CelestialObjects.Where(co => co.Name == name).ToList();

            if (objects.Count == 0) return NotFound();

            foreach(var o in objects)
            {
                o.Satellites = new List<CelestialObject>();
                o.Satellites.AddRange(_context.CelestialObjects.Where(co => co.OrbitedObjectId == o.Id));
            }
            return Ok(objects);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]CelestialObject celestial)
        {
            var o = _context.CelestialObjects.FirstOrDefault(co => co.Id == id);
            
            if (o == null) return NotFound();

            o.Name = celestial.Name;
            o.OrbitalPeriod = celestial.OrbitalPeriod;
            o.OrbitedObjectId = celestial.OrbitedObjectId;
            _context.CelestialObjects.Update(o);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var o = _context.CelestialObjects.FirstOrDefault(co => co.Id == id);

            if (o == null) return NotFound();

            o.Name = name;
            _context.CelestialObjects.Update(o);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var o = _context.CelestialObjects.Where(co => co.Id == id || co.OrbitedObjectId==id).ToList();

            if (o.Count==0) return NotFound();

            _context.CelestialObjects.RemoveRange(o);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
