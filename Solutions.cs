using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Common;
using System.Reflection;


namespace Fixit //omSupportBackup
{
    class Solutions
    {
        public Solutions()
        {
           
            PopulateCaseCatcherDict();
//            PopulateComboBoxOfCaseCatcher(cboCaseCatchers);

            PopulateSolutionsDict();
           // PopulateComboBoxOfSolutions(cboInsuranceChoices);
        }

        //Place SQL scripts in dictionaries. Public Methods to return scripts
        #region Case Catcher Setup
        
                Dictionary<string, string> dictCaseCatcher = new Dictionary<string, string>();

        public void PopulateCaseCatcherDict()
        {//todo: Code reaction when these return a record meeting the condition

            dictCaseCatcher.Add("00000523 Error 217887 when selecting patient's insurance ledger", @"SELECT [itmtrn_no]
                                                                                          ,[feeslip_no]
                                                                                          ,[slipitm_no]
                                                                                          ,[itmtrn_reason]
                                                                                          ,len(itmtrn_reason) as Length                                                                                          
                                                                                          FROM fee_slip_items_trans
                                                                                          WHERE len(itmtrn_reason) > 75
                                                                                          order by Length desc");

            
            //1/24/2014 CW Adding new solution to address illegal characters
            dictCaseCatcher.Add("00000726 Third Party Processing - Error 217900- Incorrect syntax", @"SELECT patient_no, address_no, address_no_old, guarantor_no, mid_name, last_name, suffix, title, nickname, employer, first_name
                                                                                                        FROM patient
                                                                                                        WHERE last_name LIKE '%[^a-z0-9-'']%'  OR last_name LIKE '%''%'");

            //This is my logic for the unknown ins condition: "SELECT feeslip_no FROM fee_slip_items WHERE slipitm_ins_total > 0 and insurance_no =0"
            //But the original logic is checking for HCFA items in another table
            //12/19/2013 CW Unknown Ins now has a special module bypassing this sql. See the class - FeeslipsWithUnknownIns 
            //12/23/2013 CW Modified the sql to ensure the fee slip was not voided.
            dictCaseCatcher.Add("00001466 Unknown Insurance on Aging Report", @"Select feeslip_no,slipitm_no, slipitm_ins_total, slipitm_pat_total 
                                                                                from Fee_Slip_Items
                                                                                WHERE slipitm_no NOT IN (SELECT slipitm_no FROM Fee_Slip_HCFA_CPT_Items) 
                                                                                AND slipitm_ins_total <> 0 
                                                                                AND feeslip_no NOT IN (SELECT feeslip_no FROM fee_slip WHERE void_date is not null)
                                                                                Order By feeslip_no, slipitm_no");


            dictCaseCatcher.Add("00001585 Send Statement is NOT checked", @"SELECT patient_no,last_name,first_name FROM patient WHERE send_statement = 0 AND send_to_collection = 0 AND patient_type = 0 Order by last_name");

            dictCaseCatcher.Add("00002737 Runtime Error -52 when opening Location Maintenance", @"SELECT LocDisplayName FROM Location WHERE LogoLink IS NOT NULL");            
            
            dictCaseCatcher.Add("00003081 Error 94 Invalid use of Null when going to Soft Rx or Hard Rx.", @"SELECT * FROM soft_contact_lens_rx where BASE_CURVE_MM IS NULL OR ADD_DIOPTER IS NULL ");

            //Just placeholder sql, since this solution requires the user to provide the Feeslip number 
            dictCaseCatcher.Add("00003085 Fee slip stuck on hold unable to record or delete", @"SELECT * FROM Fee_slip ");

            dictCaseCatcher.Add("00007563 EHR client reports not being able to send any demographic", @"SELECT patient_no,last_name,first_name FROM patient where creditcrd_no like '________________' or creditcrd_no like '___________________'");
            
            
            //todo: see dictSolution about needing solution 4668 to accompany sol 4347, to get the orphaned recs also
            //then, activate this
            //dictCaseCatcher.Add("00004347 Error 94 Invalid use of Null when trying to create new ophthalmic lense", @"SELECT Attribute_description FROM Attribute WHERE attribute_dev_cd is null and attribute_category_id = 10");            

            
            //CW 02/10/2014 Added MU, but will move to separate MU application
            //Need UI to obtain start/end date and provider ID
            //Need to confirm if this is the list of patients in the numerator or denominator
            //source is sp_MU_Calc_Med_Allergy
//            dictCaseCatcher.Add("00007001 MU-Meeting Core Measure 6", @"SELECT ex.PatId,p.first_name,p.last_name, ex.ExamRec, ex.RecID
//                                                                    FROM EWExam ex
//		                                                                     join Patient p
//		                                                                     on p.patient_no = ex.PatId		 
//		                                                                     WHERE 
//		                                                                    (convert(datetime,ex.visitdateYMD) >= '01/01/2001' AND convert(datetime,ex.visitdateYMD) <= '03/01/2014')
//		                                                                    AND ex.Provider_No = 1		
//		                                                                    AND ex.Closed <> 'D'
//		                                                                    AND ex.ExamBillable = 'B'");
                        

        }

        public string GetSQLFromCaseCatcher(string _solutionNumber)
        {
            string _sqlScript = "";

            foreach (var pair in this.dictCaseCatcher)
            {
                if (_solutionNumber == pair.Key)
                {
                    _sqlScript = pair.Value;
                    return pair.Value;
                }
            }
            return _sqlScript;
        }

        public  void PopulateComboBoxOfCaseCatcher(ComboBox obj)
        {
            #region the following code was used when run from the form pulling from the class using reflection
            //Solutions solutionList = new Solutions();
            //foreach (System.Reflection.MemberInfo mi in solutionList.GetType().GetFields())
            //{
            //    obj.Items.Add(mi.Name.ToString());
            //}
            #endregion
            //now fill the combobox from the dictionary in this class
            foreach (string s in dictCaseCatcher.Keys)                
            {
                obj.Items.Add(s);
            }
        }
        #endregion  

        //Place SQL scripts in dictionaries. Public Methods to return scripts
        #region Fix it Setup
        public void PopulateComboBoxOfSolutions(ComboBox obj)
        {

            #region the following code was used when run from the form pulling from the class using reflection
            //Solutions solutionList = new Solutions();
            //foreach (System.Reflection.MemberInfo mi in solutionList.GetType().GetFields())
            //{
            //    obj.Items.Add(mi.Name.ToString());
            //}
            #endregion
            foreach (string s in dictSolutions.Keys)
            {
                obj.Items.Add(s);
            }
        }

        Dictionary<string, string> dictSolutions = new Dictionary<string, string>();
        public void PopulateSolutionsDict()
        {
            
            //These are solutions that will not present a UI to the user, and will run unattended.

            dictSolutions.Add("00000238 iBal - Run the iBal Application", @"SELECT SERVERPROPERTY('ServerName')");//just valid sql to get through the findandfix, which looks for sql code. We will just launch ibal.exe

            //A UI form exists for this solution #1579 ; This is just valid sql to get through the findandfix, which looks for sql code. This requires user interaction
            dictSolutions.Add("00001579 Error 94: Invalid Use of Null clicking Insurance Tab in Patient Demographic",
                                    @"SELECT pins.insurance_no, ins.insurance_name 
                                    FROM patient_insurances pins
                                    INNER JOIN insurance ins
                                    ON
                                    pins.insurance_no = ins.insurance_no
                                    WHERE patient_no =2");


            dictSolutions.Add("00002737 Runtime Error -52 when opening Location Maintenance", @"UPDATE Location SET LogoLink = NULL");

            //The following solution has a very long script and is encompassed in a region. Move out later.
            //dictSolutions.Add("00003050 Error when trying click F2-Find in the Appointment Scheduler")
            dictSolutions.Add("00003050 Error when trying click F2-Find in the Appointment Scheduler",
            #region StoredProcedure [dbo].[Scheduler_FindPatients_Get]
                             @"ALTER PROCEDURE [dbo].[Scheduler_FindPatients_Get]
                                  @LastName   NVARCHAR(30) = NULL,
                                  @FirstName  NVARCHAR(40) = NULL,
                                  @NickName   NVARCHAR(40) = NULL,
                                  @SSN        NVARCHAR(11) = NULL,
                                  @Address    NVARCHAR(40) = NULL,
                                  @DOB        DATETIME = NULL,
                                  @LocationID INT = NULL,
                                  @City       NVARCHAR(40) = NULL,
                                  @Homephone  NVARCHAR(30) = NULL,
                                  @PatientID  INT = NULL,
                                  @ChartNo    NVARCHAR(50) = NULL,
                                  @IncludeInactive  BIT = NULL

                            AS
                            BEGIN
                                  SET NOCOUNT ON;
                                  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

                                  IF @IncludeInactive IS NULL 
                                        SET @IncludeInactive = 'False'

                                  (SELECT dbo.patient.last_name
                                              ,dbo.patient.first_name
                                              ,dbo.patient.mid_name
                                              ,dbo.patient.nickname
                                              ,dbo.patient.suffix
                                              ,dbo.patient.chart_no
                                              ,dbo.patient.patient_no
                                              ,dbo.address.address1
                                              ,dbo.patient.LocationID
                                              ,dbo.patient.use_guarantor_address
                                              ,dbo.patient.last_exam_date
                                              ,dbo.patient.patient_type
                                              ,ga.address1 AS Guarantor_address1
                                              ,ga.city AS Guarantor_city
                                              ,dbo.Location.LocDisplayName
                                              ,ga.phone1 AS Guarantor_phone1
                                              ,dbo.address.city
                                              ,dbo.address.phone1
                                              ,dbo.patient.ss_no
                                              ,dbo.patient.birth_date
                                              ,dbo.patient.active
                                              ,dbo.patient.HIPAA_ReadNotice
                                              ,dbo.patient.HIPAA_ModifyForm
                                  FROM    dbo.patient WITH (NOLOCK) 
                                              LEFT OUTER JOIN dbo.Location WITH (NOLOCK) ON dbo.Location.LocationId = dbo.patient.LocationID
                                              LEFT OUTER JOIN dbo.address WITH (NOLOCK) ON dbo.patient.address_no = dbo.address.address_no 
                                              LEFT OUTER JOIN dbo.patient AS g WITH (NOLOCK) ON dbo.patient.guarantor_no = g.patient_no 
                                              INNER JOIN dbo.address AS ga WITH (NOLOCK) ON g.address_no = ga.address_no 
                                  WHERE (dbo.patient.use_guarantor_address = 'True') AND (dbo.patient.patient_type = 0 OR
                                        dbo.patient.patient_type = 4)
                                              AND COALESCE(patient.last_name, '') =(
                                                    case when (@LastName is not null) then (
                                                          case when @LastName = '?' and len(COALESCE(patient.last_name, '')) > 0 then
                                                                COALESCE(patient.last_name, '')
                                                          else (
                                                                case when COALESCE(patient.last_name, '') LIKE @LastName + '%' then
                                                                      COALESCE(patient.last_name, '')
                                                                else 
                                                                      NULL
                                                                end)
                                                          end)
                                                    else
                                                          COALESCE(patient.last_name, '')
                                                    end )
                                              AND COALESCE(patient.first_name, '') =(
                                                    case when (@FirstName is not null) then (
                                                          case when @FirstName = '?' and len(COALESCE(patient.first_name, '')) > 0 then
                                                                COALESCE(patient.first_name, '')
                                                          else (
                                                                case when COALESCE(patient.first_name, '') LIKE @FirstName + '%' then
                                                                      COALESCE(patient.first_name, '')
                                                                else 
                                                                      NULL
                                                                end)
                                                          end)
                                                    else
                                                          COALESCE(patient.first_name, '')
                                                    end )
                                              AND COALESCE(patient.nickname, '') =(
                                                    case when (@NickName is not null) then (
                                                          case when @NickName = '?' and len(COALESCE(patient.nickname, '')) > 0 then
                                                                COALESCE(patient.nickname, '')
                                                          else (
                                                                case when COALESCE(patient.nickname, '') LIKE @NickName + '%' then
                                                                      COALESCE(patient.nickname, '')
                                                                else 
                                                                      NULL
                                                                end)
                                                          end)
                                                    else
                                                          COALESCE(patient.nickname, '')
                                                    end )
                                              AND COALESCE(patient.ss_no, '') =(
                                                    case when (@SSN is not null) then (
                                                          case when @SSN = '?' and len(COALESCE(patient.ss_no, '')) > 0 then
                                                                COALESCE(patient.ss_no, '')
                                                          else (
                                                                case when COALESCE(patient.ss_no, '') LIKE @SSN + '%' then
                                                                      COALESCE(patient.ss_no, '')
                                                                else 
                                                                      NULL
                                                                end)
                                                          end)
                                                    else
                                                          COALESCE(patient.ss_no, '')
                                                    end )
                                              AND COALESCE(ga.address1, '') =(
                                                    case when (@Address is not null) then (
                                                          case when @Address = '?' and len(COALESCE(ga.address1, '')) > 0 then
                                                                COALESCE(ga.address1, '')
                                                          else (
                                                                case when COALESCE(ga.address1, '') LIKE @Address + '%' then
                                                                      COALESCE(ga.address1, '')
                                                                else 
                                                                      NULL
                                                                end)
                                                          end)
                                                    else
                                                          COALESCE(ga.address1, '')
                                                    end )
                                              AND COALESCE(patient.birth_date, 0) =(
                                                    case when (@DOB is not null) then (
                                                          case when @DOB = patient.birth_date then
                                                                patient.birth_date
                                                          else 
                                                                NULL
                                                          end)
                                                    else
                                                          COALESCE(patient.birth_date, 0)
                                                    end )
                                              AND COALESCE(patient.LocationID, 0) =(
                                                                case when (@LocationID is not null) then (
                                                                      case when COALESCE(patient.LocationID, 0) = @LocationID then
                                                                            COALESCE(patient.LocationID, 0)
                                                                      else 
                                                                            NULL
                                                                      end)
                                                                else
                                                                      COALESCE(patient.LocationID, 0)
                                                                end ) 
                                              AND COALESCE(address.city, '') =(
                                                    case when (@City is not null) then (
                                                          case when @City = '?' and len(COALESCE(address.city, '')) > 0 then
                                                                address.city
                                                          else (
                                                                case when COALESCE(address.city, '') LIKE @City + '%' then
                                                                      COALESCE(address.city, '')
                                                                else 
                                                                      NULL
                                                                end)
                                                          end)
                                                    else
                                                          COALESCE(address.city, '')
                                                    end )
                                              AND COALESCE(ga.phone1, '') =(
                                                    case when (@Homephone is not null) then (
                                                          case when @Homephone = '?' and len(COALESCE(ga.phone1, '')) > 0 then
                                                                COALESCE(ga.phone1, '')
                                                          else (
                                                                case when COALESCE(ga.phone1, '') LIKE @Homephone + '%' then
                                                                      COALESCE(ga.phone1, '')
                                                                else 
                                                                      NULL
                                                                end)
                                                          end)
                                                    else
                                                          COALESCE(ga.phone1, '')
                                                    end )
                                              AND patient.patient_no =(
                                                    case when (@PatientID is not null) then (
                                                          case when @PatientID = patient.patient_no then
                                                                patient.patient_no
                                                          else
                                                                NULL
                                                          end)
                                                    else
                                                          patient.patient_no
                                                    end )
                                              AND COALESCE(patient.chart_no, '') =(
                                                    case when (@ChartNo is not null) then (
                                                          case when @ChartNo = '?' and len(COALESCE(patient.chart_no, '')) > 0 then
                                                                COALESCE(patient.chart_no, '')
                                                          else (
                                                                case when COALESCE(patient.chart_no, '') LIKE @ChartNo + '%' then
                                                                      COALESCE(patient.chart_no, '')
                                                                else 
                                                                      NULL
                                                                end)
                                                          end)
                                                    else
                                                          COALESCE(patient.chart_no, '')
                                                    end )
                                              AND patient.active =(
                                                    case when (@IncludeInactive = 'False') then (
                                                          case when patient.active = 1 then
                                                                patient.active
                                                          else 
                                                                NULL
                                                          end)
                                                    else
                                                          patient.active
                                                    end ))
                                  UNION
                                  (SELECT patient_1.last_name
                                              ,patient_1.first_name
                                              ,patient_1.mid_name
                                              ,patient_1.nickname
                                              ,patient_1.suffix
                                              ,patient_1.chart_no
                                              ,patient_1.patient_no
                                              ,address_1.address1
                                              ,patient_1.LocationID
                                              ,patient_1.use_guarantor_address
                                              ,patient_1.last_exam_date
                                              ,patient_1.patient_type
                                              ,ga.address1 AS Guarantor_address1
                                              ,ga.city AS Guarantor_city
                                              ,Location_1.LocDisplayName
                                              ,ga.phone1 AS Guarantor_phone1
                                              ,address_1.city
                                              ,address_1.phone1
                                              ,patient_1.ss_no
                                              ,patient_1.birth_date
                                              ,patient_1.active
                                              ,patient_1.HIPAA_ReadNotice
                                              ,patient_1.HIPAA_ModifyForm
                                  FROM    dbo.patient AS patient_1 WITH (NOLOCK) 
                                              LEFT OUTER JOIN dbo.Location AS Location_1 WITH (NOLOCK) ON Location_1.LocationId = patient_1.LocationID
                                              LEFT OUTER JOIN dbo.address AS address_1 WITH (NOLOCK) ON patient_1.address_no = address_1.address_no 
                                              LEFT OUTER JOIN dbo.patient AS g WITH (NOLOCK) ON patient_1.guarantor_no = g.patient_no 
                                              INNER JOIN dbo.address AS ga WITH (NOLOCK) ON g.address_no = ga.address_no 
                                  WHERE (patient_1.use_guarantor_address = 'False') AND (patient_1.patient_type = 0 OR
                                        patient_1.patient_type = 4)
                                              AND COALESCE(patient_1.last_name, '') =(
                                                    case when (@LastName is not null) then (
                                                          case when @LastName = '?' and len(COALESCE(patient_1.last_name, '')) > 0 then
                                                                COALESCE(patient_1.last_name, '')
                                                          else (
                                                                case when COALESCE(patient_1.last_name, '') LIKE @LastName + '%' then
                                                                      COALESCE(patient_1.last_name, '')
                                                                else 
                                                                      NULL
                                                                end)
                                                          end)
                                                    else
                                                          COALESCE(patient_1.last_name, '')
                                                    end )
                                              AND COALESCE(patient_1.first_name, '') =(
                                                    case when (@FirstName is not null) then (
                                                          case when @FirstName = '?' and len(COALESCE(patient_1.first_name, '')) > 0 then
                                                                COALESCE(patient_1.first_name, '')
                                                          else (
                                                                case when COALESCE(patient_1.first_name, '') LIKE @FirstName + '%' then
                                                                      COALESCE(patient_1.first_name, '')
                                                                else 
                                                                      NULL
                                                                end)
                                                          end)
                                                    else
                                                          COALESCE(patient_1.first_name, '')
                                                    end )
                                              AND COALESCE(patient_1.nickname, '') =(
                                                    case when (@NickName is not null) then (
                                                          case when @NickName = '?' and len(COALESCE(patient_1.nickname, '')) > 0 then
                                                                COALESCE(patient_1.nickname, '')
                                                          else (
                                                                case when COALESCE(patient_1.nickname, '') LIKE @NickName + '%' then
                                                                      COALESCE(patient_1.nickname, '')
                                                                else 
                                                                      NULL
                                                                end)
                                                          end)
                                                    else
                                                          COALESCE(patient_1.nickname, '')
                                                    end )
                                              AND COALESCE(patient_1.ss_no, '') =(
                                                    case when (@SSN is not null) then (
                                                          case when @SSN = '?' and len(COALESCE(patient_1.ss_no, '')) > 0 then
                                                                COALESCE(patient_1.ss_no, '')
                                                          else (
                                                                case when COALESCE(patient_1.ss_no, '') LIKE @SSN + '%' then
                                                                      COALESCE(patient_1.ss_no, '')
                                                                else 
                                                                      NULL
                                                                end)
                                                          end)
                                                    else
                                                          COALESCE(patient_1.ss_no, '')
                                                    end )
                                              AND COALESCE(ga.address1, '') =(
                                                    case when (@Address is not null) then (
                                                          case when @Address = '?' and len(COALESCE(ga.address1, '')) > 0 then
                                                                COALESCE(ga.address1, '')
                                                          else (
                                                                case when COALESCE(ga.address1, '') LIKE @Address + '%' then
                                                                      COALESCE(ga.address1, '')
                                                                else 
                                                                      NULL
                                                                end)
                                                          end)
                                                    else
                                                          COALESCE(ga.address1, '')
                                                    end )
                                              AND COALESCE(patient_1.birth_date, 0) =(
                                                    case when (@DOB is not null) then (
                                                          case when @DOB = patient_1.birth_date then
                                                                patient_1.birth_date
                                                          else 
                                                                NULL
                                                          end)
                                                    else
                                                          COALESCE(patient_1.birth_date, 0)
                                                    end )
                                              AND COALESCE(patient_1.LocationID, 0) =(
                                                                case when (@LocationID is not null) then (
                                                                      case when COALESCE(patient_1.LocationID, 0) = @LocationID then
                                                                            COALESCE(patient_1.LocationID, 0)
                                                                      else 
                                                                            NULL
                                                                      end)
                                                                else
                                                                      COALESCE(patient_1.LocationID, 0)
                                                                end ) 
                                              AND COALESCE(address_1.city, '') =(
                                                    case when (@City is not null) then (
                                                          case when @City = '?' and len(COALESCE(address_1.city, '')) > 0 then
                                                                address_1.city
                                                          else (
                                                                case when COALESCE(address_1.city, '') LIKE @City + '%' then
                                                                      COALESCE(address_1.city, '')
                                                                else 
                                                                      NULL
                                                                end)
                                                          end)
                                                    else
                                                          COALESCE(address_1.city, '')
                                                    end )
                                              AND COALESCE(ga.phone1, '') =(
                                                    case when (@Homephone is not null) then (
                                                          case when @Homephone = '?' and len(COALESCE(ga.phone1, '')) > 0 then
                                                                COALESCE(ga.phone1, '')
                                                          else (
                                                                case when COALESCE(ga.phone1, '') LIKE @Homephone + '%' then
                                                                      COALESCE(ga.phone1, '')
                                                                else 
                                                                      NULL
                                                                end)
                                                          end)
                                                    else
                                                          COALESCE(ga.phone1, '')
                                                    end )
                                              AND patient_1.patient_no =(
                                                    case when (@PatientID is not null) then (
                                                          case when @PatientID = patient_1.patient_no then
                                                                patient_1.patient_no
                                                          else
                                                                NULL
                                                          end)
                                                    else
                                                          patient_1.patient_no
                                                    end )
                                              AND COALESCE(patient_1.chart_no, '') =(
                                                    case when (@ChartNo is not null) then (
                                                          case when @ChartNo = '?' and len(COALESCE(patient_1.chart_no, '')) > 0 then
                                                                COALESCE(patient_1.chart_no, '')
                                                          else (
                                                                case when COALESCE(patient_1.chart_no, '') LIKE @ChartNo + '%' then
                                                                      COALESCE(patient_1.chart_no, '')
                                                                else 
                                                                      NULL
                                                                end)
                                                          end)
                                                    else
                                                          COALESCE(patient_1.chart_no, '')
                                                    end )
                                              AND patient_1.active =(
                                                    case when (@IncludeInactive = 'False') then (
                                                          case when patient_1.active = 1 then
                                                                patient_1.active
                                                          else 
                                                                NULL
                                                          end)
                                                    else
                                                          patient_1.active
                                                    end ))
                                  ORDER BY last_name, first_name, mid_name, suffix
                            RETURN;
                            END
                            ");
            #endregion

            dictSolutions.Add("00003081 Error 94 Invalid use of Null when going to Soft Rx or Hard Rx.", @"UPDATE
                                    soft_contact_lens_rx
                                    SET
                                    BASE_CURVE_MM = ISNULL(BASE_CURVE_MM , 0),
                                    ADD_DIOPTER = ISNULL(ADD_DIOPTER, 0)");
            

            dictSolutions.Add("00003127 Error 217833 for all unity lenses and Reveal Lenses when recording and invoicing an RX/eyewear order",
                                    @"ALTER TABLE VSPLabRx 
                                    ALTER COLUMN Lid nvarchar(MAX) NULL ");

            //TODO: the solution below also requires  solution 00004668 to remove the orphaned records caused by duplicate products
            //dictSolutions.Add("00004347 Error 94 Invalid use of Null when trying to create new ophthalmic lense", @"update Attribute set attribute_dev_cd = 0 
            //                        WHERE attribute_dev_cd is null and attribute_category_id = 10");

            
           //TODO: solution 5656 below needs to take in a feeslip as a parameter unless a condition is found
           //dictSolutions.Add("00005656 Patient open charges have changed, please recreate fee slip when recording a fee slip", @"update fee_slip set edit_state ='0'where feeslip_no ='29246'");
            
            
            
            dictSolutions.Add("00004668 Product name already exists", @"SELECT SERVERPROPERTY('ServerName')");//A UI form exists for this solution; This is just valid sql to get through the findandfix, which looks for sql code. This requires user interaction

            dictSolutions.Add("00005979 Zero out on-order quantities",@"UPDATE product_loc_details SET prddtl_qty_on_order=0 WHERE (prddtl_qty_on_order > 0)");

            dictSolutions.Add("00006912 Crizal Avance UV CPT code changes to V2755 from v2750 once on fee slip and VSP insurance is selected.", @"TRUNCATE product_vsp set HCPCS_CODES='V2750' , OptionCD='QT'
                                where OptionCD='BV' and (OtherAddonsDesc = 'Crizal Alize UV (AR Coating C)' or OtherAddonsDesc = 'Crizal Alize UV')
                                update vsp_addon set lensCatOptionKey='QT' where addOnDesc='Crizal Alize UV' and lensCatOptionKey='BV'
                                update vsp_addon set lensCatOptionKey='QN' where addOnDesc='Crizal Easy UV' and lensCatOptionKey='BV'
                                update vsp_addon set lensCatOptionKey='QV' where addOnDesc='Crizal Avance UV' and lensCatOptionKey='BV'
                                ");

            dictSolutions.Add("00007698 Reset Client User ID/Password for Updox. Office locked out of Secure Messaging Portal", @"truncate table portaluser");


        }

        public string GetSQLFromSolution(string _solutionNumber)
        {

            string _sqlScript = "";

            foreach (var pair in dictSolutions)
            {
                if (_solutionNumber == pair.Key)
                {
                    _sqlScript = pair.Value;
                    return pair.Value;
                }
            }
            return _sqlScript;

        }
        #endregion
        
    }
}
