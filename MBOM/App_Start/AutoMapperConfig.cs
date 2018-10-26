using AutoMapper;
using Repository;
using MBOM.Models;
using Model;
using System;

namespace MBOM
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(ConfigAction);
        }

        public static Action<IMapperConfigurationExpression> ConfigAction = cfg =>
        {
            cfg.AddProfile<SysProfile>();
            cfg.AddProfile<TransferProfile>();
        };
    }

    internal class TransferProfile : Profile
    {
        public TransferProfile()
        {
            CreateMap<AppItem, ProcProcessItem>()
                .ForMember("ITEMID", opts => opts.MapFrom("CN_ID"));
            CreateMap<AppProcessVer, AppProcessVerView>();
            CreateMap<AppProcessVerHlink, ProcItemProcess>();
            RecognizePrefixes("CN_");
        }
    }

    internal class SysProfile : Profile
    {
        public SysProfile()
        {
            CreateMap<SysMenu, SysMenuView>().ForMember("Name", m=>m.MapFrom("MenuName")).ReverseMap();
            CreateMap<SysMenu, TreeMenuView>()
                .ForMember("name", m => m.MapFrom("MenuName"))
                .ForMember("children", m => m.Ignore());
            CreateMap<SysMenu, MenuView>()
                .ForMember("name", m => m.MapFrom("MenuName"));
            CreateMap<MenuView, SysMenu>()
                .ForMember("MenuName", m => m.MapFrom("name"));
            CreateMap<SysRole, SysRoleView>().ReverseMap();
            CreateMap<SysAction, SysActionView>().ReverseMap();
            CreateMap<SysRightAction, SysRightActionView>().ReverseMap();
            CreateMap<SysRoleRight, SysRoleRightView>().ReverseMap();
            CreateMap<SysUserRole, SysUserRoleView>().ReverseMap();
            CreateMap<UserProductLibrary, UserProductLibraryView>().ReverseMap();
            CreateMap<SysAttachment, FileResponse>()
                .ForMember("id", m => m.MapFrom("ItemId"))
                .ForMember("name", m => m.MapFrom("FileName"))
                .ForMember("size", m => m.MapFrom("FileSize"))
                .ForMember("url", m => m.MapFrom("FilePath"))
                .ForMember("thumbnailUrl", m => m.MapFrom("FilePath"));
        }
    }
}