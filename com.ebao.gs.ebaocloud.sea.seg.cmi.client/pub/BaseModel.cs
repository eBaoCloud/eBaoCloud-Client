using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace com.ebao.gs.ebaocloud.sea.seg.cmi.client.pub
{
    public class BaseModel
    {
        public override string ToString()
        {
            String logText = "";
            Type t = this.GetType();
            logText = "\n---------------" + t.Name + "------" + "Start--------------\n";
            PropertyInfo[] propertyList = t.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            foreach (PropertyInfo item in propertyList)
            {
                string name = item.Name;
                Object value = item.GetValue(this, null);

                logText = logText + name + ": " + value + "\n";

            }
            logText = logText + "\n---------------" + t.Name + "------" + "End--------------\n";
            return logText;
        }

        public Object GetValueOfProperty(string propName)
        {
            Type type = this.GetType();
            PropertyInfo propInfo = type.GetProperty(propName);
            return propInfo.GetValue(this, null);
        }
    }
}
