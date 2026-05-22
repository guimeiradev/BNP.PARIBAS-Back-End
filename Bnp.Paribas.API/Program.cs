using Bnp.Paribas.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilog();

builder.Services.AddApi();
builder.Services.AddVersioning();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddCorsPolicy(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseApi();

await app.SeedDatabaseAsync();

app.Run();