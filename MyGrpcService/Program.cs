using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MyGrpcService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseInMemoryDatabase("MyInMemoryDb"));
builder.Services.AddGrpc();

var app = builder.Build();
app.MapGrpcService<OrderServiceImpl>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();
