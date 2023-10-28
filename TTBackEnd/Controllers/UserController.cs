using System;
using Microsoft.AspNetCore.Mvc;
using TTBackend.Models;
using TTBackend.Services;

namespace TTBackend.Controllers;

[Controller]
[Route("user/[controller]")]
public class UserController: Controller {
    private readonly MongoDBService _mongoDBService;

    public UserController(MongoDBService mongoDBService){
        _mongoDBService = mongoDBService;
    }

    [HttpGet("{id}")]
    public async Task<User> GetUser(string id) {
        User user = await _mongoDBService.GetUser(id);
        return user;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user) {
        await _mongoDBService.CreateUser(user);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> updateUser(string id, [FromBody] User user) {
        await _mongoDBService.UpdateUser(id, user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id) {
        await _mongoDBService.DeleteUser(id);
        return NoContent();
    }

}