using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CQUTMyXueba.openapi
{
    public partial class web : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            string action = "";

            if (Request.QueryString["action"] != null)
            {
                action = Request.QueryString["action"].ToString();
            }
            else
            {
                //action字段为空返回没有操作
                Response.Write("{msg:\"error-noaction\"}");
                return;
            }

            switch (action)
            {
                //vote操作
                //GET方式发送目标的ID，如果成功返回msg为ok，否则，返回error-vote
                //如果不存在ID，则返回没有目标（error-no-target）
                case "vote":
                    {
                        string id = Request.QueryString["id"];
                        if (id != null)
                        {
                            id = id.Replace(" ", "");
                            if (!id.Equals(""))
                            {
                                Response.Write("{msg:\"" + (Model.model.vote(getip(), id) ? "ok" : "error-vote") + "\"}");
                                return;
                            }
                        }
                        Response.Write("{msg:\"error-no-target\"}");
                        return;

                    }
                //获取人员列表
                //如果数据为空，则返回"error-no-target"表示出现异常
                case "getperson":
                    {
                        string data = Model.model.getPersonData();
                        Response.Write(data == null ? "{msg:\"error-no-target\"}" : data);
                        return;
                    }
                //输出当前投票情况
                //包括 id,num,name三个字段。若成功，则msg为ok并且包含person数组,否则返回无数据错误
                case "count":
                    {
                        string data = Model.model.getVoteData();
                        Response.Write(data == null ? "{msg:\"error-no-data\"}" : data);
                        return;
                    }
                //密码错误返回error-password
                case "addperson":
                    {
                        if (!isLogin())
                        {
                            Response.Write("{\"msg\":\"error-nopermission\"}");
                            return;
                        }
                        /*
                        const string password = "563754F4473E1DD325709C758541871A";

                        string pass = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile( Request.Form["password"],"MD5");
                        if (pass == null || !pass.Equals(password)) {
                            Response.Write("{msg:\"error-password\"}");
                            return;
                        }
                         * 
                         * */
                        string name = Request.Form["name"];
                        string des = Request.Form["des"];
                        string title = Request.Form["title"];
                        string awards = Request.Form["awards"];
                        string intro = Request.Form["intro"];

                        string type = Request.Form["type"];
                        //string pic = uploadPic(name);

                        string pic = (String)Session["upload"];

                        if (Session["lastquery"] == null || !Session["lastquery"].Equals("false"))
                        {
                            pic = "nopic";
                            Session.Remove("lastquery");
                            Session.Remove("upload");
                        }
                        else
                        {
                            Session["lastquery"] = "true";
                        }

                        Response.Write("{msg:\"" + (Model.model.addPerson(name, des, awards, title, intro, pic, type) ? "success" : "failed") + "\"}");
                        return;
                    }
                case "login":
                    {
                        string name = Request.Form["name"];
                        string pass = Request.Form["password"];
                        string code = Request.Form["check"];

                        if (Session["code"] == null || !Session["code"].Equals(code))
                        {
                            Response.Write("{msg:\"error-code-" + Session["code"] + "\"}");
                            return;
                        }

                        Boolean login = Model.model.Login(name, pass);
                        if (login)
                        {
                            Session.Add("username", name);
                        }

                        Response.Write("{msg:\"" + ((login) ? "ok\",page:\"admin.aspx" : "error-login") + "\"}");

                        return;
                    }
                case "addadmin":
                    {
                        string name = Request.Form["name"];
                        string pass = Request.Form["password"];
                        string code = Request.Form["check"];

                        if (Session["code"] == null || !Session["code"].Equals(code))
                        {
                            Response.Write("{msg:\"error-code\"}");
                            return;
                        }

                        Response.Write("{msg:\"" + (Model.model.addNewUser(name, pass) ? "ok" : "error-adduser") + "\"}");

                        return;
                    }
                case "removeuser":
                    {
                        string name = Request.Form["name"];

                        string code = Request.Form["check"];

                        if (Session["code"] == null || !Session["code"].Equals(code))
                        {
                            Response.Write("{msg:\"error-code\"}");
                            return;
                        }

                        Response.Write("{msg:\"" + (Model.model.removeUser(name) ? "ok" : "error-remove") + "\"}");

                        return;
                    }
                case "changepassword":
                    {
                        string name = Request.Form["name"];
                        string old = Request.Form["old"];
                        string newpass = Request.Form["newpass"];

                        string code = Request.Form["check"];
                        if (code != null) code = code.ToUpper();

                        if (Session["code"] == null || !Session["code"].Equals(code))
                        {
                            Response.Write("{msg:\"error-code-" + Session["code"] + "\"}");
                            return;
                        }
                        Response.Write("{msg:\"" + (Model.model.changePassword(name, old, newpass) ? "ok" : "error-change") + "\"}");

                        return;
                    }
                //一键安装操作，如果存在install.lock则会无法执行安装。
                case "install":
                    {
                        Response.Write(Model.model.install() ? "ok" : "failed");
                        return;
                    }
                //返回错误信息为未定义操作

                default: Response.Write("{msg:\"error-action-not-defined\"}"); break;
            }
        }

        public string uploadPic(string name)
        {

            HttpPostedFile pFile = Request.Files["pic"];
            if (pFile == null || pFile.ContentLength <= 0) return "nopic";

            string filename = System.IO.Path.GetFileName(pFile.FileName);
            if (!filename.Equals(""))
            {
                string fileext = System.IO.Path.GetExtension(filename);
                if (fileext.Equals("jpg") || fileext.Equals("gif") || fileext.Equals("bmp") || fileext.Equals("jpeg") || fileext.Equals("png"))
                {
                    //保存文件
                    string final = System.Web.HttpContext.Current.Request.MapPath("/xuebapic/") + name + "_" + DateTime.Now.ToLongDateString() + "_" + filename.Replace(" ", "");
                    pFile.SaveAs(final);
                    return "xuebapic/" + name + "_" + DateTime.Now.ToLongDateString() + "_" + filename.Replace(" ", "");
                }
                return "nopic";
            }
            else
            {
                return "nopic";
            }
        }

        public Boolean isLogin()
        {
            return !(Session["username"] == null || Session["username"].Equals(""));
        }
        public string getip()
        {
            string ip = Request.ServerVariables["REMOTE_ADDR"];

            return ip;
        }
    }
}