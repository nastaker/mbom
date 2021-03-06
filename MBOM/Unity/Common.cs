﻿using System.Linq;
using MBOM.Models;
using Model;
using Repository;

namespace MBOM.Unity
{
    public class Common
    {
        internal static IQueryable<T> GetQueryFilter<T>(IQueryable<T> query) where T : GroupEntity
        {
            var user = LoginUserInfo.GetLoginUser();
            if (user.UserId > 0)
            {
                return from x
                       in query
                       where x.GROUPCODE == user.groupname
                       select x;
            }
            return query;
        }
        internal static IQueryable<ViewProjectProductPbom> GetQueryFilterUserId(IQueryable<ViewProjectProductPbom> query)
        {
            UserInfo user = LoginUserInfo.GetUserInfo();
            if (user.UserId > 0)
            {
                return from x
                       in query
                       where x.OWNER_ID == user.UserId
                       select x;
            }
            return query;
        }
    }
}