using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseProcessing
{
    public class TotalObject
    {
        /// <summary>
        /// 选择字段
        /// </summary>
        private string select = string.Empty;

        /// <summary>
        /// Group By字段
        /// </summary>
        private string groupby = string.Empty;

        /// <summary>
        /// 获取选择字段
        /// </summary>
        /// <returns></returns>
        public string GetSelect()
        {
            return select;
        }

        /// <summary>
        /// 获取Group By字段
        /// </summary>
        /// <returns></returns>
        public string GetGroupby()
        {
            return groupby;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">total字段</param>
        /// <param name="totalObjectType">total条件类型</param>
        /// <param name="value">显示名称</param>
        public TotalObject(string key, TotalObjectType totalObjectType, string value)
        {
            try
            {
                if (key.Trim().Length == 0) { throw new Exception("字段名不可为空!"); }
                if (value.Trim().Length == 0) { throw new Exception("显示名不可为空!"); }

                if (totalObjectType == TotalObjectType.GroupBy) { groupby = key; }

                select = string.Format(GetType(totalObjectType), key, value);          
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取total语句
        /// </summary>
        /// <param name="totalObjectType">total条件类型</param>
        /// <returns></returns>
        private string GetType(TotalObjectType totalObjectType)
        {
            switch (totalObjectType)
            {
                case TotalObjectType.GroupBy:
                    return "{0} as '{1}'";
                case TotalObjectType.Sum:
                    return "sum({0}) as '{1}'";
                case TotalObjectType.Max:
                    return "max({0}) as '{1}'";
                case TotalObjectType.Min:
                    return "min({0}) as '{1}'";
                case TotalObjectType.Avg:
                    return "avg({0}) as '{1}'";
                case TotalObjectType.Count:
                    return "count({0}) as '{1}'";
                default :
                    return "";
            }

        }
    }
}
