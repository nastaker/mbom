using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Collections;
using Model;

namespace Repository
{

    public static class Proc
    {
        //需要修改名字
        const string PROC_GET_PRODUCT_ITEM_HAVE_PROCESS = "PROC_GET_PRODUCT_ITEM_HAVE_PROCESS @prod_itemcode";
        const string PROC_MBOM_INTEGRITY_CHECK = "PROC_MBOM_INTEGRITY_CHECK @prod_itemcode";
        const string PROC_LIBRARY_ITEMS_ADD = "PROC_LIBRARY_ITEMS_ADD @libid,@ids,@userid,@name,@login";
        const string PROC_PRODUCT_CHANGE_DETAIL = "PROC_PRODUCT_CHANGE_DETAIL @prod_itemcode";
        const string PROC_SWITCH_ITEM_TYPE = "PROC_SWITCH_ITEM_TYPE @itemid,@userid,@name,@login";
        const string PROC_SET_ITEM_TYPE = "PROC_SET_ITEM_TYPE @itemid,@typeid,@userid,@name,@login";
        const string PROC_SET_PRODUCT_SELLITEM = "PROC_SET_PRODUCT_SELLITEM @prod_itemcode,@str,@userid,@name,@login";
        const string PROC_OPTIONALITEMS_SET = "PROC_OPTIONALITEMS_SET @itemids,@userid,@name,@login";
        const string PROC_OPTIONALITEM_MAP_ADD = "PROC_OPTIONALITEM_MAP_ADD @itemid,@itemids,@userid,@name,@login";

        const string PROC_OPTIONALITEM_MAP_REMOVE = "PROC_OPTIONALITEM_MAP_REMOVE @hlinkids";
        //产品数据操作
        const string PROC_MBOM_VER_CREATE = "PROC_MBOM_VER_CREATE @prod_itemcode,@ver,@dt_effective,@dt_expiry,@desc,@pbomver_guid,@userid,@name,@login";
        const string PROC_PRODUCT_VER_CREATE = "PROC_PRODUCT_VER_CREATE @prod_itemcode,@vername,@desc,@userid,@name,@login";
        const string PROC_PRODUCT_TRANSFER_INITIATTE = "PROC_PRODUCT_TRANSFER_INITIATTE @prod_itemcode,@userid,@name,@login";
        const string PROC_PRODUCT_TRANSFER_PUBLISH = "PROC_PRODUCT_TRANSFER_PUBLISH @prod_itemcode,@userid,@name,@login";
        const string PROC_MBOM_RELEASE = "PROC_MBOM_RELEASE @prod_itemcode,@userid,@name,@login";
        //物料数据获取
        const string PROC_GET_ITEM_PARENTS = "PROC_GET_ITEM_PARENTS @prod_itemcode";
        const string PROC_GET_ITEM_TREE = "PROC_GET_ITEM_TREE @itemcode";
        const string PROC_UPDATE_ITEM = "PROC_UPDATE_ITEM @itemcode,@itemname,@weight,@unit,@productlinecode,@typeids,@userid,@name,@login";
        //产品数据获取
        const string PROC_GET_PRODUCT_PBOM = "PROC_GET_PRODUCT_PBOM @prod_itemcode,@date";
        const string PROC_GET_PRODUCT_MBOM = "PROC_GET_PRODUCT_MBOM @prod_itemcode,@date";
        const string PROC_GET_PRODUCT_PBOM_VER_LIST = "PROC_GET_PRODUCT_PBOM_VER_LIST @prod_itemcode";
        const string PROC_GET_PRODUCT_MBOM_VER_LIST = "PROC_GET_PRODUCT_MBOM_VER_LIST @prod_itemcode";

