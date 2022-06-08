using System.Text.Json.Serialization;
using WebApi.Authorization;
using WebApi.Helpers;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

{
    var services = builder.Services;
    var env = builder.Environment;

    services.AddDbContext<DataContext>();
    services.AddCors();
    services.AddControllers().AddJsonOptions( x =>
    {
        // serializer l'énumération en string dans les réponses d'API
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        // ignorer les paramètres omis sur les modèles pour activer les paramètres facultatifs (par exemple, la mise à jour de l'utilisateur)
        x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // Configurer les paramètres
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

    // Configure le DI pour les services
    services.AddScoped<IJwtUtils, JwtUtils>();
    services.AddScoped<IUserService, UserService>();

}


var app = builder.Build();


{
    // global CORS policy
    app.UseCors( x => x 
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
    
    // Les middlewares
    app.UseMiddleware<ErrorHandlerMiddleware>();
    app.UseMiddleware<JwtMiddleware>();
    app.MapControllers();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.Run();
