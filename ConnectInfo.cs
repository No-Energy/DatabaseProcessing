using System.Data.SqlClient;
using System.Xml;
using System;

namespace DatabaseProcessing
{
    static public class ConnectInfo
    {
        /// <summary>
        /// 数据源
        /// </summary>
        static string DataSource = string.Empty;
        /// <summary>
        /// 数据库
        /// </summary>
        static string Database = string.Empty;
        /// <summary>
        /// 用户名
        /// </summary>
        static string Uid = string.Empty;
        /// <summary>
        /// 密码
        /// </summary>
        static string Pwd = string.Empty;

        /// <summary>
        /// 获取配置
        /// </summary>
        static void ReadLogInMsg()
        {
            try
            {
                string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

                XmlDocument doc = new XmlDocument();
                doc.Load(path + "config.config");

                XmlNodeList Node = doc.SelectSingleNode("config").ChildNodes;

                for (int n = 0; n < Node.Count; n++)
                {
                    XmlNodeList cNode = Node.Item(n).ChildNodes;
                    int nodeNum = cNode.Count;

                    if (Node.Item(n).Name.Equals("Database"))
                    {
                        for (int i = 0; i < nodeNum; i++)
                        {
                            try
                            {
                                if (cNode[i].Name.Equals("DataSource")) { DataSource = cNode[i].InnerXml; }
                                if (cNode[i].Name.Equals("Database")) { Database = cNode[i].InnerXml; }
                            }
                            catch
                            {
                                throw new Exception("数据库信息配置数值错误！");
                            }
                        }
                    }

                    if (Node.Item(n).Name.Equals("LogIn"))
                    {
                        for (int i = 0; i < nodeNum; i++)
                        {
                            try
                            {
                                if (cNode[i].Name.Equals("Uid")) { Uid = cNode[i].InnerXml; }
                                if (cNode[i].Name.Equals("Pwd")) { Pwd = cNode[i].InnerXml; }
                            }
                            catch
                            {
                                throw new Exception("登录信息配置数值错误！");
                            }
                        }
                    }
                }
                if (string.IsNullOrEmpty(DataSource) || string.IsNullOrEmpty(Database) || string.IsNullOrEmpty(Uid) || string.IsNullOrEmpty(Pwd)) { throw new Exception("配置信息不完整!"); }
            }
            catch
            {
                throw;
            }
        }

        static public SqlConnection GetConnection()
        {
            try
            {
                if (DataSource.Equals(string.Empty) || Database.Equals(string.Empty) || Uid.Equals(string.Empty) ||
               Pwd.Equals(string.Empty))
                {
                    ReadLogInMsg();
                }

                string SQLConnStr = string.Format("Data Source={0};Database={1};Uid={2};Pwd={3}", DataSource, Database, Uid, Pwd);
                SqlConnection conn = new SqlConnection(SQLConnStr);
                return conn;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }


}
