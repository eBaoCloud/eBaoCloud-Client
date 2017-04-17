﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace com.ebao.gs.ebaocloud.sea.seg.client.pub
{
    public class BaseModel
    {
        public override string ToString()
        {
            String logText = "";
            Type t = this.GetType();
            logText = "\n---------------" + t.Name + "------"  + "Start--------------\n";
            PropertyInfo[] propertyList = t.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            foreach (PropertyInfo item in propertyList)
            {
                string name = item.Name;
                Object value = item.GetValue(this, null);
                if (String.IsNullOrEmpty(value))
                {
                    logText = logText + name + ": " + "woshiarray" + "\n";

                }
                else
                {
                    logText = logText + name + ": " + value + "\n";
                }
            }
            logText = logText + "\n---------------" + t.Name + "------" + "End--------------\n";
            return logText;
        }



    }
}