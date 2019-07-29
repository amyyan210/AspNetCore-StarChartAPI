using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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
            var celestialObjs = _context.CelestialObjects.Where(e => e.Name == name).ToList();

            if (!celestialObjs.Any())
            {
                return NotFound();
            }

            foreach (var celestialObj in celestialObjs)
            {
                celestialObj.Satellites = _context.CelestialObjects.Where(oo => oo.OrbitedObjectId == celestialObj.Id).ToList();
            }

            return Ok(celestialObjs);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjs = _context.CelestialObjects.ToList();

            foreach (var celestialObj in celestialObjs)
            {
                celestialObj.Satellites = celestialObjs.Where(oo => oo.OrbitedObjectId == celestialObj.Id).ToList();
            }

            return Ok(celestialObjs);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObj)
        {
            _context.CelestialObjects.Add(celestialObj);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = celestialObj.Id }, celestialObj);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, CelestialObject celestialObj)
        {
            var existingObject = _context.CelestialObjects.Find(celestialObj);

            if (existingObject == null)
            {
                return NotFound();
            }

            existingObject.Name = celestialObj.Name;
            existingObject.OrbitedObjectId = celestialObj.OrbitedObjectId;
            existingObject.OrbitalPeriod = celestialObj.OrbitalPeriod;

            _context.CelestialObjects.Update(existingObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id:int}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var result = _context.CelestialObjects.Find(id);
            if (result == null)
            {
                return NotFound();
            }

            result.Name = name;
            _context.CelestialObjects.Update(result);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var celestialObjects = _context.CelestialObjects.Where(co => (co.Id == id || co.OrbitedObjectId == id)).ToList();

            if (!celestialObjects.Any())
            {
                return NotFound();
            }

            _context.CelestialObjects.RemoveRange(celestialObjects);
            _context.SaveChanges();

            return NoContent();
        }

    }
}
