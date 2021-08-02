using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NerdMonkey.Models;

namespace NerdMonkey.Function
{
    public class UserFunctions
    {
        private readonly HomeTownDbContext _context;

        public UserFunctions(HomeTownDbContext context)
        {
            _context = context;
        }

        [FunctionName("NewUser")]
        public async Task<IActionResult> NewUser(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "user/new")] HttpRequest req,
            ILogger log)
        {
            
            
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation("Creating new user. {Request}", requestBody);
            UserModel user;
            try
            {
                user = JsonConvert.DeserializeObject<UserModel>(requestBody);    
            }
            catch (Exception)
            {
                return new BadRequestObjectResult("Invalid JSON. JSON is not a valid User");
            }
            //check if user already exists
            var existingUser = await _context.Users.FindAsync(user.Id);
            
            if (existingUser != null)
            {
                return new BadRequestObjectResult("Unable to create a new user. A user with that Id already exists");
            } 
            //save to the database
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            return new OkObjectResult(user);
        }

        [FunctionName("GetUser")]
        public async Task<IActionResult> GetUser(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "user/{id}")] HttpRequest req,string id,
            ILogger log)
        {
            log.LogInformation("Getting new user with ID: {Id}", id);
            
            
            //check if user already exists
            var user = await _context.Users.FindAsync(id);
            
            if (user == null)
            {
               
                return new NotFoundObjectResult($"There is no user with the id {id}");
            } 
            return new OkObjectResult(user);
        }

        [FunctionName("UpdateUser")]
        public async Task<IActionResult> UpdateUser(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "user/{id}")] HttpRequest req,string id,
            ILogger log)
        {
            log.LogInformation("Getting new user with ID: {Id}", id);
            
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation("Updating user. {Request}", requestBody);
            UserModel user;
            try
            {
                user = JsonConvert.DeserializeObject<UserModel>(requestBody);    
            }
            catch (Exception)
            {
                return new BadRequestObjectResult("Invalid JSON. JSON is not a valid User");
            }
            if (user.Id != id)
            {
                log.LogError("Attempting to update a user with another Id");
                return new BadRequestObjectResult("The User Id in the body does not match the User Id in the URL");
            }
            
            //check if user already exists
            var existingUser = await _context.Users.FindAsync(id);
            
            if (existingUser == null)
            {   
                return new NotFoundObjectResult($"There is no user with the id {id}");
            } 
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return new OkObjectResult(user);
        }
    }
}
