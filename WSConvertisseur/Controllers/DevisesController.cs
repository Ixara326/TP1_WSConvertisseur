using Microsoft.AspNetCore.Mvc;
using WSConvertisseur.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WSConvertisseur.Controllers
{
    /// <summary>
    /// Contrôleur REST permettant de gérer les devises (et leur taux de conversion à partir de l'euro).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DevisesController : ControllerBase
    {
        private List<Devise> devises;

        /// <summary>
        /// Constructeur : initialise la liste des devises (recréée à chaque appel car l'API est sans état).
        /// </summary>
        public DevisesController()
        {
            devises = new List<Devise>();
            devises.Add(new Devise(1, "Dollar", 1.08));
            devises.Add(new Devise(2, "Franc Suisse", 1.07));
            devises.Add(new Devise(3, "Yen", 120));
        }

        /// <summary>
        /// Get all currencies.
        /// </summary>
        /// <returns>Http response</returns>
        /// <response code="200">The list of all currencies</response>
        // GET: api/<DevisesController>
        [HttpGet]
        [ProducesResponseType(200)]
        public IEnumerable<Devise> GetAll()
        {
            return devises;
        }

        /// <summary>
        /// Get a single currency.
        /// </summary>
        /// <returns>Http response</returns>
        /// <param name="id">The id of the currency</param>
        /// <response code="200">When the currency id is found</response>
        /// <response code="404">When the currency id is not found</response>
        // GET api/<DevisesController>/5
        [HttpGet("{id}", Name = "GetDevise")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<Devise> GetById(int id)
        {
            Devise? devise = devises.FirstOrDefault((d) => d.Id == id);
            if (devise == null)
            {
                return NotFound();
            }
            return devise;
        }

        /// <summary>
        /// Create a currency.
        /// </summary>
        /// <remarks>
        /// Exemple de requête :
        ///
        ///     POST /api/Devises
        ///     {
        ///        "id": 4,
        ///        "nomDevise": "Rouble",
        ///        "taux": 89.5
        ///     }
        ///
        /// </remarks>
        /// <returns>Http response</returns>
        /// <param name="devise">The currency to create (Json in the body)</param>
        /// <response code="201">When the currency is created</response>
        /// <response code="400">When the currency model is not valid</response>
        // POST api/<DevisesController>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult<Devise> Post([FromBody] Devise devise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            devises.Add(devise);
            return CreatedAtRoute("GetDevise", new { id = devise.Id }, devise);
        }

        /// <summary>
        /// Update a currency.
        /// </summary>
        /// <returns>Http response</returns>
        /// <param name="id">The id of the currency to update</param>
        /// <param name="devise">The updated currency (Json in the body)</param>
        /// <response code="204">When the currency is updated</response>
        /// <response code="400">When the model is not valid or the ids are different</response>
        /// <response code="404">When the currency id is not found</response>
        // PUT api/<DevisesController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult Put(int id, [FromBody] Devise devise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != devise.Id)
            {
                return BadRequest();
            }
            int index = devises.FindIndex((d) => d.Id == id);
            if (index < 0)
            {
                return NotFound();
            }
            devises[index] = devise;
            return NoContent();
        }

        /// <summary>
        /// Delete a currency.
        /// </summary>
        /// <returns>Http response</returns>
        /// <param name="id">The id of the currency to delete</param>
        /// <response code="200">When the currency is deleted (the deleted currency is returned)</response>
        /// <response code="404">When the currency id is not found</response>
        // DELETE api/<DevisesController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<Devise> Delete(int id)
        {
            Devise? devise = devises.FirstOrDefault((d) => d.Id == id);
            if (devise == null)
            {
                return NotFound();
            }
            devises.Remove(devise);
            return devise;
        }
    }
}
