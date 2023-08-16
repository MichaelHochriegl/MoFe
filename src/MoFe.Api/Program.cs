using FastEndpoints;
using FastEndpoints.Swagger;
using MoFe.Api;
using MoFe.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFastEndpoints(opt =>
{
    opt.IncludeAbstractValidators = true;
});
builder.Services.AddSwaggerDoc();

var connectionString = builder.Configuration.GetConnectionString("MoFeDb");
ArgumentException.ThrowIfNullOrEmpty(connectionString);
builder.Services.AddPersistence(connectionString);

var app = builder.Build();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseFastEndpoints();
app.UseSwaggerGen();

app.Run();