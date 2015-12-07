using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lex.Db;

namespace Titanium.Web.Proxy.Test
{
    public class Dal
    {
        static DbInstance mDB;

        static Dal()
        {
            mDB = new DbInstance("storage");
            mDB.Map<BO.BlackListRecord>().Automap(rec => rec.Id, true);

            mDB.Initialize();
        }

        public static List<BO.BlackListRecord> GetAllBlacklistRecords()
        {
            return mDB.Table<BO.BlackListRecord>().LoadAll().ToList();
        }

        public static void AddBlacklistRecord(BO.BlackListRecord addMe)
        {
            mDB.Table<BO.BlackListRecord>().Save(addMe);
        }
    }
}
