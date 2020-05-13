using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
    }
}
