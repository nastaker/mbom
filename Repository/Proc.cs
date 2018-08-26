﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using System.Collections;
using Model;

namespace Repository
{

    public static class Proc
    {
        const string PROC_GET_ITEM_PBOM_TREE = "PROC_GET_ITEM_PBOM_TREE @code";
        const string PROC_GET_ITEM_MBOM_TREE = "PROC_GET_ITEM_MBOM_TREE @code";
        const string PROC_GET_ITEM_SALESETINFO = "PROC_GET_ITEM_SETINFO @code";
        const string PROC_GET_ITEM_LIST = "PROC_GET_ITEM_LIST @code";
        const string PROC_GET_PROCESS_ITEM_LIST = "PROC_GET_PROCESS_ITEM @code";
        const string PROC_GET_ITEM_CATE_LIST = "PROC_GET_ITEM_CATE_LIST @code,@catename";
        const string PROC_PRODUCT_TRANSFER_INITIATTE = "PROC_PRODUCT_TRANSFER_INITIATTE @code,@userid,@name,@login";
        const string PROC_MBOM_VER_CREATE = "PROC_MBOM_VER_CREATE @code,@ver,@dt_effective,@dt_expiry,@desc,@pbom_ver_guid,@userid,@name,@login";
        const string PROC_GET_MBOM_LIST = "PROC_GET_MBOM_MAINTENANCETREE @code";
        const string PROC_GET_PRODUCT_DISCRETE_LIST = "PROC_GET_PRODUCT_DISCRETE_LIST @code";
        //虚件
        const string PROC_VIRTUAL_ITEM_SET = "PROC_VIRTUAL_ITEM_SET @prodcode,@guid,@userid,@name,@login";
        const string PROC_VIRTUAL_ITEM_DROP = "PROC_VIRTUAL_ITEM_DROP @prodcode,@guid,@userid,@name,@login";
        const string PROC_VIRTUAL_ITEM_LINK = "PROC_VIRTUAL_ITEM_LINK @prodcode,@parentcode,@guid,@userid,@name,@login";
        const string PROC_VIRTUAL_ITEM_UNLINK = "PROC_VIRTUAL_ITEM_UNLINK @prodcode,@guid,@userid,@name,@login";
        //合件
        const string PROC_COMPOSITE_ITEM_SET = "PROC_COMPOSITE_ITEM_SET @prodcode,@guids,@type,@userid,@name,@login";
        const string PROC_COMPOSITE_ITEM_DROP = "PROC_COMPOSITE_ITEM_DROP @prodcode,@guid,@userid,@name,@login";
        const string PROC_COMPOSITE_ITEM_LINK = "PROC_COMPOSITE_ITEM_LINK @prodcode,@parentcode,@guid,@userid,@name,@login";
        const string PROC_COMPOSITE_ITEM_UNLINK = "PROC_COMPOSITE_ITEM_UNLINK @prodcode,@guid,@userid,@name,@login";
        const string PROC_COMPOSITE_EDIT_NAME = "PROC_COMPOSITE_EDIT_NAME @itemid,@name";
        const string PROC_MBOM_RELEASE = "PROC_MBOM_RELEASE @code,@userid,@name,@login";
        //产品变更
        const string PROC_PRODUCT_CHANGE_APPLY_CHANGES = "PROC_PRODUCT_CHANGE_APPLY_CHANGES @code,@reason,@userid,@name,@login";
        const string PROC_PRODUCT_CHANGE_APPLY_VIRTUAL_CHANGES = "PROC_PRODUCT_CHANGE_APPLY_VIRTUAL_CHANGES @code,@reason,@userid,@name,@login";
        const string PROC_SET_ITEM_KL = "PROC_SET_ITEM_KL @bomhlinkids, @processhlinkid,@userid,@name,@login";
        const string PROC_MBOM_INTEGRITY_CHECK = "PROC_MBOM_INTEGRITY_CHECK @code";
        const string PROC_USER_PROD_LIB_LINK_ADD = "PROC_USER_PROD_LIB_LINK_ADD @libid,@ids,@userid,@name,@login";
        const string PROC_GET_BOM_DIFF = "PROC_GET_BOM_DIFF @bomid";
        const string PROC_ITEM_LINK = "PROC_ITEM_LINK @code,@pid,@plink,@itemid,@quantity,@userid,@name,@login";
        const string PROC_ITEM_UNLINK = "PROC_ITEM_UNLINK @code,@hlinkid,@userid,@name,@login";
        const string PROC_ITEM_EDITLINKQUANTITY = "PROC_ITEM_EDITLINKQUANTITY @hlinkid,@quantity";
        const string PROC_OPTIONALITEMS_SET = "PROC_OPTIONALITEMS_SET @itemids,@userid,@name,@login";
        const string PROC_OPTIONALITEM_MAP_ADD = "PROC_OPTIONALITEM_MAP_ADD @itemid,@itemids,@userid,@name,@login";
        const string PROC_OPTIONALITEM_MAP_REMOVE = "PROC_OPTIONALITEM_MAP_REMOVE @hlinkids";
        const string PROC_PRODUCT_CHANGE_DETAIL = "PROC_PRODUCT_CHANGE_DETAIL @prodcode";
        const string PROC_GET_BOM_HLINK_CHILDREN = "PROC_GET_BOM_HLINK_CHILDREN @itemcode";
        const string PROC_BOMHLINK_CHILD_ADD = "PROC_BOMHLINK_CHILD_ADD @parentitemcode,@itemid,@hlinkid,@bywhat,@userid,@name,@login";
        const string PROC_BOMHLINK_ADD = "PROC_BOMHLINK_ADD @parentitemcode,@itemid,@bywhat,@userid,@name,@login";
        const string PROC_DISABLE_BOMHLINK = "PROC_DISABLE_BOMHLINK @hlinkid,@userid,@name,@login";
        const string PROC_APPLY_BOM_CHANGE = "PROC_APPLY_BOMHLINK @hlinkid,@bywhat,@userid,@name,@login";
        const string PROC_ITEM_TYPE_TRANS = "PROC_ITEM_TYPE_TRANS @itemid,@userid,@name,@login";
        const string PROC_ITEM_SET_TYPE = "PROC_ITEM_SET_TYPE @itemid,@typeid,@userid,@name,@login";
        const string PROC_SET_SALELIST = "PROC_SET_SALELIST @code,@str,@userid,@name,@login";

