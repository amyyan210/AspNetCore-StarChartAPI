using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var result = _context.CelestialObjects.Find(id);

            if (result == null)
            {
                return NotFound();
            }


            result.Satellites = _context.CelestialObjects.Where(oo => oo.OrbitedObjectId == id).ToList();

            return Ok(result);

        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObj = _context.CelestialObjects.Find(name);

            if (celestialObj == null)
            {
                return NotFound();
            }

            celestialObj.Satellites = _context.CelestialObjects.Where(oo => oo.OrbitedObjectId == celestialObj.Id).ToList();

            return Ok(celestialObj);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjs = _context.CelestialObjects.ToList();

            foreach(var celestialObj in celestialObjs)
            {
                celestialObj.Satellites = celestialObjs.Where(oo => oo.OrbitedObjectId == celestialObj.Id).ToList();
            }

            return Ok(celestialObjs);
        }

    }
}
