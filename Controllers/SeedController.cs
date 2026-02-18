using TheSignal.Data;
using Microsoft.AspNetCore.Mvc;

namespace TheSignal.Controllers;

public class SeedController : Controller
{
    private readonly ScenarioSeeder _seeder;

    public SeedController(ScenarioSeeder seeder)
    {
        _seeder = seeder;
    }

    [HttpGet("/seed")]
    public async Task<IActionResult> Index()
    {
        try
        {
            await _seeder.SeedAsync();
            return Content("Database seeded successfully!");
        }
        catch (Exception ex)
        {
            return Content($"Error seeding database: {ex.Message}");
        }
    }
}
