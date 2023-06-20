using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
using Medidata.Core.Objects;
using Medidata.Core.Common;
using Medidata.Core.Common.Utilities;
using Medidata.Utilities;
using Medidata.Utilities.Interfaces;
using System.Configuration;
using System.IO;
using Medidata;


namespace CustomFunctions
{
    public class CustomFunctions1
    {
        public object mainfunc(object ThisObject){
			/*
                Name: Get_A_Unique_Field
                This CF should be used at(EC check/EC action/Derivation/Supportive CF): Supportive CF
                Describtion: get all required information to get a unique active field.
                Inputs: [
                    string fieldOID
                    string formOID
                    string folderOID
                    Subject
                    bool activeOnly
                ]
                outputs: Datapoint
            */
            object[] inputs = (object[]) ThisObject;
            if (inputs == null || inputs.Length < 5 || 
                !(inputs[0] is string fieldOID) || 
                !(inputs[1] is string formOID) || 
                !(inputs[2] is string folderOID) || 
                !(inputs[3] is Subject currSub) ||
                !(inputs[4] is bool activeOnly)
                ) return 0;

            DataPoints foundDatapoints = CustomFunction.FetchAllDataPointsForOIDPath(fieldOID,formOID,folderOID,currSub,activeOnly);
            if(foundDatapoints.Count == 0) return null;
            if(foundDatapoints.Count == 1) return foundDatapoints[0];
            // in this point, we have several datapoints in a OID path..
            else return foundDatapoints[0];
        }

        public object getAUniqueField(string fieldOID, string formOID, string folderOID, Subject currSub, bool activeOnly){
            /*
                Name: Get_A_Unique_Field
                This CF should be used at(EC check/EC action/Derivation/Supportive CF): Supportive CF (embedded version)
                Describtion: get all required information to get a unique active field.
                Inputs: 
                    string fieldOID
                    string formOID
                    string folderOID
                    Subject
                    bool activeOnly
                outputs: Datapoint
            */
            DataPoints foundDatapoints = CustomFunction.FetchAllDataPointsForOIDPath(fieldOID,formOID,folderOID,currSub,activeOnly);
            if(foundDatapoints.Count == 0) return null;
            if(foundDatapoints.Count == 1) return foundDatapoints[0];
            // in this point, we have several datapoints in a OID path..
            else return foundDatapoints[0];
        }
    }
}
