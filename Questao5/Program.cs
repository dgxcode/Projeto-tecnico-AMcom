using MediatR;
using Questao5.Infrastructure.Sqlite;
using Questao5.Infrastructure.Database.QueryStore;
using Questao5.Infrastructure.Database.QueryStore.Interfaces;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.CommandStore.Interfaces;
using System.Reflection;
using Questao5.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// MediatR
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

// Sqlite
builder.Services.AddSingleton(new DatabaseConfig { Name = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite") });
builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

builder.Services.AddScoped<IContaCorrenteQueryStore, ContaCorrenteQueryStore>();
builder.Services.AddScoped<IMovimentoCommandStore, MovimentoCommandStore>();
builder.Services.AddScoped<IIdempotenciaCommandStore, IdempotenciaCommandStore>();


// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure Swagger and middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Services.GetService<IDatabaseBootstrap>()?.Setup();

app.Run();
