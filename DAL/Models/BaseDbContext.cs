namespace DAL
{
    using Models;
    using System.Data.Entity;

    public partial class BaseDbContext : DbContext
    {
        public virtual DbSet<AppProject> TN_80_APP_0005_PROJECT { get; set; }
        public virtual DbSet<AppBom> TN_80_APP_0025_BOM { get; set; }
        public virtual DbSet<TN_80_APP_0030_PBOM> TN_80_APP_0030_PBOM { get; set; }
        public virtual DbSet<TN_00_SYS_0500_CLASS_BASE> TN_00_SYS_0500_CLASS_BASE { get; set; }
        public virtual DbSet<TN_00_SYS_0505_CLASS_TYPE_DEF> TN_00_SYS_0505_CLASS_TYPE_DEF { get; set; }
        public virtual DbSet<TN_00_SYS_0510_CLASS> TN_00_SYS_0510_CLASS { get; set; }
        public virtual DbSet<TN_00_SYS_0515_CLASS_FIELD> TN_00_SYS_0515_CLASS_FIELD { get; set; }
        public virtual DbSet<DictItemType> TN_50_DIC_0005_ITEMTYPE { get; set; }
        public virtual DbSet<AppItem> TN_80_APP_0000_ITEM { get; set; }
        public virtual DbSet<AppItemHLink> TN_80_APP_0000_ITEM_HLINK { get; set; }
        public virtual DbSet<TN_80_APP_0000_ITEM_LLINK> TN_80_APP_0000_ITEM_LLINK { get; set; }
        public virtual DbSet<TN_80_APP_0000_ITEM_LOG> TN_80_APP_0000_ITEM_LOG { get; set; }
        public virtual DbSet<TN_80_APP_0000_ITEM_SIGN> TN_80_APP_0000_ITEM_SIGN { get; set; }
        public virtual DbSet<TN_80_APP_0000_ITEM_TRACKSTEP> TN_80_APP_0000_ITEM_TRACKSTEP { get; set; }
        public virtual DbSet<AppProjectHLink> TN_80_APP_0005_PROJECT_HLINK { get; set; }
        public virtual DbSet<TN_80_APP_0005_PROJECT_LLINK> TN_80_APP_0005_PROJECT_LLINK { get; set; }
        public virtual DbSet<TN_80_APP_0005_PROJECT_REL> TN_80_APP_0005_PROJECT_REL { get; set; }
        public virtual DbSet<AppProduct> TN_80_APP_0010_PRODUCT { get; set; }
        public virtual DbSet<TN_80_APP_0020_PROCESS> TN_80_APP_0020_PROCESS { get; set; }
        public virtual DbSet<TN_80_APP_0020_PROCESS_HLINK> TN_80_APP_0020_PROCESS_HLINK { get; set; }
        public virtual DbSet<TN_80_APP_0020_PROCESS_LLINK> TN_80_APP_0020_PROCESS_LLINK { get; set; }
        public virtual DbSet<TN_80_APP_0020_PROCESS_LOG> TN_80_APP_0020_PROCESS_LOG { get; set; }
        public virtual DbSet<TN_80_APP_0020_PROCESS_TRACKSTEP> TN_80_APP_0020_PROCESS_TRACKSTEP { get; set; }
        public virtual DbSet<AppProcessVer> TN_80_APP_0020_PROCESS_VER { get; set; }
        public virtual DbSet<AppProcessVerHlink> TN_80_APP_0020_PROCESS_VER_HLINK { get; set; }
        public virtual DbSet<AppBomHlink> TN_80_APP_0025_BOM_HLINK { get; set; }
        public virtual DbSet<AppOptionalItemHlink> AppOptionalItemHlinks { get; set; }
        public virtual DbSet<TN_80_APP_0030_PBOM_HLINK> TN_80_APP_0030_PBOM_HLINK { get; set; }
        public virtual DbSet<TN_80_APP_0030_PBOM_LLINK> TN_80_APP_0030_PBOM_LLINK { get; set; }
        public virtual DbSet<TN_80_APP_0030_PBOM_LOG> TN_80_APP_0030_PBOM_LOG { get; set; }
        public virtual DbSet<TN_80_APP_0030_PBOM_TRACKSTEP> TN_80_APP_0030_PBOM_TRACKSTEP { get; set; }
        public virtual DbSet<AppPbomVer> TN_80_APP_0030_PBOM_VER { get; set; }
        public virtual DbSet<TN_80_APP_0040_MBOM> TN_80_APP_0040_MBOM { get; set; }
        public virtual DbSet<TN_80_APP_0040_MBOM_HLINK> TN_80_APP_0040_MBOM_HLINK { get; set; }
        public virtual DbSet<TN_80_APP_0040_MBOM_LLINK> TN_80_APP_0040_MBOM_LLINK { get; set; }
        public virtual DbSet<TN_80_APP_0040_MBOM_LOG> TN_80_APP_0040_MBOM_LOG { get; set; }
        public virtual DbSet<TN_80_APP_0040_MBOM_TRACKSTEP> TN_80_APP_0040_MBOM_TRACKSTEP { get; set; }
        public virtual DbSet<AppMbomVer> TN_80_APP_0040_MBOM_VER { get; set; }
        public virtual DbSet<DicItemUnit> DicItemUnits { get; set; }

        public virtual DbSet<ViewProjectProductPbom> ViewProjectProductPboms { get; set; }
        public virtual DbSet<ViewMbomMaintenance> ViewMbomMaintenances { get; set; }
        public virtual DbSet<ViewMaterialBillboards> ViewMaterialBillboardses { get; set; }
        public virtual DbSet<ViewProductBillboards> ViewProductBillboardses { get; set; }
        public virtual DbSet<ViewItemWithProcess> ViewItemWithProcesses { get; set; }
        public virtual DbSet<ViewItemMaintenance> ViewItemMaintenances { get; set; }
        public virtual DbSet<ViewNoOptionalItem> ViewNoOptionalItems { get; set; }
        public virtual DbSet<ViewOptionalItem> ViewOptionalItems { get; set; }
        public virtual DbSet<ViewPbomChangeProduct> ViewPbomChangeProducts { get; set; }
        public virtual DbSet<ViewPbomChangeItem> ViewPbomChangeItems { get; set; }
        public virtual DbSet<ViewPbomChangeItemAll> ViewPbomChangeItemsAll { get; set; }
        public virtual DbSet<ViewItemChangeDetail> ViewItemChangeDetails { get; set; }
        public virtual DbSet<ViewCreatePublishDetail> ViewCreatePublishDetails { get; set; }
        public virtual DbSet<ViewItemByType> ViewItemsByType { get; set; }
    }
}
