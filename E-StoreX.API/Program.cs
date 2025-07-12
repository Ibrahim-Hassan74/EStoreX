using EStoreX.Infrastructure;
using EStoreX.Core;
using Microsoft.Extensions.FileProviders;
using E_StoreX.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddMemoryCache();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(
        System.IO.Path.Combine(System.AppContext.BaseDirectory, "E-StoreX.API.xml"),
        includeControllerXmlComments: true
    );
});

builder.Services.AddSingleton<IFileProvider>(
    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
);

builder.Services.ConfigureInfrastructure(builder.Configuration);

builder.Services.ConfigureCore();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandlingMiddleware();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
