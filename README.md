# Dotnet Web API X Sql X VSCode

## Configuration

Pour créer le projet calibré pour une API avec DB Sql, dans un terminal :

```bash
dotnet new webapi -o ProjectName
cd ProjectName
dotnet add package Pomelo.EntityFrameworkCore.MySql
dotnet add package AutoMapper --version 11.0.1
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 11.0.0
dotnet add package BCrypt.Net-Next --version 4.0.3
code -r ../ProjectName
```

Supprimer les deux fichiers Weatherforecast, ainsi que la ligne `<Nullable>enable</Nullable>` dans le `.csproj`

Dans le `launchSettings.json` changer les deux `"launchUrl": "Swagger"`  par `"launchUrl": "api/project"` 

Et mettre à jour le `appsettings.json` avec la ConnectionString avec les infos de la DataBase

```json
  "ConnectionStrings": {
    "TodoDatabase": "server=localhost; database=projectname; user=root; password=password;"
  },

```

### Créer les Entities

Faire un dossier Entities et déclarer les Model de ses entité en ajoutant la Serialization en Json pour l'API. 
Si des infos ne doivent pas passer dans la réponse de l'API, comme le password par exemple, ajouter un attribut [JsonIgnore] avant.

### Créer le DataContext pour utiliser MySql

Créer un dossier `Helpers` et faire un `DataContext.cs` pour configurer la connection à la DB en récupérant la ConnectionString et sortir les entities qui passeront par dans la dB

```cs
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connectionString = Configuration.GetConnectionString("WebApiDatabase");
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }

    public DbSet<User> Users { get; set; }
```

### Models

Créer les Models d'entité selon le profil des attributs de la table. *ex: CreateRequest, UpdateRequest* en rajoutant les Data annotations afin de bien déterminer les types

### Services

Créer un Service pour créer les méthodes de l'API en fonction des entités mais aussi des Models puisque la réaction des entités va varier selon le profil du Model. Commencer par définir les méthodes dans une interface, et ensuite dans le service récupérer le DataContext pour faire la jonction avec la DB.

### Controller

Après le service, nous allons créer le controller, qui va, en s'appuyant sur le service, mapper les méthodes de ce dernier avec les requêtes HTTP adéquates *(GET,POST,UPDATE,PUT,PATCH,DELETE)* afin d'en faire des endpoints pour l'API. 

Commencer par mettre l'attribut `[ApiController]` ainsi que la route `[Route("[controller]")]`
Déclarer la class du controller avec l'attribut `ControllerBase` et relier le service avant de commencer la déclaration des requêtes avec les entêtes HTTP.

*_Exemple :_*

```cs
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _userService;
    private IMapper _mapper;

    public UsersController(
        IUserService userService,
        IMapper mapper
    )
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult getAll()
    {
        var users = _userService.GetAll();
        return Ok(users);
    }
}
```

### Helpers

Petites classes sur lesquelles on peut s'appuyer, comme par exemple le AppException qui permettra d'envoyer des messages en cas d'erreur à l'API. Ou un Middleware HandlerError qui s'occupera d'intercepter les erreur afin d'envoyer les bons statuts http aux réponse d'API.