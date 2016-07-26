using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseProcessing
{
    public class EntityBase 
    {
        /// <summary>
        /// 数据源
        /// </summary>
        public string source;
        /// <summary>
        /// 成员列表
        /// </summary>
        private List<string> memberList;
        /// <summary>
        /// 数据存储源
        /// </summary>
        private Dictionary<string, object> data = new Dictionary<string, object>();

        /// <summary>
        /// 数据实例
        /// </summary>
        public EntityBase()
        {
            source = "";
        }

        /// <summary>
        /// 数据实例
        /// </summary>
        /// <param name="dataSource"></param>
        public EntityBase(string dataSource)
        {
            source = dataSource;
            //存在数据源则获取成员列表
            if (source.Trim().Length > 0)
            {
                memberList = new EntityManager().GetMemberList(source);
            }
        }

        /// <summary>
        /// get;set
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get { return GetItem(key); }
            set { SetItem(key, value); }     
        }
 
        /// <summary>
        /// set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void SetItem(string key,object value)
        {
            try
            {
                if (!memberList.Equals(null) && !memberList.Contains(key)){ throw new Exception("表中无该字段!"); }

                if (data.ContainsKey(key))
                {
                    data[key] = value; 
                }
                else
                {
                    data.Add(key, value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private object GetItem(string key)
        {
            try
            {
                if (!memberList.Equals(null) && !memberList.Contains(key)) { throw new Exception("表中无该字段!"); }

                if (data.ContainsKey(key))
                {
                    return data[key];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取所有已添加列名
        /// </summary>
        /// <returns></returns>
        public List<string> GetKeys()
        {
            try
            {
                return data.Keys.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
