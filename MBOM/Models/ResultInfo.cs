using AutoMapper;
using Repository;
using Localization;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MBOM.Models
{
    public class ResultInfo
    {
        public bool success;
        public string msg;
        public object data;

        public static ResultInfo Fail(string msg)
        {
            return new ResultInfo
            {
                success = false,
                msg = msg
            };
        }

        internal static ResultInfo Success()
        {
            return new ResultInfo
            {
                success = true
            };
        }

        internal static ResultInfo Success(object data)
        {
            return new ResultInfo
            {
                success = true,
                data = data
            };
        }

        internal static ResultInfo Success(string msg)
        {
            return new ResultInfo
            {
                success = true,
                msg = msg
            };
        }

        internal static ResultInfo Success(string msg, object data)
        {
            return new ResultInfo
            {
                success = true,
                msg = msg,
                data = data
            };
        }

        internal static ResultInfo Parse(bool success, string msg, object data)
        {
            return new ResultInfo
            {
                success = success,
                msg = msg,
                data = data
            };
        }

        internal static ResultInfo Parse(ProcReturnMsg procReturnMsg)
        {
            return new ResultInfo
            {
                success = procReturnMsg.success,
                msg = procReturnMsg.msg
            };
        }

        internal static ResultInfo Parse(bool success, string msg)
        {
            return new ResultInfo
            {
                success = success,
                msg = msg
            };
        }

        internal static object Parse(List<IEnumerable> list, Type source, Type dest)
        {
            if (list.Count != 2)
            {
                return Fail(Lang.ParamIsEmpty);
            }
            var rts = list[0] as List<ProcReturnMsg>;
            if (rts == null || rts.Count == 0)
            {
                return Fail(Lang.ConvertDtoFail);
            }
            return new ResultInfo
            {
                msg = rts[0].msg,
                success = rts[0].success,
                data = Mapper.Map(list[1], source, dest)
            };
        }

        internal static ResultInfo Parse(List<IEnumerable> list)
        {
            if (list.Count != 2)
            {
                return Fail(Lang.ParamIsEmpty);
            }
            var rts = list[0] as List<ProcReturnMsg>;
            if (rts == null || rts.Count == 0)
            {
                return Fail(Lang.ConvertDtoFail);
            }
            return new ResultInfo
            {
                msg = rts[0].msg,
                success = rts[0].success,
                data = list[1]
            };
        }
    }

    public class Pager
    {
        public int total { get; set; }
        public int page { get; set; }
        public object data { get; set; }
    }
}