using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Model
{
    public class model
    {
        public static DataTable getUserTable()
        {
            return LibControl.vote.getPersonInfoByWeb();
        }
        public static Boolean vote(string ip, string id)
        {
            return LibControl.vote.votePerson(id, ip);
        }

        public static Boolean addPerson(string name, string des, string award, string title, string intro, string pic, string type)
        {
            return LibControl.vote.addPerson(name, award, des, title, pic, intro, type.ElementAt(0) - '0');
        }

        public static Boolean install()
        {
            return LibControl.vote.initDatabase();
        }

        public static string getVoteData()
        {
            return LibControl.vote.getTicketData();
        }

        public static string getPersonData()
        {
            return LibControl.vote.getPersonInfo();
        }
        public static DataTable getPersonInfoById(string id)
        {
            return LibControl.vote.getPersonInfoById(id);
        }
        public static DataTable getTicketDataNormal()
        {
            return LibControl.vote.getTicketDataNormal();
        }
        public static string getVoteById(string id)
        {
            return LibControl.vote.getVoteById(id);
        }

        public static Boolean Login(string name, string password)
        {
            return LibControl.vote.checkAuth(name, password);
        }

        public static Boolean changePassword(string name, string old, string password)
        {
            return LibControl.vote.changePassword(name, old, password);
        }

        public static Boolean addNewUser(string name, string password)
        {
            return LibControl.vote.addNewUser(name, password);
        }

        public static Boolean removeUser(string username)
        {
            return LibControl.vote.removeUser(username);
        }
    }
}
