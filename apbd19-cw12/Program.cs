using apbd19_cw12.Data;
using apbd19_cw12.Services;
using Microsoft.EntityFrameworkCore;

namespace apbd19_cw12;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddDbContext<ApbdContext>(options => 
            options.UseSqlServer("Name=ConnectionStrings:DefaultConnection"));
        builder.Services.AddScoped<IDbService, DbService>();

        var app = builder.Build();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}