        const string PROC_GET_PRODUCT_SELLITEM_LATEST = "PROC_GET_PRODUCT_SELLITEM_LATEST @prod_itemcode";
        const string PROC_GET_PRODUCT_SELLITEM = "PROC_GET_PRODUCT_SELLITEM @prod_itemcode,@guid_ver";
        const string PROC_GET_PRODUCT_ITEM_WITH_CATEGORY = "PROC_GET_PRODUCT_ITEM_WITH_CATEGORY @prod_itemcode";
        const string PROC_GET_PRODUCT_ITEM_BY_CATEGORYNAME = "PROC_GET_PRODUCT_ITEM_BY_CATEGORYNAME @prod_itemcode,@categoryname";
        const string PROC_GET_PRODUCT_MBOM_LATEST = "PROC_GET_PRODUCT_MBOM_LATEST @prod_itemcode";
        //虚件
        const string PROC_VIRTUAL_ITEM_SET = "PROC_VIRTUAL_ITEM_SET @prod_itemcode,@guid,@userid,@name,@login";
        const string PROC_VIRTUAL_ITEM_DROP = "PROC_VIRTUAL_ITEM_DROP @prod_itemcode,@guid,@userid,@name,@login";
        const string PROC_VIRTUAL_ITEM_LINK = "PROC_VIRTUAL_ITEM_LINK @prod_itemcode,@itemcode_parent,@guid,@userid,@name,@login";
        const string PROC_VIRTUAL_ITEM_UNLINK = "PROC_VIRTUAL_ITEM_UNLINK @prod_itemcode,@guid,@userid,@name,@login";
        //合件
        const string PROC_COMPOSITE_ITEM_SET = "PROC_COMPOSITE_ITEM_SET @prod_itemcode,@guids,@type,@itemtype,@userid,@name,@login";
        const string PROC_COMPOSITE_ITEM_DROP = "PROC_COMPOSITE_ITEM_DROP @prod_itemcode,@guid,@userid,@name,@login";
        const string PROC_COMPOSITE_ITEM_LINK = "PROC_COMPOSITE_ITEM_LINK @prod_itemcode,@itemcode_parent,@guid,@userid,@name,@login";
        const string PROC_COMPOSITE_ITEM_UNLINK = "PROC_COMPOSITE_ITEM_UNLINK @prod_itemcode,@guid,@userid,@name,@login";
        //
        const string PROC_MBOM_PROCESS_SET = "PROC_MBOM_PROCESS_SET @guid_ver,@guid_mbom,@guid_process,@type,@userid,@name,@login";
        //引用件
        const string PROC_MBOM_ADD = "PROC_MBOM_ADD @prod_itemcode,@itemcode_parent,@itemcode,@quantity,@userid,@name,@login";
        const string PROC_MBOM_REMOVE = "PROC_MBOM_REMOVE @prod_itemcode,@guids,@userid,@name,@login";
        const string PROC_ITEM_PUBLISH = "PROC_ITEM_PUBLISH @itemcode,@userid,@name,@login";
        const string PROC_MBOM_MODIFY_QUANTITY = "PROC_MBOM_MODIFY_QUANTITY @guid,@quantity";

