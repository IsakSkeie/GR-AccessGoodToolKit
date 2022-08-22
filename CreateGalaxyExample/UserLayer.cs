using ArchestrA.GRAccess;
using CreateGalaxyExample.DataManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateGalaxyExample
{
    class UserLayer
    {
        public IGalaxy galaxy;
        

        public UserLayer(IGalaxy _galaxy)
        {
            galaxy = _galaxy;
        }

        public async Task createTemplateFromCsv(string name)
        {
            Queries SpQueries = new Queries(galaxy);

            //alarms = SpQueries.queryAlarms(galaxy, tagnames);
            //string TemplateName = "GRToolTest";
            CsvImportExport csvImport = new CsvImportExport();
            csvImport.LoadTemplate("To be selected from GUI");
            List<UDATemplate> UDAs = DataFormatting.PlcCsvToGalaxyTemplate(csvImport._PlcTemplate);

            await SpQueries.CreateTemplateAsync(name, UDAs);
        }
    }
}
