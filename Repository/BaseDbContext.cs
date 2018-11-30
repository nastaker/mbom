namespace Repository
{
    using Model;
    using System.Data.Entity;

    public partial class BaseDbContext : DbContext
    {
        //您的上下文已配置为从您的应用程序的配置文件(App.config 或 Web.config)
        //使用“BaseDbContext”连接字符串。默认情况下，此连接字符串针对您的 LocalDb 实例上的
        //“DAL.BaseDbContext”数据库。
        // 
        //如果您想要针对其他数据库和/或数据库提供程序，请在应用程序配置文件中修改“BaseDbContext”
        //连接字符串。
        public BaseDbContext() 
            : base("name=BaseDbContext")
        {
        }
        public virtual DbSet<SysMenu> SysMenus { get; set; }
        public virtual DbSet<SysAction> SysActions { get; set; }
        public virtual DbSet<SysRightAction> SysRightActions { get; set; }
        public virtual DbSet<SysRole> SysRoles { get; set; }
        public virtual DbSet<SysRoleRight> SysRoleRights { get; set; }
        public virtual DbSet<SysUserRole> SysUserRoles { get; set; }
        public virtual DbSet<SysAttachment> SysAttachments { get; set; }
        public virtual DbSet<UserProductLibrary> UserProductLibraries { get; set; }
        public virtual DbSet<UserProductLibraryLink> UserProductLibraryLink { get; set; }

        public virtual DbSet<AppProject> AppProjects { get; set; }
        public virtual DbSet<AppBom> AppBoms { get; set; }
        public virtual DbSet<DictItemType> DictItemTypes { get; set; }
        public virtual DbSet<DictProductLine> DictProductLines { get; set; }
        public virtual DbSet<AppItem> AppItems { get; set; }
        public virtual DbSet<AppItemHLink> AppItemHLinks { get; set; }
        public virtual DbSet<AppProjectHLink> AppProjectHLinks { get; set; }
        public virtual DbSet<AppProduct> AppProducts { get; set; }
        public virtual DbSet<AppProductVer> AppProductVers { get; set; }
        public virtual DbSet<AppProductHlink> AppProductHlinks { get; set; }
        public virtual DbSet<AppProcess> AppProcesses { get; set; }
        public virtual DbSet<AppProcessVer> AppProcessVers { get; set; }
        public virtual DbSet<AppProcessVerHlink> AppProcessVerHlinks { get; set; }
        public virtual DbSet<AppOptionalItemHlink> AppOptionalItemHlinks { get; set; }
        public virtual DbSet<AppPbomVer> AppPbomVers { get; set; }
        public virtual DbSet<AppBomHlink> AppBomHlinks { get; set; }
        public virtual DbSet<AppMbomVer> AppMbomVers { get; set; }
        public virtual DbSet<AppPbomHlink> AppPbomHlinks { get; set; }
        public virtual DbSet<AppMbomHlink> AppMbomHlinks { get; set; }

        public virtual DbSet<AppProductionLineProcessVer> AppProductionLineProcessVers { get; set; }
        public virtual DbSet<AppProductionLineProcessVerHlink> AppProductionLineProcessVerHlinks { get; set; }
        public virtual DbSet<AppProductionLine> AppProductionLines { get; set; }
        public virtual DbSet<AppProductionLineProduct> AppProductionLineProducts { get; set; }
        public virtual DbSet<AppProductionLineProductProcess> AppProductionLineProductProcesses { get; set; }

        public virtual DbSet<DicItemUnit> DicItemUnits { get; set; }
        public virtual DbSet<DictShippingAddr> DictShippingAddrs { get; set; }
        public virtual DbSet<DictCustomer> DictCustomers { get; set; }
        public virtual DbSet<DictStatus> DictStatuses { get; set; }
        public virtual DbSet<AppWorkgroupUser> AppWorkgroupUsers { get; set; }
        public virtual DbSet<AppWorkgroup> AppWorkgroups { get; set; }

        public virtual DbSet<ViewProjectProductPbom> ViewProjectProductPboms { get; set; }
        public virtual DbSet<ViewMbomMaintenance> ViewMbomMaintenances { get; set; }
        public virtual DbSet<ViewMaterialBillboards> ViewMaterialBillboards { get; set; }
        public virtual DbSet<ViewItemWithProcess> ViewItemWithProcesses { get; set; }
        public virtual DbSet<ViewItemMaintenance> ViewItemMaintenances { get; set; }
        public virtual DbSet<ViewNoOptionalItem> ViewNoOptionalItems { get; set; }
        public virtual DbSet<ViewOptionalItem> ViewOptionalItems { get; set; }
        public virtual DbSet<ViewItemByType> ViewItemsByType { get; set; }
        public virtual DbSet<ViewItemWithType> ViewItemsWithType { get; set; }

        public virtual DbSet<SysLog> SysLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SysMenu>()
                        .HasMany(m => m.Children)
                        .WithOptional(m => m.Parent)
                        .HasForeignKey(m => m.ParentId);
            modelBuilder.Entity<UserProductLibrary>()
                        .HasMany(m => m.Children)
                        .WithOptional(m => m.Parent)
                        .HasForeignKey(m => m.ParentId);

            modelBuilder.Entity<SysRightAction>()
                        .HasRequired(m => m.MenuInfo)
                        .WithMany()
                        .HasForeignKey(m => m.MenuId);

            modelBuilder.Entity<SysRightAction>()
                        .HasRequired(m => m.ActionInfo)
                        .WithMany()
                        .HasForeignKey(m => m.ActionId);
            Database.SetInitializer(new BaseDbInitializer());
        }
    }
}