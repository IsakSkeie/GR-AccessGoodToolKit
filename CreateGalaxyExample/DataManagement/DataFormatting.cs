using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateGalaxyExample.DataManagement
{
    static class DataFormatting
    {
        static public List<UDATemplate> PlcCsvToGalaxyTemplate(List<PlcTemplate> PlcCsv)
        {
            List<UDATemplate> UDAs = new List<UDATemplate>();
            
            foreach(PlcTemplate atrb in PlcCsv)
            {
                UDATemplate _UDA = new UDATemplate(atrb.Name, atrb.DataType, atrb.Description);
                UDAs.Add(_UDA);
            }
            return UDAs;
        }
        public static Alarm AlarmsFromCSV(string csvLine)
        {
            string[] values = csvLine.Split(',');
            Alarm alarm = new Alarm();
            alarm.TemplateName = values[0];
            alarm.Area = values[1];
            alarm.ObjName = values[2];
            alarm.AlarmName = values[3];
            alarm.Priority = Convert.ToInt32(values[4]);
            alarm.AlarmDesc = values[5];
            if (values[6].Length > 0) alarm.Changed = true;
            else alarm.Changed = false; ;
            return alarm;
        }
    }
}
