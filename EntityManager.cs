using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Xml;

namespace DatabaseProcessing
{

    public class EntityManager
    {

        //string SQLConnStr;
        ///// <summary>
        ///// 数据源
        ///// </summary>
        //string DataSource;
        ///// <summary>
        ///// 数据库
        ///// </summary>
        //string Database;
        ///// <summary>
        ///// 用户名
        ///// </summary>
        //string Uid;
        ///// <summary>
        ///// 密码
        ///// </summary>
        //string Pwd;

        public EntityManager()
        {
            try
            {
                //ReadLogInMsg();
                //SQLConnStr = string.Format("Data Source={0};Database={1};Uid={2};Pwd={3}", DataSource, Database, Uid, Pwd);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>'
        /// 获取成员列表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public List<string> GetMemberList(string tableName)
        {
            try
            {
                SqlConnection conn = ConnectInfo.GetConnection();
                conn.Open();
                //通过数据库查询语句限制仅仅获取列名
                SqlCommand cmd = new SqlCommand(string.Format("select * from {0} where 1=0", tableName), conn);
                //SqlDataReader dr = cmd.ExecuteReader();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<string> tableColumnNameList = new List<string>();

                foreach (DataColumn dc in dt.Columns)
                {
                    tableColumnNameList.Add(dc.ColumnName);
                }

                dt.Dispose();
                da.Dispose();
                conn.Close();
                conn.Dispose();

                return tableColumnNameList; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取不可为空字段列表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        private List<string> GetNotAllowNullList(string tableName)
        {
            try
            {
                SqlConnection conn = ConnectInfo.GetConnection();
                conn.Open();
                //通过数据库查询语句限制仅仅获取列名
                SqlCommand cmd = new SqlCommand(string.Format("SELECT COLUMN_NAME,IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS col inner join sysobjects tbs ON tbs.name=col.TABLE_NAME WHERE tbs.name='{0}'", tableName), conn);
                //SqlDataReader dr = cmd.ExecuteReader();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<string> notAllowNullList = new List<string>();

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["IS_NULLABLE"].Equals("NO"))
                    {
                        notAllowNullList.Add(dr["COLUMN_NAME"].ToString());
                    }
                }

                dt.Dispose();
                da.Dispose();
                conn.Close();
                conn.Dispose();

                return notAllowNullList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取主键列表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        private List<string> GetPrimaryKeyList(string tableName)
        {
            try
            {
                SqlConnection conn = ConnectInfo.GetConnection();
                conn.Open();
                //通过数据库查询语句限制仅仅获取列名
                SqlCommand cmd = new SqlCommand(string.Format("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME='{0}'", tableName), conn);
                //SqlDataReader dr = cmd.ExecuteReader();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                List<string> primaryKeyList = new List<string>();

                foreach (DataRow dr in dt.Rows)
                {
                    primaryKeyList.Add(dr["COLUMN_NAME"].ToString());
                }

                dt.Dispose();
                da.Dispose();
                conn.Close();
                conn.Dispose();

                return primaryKeyList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <param name="dataNum">选取数据数量,0为无限制</param>
        /// <param name="orderColumn">排序字段</param>
        /// <returns></returns>
        public DataTable GetTable(EntityBase entity, int dataNum, string orderColumn)
        {
            try
            {
                string topStr = dataNum == 0 ? "" : string.Format("TOP{0}", dataNum);
                string sqlQueryStr = string.Format("select {0} * from {1}", topStr, entity.source);
                if (orderColumn.Trim().Length > 0) { sqlQueryStr += string.Format(" order by {0}", orderColumn); }

                SqlConnection conn = ConnectInfo.GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlQueryStr, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                da.Dispose();
                conn.Close();
                conn.Dispose();

                return dt;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 获取数据表(where条件)
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <param name="whereObjectList">where对象集合</param>
        /// <param name="dataNum">选取数据数量,0为无限制</param>
        /// <param name="orderColumn">排序字段</param>
        /// <returns></returns>
        public DataTable GetTableEx(EntityBase entity, WhereObjectList whereObjectList, int dataNum, string orderColumn)
        {
            try
            {
                string topStr = dataNum == 0 ? "" : string.Format("TOP{0}", dataNum);
                string sqlQueryStr = string.Format("select {0} * from {1} {2}", topStr, entity.source, whereObjectList.where);
                if (orderColumn.Trim().Length > 0) { sqlQueryStr += string.Format(" order by {0}", orderColumn); }

                SqlConnection conn = ConnectInfo.GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlQueryStr, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                da.Dispose();
                conn.Close();
                conn.Dispose();

                return dt;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 获取数据表(total条件)
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <param name="totalObjectList">total条件集合</param>
        /// <param name="dataNum">选取数据数量,0为无限制</param>
        /// <param name="orderColumn">排序字段</param>
        /// <returns></returns>
        public DataTable GetTableEx(EntityBase entity,TotalObjectList totalObjectList, int dataNum, string orderColumn)
        {
            try
            {
                if (totalObjectList.GetGroup().Length == 0) { throw new Exception("没有包含 GROUP BY 子句!"); }

                string topStr = dataNum == 0 ? "" : string.Format("TOP{0}", dataNum);
                string sqlQueryStr = string.Format("select {0} {1} from {1} group by {2}", topStr, totalObjectList.GetTotal(), entity.source, totalObjectList.GetGroup());
                if (orderColumn.Trim().Length > 0) { sqlQueryStr += string.Format(" order by {0}", orderColumn); }

                SqlConnection conn = ConnectInfo.GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlQueryStr, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                da.Dispose();
                conn.Close();
                conn.Dispose();

                return dt;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 获取数据表(where\total条件)
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <param name="totalObjectList">total对象集合</param>
        /// <param name="whereObjectList">where对象集合</param>
        /// <param name="dataNum">选取数据数量,0为无限制</param>
        /// <param name="orderColumn">排序字段</param>
        /// <returns></returns>
        public DataTable GetTableEx(EntityBase entity, TotalObjectList totalObjectList, WhereObjectList whereObjectList, int dataNum, string orderColumn)
        {
            try
            {
                if (totalObjectList.GetGroup().Length == 0) { throw new Exception("没有包含 GROUP BY 子句!"); }

                string topStr = dataNum == 0 ? "" : string.Format("TOP{0}", dataNum);
                string sqlQueryStr = string.Format("select {0} {1} from {2} {3} group by {4}", topStr, totalObjectList.GetTotal(), entity.source, whereObjectList.where, totalObjectList.GetGroup());
                if (orderColumn.Trim().Length > 0) { sqlQueryStr += string.Format(" order by {0}", orderColumn); }

                SqlConnection conn = ConnectInfo.GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlQueryStr, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                
                da.Dispose();
                conn.Close();
                conn.Dispose();

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="entityBase">数据实体</param>
        /// <returns></returns>
        public bool GetEntity(ref EntityBase entityBase)
        {
            try
            {
                string sqlQueryStr = string.Format("select * from {0}", entityBase.source);

                List<string> primarykeymember = GetPrimaryKeyList(entityBase.source);
                
                if (primarykeymember.Count == 0) { throw new Exception("Table [" + entityBase.source + "] has no Primary Key!"); }

                bool firstCondition = true;
                string where = " where ";
                foreach (string wheremember in primarykeymember)
                {
                    if (!firstCondition) { where += " and "; firstCondition = false; }
                    if (entityBase[wheremember] == null) { throw new Exception("Primary Key [" + wheremember + "] not assign!"); }
                    where += wheremember + "='" + entityBase[wheremember] + "'";
                }
                sqlQueryStr += where;

                SqlConnection conn = ConnectInfo.GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlQueryStr, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    List<string> memberList = GetMemberList(entityBase.source);
                    foreach (string member in memberList)
                    {
                        entityBase[member] = dr[member];
                    }
                    dr.Close();
                    conn.Close();
                    dr.Dispose();
                    conn.Dispose();
                    return true;
                }
                else
                {
                    dr.Close();
                    conn.Close();
                    dr.Dispose();
                    conn.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取数据(where条件)
        /// </summary>
        /// <param name="entityBase">数据实体</param>
        /// <param name="whereObjectList">where对象集合</param>
        /// <param name="orderColumn">排序字段</param>
        /// <returns></returns>
        public bool GetEntityEx(ref EntityBase entityBase, WhereObjectList whereObjectList, string orderColumn)
        {
            try
            {
                string sqlQueryStr = string.Format("select * from {0} {1}", entityBase.source, whereObjectList.where);

                if (orderColumn.Trim().Length > 0) { sqlQueryStr += string.Format(" order by {0}", orderColumn); }

                SqlConnection conn = ConnectInfo.GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlQueryStr, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                               
                if (dr.Read())
                {
                    List<string> memberList = GetMemberList(entityBase.source);
                    foreach (string member in memberList)
                    {
                        entityBase[member] = dr[member];
                    }
                    dr.Close(); 
                    conn.Close();
                    dr.Dispose();
                    conn.Dispose();
                    return true;
                }
                else
                {
                    dr.Close();
                    conn.Close();
                    dr.Dispose();
                    conn.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取数据(total\where条件)
        /// </summary>
        /// <param name="entityBase">数据实体</param>
        /// <param name="totalObjectList">total对象集合</param>
        /// <param name="whereObjectList">where对象集合</param>
        /// <param name="orderColumn">排序字段</param>
        /// <returns></returns>
        public bool GetEntityEx(ref EntityBase entityBase, TotalObjectList totalObjectList, WhereObjectList whereObjectList, string orderColumn) 
        {
            try
            {
                if (totalObjectList.GetGroup().Length == 0) { throw new Exception("没有包含 GROUP BY 子句!"); }

                string sqlQueryStr = string.Format("select {0} from {1} {2} group by {3}", totalObjectList.GetTotal(), entityBase.source, whereObjectList.where, totalObjectList.GetGroup());

                if (orderColumn.Trim().Length > 0) { sqlQueryStr += string.Format(" order by {0}", orderColumn); }

                SqlConnection conn = ConnectInfo.GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlQueryStr, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    List<string> memberList = new List<string>();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        memberList.Add(dr.GetName(i));
                    }

                    foreach (string member in memberList)
                    {
                        entityBase[member] = dr[member];
                    }
                    dr.Close();
                    conn.Close();
                    dr.Dispose();
                    conn.Dispose();
                    return true;
                }
                else
                {
                    dr.Close();
                    conn.Close();
                    dr.Dispose();
                    conn.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entityBase">数据实体</param>
        public void DeleteEntity(EntityBase entityBase)
        {
            try
            {
                string sqlQueryStr = string.Format("delete from {0}", entityBase.source);

                List<string> primarykeymember = GetPrimaryKeyList(entityBase.source);

                if (primarykeymember.Count == 0) { throw new Exception("Table [" + entityBase.source + "] has no Primary Key!"); }

                bool firstCondition = true;
                string where = " where ";
                foreach (string wheremember in primarykeymember)
                {
                    if (!firstCondition) { where += " and "; firstCondition = false; }
                    if (entityBase[wheremember] == null) { throw new Exception("Primary Key [" + wheremember + "] not assign!"); }
                    where += wheremember + "='" + entityBase[wheremember] + "'";
                }
                sqlQueryStr += where;

                SqlConnection conn = ConnectInfo.GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlQueryStr, conn);
                SqlDataReader dr = cmd.ExecuteReader();
               
                conn.Close();
                conn.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除数据(where条件)
        /// </summary>
        /// <param name="entityBase">数据实体</param>
        /// <param name="whereObjectList">where条件集合</param>
        public void DeleteEntityEx(EntityBase entityBase, WhereObjectList whereObjectList)
        {
            try
            {
                string sqlQueryStr = string.Format("delete from {0} {1}", entityBase.source, whereObjectList.where);

                SqlConnection conn = ConnectInfo.GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlQueryStr, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                
                conn.Close();
                conn.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="entityBase">数据实体</param>
        public void AddNewEntity(EntityBase entityBase)
        {
            try
            {
                string sqlQueryStr = string.Format("insert into {0}", entityBase.source);

                List<string> memberlist = GetMemberList(entityBase.source);
                List<string> notallownullmember = GetNotAllowNullList(entityBase.source);

                string cols = "";
                string values = "";
                List<string> collist = new List<string>();
                List<string> valuelist = new List<string>();

                foreach (string member in memberlist)
                {
                    object value = entityBase[member];
                    if (notallownullmember.Contains(member) && value == null) { throw new Exception("Key [" + member + "] is Not Allow Null!"); }
                    if (value != null)
                    {
                        collist.Add(member);
                        valuelist.Add(string.Format("'{0}'", value.ToString()));
                    }
                }

                cols = string.Format("({0})", string.Join(",", collist.ToArray()));
                values = string.Format("({0})", string.Join(",", valuelist.ToArray()));

                sqlQueryStr = string.Format("{0}{1} values{2}", sqlQueryStr, cols, values);

                SqlConnection conn = ConnectInfo.GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlQueryStr, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                conn.Close();
                conn.Dispose();                                         
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entityBase">数据实体</param>
        public void UpdateEntity(EntityBase entityBase)
        {
            try
            {
                string sqlQueryStr = string.Format("update {0}", entityBase.source);

                List<string> primarykeymember = GetPrimaryKeyList(entityBase.source);
                List<string> keylist = entityBase.GetKeys();

                if (primarykeymember.Count == 0) { throw new Exception("Table [" + entityBase.source + "] has no Primary Key!"); }

                bool firstCondition = true;
                string where = " where ";
                foreach (string primarykey in primarykeymember)
                {
                    if (!firstCondition) { where += " and "; firstCondition = false; }
                    if (entityBase[primarykey] == null) { throw new Exception("Primary Key [" + primarykey + "] not assign!"); }
                    where += primarykey + "='" + entityBase[primarykey] + "'";
                    keylist.Remove(primarykey);
                }

               
                string updatestr;
                if (keylist.Count > 0) { updatestr = " set"; }
                else { throw new Exception("No data will be updated!"); }

                List<string> templist = new List<string>();
                foreach (string key in keylist)
                {
                    object value = entityBase[key];
                    if (value != null)
                    {
                        templist.Add(string.Format(" {0} = '{1}'", key, value));
                    }
                }
                updatestr += string.Join(",", templist.ToArray());

                sqlQueryStr += updatestr + where;

                SqlConnection conn = ConnectInfo.GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlQueryStr, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                conn.Close();
                conn.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新数据(where条件)
        /// </summary>
        /// <param name="entityBase">数据实体</param>
        /// <param name="whereObjectList">where条件集合</param>
        public void UpdateEntityEx(EntityBase entityBase, WhereObjectList whereObjectList)
        {
            try
            {
                string sqlQueryStr = string.Format("update {0}", entityBase.source);

                List<string> keylist = entityBase.GetKeys();

                string updatestr;
                if (keylist.Count > 0) { updatestr = " set"; }
                else { throw new Exception("No data will be updated!"); }

                List<string> templist = new List<string>();
                foreach (string key in keylist)
                {
                    object value = entityBase[key];
                    if (value != null)
                    {
                        templist.Add(string.Format(" {0} = '{1}'", key, value));
                    }
                }
                updatestr += string.Join(",", templist.ToArray());

                sqlQueryStr += updatestr + whereObjectList .where;

                SqlConnection conn = ConnectInfo.GetConnection();
                conn.Open();
                SqlCommand cmd = new SqlCommand(sqlQueryStr, conn);
                SqlDataReader dr = cmd.ExecuteReader();

                conn.Close();
                conn.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
