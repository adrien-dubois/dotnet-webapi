using System.Text.Json.Serialization;
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

    // Configure le DI pour les services
    services.AddScoped<IUserService, UserService>();

}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


var app = builder.Build();

// Configure the HTTP request pipeline.

{
    // global CORS policy
    app.UseCors( x => x 
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
    
    // Le handler error middleware
    app.UseMiddleware<ErrorHandlerMiddleware>();
    app.MapControllers();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.Run();
