using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseProcessing
{
    public class WhereObjectList
    {
        /// <summary>
        /// where语句
        /// </summary>
        public string where = string.Empty;

        /// <summary>
        /// 增加where条件
        /// </summary>
        /// <param name="whereObject">where条件</param>
        public void add(WhereObject whereObject)
        {
            try
            {
                if (where.Length == 0)
                {
                    where += " WHERE " + whereObject.GetCondition();
                }
                else
                {
                    where += " AND " + whereObject.GetCondition();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 增加where条件
        /// </summary>
        /// <param name="key">where字段</param>
        /// <param name="whereObjectType">where条件类型</param>
        /// <param name="value">值</param>
        public void add(string key, WhereObjectType whereObjectType, string value)
        {
            try
            {
                add(new WhereObject(key, whereObjectType, value));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 增加where条件
        /// </summary>
        /// <param name="key">where字段</param>
        /// <param name="whereObjectType">where条件类型</param>
        /// <param name="valueList">值</param>
        public void add(string key, WhereObjectType whereObjectType, List<string> valueList)
        {
            try
            {
                add(new WhereObject(key, whereObjectType, valueList));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 增加where条件OR
        /// </summary>
        /// <param name="whereObject1">where条件1</param>
        /// <param name="whereObject2">where条件2</param>
        public void addOr(WhereObject whereObject1, WhereObject whereObject2)
        {
            try
            {
                if (where.Length == 0)
                {
                    where += " WHERE (" + whereObject1.GetCondition() + " OR " + whereObject2.GetCondition() + ")";
                }
                else
                {
                    where += " AND (" + whereObject1.GetCondition() + " OR " + whereObject2.GetCondition() + ")";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 增加多个where条件OR
        /// </summary>
        /// <param name="listWhereObject">where条件List集合</param>
        public void addMoreOr(List<WhereObject> listWhereObject)
        {
            try
            {
                if (listWhereObject.Equals(null)) { throw new Exception("请输入有效的参数集合!"); }
                if (listWhereObject.Count == 0) {throw new Exception("请输入有效的参数集合!");}
                if (where.Length == 0)
                {
                    where += "WHERE (";
                    for (int i = 0; i < listWhereObject.Count - 1; i++)
                    {
                        where += listWhereObject[i].GetCondition();
                        where += " OR ";
                    }
                    where += listWhereObject.Last().GetCondition();
                    where += ")";
                }
                else
                {
                    where += "AND (";
                    for (int i = 0; i < listWhereObject.Count - 1; i++)
                    {
                        where += listWhereObject[i].GetCondition();
                        where += " OR ";
                    }
                    where += listWhereObject.Last().GetCondition();
                    where += ")";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
