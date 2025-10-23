using GatiToolkit.Core.WeightedRandom;
using Microsoft.AspNetCore.Mvc;
using TSID.Creator.NET;

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
        public IActionResult GetTable([FromQuery] string id)
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
            var newTable = new WeightedRandomTable();
            _weightedRandomTables.Add(newTable);
            return Ok(newTable);
        }

        [HttpDelete("table")]
        public IActionResult DeleteTable([FromQuery] string id)
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
        public IActionResult AddRows([FromQuery] string tableID, [FromBody] uint[] weights)
        {
            var table = _weightedRandomTables.SingleOrDefault(t => t.ID == tableID);
            if (table == null)
            {
                return NotFound();
            }

            return Ok(table.AddRows(weights));
        }

        [HttpPut("row")]
        public IActionResult AddRow([FromQuery] string tableID, [FromBody] uint weight)
        {
            var table = _weightedRandomTables.SingleOrDefault(t => t.ID == tableID);
            if (table == null)
            {
                return NotFound();
            }

            return Ok(table.AddRow(weight));
        }

        [HttpPatch("name")]
        public IActionResult ChangeName([FromQuery] string tableID, [FromBody] string name)
        {
            var table = _weightedRandomTables.SingleOrDefault(t => t.ID == tableID);
            if (table == null)
            {
                return NotFound();
            }
            table.ChangeName(name);
            return Ok();
        }

        [HttpPatch("description")]
        public IActionResult ChangeDescription([FromQuery] string tableID, [FromBody] string description)
        {
            var table = _weightedRandomTables.SingleOrDefault(t => t.ID == tableID);
            if (table == null)
            {
                return NotFound();
            }
            table.ChangeDescription(description);
            return Ok();
        }

        [HttpPatch("rows")]
        public IActionResult ChangeRows([FromQuery] string tableID, [FromBody] Dictionary<uint, uint> weights)
        {
            var table = _weightedRandomTables.SingleOrDefault(t => t.ID == tableID);
            if (table == null)
            {
                return NotFound();
            }
            table.ChangeWeights(weights);
            return Ok();
        }

        [HttpGet("pick")]
        public IActionResult Pick([FromQuery] string tableID, [FromQuery] int count)
        {
            var table = _weightedRandomTables.SingleOrDefault(t => t.ID == tableID);
            if (table == null)
            {
                return NotFound();
            }
            return Ok(table.Pick(count));
        }

        [HttpGet("rates")]
        public IActionResult GetRates([FromQuery] string tableID)
        {
            var table = _weightedRandomTables.SingleOrDefault(t => t.ID == tableID);
            if (table == null)
            {
                return NotFound();
            }
            return Ok(table.Rates);
        }
    }
}
