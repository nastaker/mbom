namespace DAL
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