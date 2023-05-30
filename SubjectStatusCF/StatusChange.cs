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
                Name: Status Change
                This CF should be used at(EC check/EC action/Derivation/Supportive CF): EC action
                Describtion: Change subject status based on INEX condition, DOS, and Study Completion Status
            */

            ActionFunctionParams afp = (ActionFunctionParams) ThisObject;
            DataPoint dpThis = afp.ActionDataPoint;
            Subject current_Subject = dpThis.Record.Subject;
            DataPoints dps = current_Subject.GetAllDataPoints();

            // inex
            DataPoint inexPass = dps.FindByFieldOID("IEELIGYN");
            DataPoint inexPassDT = dps.FindByFieldOID("ICCDT");
            // OP date
            DataPoint OPdate = dps.FindByFieldOID("SURGDTC");
            // SC status
            DataPoint studyCompletionStatus = dps.FindByFieldOID("SCSTATUS");

            // Screening = 1
            int changecode = 1;
            if (inexPass.Data == "N"){
                // Screen Failed = 2
                changecode = 2;
            }
            else if(inexPass.Data == "Y" && inexPassDT.StandardValue() is DateTime)
            {
                // Enrolled = 3
                changecode = 3;
            }
            if(OPdate != null)
            {
                if (OPdate.StandardValue() is DateTime)
                {
                    // Follow up = 7
                    changecode = 7;
                }
            }

            if (studyCompletionStatus != null)
            {
                string[] earlyterminatedSC = {"3", "4", "5", "6", "7", "8"};
                string[] screenfailSC = {"1"};
                string completedSC = "2";

                if (studyCompletionStatus.Data == completedSC)
                {
                    // Completed = 5
                    changecode = 5;
                }
                else if(Array.IndexOf(earlyterminatedSC, studyCompletionStatus.Data) > -1)
                {
                    // early terminated = 4
                    changecode = 4;
                }
                else if(Array.IndexOf(screenfailSC, studyCompletionStatus.Data) > -1)
                {
                    // Screen Failed = 2
                    changecode = 2;
                }
            }

            current_Subject.SubjectStatus = changecode;
            return true;
        }
    }
}
