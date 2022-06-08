namespace WebApi.Helpers;

using AutoMapper;
using WebApi.Entities;
using WebApi.Models.Users;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        /*----- User -> Authentication -----*/
        CreateMap<User, AuthenticateResponse>();

        /*----- Pour le MODEL CreateRequest -----*/
        CreateMap<CreateRequest, User>();

        /*----- Pour le MODEL UpdateRequest -----*/
        CreateMap<UpdateRequest, User>()
            .ForAllMembers( x => x.Condition(
                (src, dest, prop) =>
                {
                    // Ignore null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                    // ignore null role
                    if (x.DestinationMember.Name == "Role" && src.Role == null) return false;

                    return true;
                }
            ));

    }
}