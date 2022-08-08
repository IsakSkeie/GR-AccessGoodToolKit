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

        public List<Alarm> queryAlarms(IGalaxy galaxy, string[] tagnames)
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

        public void  CreateTemplate(IGalaxy galaxy)
        {

            //string path = @"C:\Users\amoe\Documents\GRAccessToolKit\$GRToolUserDefined.aaPKG";
            //string[] templateName = { "$GRToolUserDefined" };
            ////Import UserDefined
            //bool FileExists = File.Exists(path);
            //if (FileExists)
            //{
            //    galaxy.ImportObjects(path, true);

            //}
            //else
            //{
            //    Console.WriteLine("Template file doesnt exist");
            //}
            //IgObjects Objects = galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsTemplate, ref templateName);





            

            string[] tagnames = { "$UserDefined" };
            IgObjects queryResult = galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsTemplate, ref tagnames);
            //cmd = galaxy.CommandResult;
            //if (!cmd.Successful)
            //{
            //    Console.WriteLine("QueryObjectsByName Failed for $UserDefined Template :" + cmd.Text + " : " + cmd.CustomMessage);
            //    return;
            //}
            ITemplate userDefinedTemplate = (ITemplate)queryResult[1];
            string instanceName = "$GRToolTest";
            //IInstance sampleinst = userDefinedTemplate.CreateInstance(instanceName, true);
            ITemplate sampleTemplate = userDefinedTemplate.CreateTemplate(instanceName, true);
            //Adds Attributes
            CsvImportExport csvImport = new CsvImportExport();
            csvImport.LoadTemplate("To be selected from GUI");
            List<UDATemplate> UDAs = DataFormatting.PlcCsvToGalaxyTemplate(csvImport._PlcTemplate);


            sampleTemplate.CheckOut();
            foreach (var UDA in UDAs)
            {
                Console.WriteLine(UDA.DataType);

                sampleTemplate.AddUDA(UDA.Names, UDA.DataType, UDA.Category, UDA.Security, UDA.IsArray, UDA.ArrayElementCount);
                //sampleTemplate.AddUDA(UDA.Names, UDA.DataType, UDA.Category, MxSecurityClassification.MxSecurity, true, 5);
            }


            sampleTemplate.AddUDA("Names", MxDataType.MxString, MxAttributeCategory.MxCategoryWriteable_USC_Lockable, MxSecurityClassification.MxSecurityOperate, true, 5);
            //IAttributes attrs = sampleinst.ConfigurableAttributes;
            sampleTemplate.Save();
            sampleTemplate.CheckIn();
        }
    }
}
