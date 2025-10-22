using GatiToolkit.Core.WeightedRandom;
using Microsoft.AspNetCore.Mvc;

namespace GatiToolkit.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeightedRandomController : ControllerBase
    {
        private readonly ILogger<WeightedRandomController> _logger;

        private static readonly List<WeightedRandomTable> _weightedRandomTables = [];

        public WeightedRandomController(ILogger<WeightedRandomController> logger)
        {
            _logger = logger;
        }

        [HttpGet("tables")]
        public IActionResult GetTables()
        {
            return Ok(_weightedRandomTables);
        }

        [HttpGet("table")]
        public IActionResult GetTable([FromQuery] Guid id)
        {
            var table = _weightedRandomTables.SingleOrDefault(t => t.ID == id);
            if (table == null)
            {
                return NotFound();
            }
            return Ok(table);
        }

        [HttpPut("table")]
        public IActionResult CreateTable()
        {
            var newTable = new WeightedRandomTable(Guid.NewGuid());
            _weightedRandomTables.Add(newTable);
            return Ok(newTable);
        }

        [HttpDelete("table")]
        public IActionResult DeleteTable([FromQuery] Guid id)
        {
            var table = _weightedRandomTables.SingleOrDefault(t => t.ID == id);
            if (table == null)
            {
                return NotFound();
            }
            _weightedRandomTables.Remove(table);
            return Ok();
        }

        [HttpPut("rows")]
        public IActionResult AddRows([FromQuery] Guid tableID, [FromBody] uint[] weights)
        {
            var table = _weightedRandomTables.SingleOrDefault(t => t.ID == tableID);
            if (table == null)
            {
                return NotFound();
            }

            return Ok(table.AddRows(weights));
        }

        [HttpPut("row")]
        public IActionResult AddRow([FromQuery] Guid tableID, [FromBody] uint weight)
        {
            var table = _weightedRandomTables.SingleOrDefault(t => t.ID == tableID);
            if (table == null)
            {
                return NotFound();
            }

            return Ok(table.AddRow(weight));
        }

        [HttpPatch("name")]
        public IActionResult ChangeName([FromQuery] Guid tableID, [FromBody] string name)
        {
            var table = _weightedRandomTables.SingleOrDefault(t => t.ID == tableID);
            if (table == null)
            {
                return NotFound();
            }
            table.Name = name;
            return Ok();
        }

        [HttpPatch("description")]
        public IActionResult ChangeDescription([FromQuery] Guid tableID, [FromBody] string description)
        {
            var table = _weightedRandomTables.SingleOrDefault(t => t.ID == tableID);
            if (table == null)
            {
                return NotFound();
            }
            table.Description = description;
            return Ok();
        }

        [HttpGet("pick")]
        public IActionResult Pick([FromQuery] Guid tableID, [FromQuery] int count)
        {
            var table = _weightedRandomTables.SingleOrDefault(t => t.ID == tableID);
            if (table == null)
            {
                return NotFound();
            }
            return Ok(table.Pick(count));
        }
    }
}
