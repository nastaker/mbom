using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using System.Collections;
using Model;

namespace DAL
{

    public partial class PROCDAL : BaseDAL<object>
    {
        const string PROC_GET_ITEM_TREE = "PROC_GET_ITEM_TREE @code";
        const string PROC_GET_ITEM_SALESETINFO = "PROC_GET_ITEM_SETINFO @code";
        const string PROC_GET_ITEM_LIST = "PROC_GET_ITEM_LIST @code";
        const string PROC_GET_PROCESS_ITEM_LIST = "PROC_GET_PROCESS_ITEM @code";
        const string PROC_GET_ITEM_PROCESS = "PROC_GET_ITEM_PROCESS @code";
        const string PROC_GET_ITEM_CATE_LIST = "PROC_GET_ITEM_CATE_LIST @code,@catename";
        const string PROC_PRODUCT_TRANSFER_INITIATTE = "PROC_PRODUCT_TRANSFER_INITIATTE @code";
        const string PROC_MBOM_MAINTENANCE = "PROC_MBOM_MAINTENANCE @code,@userid,@name,@login";
        const string PROC_GET_MBOM_LIST = "PROC_GET_MBOM_MAINTENANCETREE @code";
        const string PROC_DISCRETE_LIST_GET = "PROC_GET_ITEM_VIRTUAL @code";

        const string PROC_VIRTUAL_ITEM_SET = "PROC_SET_ITEM_VIRTUAL @bomid,@itemid,@show,@userid,@name,@login";
        const string PROC_VIRTUAL_ITEM_DROP = "PROC_DROP_ITEM_VIRTUAL @itemid";
        const string PROC_VIRTUAL_ITEM_LINK = "PROC_LINK_VIRTUAL_ITEM @parentitemid,@itemid,@parentlink,@link,@userid,@name,@login";
        const string PROC_VIRTUAL_ITEM_UNLINK = "PROC_VIRTUAL_ITEM_UNLINK @itemid,@link";
        const string PROC_COMPOSITE_ITEM_SET = "PROC_COMPOSITE_ITEM_SET @bomid,@link,@itemids,@userid,@name,@login";
        const string PROC_COMPOSITE_ITEM_DROP = "PROC_COMPOSITE_ITEM_DROP @itemid";
        const string PROC_COMPOSITE_ITEM_LINK = "PROC_COMPOSITE_ITEM_LINK @parentitemid,@itemid,@parentlink,@link,@userid,@name,@login";
        const string PROC_COMPOSITE_ITEM_UNLINK = "PROC_COMPOSITE_ITEM_UNLINK @itemid,@bomid,@link";
        const string PROC_COMPOSITE_EDIT_NAME = "PROC_COMPOSITE_EDIT_NAME @itemid,@name";
        const string PROC_MBOM_RELEASE = "PROC_MBOM_RELEASE @code,@userid,@name,@login";
        const string PROC_SET_ITEM_KL = "PROC_SET_ITEM_KL @bomhlinkids, @processhlinkid,@userid,@name,@login";
        const string PROC_MBOM_INTEGRITY_CHECK = "PROC_MBOM_INTEGRITY_CHECK @code";
        const string PROC_USER_PROD_LIB_LINK_ADD = "PROC_USER_PROD_LIB_LINK_ADD @libid,@ids,@userid,@name,@login";
        const string PROC_GET_BOM_DIFF = "PROC_GET_BOM_DIFF @bomid";
        const string PROC_ITEM_LINK = "PROC_ITEM_LINK @pid,@plink,@itemid,@quantity,@userid,@name,@login";
        const string PROC_ITEM_UNLINK = "PROC_ITEM_UNLINK @hlinkid";
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

