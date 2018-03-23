using DAL;
using DAL.Models;
using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PROCBLL : BaseBLL<object>
    {
        new PROCDAL dal = new PROCDAL();

        public List<ProcItemTree> ProcGetItemTree(string code)
        {
            return dal.ProcGetItemTree(code);
        }

        public List<ProcItemSetInfo> ProcGetItemSaleSetInfo(string code)
        {
            return dal.ProcGetItemSaleSetInfo(code);
        }

        public List<ProcItem> ProcGetItemList(string code)
        {
            return dal.ProcProductList(code);
        }

        public List<ProcProcessItem> ProcGetProcessItemList(string code)
        {
            return dal.ProcGetProcessItemList(code);
        }

        public List<ProcItemProcess> ProcGetItemProcess(string code)
        {
            return dal.ProcGetItemProcess(code);
        }

        public List<ProcBomDiff> ProcGetBomDiff(int bomid)
        {
            return dal.ProcGetBomDiff(bomid);
        }

        public List<ProcCateItem> ProcGetItemCateList(string code, string catename)
        {
            return dal.ProcGetItemCateList(code, catename);
        }

        public List<ProcProductChangeDetail> ProcProductChangeDetail(string prodcode)
        {
            return dal.ProcProductChangeDetail(prodcode);
        }

        public List<AppBomHlink> ProcGetBomHlinkChildren(string itemcode)
        {
            return dal.ProcGetBomHlinkChildren(itemcode);
        }

        public ProcReturnMsg ProcProductTransferInitiate(string code)
        {
            return dal.ProcProductTransferInitiate(code);
        }

        public int SetItemDisabled(IEnumerable<int> removeList)
        {
            string removestr = null;
            if(removeList != null)
            {
                removestr = string.Join(",", removeList);
            }
            return dal.ProcSetItemHLDisabled(removestr);
        }

        public List<IEnumerable> ProcMaterialBillboards(string code)
        {
            return dal.ProcMaterialBillboards(code);
        }

        public int SetProductSaleSet(string code)
        {
            return dal.ProcProductSaleSet(code);
        }
        //MBOM维护进入检查
        public ProcReturnMsg ProcMbomMaintenance(string code, UserInfo userinfo)
        {
            return dal.ProcMbomMaintenance(code, userinfo);
        }

        // MBOM完整性核查
        public AppProduct ProcMbomIntegrityCheck(string code)
        {
            return dal.ProcMbomIntegrityCheck(code);
        }

        //MBOM主树获取
        public List<ProcItemTree> ProcGetMbomList(string code)
        {
            return dal.ProcGetMbomList(code);
        }
        //MBOM离散区获取
        public List<ProcItemTree> ProcDiscreteList(string code)
        {
            return dal.ProcDiscreteList(code);
        }
        //MBOM虚件设置
        public ProcReturnMsg ProcVirtualItemSet(int bomid, int itemid, UserInfo userinfo)
        {
            return dal.ProcVirtualItemSet(bomid, itemid, userinfo);
        }
        //MBOM取消虚件
        public ProcReturnMsg ProcVirtualItemDrop(int itemid)
        {
            return dal.ProcVirtualItemDrop(itemid);
        }
        //MBOM引用虚件
        public ProcReturnMsg ProcVirtualItemLink(int parentitemid, int itemid, string parentlink, string link, UserInfo userinfo)
        {
            return dal.ProcVirtualItemLink(parentitemid, itemid, parentlink, link, userinfo);
        }
        //MBOM取消引用虚件
        public ProcReturnMsg ProcVirtualItemUnlink(int itemid, string link)
        {
            return dal.ProcVirtualItemUnlink(itemid, link);
        }
        //MBOM设置为合件
        public ProcReturnMsg ProcCompositeItemSet(int bomid, string link, string itemids, UserInfo userinfo)
        {
            return dal.ProcCompositeItemSet(bomid, link, itemids, userinfo);
        }
        //MBOM删除合件
        public ProcReturnMsg ProcCompositeItemDrop(int itemid)
        {
            return dal.ProcCompositeItemDrop(itemid);
        }
        //MBOM合件引用
        public ProcReturnMsg ProcCompositeItemLink(int parentitemid, int itemid, string parentlink, string link, UserInfo userinfo)
        {
            return dal.ProcCompositeItemLink(parentitemid, itemid, parentlink, link, userinfo);
        }
        //MBOM合件取消引用
        public ProcReturnMsg ProcCompositeItemUnlink(int itemid, int bomid, string link)
        {
            return dal.ProcCompositeItemUnlink(itemid, bomid, link);
        }

        public ProcReturnMsg ProcItemDeductionSet(string bomhids, int pvhid, UserInfo userinfo)
        {
            return dal.ProcItemDeductionSet(bomhids, pvhid, userinfo);
        }

        public ProcReturnMsg ProcMbomRelease(string code, UserInfo userinfo)
        {
            return dal.ProcMbomRelease(code, userinfo);
        }

        public ProcReturnMsg ProcEditCombineName(int itemid, string name)
        {
            return dal.ProcEditCombineName(itemid, name);
        }

        public ProcReturnMsg ProcUserProductLibraryLinkAdd(int libid, string ids, UserInfo userinfo)
        {
            return dal.ProcUserProductLibraryLinkAdd(libid, ids, userinfo);
        }

        public ProcReturnMsg ProcItemLink(int pid, string plink, int itemid, float quantity, UserInfo userinfo)
        {
            return dal.ProcItemLink(pid, plink, itemid, quantity, userinfo);
        }

        public ProcReturnMsg ProcItemUnLink(int hlinkid)
        {
            return dal.ProcItemUnLink(hlinkid);
        }

        public ProcReturnMsg ProcItemEditQuantity(int hlinkid, float quantity)
        {
            return dal.ProcItemUnLink(hlinkid, quantity);
        }

        public ProcReturnMsg ProcSetOptionalItems(string itemids, UserInfo userInfo)
        {
            return dal.ProcSetOptionalItems(itemids, userInfo);
        }

        public ProcReturnMsg OptionalItemMapAdd(int itemid, string itemids, UserInfo userInfo)
        {
            return dal.OptionalItemMapAdd(itemid, itemids, userInfo); 
        }

        public ProcReturnMsg OptionalItemMapRemove(string hlinkids)
        {
            return dal.OptionalItemMapRemove(hlinkids);
        }

        public ProcReturnMsg ProcBomHlinkChildAdd(string parentitemcode, int itemid, int hlinkid, string bywhat, UserInfo userInfo)
        {
            return dal.ProcBomHlinkChildAdd(parentitemcode, itemid, hlinkid, bywhat, userInfo);
        }

        public ProcReturnMsg ProcBomHlinkAdd(string parentitemcode, int itemid, string bywhat, UserInfo userInfo)
        {
            return dal.ProcBomHlinkAdd(parentitemcode, itemid, bywhat, userInfo);
        }

        public ProcReturnMsg ProcDisableBomHlink(int hlinkid, UserInfo userInfo)
        {
            return dal.ProcDisableBomHlink(hlinkid, userInfo);
        }

        public ProcReturnMsg ProcApplyBomChange(int hlinkid, string bywhat, UserInfo userInfo)
        {
            return dal.ProcApplyBomChange(hlinkid, bywhat, userInfo);
        }

        public ProcReturnMsg ProcItemTypeTrans(int itemid, UserInfo userInfo)
        {
            return dal.ProcItemTypeTrans(itemid, userInfo);
        }
    }
}
