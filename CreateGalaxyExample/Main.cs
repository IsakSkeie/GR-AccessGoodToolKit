// ------------------------------------------------------------------------------------------------------------
// <copyright company="AVEVA Software, LLC" file="Main.cs">
//   © 2020 AVEVA Software, LLC. All rights reserved.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
// </copyright>
// <summary>
//
// </summary>
// ------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using ArchestrA.GRAccess;
using CreateGalaxyExample;
using CreateGalaxyExample.DataConnection;
using static CreateGalaxyExample.Queries;


public class Alarm
{
    public string TemplateName { get; set; }
    public string Area { get; set; }
    public string ObjName { get; set; }
    public string AlarmName { get; set; }
    public int Priority { get; set; }
    public string AlarmDesc { get; set; }
    public bool Changed { get; set; }

    public Alarm()
    {

    }


    public Alarm(string TemplateName, string Area,string ObjName, string AlarmName, int Priority, string AlarmDesc,bool Changed = false)
    {
        this.ObjName = ObjName;
        this.AlarmName = AlarmName;
        this.Priority= Priority;
        this.AlarmDesc = AlarmDesc;
        this.TemplateName = TemplateName;
        this.Area = Area;
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




class _CreateGalaxyExample
{
    static GRAccessApp grAccess = new GRAccessAppClass();

    [STAThread]
    static void Main()
    {
        List<Alarm> alarms = new List<Alarm>();
        string nodeName = Environment.MachineName;
        string galaxyName = "NK_20211210";
        string path = @"c:\tmp\Alarms_20211210.csv";



        GalaxyConnection.Connect();
        GalaxyConnection.Login();
        Console.WriteLine("Logged in to galaxy");
        Console.WriteLine("");
        



        
        Queries SpQueries = new Queries();

        //alarms = SpQueries.queryAlarms(galaxy, tagnames);
        SpQueries.CreateTemplate(galaxy);
       
        //CreateCSVTextFile(alarms,path);


        //List<Alarm> csvAlarms = LoadAlarmsFromCSV(path);

        //UpdateObjects(galaxy, csvAlarms);

        //foreach (Alarm alarm in changedAlarm)
        //{
        //    Console.WriteLine("ReadBack" + alarm.ObjName + ", " + alarm.AlarmName + ", " + alarm.Priority + ", " + alarm.AlarmDesc+ "," + alarm.Changed);
        //}

        Console.WriteLine();
        Console.Write("Press ENTER to quit: ");
        string dummy;
        dummy = Console.ReadLine();
        galaxy.Logout();
    }





    
    //private static List<Alarm> queryAlarms(IGalaxy galaxy, string[] tagnames)
    //{
    //    List<Alarm> alarms = new List<Alarm>();
    //    //var queryResult = galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsInstance, ref tagnames);
    //    //Loads objects
    //    var queryResult = galaxy.QueryObjects(EgObjectIsTemplateOrInstance.gObjectIsInstance, EConditionType.basedOn, "$UserDefined", EMatch.MatchCondition);
 
    //    ICommandResult cmd;
    //    cmd = galaxy.CommandResult;


    //    var numbObjects = queryResult.count;
    //    var i = 0;

    //    if (!cmd.Successful)
    //    {
    //        Console.WriteLine("QueryObjectsByName Failed for $UserDefined Template :" +
    //                          cmd.Text + " : " +
    //                          cmd.CustomMessage);
    //        return null;
    //    }

    //    foreach (IInstance instance in queryResult)
    //    {
    //        i = i + 1;
    //        //if (instance.CheckoutStatus == ECheckoutStatus.checkedOutToSomeoneElse)
    //        //{
    //        //    Console.WriteLine("Object is checked out by: " + instance.checkedOutBy);
    //            //continue;
    //        //}

    //        //if (instance.CheckoutStatus == ECheckoutStatus.notCheckedOut)
    //        //{
    //            //instance.CheckOut();
    //            //Console.WriteLine("Check out: " + instance.Tagname);
    //        //}

    //        //Console.WriteLine("Checked Out status: " + template.CheckoutStatus);

    //        try
    //        {
    //            Console.WriteLine(i + "/" + numbObjects + ": " + instance.Tagname);


    //            var instanceConfigurableAttributes = instance.ConfigurableAttributes;
    //            foreach (IAttribute att in instanceConfigurableAttributes)
    //            {
                
    //                //if (!att.Name.StartsWith("a") || att.Name.Contains("_")) continue;
    //                var type = att.value.GetDataType();

    //                if (att.Name.Contains("Priority"))
    //                {
    //                    switch (type)
    //                    {
    //                        case MxDataType.MxInteger:
    //                            {
                                   
    //                                var alarmName = att.Name.Substring(0, att.Name.Length - 9);
    //                                IAttribute alarmDesc = instanceConfigurableAttributes[alarmName + ".Description"];

    //                                if (alarmDesc != null)
    //                                {                                       
    //                                }
    //                                else
    //                                {
    //                                    var attrName = att.Name.Substring(0, att.Name.IndexOf("."));
    //                                    alarmDesc = instanceConfigurableAttributes[attrName + ".Description"];
    //                                }
                                    
    //                                alarms.Add(new Alarm(instance.DerivedFrom, instance.Area, instance.Tagname, alarmName, att.value.GetInteger(), alarmDesc.value.GetString().Replace(",",".")));
    //                                break;
    //                            }
    //                    }
    //                }
                    
    //            }
              
    //        }
            
    //        catch (Exception e)
    //        {
    //            Console.WriteLine(e.ToString());
    //        }
    //        finally
    //        {

    //        }
            
    //    }
    //    return alarms;
    //}




    private static List<Alarm> LoadAlarmsFromCSV(string path)
    {
        List<Alarm> csvAlarms = File.ReadAllLines(path)
                       .Skip(1)
                       .Select(v => Alarm.AlarmsFromCSV(v))
                       .ToList();
        return csvAlarms;
    }

    private static void UpdateObjects(IGalaxy galaxy, List<Alarm> alarms)
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

            result = SaveInstance(galaxy, instance);
            result = CheckInInstance(galaxy, instance);
            
        }
    }


