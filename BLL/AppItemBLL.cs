using Repository;
using System.Collections.Generic;

namespace BLL
{
    public class AppItemBLL 
    {
        public List<AppItem> GetParentList(int itemid)
        {
            return dal.SqlQuery(@"
SELECT PITEM.*
FROM TN_80_APP_0025_BOM_HLINK AS BOMH
INNER JOIN TN_80_APP_0000_ITEM AS ITEM ON ITEM.CN_ID = BOMH.CN_COMPONENT_OBJECT_ID AND ITEM.CN_ID = @p0
INNER JOIN TN_80_APP_0025_BOM AS BOM ON BOM.CN_ID = BOMH.CN_BOM_ID
INNER JOIN TN_80_APP_0000_ITEM AS PITEM ON PITEM.CN_CODE = BOM.CN_CODE
WHERE (BOMH.CN_STATUS_MBOM = 'Y' OR CN_STATUS_MBOM = '')
            ", itemid);
        }
    }
}
