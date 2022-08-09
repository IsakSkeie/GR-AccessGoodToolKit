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

       
        SpQueries.CreateTemplate(galaxy);
       
       

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
