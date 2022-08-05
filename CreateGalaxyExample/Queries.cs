using ArchestrA.GRAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void  CreateTemplate(string name)
        {
            
        }
    }
}
