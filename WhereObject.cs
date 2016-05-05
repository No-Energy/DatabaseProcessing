using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseProcessing
{
    public class WhereObject
    {
        /// <summary>
        /// where条件
        /// </summary>
        private string condition = string.Empty;

        /// <summary>
        /// 获取Where条件
        /// </summary>
        /// <returns></returns>
        public string GetCondition()
        {
            return condition;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">where字段</param>
        /// <param name="whereObjectType">where条件类型</param>
        /// <param name="value">值</param>
        public WhereObject(string key, WhereObjectType whereObjectType, string value)
        {
            try
            {
                if (key.Trim().Length == 0) { throw new Exception("字段名不可为空!"); }
                if (whereObjectType == WhereObjectType.包括 || whereObjectType == WhereObjectType.不包括) { throw new Exception("条件类型与值不匹配!"); }

                condition = string.Format("{0} {1} '{2}'", key, GetType(whereObjectType), value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">where字段</param>
        /// <param name="whereObjectType">where条件类型</param>
        /// <param name="valueList">值</param>
        public WhereObject(string key, WhereObjectType whereObjectType, List<string> valueList)
        {
            try
            {
                if (key.Trim().Length == 0) { throw new Exception("字段名不可为空!"); }
                if (whereObjectType != WhereObjectType.包括 && whereObjectType != WhereObjectType.不包括) { throw new Exception("条件类型与值不匹配!"); }
                if (valueList.Count == 0) { throw new Exception("查询条件列表为空!"); }

                List<string> tempList = new List<string>();
                foreach (string v in valueList)
                {
                    tempList.Add(string.Format("'{0}'", v));
                }
                string value = string.Join(",", tempList.ToArray());

                condition = string.Format("{0} {1} ({2})", key, GetType(whereObjectType), value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取where条件
        /// </summary>
        /// <param name="whereObjectType"></param>
        /// <returns></returns>
        private string GetType(WhereObjectType whereObjectType)
        {
            switch (whereObjectType)
            {
                case WhereObjectType.等于 :
                    return "=";
                case WhereObjectType.不等于:
                    return "<>";
                case WhereObjectType.包括:
                    return "IN";
                case WhereObjectType.不包括:
                    return "NOT IN";
                case WhereObjectType.大于:
                    return ">";
                case WhereObjectType.小于:
                    return "<";
                default :
                    return "";
            }
        }
    }
}
