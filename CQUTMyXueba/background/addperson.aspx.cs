using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CQUTMyXueba.background
{
    public partial class addperson : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null) Response.Redirect("/default.aspx");
        }

        protected void uploadpic_Click(object sender, EventArgs e)
        {
            string uploadName = pic.Value;//获取待上传图片的完整路径，包括文件名
            //string uploadName = InputFile.PostedFile.FileName;
            string pictureName = "";//上传后的图片名，以当前时间为文件名，确保文件名没有重复
            if (pic.Value != "")
            {
                int idx = uploadName.LastIndexOf(".");
                string suffix = uploadName.Substring(idx+1);//获得上传的图片的后缀名
                if (suffix.Equals("jpg") || suffix.Equals("gif") || suffix.Equals("bmp") || suffix.Equals("jpeg") || suffix.Equals("png"))
                    pictureName = DateTime.Now.Ticks.ToString() + "."+ suffix;
                else {
                    Response.Write("suffix error "+suffix);
                    return;
                }
            }
            try
            {
                if (uploadName != "")
                {
                    string path = Server.MapPath("~/xuebapic/");
                    pic.PostedFile.SaveAs(path + pictureName);
                    Session.Add("upload", pictureName);
                    Session.Add("lastquery", "false");
                    Response.Redirect("addperson.html");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }
    }
}