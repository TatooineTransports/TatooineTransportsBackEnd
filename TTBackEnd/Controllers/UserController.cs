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

    [HttpGet("{uid}")]
    public async Task<User> GetUser(string uid) {
        User user = await _mongoDBService.GetUser(uid);
        return user;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user) {
        await _mongoDBService.CreateUser(user);
        return NoContent();
    }

    [HttpPost("/validUser")]
    public async Task<bool> ValidateNewUser([FromBody] User user) {
        return await _mongoDBService.ValidateUser(user);
    }

    [HttpPut("{uid}")]
    public async Task<IActionResult> updateUser(string uid, [FromBody] User user) {
        await _mongoDBService.UpdateUser(uid, user);
        return NoContent();
    }

    [HttpDelete("{uid}")]
    public async Task<IActionResult> Delete(string uid) {
        await _mongoDBService.DeleteUser(uid);
        return NoContent();
    }

}