        public static List<ProcItemTree> ProcGetItemPBomTree(BaseDbContext db, string prod_itemcode, DateTime date)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode),
                new SqlParameter("@date", date)
            };
            var result = db.Database.SqlQuery<ProcItemTree>(PROC_GET_PRODUCT_PBOM, param).ToList();
            return result;
        }

        public static List<ProcItemTree> ProcGetItemMBomTree(BaseDbContext db, string prod_itemcode, DateTime date)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode),
                new SqlParameter("@date", date)
            };
            var result = db.Database.SqlQuery<ProcItemTree>(PROC_GET_PRODUCT_MBOM, param).ToList();
            return result;
        }

        public static List<ProcItemSetInfo> ProcGetProductSellInfo(BaseDbContext db, string prod_itemcode)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode)
            };
            var result = db.Database.SqlQuery<ProcItemSetInfo>(PROC_GET_PRODUCT_SELLITEM_LATEST, param).ToList();
            return result;
        }

        public static List<ProcCateItem> ProcGetProductSellInfo(BaseDbContext db, string prod_itemcode, Guid guid)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode),
                new SqlParameter("@guid_ver", guid)
            };
            var result = db.Database.SqlQuery<ProcCateItem>(PROC_GET_PRODUCT_SELLITEM, param).ToList();
            return result;
        }

        public static ProcReturnMsg ProcUpdateItem(BaseDbContext db, string itemcode, string itemname, double weight, string unit, string productlinecode, string typeids, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@itemcode", itemcode),
                new SqlParameter("@itemname", itemname),
                new SqlParameter("@weight", weight),
                new SqlParameter("@unit", unit),
                new SqlParameter("@productlinecode", productlinecode),
                new SqlParameter("@typeids", typeids),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_UPDATE_ITEM, param).SingleOrDefault();
            return result;
        }


        public static ProcReturnMsg ProcCreateProductVer(BaseDbContext db, string prod_itemcode, string name, string desc, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode),
                new SqlParameter("@vername", name),
                new SqlParameter("@desc", desc),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_PRODUCT_VER_CREATE, param).SingleOrDefault();
            return result;
        }

        public static List<ProcBomVer> ProcGetPbomVerList(BaseDbContext db, string prod_itemcode)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode)
            };
            var result = db.Database.SqlQuery<ProcBomVer>(PROC_GET_PRODUCT_PBOM_VER_LIST, param).ToList();
            return result;
        }

        public static List<ProcBomVer> ProcGetMbomVerList(BaseDbContext db, string prod_itemcode)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode)
            };
            var result = db.Database.SqlQuery<ProcBomVer>(PROC_GET_PRODUCT_MBOM_VER_LIST, param).ToList();
            return result;
        }

        public static List<ProcItem> ProcProductList(BaseDbContext db, string prod_itemcode)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode)
            };
            var result = db.Database.SqlQuery<ProcItem>(PROC_GET_PRODUCT_ITEM_WITH_CATEGORY, param).ToList();
            return result;
        }

        public static List<ProcProcessItem> ProcGetProcessItemList(BaseDbContext db, string prod_itemcode)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode)
            };
            var result = db.Database.SqlQuery<ProcProcessItem>(PROC_GET_PRODUCT_ITEM_HAVE_PROCESS, param).ToList();
            return result;
        }

        public static List<ProcCateItem> ProcGetItemCateList(BaseDbContext db, string prod_itemcode, string catename)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode),
                new SqlParameter("@categoryname", catename)
            };
            var result = db.Database.SqlQuery<ProcCateItem>(PROC_GET_PRODUCT_ITEM_BY_CATEGORYNAME, param).ToList();
            return result;
        }

        public static List<ProcProductChangeDetail> ProcProductChangeDetail(BaseDbContext db, string prod_itemcode)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode)
            };
            var result = db.Database.SqlQuery<ProcProductChangeDetail>(PROC_PRODUCT_CHANGE_DETAIL, param).ToList();
            return result;
        }

        public static ProcReturnMsg ProcSetSaleList(BaseDbContext db, string prod_itemcode, string str, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode),
                new SqlParameter("@str", str),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_SET_PRODUCT_SELLITEM, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg ProcItemSetType(BaseDbContext db, int itemid, int typeid, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@itemid", itemid),
                new SqlParameter("@typeid", typeid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_SET_ITEM_TYPE, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg ProcItemTypeSwitch(BaseDbContext db, int itemid, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@itemid", itemid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_SWITCH_ITEM_TYPE, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg ProcMbomRelease(BaseDbContext db, string prod_itemcode, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_MBOM_RELEASE, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg OptionalItemMapRemove(BaseDbContext db, string hlinkids)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@hlinkids", hlinkids)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_OPTIONALITEM_MAP_REMOVE, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg OptionalItemMapAdd(BaseDbContext db, int itemid, string itemids, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@itemid", itemid),
                new SqlParameter("@itemids", itemids),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_OPTIONALITEM_MAP_ADD, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg ProcSetOptionalItems(BaseDbContext db, string itemids, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@itemids", itemids),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_OPTIONALITEMS_SET, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg ProcItemEditQuantity(BaseDbContext db, Guid guid, float quantity)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@guid", guid),
                new SqlParameter("@quantity", quantity)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_MBOM_MODIFY_QUANTITY, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg ProcItemUnLink(BaseDbContext db, string prod_itemcode, string guids, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode),
                new SqlParameter("@guids", guids),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_MBOM_REMOVE, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg ProcItemLink(BaseDbContext db, string prod_itemcode, string itemcode_parent, string itemcode, float quantity, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode),
                new SqlParameter("@itemcode_parent", itemcode_parent),
                new SqlParameter("@itemcode", itemcode),
                new SqlParameter("@quantity", quantity),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_MBOM_ADD, param).SingleOrDefault();
            return result;
        }

        public static object ProcItemPublish(BaseDbContext db, string itemcode, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@itemcode", itemcode),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_ITEM_PUBLISH, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg ProcUserProductLibraryLinkAdd(BaseDbContext db, int libid, string ids, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@libid", libid),
                new SqlParameter("@ids", ids),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_LIBRARY_ITEMS_ADD, param).SingleOrDefault();
            return result;
        }

        public static List<ProcProcessItem> ProcGetItemParent(BaseDbContext db, string prod_itemcode)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode)
            };
            var result = db.Database.SqlQuery<ProcProcessItem>(PROC_GET_ITEM_PARENTS, param).ToList();
            return result;
        }

        public static ProcReturnMsg ProcProductTransferInitiate(BaseDbContext db, string prod_itemcode, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_PRODUCT_TRANSFER_INITIATTE, param).First();
            return result;
        }

        public static ProcReturnMsg ProcProductTransferPublish(BaseDbContext db, string prod_itemcode, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_PRODUCT_TRANSFER_PUBLISH, param).First();
            return result;
        }

        //MBOM
        //MBOM创建新版本
        public static ProcReturnMsg ProcCreateMbomVer(BaseDbContext db, string prod_itemcode, string ver, DateTime dtef, DateTime dtex, string pbomver_guid, string desc, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode),
                new SqlParameter("@ver", ver),
                new SqlParameter("@dt_effective", dtef),
                new SqlParameter("@dt_expiry", dtex),
                new SqlParameter("@desc", desc),
                new SqlParameter("@pbomver_guid", pbomver_guid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_MBOM_VER_CREATE, param).SingleOrDefault();
            return result;
        }

        //MBOM完整性核查
        public static AppProduct ProcMbomIntegrityCheck(BaseDbContext db, string prod_itemcode)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode)
            };
            var result = db.Database.SqlQuery<AppProduct>(PROC_MBOM_INTEGRITY_CHECK, param).SingleOrDefault();
            return result;
        }
        //MBOM 物料看板
        public static List<IEnumerable> ProcMaterialBillboards(BaseDbContext db, string prod_itemcode)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode)
            };
            var result = db.MultipleResults(PROC_MBOM_INTEGRITY_CHECK, param)
                .With<ProcReturnMsg>()
                .With<ProcItem>()
                .Execute();
            return result;
        }

        //MBOM主树获取
        public static List<ProcItemTree> ProcGetMbomList(BaseDbContext db, string prod_itemcode)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode)
            };
            var result = db.Database.SqlQuery<ProcItemTree>(PROC_GET_PRODUCT_MBOM_LATEST, param).ToList();
            return result;
        }

        //MBOM物料单层数据
        public static List<ProcItemTree> ProcGetItemList(BaseDbContext db, string itemcode)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@itemcode", itemcode)
            };
            var result = db.Database.SqlQuery<ProcItemTree>(PROC_GET_ITEM_TREE, param).ToList();
            return result;
        }

        public static ProcReturnMsg ProcSetMbomProcess(BaseDbContext db, Guid guid_ver, Guid guid_mbom, string guid_process, int type, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@guid_ver", guid_ver),
                new SqlParameter("@guid_mbom", guid_mbom),
                new SqlParameter("@guid_process", guid_process),
                new SqlParameter("@type", type),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_MBOM_PROCESS_SET, param).SingleOrDefault();
            return result;
        }

        #region 虚件操作
        public static ProcReturnMsg ProcVirtualItemSet(BaseDbContext db, string prod_itemcode, Guid guid, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode),
                new SqlParameter("@guid", guid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_VIRTUAL_ITEM_SET, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg ProcVirtualItemDrop(BaseDbContext db, string prod_itemcode, Guid guid, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode),
                new SqlParameter("@guid", guid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_VIRTUAL_ITEM_DROP, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg ProcVirtualItemLink(BaseDbContext db, string prod_itemcode, string itemcode_parent, Guid guid, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode),
                new SqlParameter("@itemcode_parent", itemcode_parent),
                new SqlParameter("@guid", guid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_VIRTUAL_ITEM_LINK, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg ProcVirtualItemUnlink(BaseDbContext db, string prod_itemcode, Guid guid, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode),
                new SqlParameter("@guid", guid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_VIRTUAL_ITEM_UNLINK, param).SingleOrDefault();
            return result;
        }
        #endregion
        #region 合件
        //设置合件
        public static ProcReturnMsg ProcCompositeItemSet(BaseDbContext db,string prod_itemcode, string guids, string type, string itemtype, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode),
                new SqlParameter("@guids", guids),
                new SqlParameter("@type", type),
                new SqlParameter("@itemtype", itemtype),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_COMPOSITE_ITEM_SET, param).SingleOrDefault();
            return result;
        }
        //删除合件
        public static ProcReturnMsg ProcCompositeItemDrop(BaseDbContext db, string prod_itemcode, Guid guid, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode),
                new SqlParameter("@guid", guid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_COMPOSITE_ITEM_DROP, param).SingleOrDefault();
            return result;
        }
        //引用合件
        public static ProcReturnMsg ProcCompositeItemLink(BaseDbContext db,string prod_itemcode, string itemcode_parent, Guid guid, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode),
                new SqlParameter("@itemcode_parent", itemcode_parent),
                new SqlParameter("@guid", guid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_COMPOSITE_ITEM_LINK, param).SingleOrDefault();
            return result;
        }
        //删除合件引用
        public static ProcReturnMsg ProcCompositeItemUnlink(BaseDbContext db,string prod_itemcode, Guid guid, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prod_itemcode", prod_itemcode),
                new SqlParameter("@guid", guid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_COMPOSITE_ITEM_UNLINK, param).SingleOrDefault();
            return result;
        }
        #endregion

    }
}
