using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTCore.ViewModel
{
    public class SearchModelInspectionReports
    {
        public string Division, District, BedCategory, SectorType, InspectionCategoryName, InspectionTypeName, CalendarYear, FiscalYear, Latest,OrgType;


    }
    public class SearchModelInspection {
        public int Inspection_Weeks_Seq, Inspection_Year_Seq;
    

    }
    public class SearchModelChallan
    {
        public string Challan_No, Challan_Reason_Name,  Challan_Status, Payment_Type, Type;
        public int page, pageSize, ModuleID, Challan_Source_SEQ, is_Internally_Verified;
        public DateTime? FromDate;
        public DateTime? ToDate;


    }
    public class SearchModel
    {
        public string search, Registration_Master_OID, FK_HOLD_Seq,FK_History,Licensing_Master_OID, Complaint_ID,PlanStatus,id,RegNo,HCEName;
        public int page, pageSize, FK_Application_Type, FK_Employee_Type_Seq,WeekID, Management_ID, Licensing_Management_Seq, User_Type_Seq, Management_Detail_Seq;
        public DateTime? ReceivingDate;
        public DateTime? FromDate;
        public DateTime? ToDate;
        public int? moduleID, AssignTo,SurveyType,InspectionCategoryOID;
        public Boolean? is_Active, is_blocked;
        public long? UserOID;
    }
    public class bedstrengthModel
    {

        public int? category_id, HCE_Type_ID, Payment_Type_ID, Limit;

    

    }
    public class PaginationModel {
        public int page, pageSize;
    }
    public class UserListSearchModel
    {
        public int page, pageSize,Access_Type_ID, Request_Status_Seq, User_Type_Seq, is_blocked, is_Active, User_Role_Seq, User_ID, is_Change_Password;
        public string AWF_Module_Seq, Full_Name,HCE, Module;
        
    }

    public class UserSearchModel
    {
        public int page, pageSize;
        public string Full_Name, is_blocked, is_Active;

    }
    public class SearchPerformanceModel {

        public string Assessment_Title;
        public int Assessment_Status_ID;
        public int? Performance_Assessment_SEQ;
        public string HCENameReg;
        public int pageSize;
        public int page;
        public DateTime? From_Due_Date;
        public DateTime? To_Due_Date;
        public DateTime? From_Submission_Date;
        public DateTime? To_Submission_Date;
        public int Assessment_Orientation_SEQ;


    }
    public class ReportPerformanceModel
    {
        public string fileName;
        public string fileTitle;
        public Boolean exportRequest;
        public int Performance_Assessment_SEQ;
        public string IndicatorType;
        public int Assessment_Status_ID;
        public string TabType;
        public int? pageSize;
        public int? page;
        public DateTime? From_Submission_Date;
        public DateTime? To_Submission_Date;
        


    }
    public class ExportHCEtoExcelModel
    {
        public string fileName;
        public string fileTitle, search, Licensing_Master_OID;
        public Boolean exportRequest;
        public int FK_Employee_Type_Seq;
       
        public int? pageSize;
        public int? page;
      



    }
    public class SearchRequestModel
    {
        public string HCE_Name, Registration_Master_OID;
        public string Licensing_Master_OID;
        public int? page, pageSize, RequestType, Request, RequestStatus, FK_Employee_Type_Seq;
        public string Datefrom;
        public string Dateto;


    }
    public class AssignToModel
    {
        public string assignStatus, Registration_OID;
        public int FKRole,FKUser;

    }
    public class AQCReportSearch
    {
        public string typeID { get; set; }
        public string Division { get; set; }
        public string District { get; set; }
        public string Tehsil { get; set; }
        public string Town { get; set; }
        public string Category { get; set; }
        public int RoleID { get; set; }        
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int RequestTypeID { get; set; }
        public string Type { get; set; }
        public string fileName { get; set; }
        public string fileTitle { get; set; }
        public Boolean exportRequest { get; set; }
        public string Datefrom { get; set; }
        public string Dateto { get; set; }
        public int UserID { get; set; }
        public string PlanCode { get; set; }
        public int? DistrictID { get; set; }
    }
    public class stringData
    {
        public string data;
    }
    public class addLUvalue {
        public string LUName;
        public int userId;

    }
    public class HCEConversationModel
    {
        public string FK_Licensing_Master_OID;
        public int? FKRole,UserOID,id;
        public long PKSubject;
    }
    public class PotentialHCESearchModel
    {
       
        public int? PotentialConfirmStatus, AssignTo, WeekID;
        public DateTime? FromDate, ToDate;
        public string Status;


    }
    public class SearchReturnChallan
    {
        public string Challan_No, Challan_Reason_Name, Challan_Status, Payment_Type;
        public int page, pageSize, ModuleID, Challan_Source_SEQ;
        public DateTime? FromDate;
        public DateTime? ToDate;
    }
    public class PaymentSearchModel
    {
        public DateTime? Receiving_date_From, Receiving_date_To, Instrument_date;
        public string Licensing_Master_OID, Registration_No, HCE_Name, App_ID, Instrument_No;
        public int? CurrentPage, PageSize, ModuleID, Payment_ID;
    }

    public class SearchChallan
    {
        public string ChallanNumber;
    }
    public class ActionModel
    {
        public string AssignedToStatus, FK_Registration_OID, FK_Licensing_Master__OID, Recommendation_Remarks;
        public int? Role_Assigned, Created_By, AppType;

    }

    public class HCERecommeModel
    {
        public string FK_Registration_OID, FK_Licensing_Master__OID, Remarks,Registration_No;
        public DateTime? Create_Date, Modified_Date, RC_Issue_Date, PL_Issue_Date,Issue_Date;
        public int HRD_Seq;
        public int? FK_Registration_Seq, FK_Licensing_Master_Seq, FK_LU_Recomendation_Seq, Application_User_Seq, Application_Role_Seq, Created_By, Modified_By;
        public Boolean? is_active, IsVerified;
    }
}