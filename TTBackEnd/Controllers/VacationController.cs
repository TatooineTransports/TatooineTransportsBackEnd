using System;
using Microsoft.AspNetCore.Mvc;
using TTBackend.Models;
using TTBackend.Services;

namespace TTBackend.Controllers;

[Controller]
[Route("vacation/[controller]")]
public class VacationController : Controller {
    private readonly MongoDBService _mongoDBService;

    public VacationController(MongoDBService mongoDBService) {
        _mongoDBService = mongoDBService;
    }

    [HttpGet("{uid}/vacations")]
    public async Task<List<Vacation>> GetVacations(string uid) {
        return await _mongoDBService.GetVacations(uid);
    }

    [HttpGet("{vacId}")]
    public async Task<Vacation> GetVacationByID(string vacId) {
        return await _mongoDBService.GetVacationById(vacId);
    }

    [HttpPost]
    public async Task<IActionResult> CreateVacation([FromBody] Vacation vacation){
        await _mongoDBService.CreateVacation(vacation);
        return NoContent();
    }

    [HttpPut("{vacId}")]
    public async Task<IActionResult> UpdateVacation(string vacId, [FromBody] Vacation vacation) {
        await _mongoDBService.UpdateVacation(vacId, vacation);
        return NoContent();
    }

    [HttpDelete("{vacId}")]
    public async Task<IActionResult> DeleteVacation(string vacId) {
        await _mongoDBService.DeleteVacation(vacId);
        return NoContent();
    }

    [HttpGet("events/{vacId}")]
    public async Task<List<Event>> GetEvents(string vacId){
        return await _mongoDBService.GetEvents(vacId);
    }

    [HttpPost("events")]
    public async Task<IActionResult> CreateEvent([FromBody] Event newEvent) {
        await _mongoDBService.CreateEvent(newEvent);
        return NoContent();
    }

    [HttpPut("events/{eventId}")]
    public async Task<IActionResult> UpdateEvent(string eventId, [FromBody] Event updatedEvent) {
        await _mongoDBService.UpdateEvent(eventId, updatedEvent);
        return NoContent();
    }

    [HttpDelete("events/{eventId}")]
    public async Task<IActionResult> DeleteEvent(string eventId) {
        await _mongoDBService.DeleteEvent(eventId);
        return NoContent();
    }
}