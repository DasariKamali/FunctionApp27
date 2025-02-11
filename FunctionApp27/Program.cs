using FunctionApp27.Helpers;
using FunctionApp27.Services;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);
builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddSingleton<CsvHelperService>();
builder.ConfigureFunctionsWebApplication();
builder.Build().Run();
