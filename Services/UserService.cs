namespace WebApi.Services;

using AutoMapper;
using BCrypt.Net;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Users;

public interface IUserService
{
    IEnumerable<User> GetAll();
    User GetById(int id);
    void Create(CreateRequest model);
    void Update(int id, UpdateRequest model);
    void Delete(int id);
}

public class UserService : IUserService
{
    private DataContext _context;
    private readonly IMapper _mapper;

    public UserService(
        DataContext context,
        IMapper mapper
    )
    {
        _context = context;
        _mapper = mapper;
    }

    public IEnumerable<User> GetAll()
    {
        return _context.Users;
    }

    public User GetById(int id)
    {
        return getUser(id);
    }

    public void Create(CreateRequest model)
    {
        // Validation
        if(_context.Users.Any( x => x.Email == model.Email ))
            throw new AppException("Un utilisateur avec le mail '" + model.Email + "' existe déjà.");

        // Map du Model pour faire un nouvel objet User
        var user = _mapper.Map<User>(model);

        // maintenant hash du password
        user.PasswordHash = BCrypt.HashPassword(model.Password);

        // avec le context de la DB on sauvegarde le nouvel user
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void Update(int id, UpdateRequest model)
    {
        var user = getUser(id);

        // validation
        if(model.Email != user.Email && _context.Users.Any( x => x.Email == model.Email ))
            throw new AppException("Un utilisateur avec le mail '" + model.Email + "' existe déjà.");
        
        // hash mot de passe si il est entré
        if(!string.IsNullOrEmpty(model.Password))
            user.PasswordHash = BCrypt.HashPassword(model.Password);
        
        // Copie le model vers l'utilisateur et on sauvegarde
        _mapper.Map(model, user);
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var user = getUser(id);
        _context.Users.Remove(user);
        _context.SaveChanges();
    }


    // méthode getUser pour les updates ou delete ou validations

    private User getUser(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) throw new KeyNotFoundException("Utilisateur introuvable.");
        return user;
    }
}