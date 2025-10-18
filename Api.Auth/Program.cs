using Api.Auth.Data;
using Api.Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()   // CONFIGURES EF TO USE AUTHORIZATION & AUTHENTICATION
    .AddEntityFrameworkStores<AppDbContext>()       // WITH GIVEN DEFAULT DB USER AND IT'S ROLE
    .AddDefaultTokenProviders();                    
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth API");
    options.RoutePrefix = string.Empty;
});
app.UseHttpsRedirection();
app.UseAuthentication(); // ENABLE AUTHENTICATION MIDDLEWARE TO BE RESPOSIBLE FOR AUTHENTICATING REQUESTS !!!
app.UseAuthorization();
app.MapControllers();

ApplyMigrations(); // Apply pending migrations automatically on startup

app.Run();

void ApplyMigrations()
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (app.Environment.IsDevelopment())
        dbContext.Database.EnsureCreated();
    var pendingMigrations = dbContext.Database.GetPendingMigrations();
    if (pendingMigrations.Any())
        dbContext.Database.Migrate();
}