    private static bool CheckOutInstance(IGalaxy galaxy, IInstance instance)
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

    private static bool CheckInInstance(IGalaxy galaxy, IInstance instance)
    {
        if (instance.CheckoutStatus == ECheckoutStatus.checkedOutToMe)
        {
            instance.CheckIn("GRAccess");
            return true;
            //Console.WriteLine("Check out: " + instance.Tagname);
        }

        return false;
    }

    private static bool SaveInstance(IGalaxy galaxy, IInstance instance)
    {
        if (instance.CheckoutStatus == ECheckoutStatus.checkedOutToMe)
        {
            instance.Save();
            return true;
            //Console.WriteLine("Check out: " + instance.Tagname);
        }

        return false;
    }


        private static void CreateCSVTextFile(List<Alarm> alarms, string path, string seperator = ",")
    {
        var result = new StringBuilder();

        var line = string.Join(seperator, "Template","Area", "ObjName",  "AlarmName",  "Priority",  "AlarmDesc", "Changed");
        Console.WriteLine(line);
        result.AppendLine(line);

        foreach (Alarm alarm in alarms)
        {
            line = string.Join(seperator,alarm.TemplateName,alarm.Area,alarm.ObjName, alarm.AlarmName, alarm.Priority, alarm.AlarmDesc,"");
            Console.WriteLine(line);
            result.AppendLine(line);
        }
        File.WriteAllText(path, result.ToString(), new UTF8Encoding(false));
    }
    static string mxValueString(MxDataType type, MxValue value)
    {
        switch (type)
        {
            case MxDataType.MxBoolean: return value.GetBoolean().ToString();
            case MxDataType.MxString: return value.GetString();
            case MxDataType.MxDouble: return value.GetDouble().ToString();
            case MxDataType.MxFloat: return value.GetFloat().ToString();
            case MxDataType.MxInteger: return value.GetInteger().ToString();
            default: return "";
        }
    }

}
