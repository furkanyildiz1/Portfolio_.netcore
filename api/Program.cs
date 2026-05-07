using api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// bize gerekli olan servisleri ekleme yeri : swagger, dbcontext, identity, vs. gibi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicaitonDBContext>(options =>{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//app.Use middleware'leri ekleme yeri : authentication, authorization, vs. gibi
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Test endpoint
app.MapGet("/api/test", () => "Hello from FinShark API!")
    .WithName("GetTest");

app.MapControllers();

app.Run();

