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

    [HttpGet("{id}/vacations")]
    public async Task<List<Vacation>> GetVacations(string id) {
        return await _mongoDBService.GetVacations(id);
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
}