using Scalar.AspNetCore;
using Porcupine.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi("v1", options => options.AddDocumentTransformer<BearerSecuritySchemeTransformer>() );

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("scalar", opts => opts
        .WithTitle("Porcupine.WebApi")
        .ShowOperationId()
        .AddPreferredSecuritySchemes(["Bearer"])
        .EnablePersistentAuthentication()
    );
    app.Map("/", () => Results.Redirect("/scalar"));

    // set up cors
    app.UseCors(policy => policy
        .AllowAnyHeader()
        .WithOrigins("https://localhost:5010")
    );

    // init db
    await app.InitialiseDatabaseAsync();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseExceptionHandler(options => { });

app.MapAllEndpoints();

app.Run();