        public static List<ProcItemTree> ProcGetItemPBomTree(BaseDbContext db, string code)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code)
            };
            var result = db.Database.SqlQuery<ProcItemTree>(PROC_GET_ITEM_PBOM_TREE, param).ToList();
            return result;
        }

        public static List<ProcItemTree> ProcGetItemMBomTree(BaseDbContext db, string code)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code)
            };
            var result = db.Database.SqlQuery<ProcItemTree>(PROC_GET_ITEM_MBOM_TREE, param).ToList();
            return result;
        }

        public static List<ProcItemSetInfo> ProcGetItemSaleSetInfo(BaseDbContext db, string code)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code)
            };
            var result = db.Database.SqlQuery<ProcItemSetInfo>(PROC_GET_ITEM_SALESETINFO, param).ToList();
            return result;
        }

        public static List<ProcItem> ProcProductList(BaseDbContext db, string code)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code)
            };
            var result = db.Database.SqlQuery<ProcItem>(PROC_GET_ITEM_LIST, param).ToList();
            return result;
        }

        public static List<ProcProcessItem> ProcGetProcessItemList(BaseDbContext db, string code)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code)
            };
            var result = db.Database.SqlQuery<ProcProcessItem>(PROC_GET_PROCESS_ITEM_LIST, param).ToList();
            return result;
        }

        public static List<ProcBomDiff> ProcGetBomDiff(BaseDbContext db, int bomid)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@bomid", bomid)
            };
            var result = db.Database.SqlQuery<ProcBomDiff>(PROC_GET_BOM_DIFF, param).ToList();
            return result;
        }

        public static List<ProcCateItem> ProcGetItemCateList(BaseDbContext db, string code, string catename)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code),
                new SqlParameter("@catename", catename)
            };
            var result = db.Database.SqlQuery<ProcCateItem>(PROC_GET_ITEM_CATE_LIST, param).ToList();
            return result;
        }

        public static List<ProcProductChangeDetail> ProcProductChangeDetail(BaseDbContext db, string prodcode)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prodcode", prodcode)
            };
            var result = db.Database.SqlQuery<ProcProductChangeDetail>(PROC_PRODUCT_CHANGE_DETAIL, param).ToList();
            return result;
        }
        //BOM子件列表
        public static List<AppBomHlink> ProcGetBomHlinkChildren(BaseDbContext db, string itemcode)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@itemcode", itemcode)
            };
            var result = db.Database.SqlQuery<AppBomHlink>(PROC_GET_BOM_HLINK_CHILDREN, param).ToList();
            return result;
        }

        public static ProcReturnMsg ProcBomHlinkAdd(BaseDbContext db, string parentitemcode, int itemid, string bywhat, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@parentitemcode", parentitemcode),
                new SqlParameter("@itemid", itemid),
                new SqlParameter("@bywhat", bywhat),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_BOMHLINK_ADD, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg ProcSetSaleList(BaseDbContext db, string code, string str, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code),
                new SqlParameter("@str", str),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_SET_SALELIST, param).SingleOrDefault();
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
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_ITEM_SET_TYPE, param).SingleOrDefault();
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
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_ITEM_TYPE_TRANS, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg ProcApplyBomChange(BaseDbContext db, int hlinkid, string bywhat, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@hlinkid", hlinkid),
                new SqlParameter("@bywhat", bywhat),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_APPLY_BOM_CHANGE, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg ProcDisableBomHlink(BaseDbContext db, int hlinkid, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@hlinkid", hlinkid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_DISABLE_BOMHLINK, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg ProcBomHlinkChildAdd(BaseDbContext db, string parentitemcode, int itemid, int hlinkid, string bywhat, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@parentitemcode", parentitemcode),
                new SqlParameter("@itemid", itemid),
                new SqlParameter("@hlinkid", hlinkid),
                new SqlParameter("@bywhat", bywhat),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_BOMHLINK_CHILD_ADD, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg ProcMbomRelease(BaseDbContext db, string code, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code),
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

        public static ProcReturnMsg ProcItemEditQuantity(BaseDbContext db, int hlinkid, float quantity)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@hlinkid", hlinkid),
                new SqlParameter("@quantity", quantity)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_ITEM_EDITLINKQUANTITY, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg ProcItemUnLink(BaseDbContext db, string code, int hlinkid, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code),
                new SqlParameter("@hlinkid", hlinkid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_ITEM_UNLINK, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg ProcItemLink(BaseDbContext db, string code, int pid, string plink, int itemid, float quantity, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code),
                new SqlParameter("@pid", pid),
                new SqlParameter("@plink", plink),
                new SqlParameter("@itemid", itemid),
                new SqlParameter("@quantity", quantity),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_ITEM_LINK, param).SingleOrDefault();
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
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_USER_PROD_LIB_LINK_ADD, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg ProcEditCombineName(BaseDbContext db, int itemid, string name)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@itemid", itemid),
                new SqlParameter("@name", name)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_COMPOSITE_EDIT_NAME, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg ProcProductTransferInitiate(BaseDbContext db, string code, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_PRODUCT_TRANSFER_INITIATTE, param).First();
            return result;
        }

        //MBOM
        //MBOM创建新版本
        public static ProcReturnMsg ProcCreateMbomVer(BaseDbContext db, string code, string ver, DateTime dtef, DateTime dtex, string pbom_ver_guid, string desc, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code),
                new SqlParameter("@ver", ver),
                new SqlParameter("@dt_effective", dtef),
                new SqlParameter("@dt_expiry", dtex),
                new SqlParameter("@desc", desc),
                new SqlParameter("@pbom_ver_guid", pbom_ver_guid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_MBOM_VER_CREATE, param).SingleOrDefault();
            return result;
        }

        //MBOM完整性核查
        public static AppProduct ProcMbomIntegrityCheck(BaseDbContext db, string code)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code)
            };
            var result = db.Database.SqlQuery<AppProduct>(PROC_MBOM_INTEGRITY_CHECK, param).SingleOrDefault();
            return result;
        }
        //MBOM 物料看板
        public static List<IEnumerable> ProcMaterialBillboards(BaseDbContext db, string code)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code)
            };
            var result = db.MultipleResults(PROC_MBOM_INTEGRITY_CHECK, param)
                .With<ProcReturnMsg>()
                .With<ProcItem>()
                .Execute();
            return result;
        }

        //MBOM主树获取
        public static List<ProcItemTree> ProcGetMbomList(BaseDbContext db, string code)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code)
            };
            var result = db.Database.SqlQuery<ProcItemTree>(PROC_GET_MBOM_LIST, param).ToList();
            return result;
        }
        //MBOM离散区获取
        public static List<ProcItemTree> ProcDiscreteList(BaseDbContext db, string code)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code)
            };
            var result = db.Database.SqlQuery<ProcItemTree>(PROC_GET_PRODUCT_DISCRETE_LIST, param).ToList();
            return result;
        }

        #region 虚件操作
        public static ProcReturnMsg ProcVirtualItemSet(BaseDbContext db, string prodcode, Guid guid, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prodcode", prodcode),
                new SqlParameter("@guid", guid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_VIRTUAL_ITEM_SET, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg ProcVirtualItemDrop(BaseDbContext db, string prodcode, Guid guid, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prodcode", prodcode),
                new SqlParameter("@guid", guid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_VIRTUAL_ITEM_DROP, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg ProcVirtualItemLink(BaseDbContext db, string prodcode, string parentcode, Guid guid, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prodcode", prodcode),
                new SqlParameter("@parentcode", parentcode),
                new SqlParameter("@guid", guid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_VIRTUAL_ITEM_LINK, param).SingleOrDefault();
            return result;
        }

        public static ProcReturnMsg ProcVirtualItemUnlink(BaseDbContext db, string prodcode, Guid guid, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prodcode", prodcode),
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
        public static ProcReturnMsg ProcCompositeItemSet(BaseDbContext db,string prodcode, string guids, string type, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prodcode", prodcode),
                new SqlParameter("@guids", guids),
                new SqlParameter("@type", type),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_COMPOSITE_ITEM_SET, param).SingleOrDefault();
            return result;
        }
        //删除合件
        public static ProcReturnMsg ProcCompositeItemDrop(BaseDbContext db, string prodcode, Guid guid, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prodcode", prodcode),
                new SqlParameter("@guid", guid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_COMPOSITE_ITEM_DROP, param).SingleOrDefault();
            return result;
        }
        //引用合件
        public static ProcReturnMsg ProcCompositeItemLink(BaseDbContext db,string prodcode, string parentcode, Guid guid, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prodcode", prodcode),
                new SqlParameter("@parentcode", parentcode),
                new SqlParameter("@guid", guid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_COMPOSITE_ITEM_LINK, param).SingleOrDefault();
            return result;
        }
        //删除合件引用
        public static ProcReturnMsg ProcCompositeItemUnlink(BaseDbContext db,string prodcode, Guid guid, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prodcode", prodcode),
                new SqlParameter("@guid", guid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_COMPOSITE_ITEM_UNLINK, param).SingleOrDefault();
            return result;
        }
        #endregion

        //
        public static ProcReturnMsg ProcProductChangeApplyChanges(BaseDbContext db, string code, string reason, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code),
                new SqlParameter("@reason", reason),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_PRODUCT_CHANGE_APPLY_CHANGES, param).SingleOrDefault();
            return result;
        }
        //
        public static object ProcProductChangeApplyVirtualChanges(BaseDbContext db, string code, string reason, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code),
                new SqlParameter("@reason", reason),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_PRODUCT_CHANGE_APPLY_VIRTUAL_CHANGES, param).SingleOrDefault();
            return result;
        }


        public static ProcReturnMsg ProcItemDeductionSet(BaseDbContext db, string bomhids, int pvhid, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@bomhlinkids", bomhids),
                new SqlParameter("@processhlinkid", pvhid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = db.Database.SqlQuery<ProcReturnMsg>(PROC_SET_ITEM_KL, param).SingleOrDefault();
            return result;
        }
    }
}
