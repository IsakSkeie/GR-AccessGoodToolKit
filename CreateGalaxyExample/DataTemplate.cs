﻿using ArchestrA.GRAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateGalaxyExample
{
    class UDATemplate
    {

        public string Names { get; set; }
        public MxDataType DataType { get; set; }
        public MxAttributeCategory Category {get; set;}
        public MxSecurityClassification Security { get; set; }
        public bool IsArray { get; set; }
        public object ArrayElementCount { get; set; }

        public UDATemplate(string _name, string _DataType, string _Desc)
        {
            Names = _name;
            DataType = FindType(_DataType);
            Category = MxAttributeCategory.MxCategoryUndefined;
            Security = MxSecurityClassification.MxSecurityUndefined;
            IsArray = false;
            ArrayElementCount = 1;

        }

        public MxDataType FindType(string _DataType)
        {
         
            switch (_DataType)
            {
                case "BOOL":
                    return MxDataType.MxBoolean;
                case "DINT":
                    return MxDataType.MxInteger;
                case "REAL":
                    return MxDataType.MxFloat;
                case "lot_no_String":
                    return MxDataType.MxString;
                default:
                    return MxDataType.MxDataTypeUnknown;
            }
        }

        public MxAttributeCategory FindCategoryType(string _ExternalAccess)
        {
            switch (_ExternalAccess)
            {
                //Needs to define several acces levels
                default:
                    return MxAttributeCategory.MxCategoryUndefined;
            }
                 
        }

        public MxSecurityClassification FindSecurity(string _Security)
        {
            switch (_Security)
            {
                default:
                    return MxSecurityClassification.MxSecurityFreeAccess;
            }
        }
    }
}
