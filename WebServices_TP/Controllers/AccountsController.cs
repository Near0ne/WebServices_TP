using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebServices_TP.Contexts;
using WebServices_TP.Models;

namespace WebServices_TP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountContext _context;

        public AccountsController(AccountContext context)
        {
            _context = context;
        }

        //
        // CREATION D'UN COMPTE
        //

        // POST: api/Accounts
        // Test : https://localhost:*****/api/accounts
        // Body : 
        // {
        //  "userGender": "M",
        //  "userFirstName": "Luffy",
        //  "userLastName": "Monkey D.",
        //  "userAddress": "VogueMerry"
        // }
        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
            account.UserId = IdGenerator();
            account.UserPassword = PasswordGenerator();

            _context.Accounts.Add(account);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AccountExists(account.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAccount", new { id = account.UserId }, account);
        }

        /// <summary>
        /// Generate an user id
        /// </summary>
        /// <returns></returns>
        private string IdGenerator()
        {
            Random randomizer = new Random();

            //Bank id
            string bankId = "17895";

            //Randomize the unique 6 digits id
            var uniqueId = randomizer.Next(0, 100000);
            string generatedDigits = uniqueId.ToString("00000");

            //Randomize last id letter
            int letterIteration = randomizer.Next(0, 26);
            char generatedLetter = (char)('a' + letterIteration);

            return bankId + generatedDigits + generatedLetter;
        }

        private string PasswordGenerator()
        {
            Random randomizer = new Random();

            //Randomize the unique 6 digits id
            var pwd = randomizer.Next(0, 1000000);
            return pwd.ToString("000000");
        }

        //
        // CONNEXION
        //

        // GET: api/Accounts/login
        [HttpGet("login")]
        public async Task<ActionResult<bool>> Login(Account account)
        {
            bool IsLoginSuccess = false;

            ActionResult<Account> gotAccount = await GetAccount(account.UserId);

            if (account == null)
            {
                return NotFound();
            }

            if (account.UserPassword == gotAccount.Value.UserPassword)
            {
                IsLoginSuccess = true;
            }

            return IsLoginSuccess;
        }

        ////////////////////
        ////////////////////
        ////////////////////
        ////////////////////
        ////////////////////

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(string id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        // PUT: api/Accounts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(Account account, string id)
        {
            if (id != account.UserId)
            {
                return BadRequest();
            }

            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
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

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Account>> DeleteAccount(string id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return account;
        }

        private bool AccountExists(string id)
        {
            return _context.Accounts.Any(e => e.UserId == id);
        }
    }
}
