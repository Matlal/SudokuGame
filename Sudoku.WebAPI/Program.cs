using Sudoku.Infrastructure.Repositories;
using Sudoku.Infrastructure.Services;
using SudokuWebAPI.Extensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddTransient<ISudokuService, SudokuService>()
    .AddTransient<ISudokuRepository, SudokuRepository>();

builder.Services
    .AddControllers(options =>
    {
        options.InputFormatters.Insert(0, JsonPatchInputFormatter.GetJsonPatchInputFormatter());
    })
    .AddNewtonsoftJson()
    .AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(c =>
    {
        c.UseInlineDefinitionsForEnums();
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();