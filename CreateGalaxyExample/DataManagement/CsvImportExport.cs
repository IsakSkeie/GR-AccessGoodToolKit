using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace CreateGalaxyExample
{
    class CsvImportExport
    {
        public string PlsTemplatePath = @"C:\Users\amoe\Documents\GRAccessToolKit\MES_BI_Struct.csv";
        public List<PlcTemplate> _PlcTemplate;
        
        public void LoadTemplate(string path)
        {
            using (var reader = new StreamReader(PlsTemplatePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<PlcTemplate>();

                //Need to configure interface for this
                try
                    {
                    _PlcTemplate = records.ToList();
                }
                catch(ArgumentNullException e)
                {
                    Console.WriteLine(e);

                }
                
            }


            ////Used for debugging
            //foreach(var record in _PlcTemplate)
            //{
            //    Console.WriteLine(record.Description);
            //}
        }

        public List<Alarm> LoadAlarmsFromCSV(string path)
        {
            List<Alarm> csvAlarms = File.ReadAllLines(path)
                           .Skip(1)
                           .Select(v => Alarm.AlarmsFromCSV(v))
                           .ToList();
            return csvAlarms;
        }

        
    }
}
