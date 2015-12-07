using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVG.QA.ProxyHelper
{
    public static class StaticUtils
    {
        private static object mSync = new object();
        private static TaskbarIcon mIcon = null;
        public static TaskbarIcon NotificationIcon
        {
            get
            {
                if (mIcon == null)
                {
                    lock (mSync)
                    {
                        if (mIcon == null)
                        {
                            mIcon = new TaskbarIcon();
                        }
                    }
                }

                return mIcon;
            }
        }
    }
}
