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
    }
}
