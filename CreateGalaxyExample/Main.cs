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
