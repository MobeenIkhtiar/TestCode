using AutoMapper;
using Backend.Domain.BackendSetup;
using Backend.Domain.UserSetup;
using Backend.Dto.BackendSetup;

namespace Backend.WebHost.Extensions
{
  public static class MapperFactory
  {
    public static IMapper CreateMapper()
    {
      var cfg = new MapperConfiguration(config =>
      {
          #region User_Mapper_Region
          //config.CreateMap<User, PostUserDto>().ReverseMap();
          //config.CreateMap<User, RegisterDto>().ReverseMap();
          //config.CreateMap<UserRole, UserRoleDto>().ReverseMap();
          #endregion
          config.CreateMap<BackendTask, BackendTaskPostDto>().ReverseMap();
          config.CreateMap<BackendTask, BackendTaskPutDto>().ReverseMap();
          //config.CreateMap<UserRole, UserRoleDto>().ReverseMap();

      }).CreateMapper();
      return cfg;
    }
  }
}
