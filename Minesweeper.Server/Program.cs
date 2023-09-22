using CommitExplorerOAuth2AspNET.Domain.Repositories;
using Minesweeper.Server.Domain.Repositories;
using Minesweeper.Server.Domain.Repositories.Abstract;
using Minesweeper.Server.Domain.Repositories.EntityFramework;
using Minesweeper.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IMinesweeperModelRepository,EFMinesweeperModelRepository>(); ;
builder.Services.AddDbContext<MyDbContext>();
builder.Services.AddTransient<MinesweeperService>();
builder.Services.AddTransient<DataManager>();

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
        builder =>
        {
            builder.AllowAnyHeader()
                   .AllowAnyMethod()
                   .SetIsOriginAllowed((host) => true)
                   .AllowCredentials();
        }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
