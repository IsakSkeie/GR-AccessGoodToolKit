using CreateGalaxyExample.DataManagement;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public void CreateTemplate(IGalaxy galaxy)
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

            //Implement check out method
            sampleTemplate.CheckOut();
            foreach (var UDA in UDAs)
            {
                Console.WriteLine(UDA.DataType);

                sampleTemplate.AddUDA(UDA.Names, UDA.DataType, UDA.Category, UDA.Security, UDA.IsArray, UDA.ArrayElementCount);
            }


            sampleTemplate.AddUDA("Names", MxDataType.MxString, MxAttributeCategory.MxCategoryWriteable_USC_Lockable, MxSecurityClassification.MxSecurityOperate, true, 5);
            //IAttributes attrs = sampleinst.ConfigurableAttributes;
            sampleTemplate.Save();
            sampleTemplate.CheckIn();
        }

        private static void UpdateAlarmsObjects(IGalaxy galaxy, List<Alarm> alarms)
        {
            ICommandResult cmd;

            var changedAlarms = from alarm in alarms
                                where alarm.Changed
                                select alarm;

            string[] objList = changedAlarms.Select(x => x.ObjName).Distinct().ToArray();


            var queryResult = galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsInstance, ref objList);
            cmd = galaxy.CommandResult;


            foreach (IInstance instance in queryResult)
            {
                bool result = CheckOutInstance(galaxy, instance);

                var configurableAttributes = instance.ConfigurableAttributes;

                var changedAlarmTags = from changedAlarm in alarms
                                       where changedAlarm.ObjName == instance.Tagname
                                       select changedAlarm;

                foreach (Alarm alarmTag in changedAlarmTags)
                {
                    MxValue mxv = new MxValueClass();
                    Console.WriteLine(alarmTag.AlarmName);
                    MxValue newValue = new MxValueClass();
                    newValue.PutInteger(alarmTag.Priority);
                    configurableAttributes[alarmTag.AlarmName + ".Priority"].SetValue(newValue);

                    newValue = new MxValueClass();
                    newValue.PutString(alarmTag.AlarmDesc);
                    configurableAttributes[alarmTag.AlarmName + ".Description"].SetValue(newValue);
                }


                //Implement SaveInstance method
                result = SaveInstance(galaxy, instance);
                //Implement check in instance method
                result = CheckInInstance(galaxy, instance);

            }

        }

        public bool CheckOutInstance(IGalaxy galaxy, IInstance instance)
        {
            if (instance.CheckoutStatus == ECheckoutStatus.checkedOutToSomeoneElse)
            {
                Console.WriteLine("Object is checked out by: " + instance.checkedOutBy);
                return false;
                //continue;
            }

            if (instance.CheckoutStatus == ECheckoutStatus.notCheckedOut)
            {
                instance.CheckOut();
                return true;
                //Console.WriteLine("Check out: " + instance.Tagname);
            }

            return false;

        }

        public bool CheckInInstance(IGalaxy galaxy, IInstance instance)
        {
            if (instance.CheckoutStatus == ECheckoutStatus.checkedOutToMe)
            {
                instance.CheckIn("GRAccess");
                return true;
                //Console.WriteLine("Check out: " + instance.Tagname);
            }

            return false;
        }

        public bool SaveInstance(IGalaxy galaxy, IInstance instance)
        {
            if (instance.CheckoutStatus == ECheckoutStatus.checkedOutToMe)
            {
                instance.Save();
                return true;
                //Console.WriteLine("Check out: " + instance.Tagname);
            }

            return false;
        }


    }
}
