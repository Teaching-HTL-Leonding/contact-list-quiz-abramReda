using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Address_Book.Data;
using Address_Book.Modles;
using System.ComponentModel.DataAnnotations;

namespace Address_Book.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ApplicationContext _db;

        public ContactsController(ApplicationContext db)
        {
            _db = db;
        }

        /// <summary>
        ///  Get all people in contact list
        /// </summary>
        /// <response code="200">Successful operation</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Person>), 200)]
        public ActionResult<IEnumerable<Person>> getAllPerson()
        {
            return Ok(_db.People);
        }

        /// <summary>
        /// Adds a new person to the list of contacts
        /// </summary>
        /// <param name="newPerson"></param>
        /// <response code="201"> Person successfully created</response>
        /// <response code="400"> nvalid input (e.g. required field missing or empty)</response>
        [HttpPost]
        [ProducesResponseType(typeof(Person),201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Person>> addPerson([FromBody,Required]Person newPerson)
        {
            if (string.IsNullOrEmpty(newPerson?.FirstName))
                return BadRequest();
            _db.People.Add(newPerson);
            await _db.SaveChangesAsync();
            return Created("", newPerson);
        }

        /// <summary>
        /// Deletes a person from the list of contacts
        /// </summary>
        /// <param name="Id"></param>
        /// <response code="204">Successful operation</response>
        /// <response code="400">Invalid ID supplied</response>
        /// <response code="404">Person not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> deletePerson([FromRoute] int Id)
        {
            var user = await _db.People.FirstOrDefaultAsync(p => p.Id == Id);
            if (user == null) return NotFound();
            _db.People.Remove(user);
            await _db.SaveChangesAsync();
            return NoContent();

        }

        /// <summary>
        /// Finds person in contact list by name
        /// </summary>
        /// <param name="nameFilter"></param>
        /// <response code="200"> successful operation</response>
        /// <response code="400">Invalid or missing name</response>
        [HttpGet("findByName")]
        [ProducesResponseType(typeof(IEnumerable<Person>),200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<Person>>> findByName([FromQuery] string nameFilter)
        {
            if (string.IsNullOrEmpty(nameFilter))
                return BadRequest();
            return await _db.People
                        .Where(person => person.FirstName == nameFilter || person.LastName == nameFilter)
                        .ToListAsync();

        }


    }
}