        public List<ProcItemTree> ProcGetItemTree(string code)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code)
            };
            var result = _context.Database.SqlQuery<ProcItemTree>(PROC_GET_ITEM_TREE, param).ToList();
            return result;
        }

        public List<ProcItemSetInfo> ProcGetItemSaleSetInfo(string code)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code)
            };
            var result = _context.Database.SqlQuery<ProcItemSetInfo>(PROC_GET_ITEM_SALESETINFO, param).ToList();
            return result;
        }

        public List<ProcItem> ProcProductList(string code)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code)
            };
            var result = _context.Database.SqlQuery<ProcItem>(PROC_GET_ITEM_LIST, param).ToList();
            return result;
        }

        public List<ProcProcessItem> ProcGetProcessItemList(string code)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code)
            };
            var result = _context.Database.SqlQuery<ProcProcessItem>(PROC_GET_PROCESS_ITEM_LIST, param).ToList();
            return result;
        }

        public List<ProcItemProcess> ProcGetItemProcess(string code)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code)
            };
            var result = _context.Database.SqlQuery<ProcItemProcess>(PROC_GET_ITEM_PROCESS, param).ToList();
            return result;
        }

        public List<ProcBomDiff> ProcGetBomDiff(int bomid)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@bomid", bomid)
            };
            var result = _context.Database.SqlQuery<ProcBomDiff>(PROC_GET_BOM_DIFF, param).ToList();
            return result;
        }

        public List<ProcCateItem> ProcGetItemCateList(string code, string catename)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code),
                new SqlParameter("@catename", catename)
            };
            var result = _context.Database.SqlQuery<ProcCateItem>(PROC_GET_ITEM_CATE_LIST, param).ToList();
            return result;
        }

        public List<ProcProductChangeDetail> ProcProductChangeDetail(string prodcode)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@prodcode", prodcode)
            };
            var result = _context.Database.SqlQuery<ProcProductChangeDetail>(PROC_PRODUCT_CHANGE_DETAIL, param).ToList();
            return result;
        }
        //BOM子件列表
        public List<AppBomHlink> ProcGetBomHlinkChildren(string itemcode)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@itemcode", itemcode)
            };
            var result = _context.Database.SqlQuery<AppBomHlink>(PROC_GET_BOM_HLINK_CHILDREN, param).ToList();
            return result;
        }

        public ProcReturnMsg ProcBomHlinkAdd(string parentitemcode, int itemid, string bywhat, UserInfo userinfo)
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
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_BOMHLINK_ADD, param).SingleOrDefault();
            return result;
        }

        public ProcReturnMsg ProcSetSaleList(string code, string str, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code),
                new SqlParameter("@str", str),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_SET_SALELIST, param).SingleOrDefault();
            return result;
        }

        public ProcReturnMsg ProcItemSetType(int itemid, int typeid, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@itemid", itemid),
                new SqlParameter("@typeid", typeid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_ITEM_SET_TYPE, param).SingleOrDefault();
            return result;
        }

        public ProcReturnMsg ProcItemTypeSwtich(int itemid, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@itemid", itemid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_ITEM_TYPE_TRANS, param).SingleOrDefault();
            return result;
        }

        public ProcReturnMsg ProcApplyBomChange(int hlinkid, string bywhat, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@hlinkid", hlinkid),
                new SqlParameter("@bywhat", bywhat),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_APPLY_BOM_CHANGE, param).SingleOrDefault();
            return result;
        }

        public ProcReturnMsg ProcDisableBomHlink(int hlinkid, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@hlinkid", hlinkid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_DISABLE_BOMHLINK, param).SingleOrDefault();
            return result;
        }

        public ProcReturnMsg ProcBomHlinkChildAdd(string parentitemcode, int itemid, int hlinkid, string bywhat, UserInfo userinfo)
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
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_BOMHLINK_CHILD_ADD, param).SingleOrDefault();
            return result;
        }

        public ProcReturnMsg ProcMbomRelease(string code, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_MBOM_RELEASE, param).SingleOrDefault();
            return result;
        }

        public ProcReturnMsg OptionalItemMapRemove(string hlinkids)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@hlinkids", hlinkids)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_OPTIONALITEM_MAP_REMOVE, param).SingleOrDefault();
            return result;
        }

        public ProcReturnMsg OptionalItemMapAdd(int itemid, string itemids, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@itemid", itemid),
                new SqlParameter("@itemids", itemids),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_OPTIONALITEM_MAP_ADD, param).SingleOrDefault();
            return result;
        }

        public ProcReturnMsg ProcSetOptionalItems(string itemids, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@itemids", itemids),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_OPTIONALITEMS_SET, param).SingleOrDefault();
            return result;
        }

        public ProcReturnMsg ProcItemUnLink(int hlinkid, float quantity)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@hlinkid", hlinkid),
                new SqlParameter("@quantity", quantity)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_ITEM_EDITLINKQUANTITY, param).SingleOrDefault();
            return result;
        }

        public ProcReturnMsg ProcItemUnLink(int hlinkid)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@hlinkid", hlinkid)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_ITEM_UNLINK, param).SingleOrDefault();
            return result;
        }

        public ProcReturnMsg ProcItemLink(int pid, string plink, int itemid, float quantity, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@pid", pid),
                new SqlParameter("@plink", plink),
                new SqlParameter("@itemid", itemid),
                new SqlParameter("@quantity", quantity),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_ITEM_LINK, param).SingleOrDefault();
            return result;
        }

        public ProcReturnMsg ProcUserProductLibraryLinkAdd(int libid, string ids, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@libid", libid),
                new SqlParameter("@ids", ids),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_USER_PROD_LIB_LINK_ADD, param).SingleOrDefault();
            return result;
        }

        public ProcReturnMsg ProcEditCombineName(int itemid, string name)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@itemid", itemid),
                new SqlParameter("@name", name)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_COMPOSITE_EDIT_NAME, param).SingleOrDefault();
            return result;
        }

        public ProcReturnMsg ProcProductTransferInitiate(string code)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_PRODUCT_TRANSFER_INITIATTE, param).First();
            return result;
        }

        //MBOM
        //MBOM维护功能进入检查
        public ProcReturnMsg ProcMbomMaintenance(string code, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_MBOM_MAINTENANCE, param).SingleOrDefault();
            return result;
        }

        //MBOM完整性核查
        public AppProduct ProcMbomIntegrityCheck(string code)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code)
            };
            var result = _context.Database.SqlQuery<AppProduct>(PROC_MBOM_INTEGRITY_CHECK, param).SingleOrDefault();
            return result;
        }
        //MBOM 物料看板
        public List<IEnumerable> ProcMaterialBillboards(string code)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code)
            };
            var result = _context.MultipleResults(PROC_MBOM_INTEGRITY_CHECK, param)
                .With<ProcReturnMsg>()
                .With<ProcItem>()
                .Execute();
            return result;
        }

        //MBOM主树获取
        public List<ProcItemTree> ProcGetMbomList(string code)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code)
            };
            var result = _context.Database.SqlQuery<ProcItemTree>(PROC_GET_MBOM_LIST, param).ToList();
            return result;
        }
        //MBOM离散区获取
        public List<ProcItemTree> ProcDiscreteList(string code)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@code", code)
            };
            var result = _context.Database.SqlQuery<ProcItemTree>(PROC_DISCRETE_LIST_GET, param).ToList();
            return result;
        }

        #region 虚件操作
        public ProcReturnMsg ProcVirtualItemSet(int bomid, int itemid, int show, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@bomid", bomid),
                new SqlParameter("@itemid", itemid),
                new SqlParameter("@show", show),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_VIRTUAL_ITEM_SET, param).SingleOrDefault();
            return result;
        }

        public ProcReturnMsg ProcVirtualItemDrop(int itemid)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@itemid", itemid)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_VIRTUAL_ITEM_DROP, param).SingleOrDefault();
            return result;
        }

        public ProcReturnMsg ProcVirtualItemLink(int parentitemid, int itemid, string parentlink, string link, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@parentitemid", parentitemid),
                new SqlParameter("@itemid", itemid),
                new SqlParameter("@parentlink", parentlink),
                new SqlParameter("@link", link),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_VIRTUAL_ITEM_LINK, param).SingleOrDefault();
            return result;
        }

        public ProcReturnMsg ProcVirtualItemUnlink(int itemid, string link)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@itemid", itemid),
                new SqlParameter("@link", link)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_VIRTUAL_ITEM_UNLINK, param).SingleOrDefault();
            return result;
        }
        #endregion
        #region 合件
        //设置合件
        public ProcReturnMsg ProcCompositeItemSet(int bomid, string link, string itemids, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@bomid", bomid),
                new SqlParameter("@link", link),
                new SqlParameter("@itemids", itemids),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_COMPOSITE_ITEM_SET, param).SingleOrDefault();
            return result;
        }
        //删除合件
        public ProcReturnMsg ProcCompositeItemDrop(int itemid)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@itemid", itemid)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_COMPOSITE_ITEM_DROP, param).SingleOrDefault();
            return result;
        }
        //引用合件
        public ProcReturnMsg ProcCompositeItemLink(int parentitemid, int itemid, string parentlink, string link, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@parentitemid", parentitemid),
                new SqlParameter("@itemid", itemid),
                new SqlParameter("@parentlink", parentlink),
                new SqlParameter("@link", link),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_COMPOSITE_ITEM_LINK, param).SingleOrDefault();
            return result;
        }
        //删除合件引用
        public ProcReturnMsg ProcCompositeItemUnlink(int itemid, int bomid, string link)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@itemid", itemid),
                new SqlParameter("@bomid", bomid),
                new SqlParameter("@link", link)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_COMPOSITE_ITEM_UNLINK, param).SingleOrDefault();
            return result;
        }
        #endregion
        public ProcReturnMsg ProcItemDeductionSet(string bomhids, int pvhid, UserInfo userinfo)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@bomhlinkids", bomhids),
                new SqlParameter("@processhlinkid", pvhid),
                new SqlParameter("@userid", userinfo.UserId),
                new SqlParameter("@name", userinfo.Name),
                new SqlParameter("@login", userinfo.Login)
            };
            var result = _context.Database.SqlQuery<ProcReturnMsg>(PROC_SET_ITEM_KL, param).SingleOrDefault();
            return result;
        }
    }
}
