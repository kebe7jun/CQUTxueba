using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LibControl
{
    public class vote
    {
        public static string getHashStr(string source)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] dataToHash = System.Text.Encoding.ASCII.GetBytes(source);
            byte[] hashed = sha.ComputeHash(dataToHash);
            return System.Text.Encoding.ASCII.GetString(hashed);
        }

        public static Boolean checkAuth(string username, string password)
        {
            if (username != null && password != null)
            {
                try
                {
                    string realname = username.Replace(" ", "").Replace("*", "");
                    string realpwd = getHashStr(password);
                    string SQL = "SELECT password FROM system WHERE name=@name";

                    SQLiteParameter[] pParam = new SQLiteParameter[1];
                    pParam[0] = new SQLiteParameter("@name", DbType.String);
                    pParam[0].Value = realname;

                    DataTable pDt = dbcon.Query(SQL, pParam).Tables[0];
                    if (pDt.Rows.Count == 0) return false;
                    return realpwd.Equals(pDt.Rows[0]["password"]);
               //     return password.Equals(pDt.Rows[0]["password"]);

                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return false;
        }

        public static Boolean changePassword(string name, string old, string newpass)
        {
            if (name == null || old == null || newpass == null)
            {
                if (checkAuth(name, old))
                {
                    try
                    {
                        string realname = name.Replace(" ", "").Replace("*", "");
                        string realnew = getHashStr(newpass);

                        string sql = "UPDATE system SET name=@name , password=@password WHERE name=@name";

                        SQLiteParameter[] pParams = new SQLiteParameter[2];
                        pParams[0] = new SQLiteParameter("@name", DbType.String);
                        pParams[0].Value = realname;

                        pParams[1] = new SQLiteParameter("@password", DbType.String);
                        pParams[1].Value = realnew;

                        return dbcon.ExecuteSql(sql, pParams) > 0;
                    }
                    catch (Exception ex)
                    {
                        return false;

                    }
                }
            }
            return false;
        }

        public static Boolean removeUser(string username)
        {
            if (username == null) return false;
            string realname = username.Replace("*", "").Replace(" ", "");
            try
            {
                string sql = "DELETE FROM system WHERE name=@name";
                SQLiteParameter[] pParams = new SQLiteParameter[1];
                pParams[0] = new SQLiteParameter("@name", DbType.String);
                pParams[0].Value = realname;

                return dbcon.ExecuteSql(sql, pParams) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static Boolean addNewUser(string name, string password)
        {
            if (name == null || password == null) return false;
            string realname = name.Replace(" ", "").Replace("*", "");
            if (realname.Length > 3 && password.Length >= 6)
            {
                try
                {
                    string sql = "INSERT INTO system(name,password) VALUES(@name,@password)";

                    SQLiteParameter[] pParams = new SQLiteParameter[2];
                    pParams[0] = new SQLiteParameter("@name", DbType.String);
                    pParams[0].Value = realname;

                    pParams[1] = new SQLiteParameter("@password", DbType.String);
                    pParams[1].Value = getHashStr(password);

                    return dbcon.ExecuteSql(sql, pParams) > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }
        public static string getVoteById(string id)
        {
            if (id == null)
                return null;
            DataTable votes = getTicketDataNormal();
            for (var i = 0; i < votes.Rows.Count; i++)
            {
                if (votes.Rows[i]["id"].ToString().Equals(id))
                    return votes.Rows[i]["num"].ToString();
            }
            return null;
        }
        public static DataTable getPersonInfoByWeb()
        {
            string data = "SELECT * FROM participator";
            return dbcon.Query(data).Tables[0];

        }
        public static DataTable getPersonInfoById(string id)
        {
            string data = "SELECT * FROM participator where id=@id";
            SQLiteParameter[] pParams = new SQLiteParameter[1];
            pParams[0] = new SQLiteParameter("@id", DbType.Int32);
            pParams[0].Value = id;

            return dbcon.Query(data, pParams).Tables[0];
        }
        /**
         获取所有人的信息
         */
        public static string getPersonInfo()
        {
            string data = "SELECT * FROM participator";
            DataTable pDt = dbcon.Query(data).Tables[0];

            if (pDt.Rows.Count == 0) return null;

            string req = "{\"msg\":\"ok\",\"count\":" + pDt.Rows.Count + ",\"person\":[";

            for (int i = 0; i < pDt.Rows.Count; i++)
            {
                req += getPersonData(pDt.Rows[i]);

                if (i < pDt.Rows.Count - 1)
                {
                    req += ",";
                }
            }

            req += "]}";

            return req.Replace("\n", "");
        }
        private static string getPersonData(DataRow p)
        {
            return "{\"name\":\"" + p["name"] + "\",\"awards\":\"" + p["awards"] + "\",\"des\":\"" + p["description"] + "\",\"title\":\"" + p["title"] + "\",\"id\":\"" + p["id"] + "\",\"pic\":\"" + p["pic"] + "\",\"intro\":\"" + p["intro"] + "\",\"type\":" + p["type"] + "}";
        }

        public static DataTable getTicketDataNormal()
        {
            string data = "SELECT * FROM votenum";
            return dbcon.Query(data).Tables[0];
        }

        //获取全部票数信息
        public static string getTicketData()
        {
            string data = "SELECT * FROM votenum";
            DataTable pDt = dbcon.Query(data).Tables[0];

            if (pDt.Rows.Count == 0) return null;

            //获取到ID和名字，数值(id,name,num)
            string req = "{msg:\"ok\",\"votedata\":[";

            for (int i = 0; i < pDt.Rows.Count; i++)
            {
                req += getTicketDataToStr(pDt.Rows[i]);
                if (i < pDt.Rows.Count - 1)
                {
                    req += ",";
                }
            }
            req += "]}";

            return req;
        }

        private static string getTicketDataToStr(DataRow p)
        {
            return "{\"name\":\"" + p["name"] + "\",\"id\":\"" + p["id"] + "\",\"num\":\"" + p["num"] + "\"}";
        }

        //处理投票操作
        public static Boolean votePerson(string id, string ip)
        {
            if (addVoteInfo(ip, id))
            {
                return addTicket(id);
            }
            return false;
        }

        //将IP和投票对象ID写入数据，如果成功，则允许票数增加
        private static Boolean addVoteInfo(string ip, string id)
        {
            DateTime pDt = DateTime.Now;
            string pDate = pDt.Year + "-" + pDt.Month + "-" + pDt.Day;

            string testSql = "SELECT id FROM vote WHERE ip=@ip AND date=@date";
            SQLiteParameter[] pParamTest = new SQLiteParameter[2];
            pParamTest[0] = new SQLiteParameter("@ip", DbType.String);
            pParamTest[0].Value = ip;

            pParamTest[1] = new SQLiteParameter("@date", DbType.String);
            pParamTest[1].Value = pDate;

            DataTable pReqTable = dbcon.Query(testSql, pParamTest).Tables[0];
            if (pReqTable.Rows.Count != 0) return false;

            try
            {
                string SQL = "INSERT INTO vote(ip,id,date) VALUES(@ip,@id,@date)";
                SQLiteParameter[] pParams = new SQLiteParameter[3];

                pParams[0] = new SQLiteParameter("@ip", DbType.String);
                pParams[0].Value = ip;

                pParams[1] = new SQLiteParameter("@id", DbType.Int32);
                pParams[1].Value = id;

                pParams[2] = new SQLiteParameter("@date", DbType.String);
                pParams[2].Value = pDate;

                return dbcon.ExecuteSql(SQL, pParams) > 0;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        //增加票数
        private static Boolean addTicket(string id)
        {
            string SQL = "UPDATE votenum SET num=(SELECT num FROM votenum WHERE id=@id)+1 WHERE id=@id";
            SQLiteParameter[] pParams = new SQLiteParameter[1];
            pParams[0] = new SQLiteParameter("@id", DbType.Int32);
            pParams[0].Value = id;

            return dbcon.ExecuteSql(SQL, pParams) > 0;
        }

        //人添加
        public static Boolean addPerson(string name, string awards, string des, string title, string pic, string intro, int type)
        {

            string SQL = "INSERT INTO participator(name,description,awards,title,pic,intro,type) VALUES(@name,@description,@awards,@title,@pic,@intro,@type)";
            SQLiteParameter[] pParams = new SQLiteParameter[7];
            pParams[0] = new SQLiteParameter("@name", DbType.String);
            pParams[0].Value = name;

            pParams[1] = new SQLiteParameter("@description", DbType.String);
            pParams[1].Value = des;

            pParams[2] = new SQLiteParameter("@awards", DbType.String);
            pParams[2].Value = awards;

            pParams[3] = new SQLiteParameter("@title", DbType.String);
            pParams[3].Value = title;

            pParams[4] = new SQLiteParameter("@pic", DbType.String);
            pParams[4].Value = pic;

            pParams[5] = new SQLiteParameter("@intro", DbType.String);
            pParams[5].Value = intro;

            pParams[6] = new SQLiteParameter("@type", DbType.Int32);
            pParams[6].Value = type;

            try
            {
                if (dbcon.ExecuteSql(SQL, pParams) > 0)
                {
                    SQL = "SELECT id FROM participator WHERE name=@name ORDER BY id DESC";
                    SQLiteParameter[] pPara = new SQLiteParameter[1];
                    pPara[0] = new SQLiteParameter("@name", DbType.String);
                    pPara[0].Value = name;

                    DataTable pDt = dbcon.Query(SQL, pPara).Tables[0];

                    //将该人员添加到投票统计数据表中
                    if (pDt.Rows.Count > 0)
                    {
                        SQL = "INSERT INTO votenum(id,name,num) VALUES(@id,@name,0)";
                        SQLiteParameter[] id = new SQLiteParameter[2];
                        id[0] = new SQLiteParameter("@id", DbType.Int32);
                        id[0].Value = pDt.Rows[0]["id"];

                        id[1] = new SQLiteParameter("@name", DbType.String);
                        id[1].Value = name;

                        return dbcon.ExecuteSql(SQL, id) > 0;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }




        //===========================
        //初始化数据表

        public static Boolean initDatabase()
        {
            try
            {
                if (isLock()) return false;
                initVoteData();
                initParticipator();
                initVoteTable();
                initSystemTable();
                if (initSystemUser())
                    return createLock();
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static Boolean isLock()
        {
            return File.Exists(System.Web.HttpContext.Current.Request.MapPath("/Data/") + "install.lock");
        }

        private static Boolean createLock()
        {
            File.Create(System.Web.HttpContext.Current.Request.MapPath("/Data/") + "install.lock");
            return isLock();
        }

        private static void initVoteData()
        {
            string SQL = "CREATE TABLE IF NOT EXISTS vote(ip varchar(18) UNIQUE NOT NULL,id integer NOT NULL,date varchar(20) NOT NULL)";

            dbcon.ExecuteSql(SQL);
        }

        private static void initVoteTable()
        {
            string SQL = "CREATE TABLE IF NOT EXISTS votenum(id integer UNIQUE NOT NULL,name varchar(10) NOT NULL,num integer NOT NULL)";

            dbcon.ExecuteSql(SQL);
        }

        //id，姓名，照片引用地址，奖项，大标题，心得，简介
        //id,name,pic,awards,title,description,intro
        private static void initParticipator()
        {
            string SQL = "CREATE TABLE IF NOT EXISTS participator(id integer PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL,name varchar(10) NOT NULL,pic varchar(30) NOT NULL,description varchar(5000) NOT NULL,awards varchar(500) NOT NULL,title varchar(50) NOT NULL,intro varchar(1000) NOT NULL,type integer NOT NULL)";

            dbcon.ExecuteSql(SQL);
        }

        private static void initSystemTable()
        {
            String SQL = "CREATE TABLE IF NOT EXISTS system(id integer PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL,name varchar(20) UNIQUE NOT NULL,password varchar(40) NOT NULL)";

            dbcon.ExecuteSql(SQL);
        }

        private static Boolean initSystemUser()
        {
            string sql = "INSERT INTO system(name,password) VALUES(@name,@password)";

            SQLiteParameter[] pParams = new SQLiteParameter[2];
            pParams[0] = new SQLiteParameter("@name", DbType.String);
            pParams[0].Value = "admin";

            pParams[1] = new SQLiteParameter("@password", DbType.String);
            pParams[1].Value = getHashStr("cqutadmin");

            return dbcon.ExecuteSql(sql, pParams) > 0;
        }
    }
}
