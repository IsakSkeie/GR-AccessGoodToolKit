using ArchestrA.GRAccess;
using System;
class Example
{
    [STAThread]
    static void Main()
    {
        string nodeName = Environment.MachineName; string galaxyName = "Example1";
        // create GRAccessAppClass
        GRAccessApp grAccess = new GRAccessAppClass();
        // try to get galaxy IGalaxies
        gals = grAccess.QueryGalaxies(nodeName);
        if (gals == null || grAccess.CommandResult.Successful == false)
        { Console.WriteLine(grAccess.CommandResult.CustomMessage + grAccess.CommandResult.Text); return; }
        IGalaxy galaxy = gals[galaxyName]; ICommandResult cmd;
        // create galaxy if it doesn't already exist
        if (galaxy == null)
        {
            grAccess.CreateGalaxy(galaxyName, nodeName, false, // no security
                                                                EAuthenticationMode.galaxyAuthenticationMode, "");
            cmd = grAccess.CommandResult;
            if (!cmd.Successful)
            {
                Console.WriteLine("Create Galaxy Named Example1 Failed: " + cmd.Text + " : " + cmd.CustomMessage);
                return;
            }
            galaxy = grAccess.QueryGalaxies(nodeName)[galaxyName];
        }
        // log in
        galaxy.Login("", "");
        cmd = galaxy.CommandResult;
        if (!cmd.Successful)
        {
            Console.WriteLine("Login to galaxy Example1 Failed :" + cmd.Text + " : " + cmd.CustomMessage); return;
        }
        // get the $UserDefined template
        string[] tagnames = { "$UserDefined" };
        IgObjects queryResult = galaxy.QueryObjectsByName(EgObjectIsTemplateOrInstance.gObjectIsTemplate, ref tagnames);
        cmd = galaxy.CommandResult;
        if (!cmd.Successful)
        {
            Console.WriteLine("QueryObjectsByName Failed for $UserDefined Template :" + cmd.Text + " : " + cmd.CustomMessage);
            return;
        }
        ITemplate userDefinedTemplate = (ITemplate)queryResult[1];
        // create an instance of $UserDefined, named with current time DateTime
        DateTime now = DateTime.Now;
        string instanceName = String.Format("sample_object_{0}_{1}_{2}", now.Hour.ToString("00"), now.Minute.ToString("00"), now.Second.ToString("00"));
        IInstance sampleinst = userDefinedTemplate.CreateInstance(instanceName, true);
        //How to edit the object ?
        sampleinst.CheckOut();
        sampleinst.AddUDA("Names", MxDataType.MxString, MxAttributeCategory.MxCategoryWriteable_USC_Lockable, MxSecurityClassification.MxSecurityOperate, true, 5);
        IAttributes attrs = sampleinst.ConfigurableAttributes;
        //Diplay first 5 attribute names from collection
        for (int i = 1; i <= 5; i++)
        {
            IAttribute attrb = attrs[i]; Console.WriteLine(attrb.Name);
        }
        IAttribute attr1 = attrs["Names"]; MxValue mxv = new MxValueClass();
        // we don't need to check that attribute is array type or not 
        // because we set it as array type when we addUDA. 
        // I am just showing example, you can do like this.
        if (attr1.UpperBoundDim1 > 0)
        {
            for (int i = 1; i <= attr1.UpperBoundDim1; i++)
            {
                MxValue mxvelement = new MxValueClass();
                mxvelement.PutString("string element number " + i.ToString());
                mxv.PutElement(i, mxvelement);
            }
            attr1.SetValue(mxv);
        }
        sampleinst.Save();
        sampleinst.CheckIn("Check in after addUDA");
        galaxy.Logout();
        Console.WriteLine();
        Console.Write("Press ENTER to quit: ");
        string dummy; dummy = Console.ReadLine();
    }
}

