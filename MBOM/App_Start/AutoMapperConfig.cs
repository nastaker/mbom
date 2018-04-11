using AutoMapper;
using DAL;
using DAL.Models;
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
            CreateMap<AppProject, AppProjectView>();
            CreateMap<ViewProjectProductPbom, ViewProjectProductPbomView>();
            CreateMap<ViewMbomMaintenance, ViewMbomMaintenanceView>();
            CreateMap<ProcItemTree, ProcItemTreeView>();
            CreateMap<ProcItem, ProcItemView>();
            CreateMap<ProcProcessItem, ProcProcessItemView>();
            CreateMap<AppItem, ProcProcessItemView>()
                .ForMember("ITEMID", opts => opts.MapFrom("CN_ID"));
            CreateMap<ProcItemProcess, ProcItemProcessView>();
            CreateMap<ProcCateItem, ProcCateItemView>();
            CreateMap<AppProduct, IntegrityCheckView>();
            CreateMap<AppProcessVer, AppProcessVerView>();
            CreateMap<AppProcessVerHlink, ProcItemProcessView>();
            CreateMap<AppBom, AppBomView>();
            CreateMap<ProcBomDiff, BomDiffView>();
            
            CreateMap<ProcItemSetInfo, AppItemHLink>()
                .ForMember("CN_ID", opts => opts.MapFrom("ITEMID"))
                .ForMember("CN_HLINK_ID", opts => opts.MapFrom("ITEM_HLINK_ID"))
                .ForMember("CN_SHIPPINGADDR", opts => opts.MapFrom("SHIPPINGADDR"))
                .AfterMap((src, dest) => {
                    dest.CN_F_QUANTITY = (double)src.F_QUANTITY;
                    //以下全部为默认值
                    dest.CN_ISBORROW = false;
                    dest.CN_NUMBER = 0;
                    dest.CN_ORDER = 0;
                    dest.CN_COMPONENT_CLASS_ID = 105;
                    dest.CN_COMPONENT_OBJECT_ID = 1;
                    dest.CN_COMPONENT_OBJECT_VER_ID = 0;
                    dest.CN_COMPONENT_OBJECT_VERSION = "";
                    dest.CN_S_ATTACH_DATA = "";
                    dest.CN_ITEMSTATE_TAGGER_DATA = 0;
                    dest.CN_B_IS_ASSEMBLY = false;
                    dest.CN_S_FROM = "";
                    dest.CN_ISFOLDER = false;
                    dest.CN_CREATE_NAME = null;
                    dest.CN_ISDELETE = false;
                    dest.CN_SYS_STATUS = "";
                    dest.CN_GUID = "";
                    dest.CN_DISPLAYNAME = "销售件";
                    dest.CN_DT_CREATE = DateTime.Now;
                    dest.CN_DT_EFFECTIVE = DateTime.Parse("2100-1-1");
                    dest.CN_DT_EXPIRY = DateTime.Parse("2100-1-1");
                });
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
            CreateMap<AppItem, ViewItemMaintenance>()
                .ForMember(dest => dest.CN_DT_CREATE, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<SysAttachment, FileResponse>()
                .ForMember("id", m => m.MapFrom("ItemId"))
                .ForMember("name", m => m.MapFrom("FileName"))
                .ForMember("size", m => m.MapFrom("FileSize"))
                .ForMember("url", m => m.MapFrom("FilePath"))
                .ForMember("thumbnailUrl", m => m.MapFrom("FilePath"));
        }
    }
}