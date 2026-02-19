using TheSignal.Data;
using TheSignal.Repositories.Interfaces;
using TheSignal.Repositories.Implementations;
using TheSignal.Services;
using TheSignal.Services.Interfaces;
using TheSignal.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// Add Health Checks
builder.Services.AddHealthChecks();

// Register Repositories
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();

// Register Services
builder.Services.AddScoped<IGeminiClient, GeminiClient>();
builder.Services.AddScoped<INarrativeService, NarrativeService>();
builder.Services.AddScoped<INarrativeParserService, NarrativeParserService>();
builder.Services.AddScoped<IStartingScenarioService, StartingScenarioService>();
builder.Services.AddScoped<IGameSessionService, GameSessionService>();
builder.Services.AddScoped<IStoryProgressionService, StoryProgressionService>();
builder.Services.AddScoped<IExplorationService, ExplorationService>();
builder.Services.AddScoped<ScenarioSeeder>();

var app = builder.Build();

// Seed the database only in Development
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var seeder = services.GetRequiredService<ScenarioSeeder>();
            await seeder.SeedAsync();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// Health check endpoint
app.MapHealthChecks("/health");

app.Run();
