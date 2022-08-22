using ArchestrA.GRAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchestrA.GRAccess;
using System.IO;
using CreateGalaxyExample.DataManagement;

namespace CreateGalaxyExample
{
    class Queries
    {
        public IGalaxy galaxy;

        public Queries(IGalaxy _galaxy)
        {
            galaxy = _galaxy;
        }

        public List<Alarm> queryAlarms(string[] tagnames)
        {
            List<Alarm> alarms = new List<Alarm>();
            //var queryResult = galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsInstance, ref tagnames);
            //Loads objects
            var queryResult = galaxy.QueryObjects(EgObjectIsTemplateOrInstance.gObjectIsInstance, EConditionType.basedOn, "$UserDefined", EMatch.MatchCondition);

            ICommandResult cmd;
            cmd = galaxy.CommandResult;


            var numbObjects = queryResult.count;
            var i = 0;

            if (!cmd.Successful)
            {
                Console.WriteLine("QueryObjectsByName Failed for $UserDefined Template :" +
                                  cmd.Text + " : " +
                                  cmd.CustomMessage);
                return null;
            }

            foreach (IInstance instance in queryResult)
            {
                i = i + 1;
                //if (instance.CheckoutStatus == ECheckoutStatus.checkedOutToSomeoneElse)
                //{
                //    Console.WriteLine("Object is checked out by: " + instance.checkedOutBy);
                //continue;
                //}

                //if (instance.CheckoutStatus == ECheckoutStatus.notCheckedOut)
                //{
                //instance.CheckOut();
                //Console.WriteLine("Check out: " + instance.Tagname);
                //}

                //Console.WriteLine("Checked Out status: " + template.CheckoutStatus);

                try
                {
                    Console.WriteLine(i + "/" + numbObjects + ": " + instance.Tagname);


                    var instanceConfigurableAttributes = instance.ConfigurableAttributes;
                    foreach (IAttribute att in instanceConfigurableAttributes)
                    {

                        //if (!att.Name.StartsWith("a") || att.Name.Contains("_")) continue;
                        var type = att.value.GetDataType();

                        if (att.Name.Contains("Priority"))
                        {
                            switch (type)
                            {
                                case MxDataType.MxInteger:
                                    {

                                        var alarmName = att.Name.Substring(0, att.Name.Length - 9);
                                        IAttribute alarmDesc = instanceConfigurableAttributes[alarmName + ".Description"];

                                        if (alarmDesc != null)
                                        {
                                        }
                                        else
                                        {
                                            var attrName = att.Name.Substring(0, att.Name.IndexOf("."));
                                            alarmDesc = instanceConfigurableAttributes[attrName + ".Description"];
                                        }

                                        alarms.Add(new Alarm(instance.DerivedFrom, instance.Area, instance.Tagname, alarmName, att.value.GetInteger(), alarmDesc.value.GetString().Replace(",", ".")));
                                        break;
                                    }
                            }
                        }

                    }

                }

                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {

                }

            }
            return alarms;
        }

        public async Task CreateTemplateAsync(string TemplateName, List<UDATemplate> _UDAs)
        {

            
            
            //    IgObjects queryResult = _galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsTemplate, ref tagnames);

            //    ITemplate userDefinedTemplate = (ITemplate)queryResult[1];
            //    Console.WriteLine(TemplateName);
            //    //IInstance sampleinst = userDefinedTemplate.CreateInstance(instanceName, true);
            //    ITemplate sampleTemplate = userDefinedTemplate.CreateTemplate(TemplateName, true);
            ////Adds Attributes
            //CsvImportExport csvImport = new CsvImportExport();
            //csvImport.LoadTemplate("To be selected from GUI");
            //List<UDATemplate> UDAs = DataFormatting.PlcCsvToGalaxyTemplate(csvImport._PlcTemplate);



            //get the $UserDefined template
            string[] tagnames = { "$UserDefined" };
            IgObjects queryResult = galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsTemplate, ref tagnames);
            

            ITemplate userDefinedTemplate = (ITemplate)queryResult[1];
            // create an instance of $UserDefined, named with current time DateTime
            DateTime now = DateTime.Now;
            string instanceName = TemplateName;
            ITemplate sampleinst = userDefinedTemplate.CreateTemplate(instanceName, true);
            //How to edit the object ?
            sampleinst.CheckOut();
            sampleinst.Save();
            sampleinst.CheckIn();


            //Need a try here, or check if it already exists, and print error
            //sampleTemplate.CheckOut();

            //if (!(UDAs is null))
            //{
            //     foreach (var UDA in UDAs)
            //                    {
            //                        Console.WriteLine(UDA.DataType);
            //                        sampleTemplate.AddUDA(UDA.Names, UDA.DataType, UDA.Category, UDA.Security, UDA.IsArray, UDA.ArrayElementCount);

            //                    }
            //                    sampleTemplate.Save();
            //                    sampleTemplate.CheckIn();
            //}
               
           
            
        }
    }
}
