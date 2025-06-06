﻿using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using NPOI.POIFS.Crypt;
using PccuClub.WebAuth;
using System.Data;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using WebPccuClub.Global.Extension;
using WebPccuClub.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.ConstrainedExecution;
using MathNet.Numerics.RootFinding;

namespace WebPccuClub.DataAccess
{
    
    public class ClubActFinishDataAccess : BaseAccess
    {
        PublicFun PublicFun = new PublicFun();

        /// <summary> 查詢結果 </summary>
        public List<ClubActFinishResultModel> GetSearchResult(ClubActFinishConditionModel model, UserInfo LoginUser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            
            parameters.Add("@SchoolYear", model?.SchoolYear);
            parameters.Add("@ActVerify", model?.ActVerify);
            parameters.Add("@LoginId", LoginUser.LoginId);

            #endregion

            CommandText = $@"SELECT A.ActFinishId , A.ActID, B.ActDetailId, A.ActName, B.SchoolYear, A.ActFinishVerify, D.Text AS ActVerifyText, A.Created
                               FROM ActFinish A
                          LEFT JOIN ActDetail B ON B.ActID = A.ActID AND B.ActDetailId = A.ActDetailId
                          LEFT JOIN ActMain C ON C.ActID = B.ActID
                          LEFT JOIN Code D ON D.Code = A.ActFinishVerify AND D.Type = 'ActVerify'
                              WHERE 1 = 1
                                AND (@SchoolYear IS NULL OR B.SchoolYear = @SchoolYear)
                                AND (@LoginId IS NULL OR B.BrrowUnit = @LoginId)
                                AND (@ActVerify IS NULL OR D.Code = @ActVerify) 
                           ORDER BY A.Created DESC";


            (DbExecuteInfo info, IEnumerable<ClubActFinishResultModel> entitys) dbResult = DbaExecuteQuery<ClubActFinishResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ClubActFinishResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ClubActFinishDetailModel GetDetailData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ActFinishId", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.ActFinishId, A.ActID, A.ActDetailId, A.ClubId, A.ClubCName, A.Caseman, A.Email, A.Tel, A.ActDate, A.ActName, A.Course, 
                                    A.ShortInfo, A.ElseFile, A.ActFinishVerify, B.Text AS ActVerifyText, A.Memo
                               FROM ActFinish A
                          LEFT JOIN Code B ON B.Code = A.ActFinishVerify AND B.Type = 'ActVerify'
                              WHERE 1 = 1
                                AND (A.ActFinishId = @ActFinishId) ";


            (DbExecuteInfo info, IEnumerable<ClubActFinishDetailModel> entitys) dbResult = DbaExecuteQuery<ClubActFinishDetailModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary>
        /// 取得列印資料
        /// </summary>
        /// <param name="Ser"></param>
        /// <returns></returns>
        public ClubActFinishPrintModel GetPrintData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();


            #region 參數設定
            parameters.Add("@ActFinishPersonId", Ser);
            #endregion

            CommandText = $@"SELECT ActFinishPersonId, ActFinishId, Name, SNO, Department, Creator, Created, LastModifier, LastModified
                               FROM ActFinishPerson
                              WHERE 1 = 1
                                AND (ActFinishPersonId = @ActFinishPersonId) ";

            (DbExecuteInfo info, IEnumerable<ClubActFinishPrintModel> entitys) dbResult = DbaExecuteQuery<ClubActFinishPrintModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary>
        /// 取得列印資料
        /// </summary>
        /// <param name="Ser"></param>
        /// <returns></returns>
        public List<ClubActFinishPrintModel> GetPrintGroupData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();


            #region 參數設定
            parameters.Add("@ActFinishId", Ser);
            #endregion

            CommandText = $@"SELECT ActFinishPersonId, ActFinishId, Name, SNO, Department, Creator, Created, LastModifier, LastModified
                               FROM ActFinishPerson
                              WHERE 1 = 1
                                AND (ActFinishId = @ActFinishId) ";

            (DbExecuteInfo info, IEnumerable<ClubActFinishPrintModel> entitys) dbResult = DbaExecuteQuery<ClubActFinishPrintModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return null;
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ClubActFinishEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@ActFinishId", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"SELECT A.ActFinishId, A.ActID, A.ActDetailId, A.ClubId, A.ClubCName, A.Caseman, A.Email, A.Tel, A.ActDate, A.ActName, A.Course, 
                                    A.ShortInfo, A.ElseFile, A.ActFinishVerify, B.Text AS ActVerifyText
                               FROM ActFinish A
                          LEFT JOIN Code B ON B.Code = A.ActFinishVerify AND B.Type = 'ActVerify'
                              WHERE 1 = 1
                                AND (A.ActFinishId = @ActFinishId) ";


            (DbExecuteInfo info, IEnumerable<ClubActFinishEditModel> entitys) dbResult = DbaExecuteQuery<ClubActFinishEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 取得學號資料 </summary>
        public List<PersonModel> GetPersonData(string id)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@ActFinishId", id);

            #endregion

            CommandText = $@"SELECT A.ActFinishPersonId, A.ActFinishId, A.SNO
                               FROM ActFinishPerson A
                              WHERE 1 = 1
                                AND ActFinishId = @ActFinishId
                           ORDER BY A.ActFinishPersonId";


            (DbExecuteInfo info, IEnumerable<PersonModel> entitys) dbResult = DbaExecuteQuery<PersonModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<PersonModel>();
        }

        public List<SelectListItem> GetSchoolYear()
        {
            List<SelectListItem> LstItem = new List<SelectListItem>();

            int NowSchoolYear = int.Parse(PublicFun.GetNowSchoolYear());

            for (int i = NowSchoolYear - 2; i <= NowSchoolYear + 2; i++)
            {
                LstItem.Add(new SelectListItem() { Value = i.ToString(), Text = string.Format("{0}學年度", i) });
            }

            return LstItem;
        }

        public List<SelectListItem> GetActVerify()
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            #endregion

            CommandText = @"SELECT Code AS VALUE, Text AS TEXT FROM Code WHERE Type = 'ActVerify' AND Code <> '05'";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }

        public List<PersonModel> GetExportResult(string id)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@ActFinishId", id);

            #endregion

            CommandText = $@"SELECT A.SNO, A.Name
                               FROM ActFinishPerson A
                              WHERE 1 = 1
                                AND ActFinishId = @ActFinishId
                           ORDER BY A.ActFinishPersonId";


            (DbExecuteInfo info, IEnumerable<PersonModel> entitys) dbResult = DbaExecuteQuery<PersonModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<PersonModel>();
        }

        public List<ALLPersonModel> GetExportResult(string year, string verify, UserInfo LoginUser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@SchoolYear", year);
            parameters.Add("@ActVerify", verify);
            parameters.Add("@LoginId", LoginUser.LoginId);
            #endregion

            CommandText = $@"SELECT C.SchoolYear, B.ActID, B.ActName, A.SNO
                               FROM ActFinishPerson A
                          LEFT JOIN ActFinish B ON B.ActFinishId = A.ActFinishId
						  LEFT JOIN ActDetail C ON C.ActID = B.ActID AND C.ActDetailId = B.ActDetailId
						  LEFT JOIN Code D ON D.Code = B.ActFinishVerify AND D.Type = 'ActVerify'
                              WHERE 1 = 1
                                AND (@SchoolYear IS NULL OR C.SchoolYear = @SchoolYear)
                                AND (@LoginId IS NULL OR C.BrrowUnit = @LoginId)
                                AND (@ActVerify IS NULL OR D.Code = @ActVerify) 
                           ORDER BY B.Created, A.SNO ASC";


            (DbExecuteInfo info, IEnumerable<ALLPersonModel> entitys) dbResult = DbaExecuteQuery<ALLPersonModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<ALLPersonModel>();
        }

        public List<SelectListItem> GetActFinishNames(string id)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            #region 參數設定

            parameters.Add("@id", id);

            #endregion

            CommandText = @"SELECT ActFinishPersonId AS VALUE, Name + ' (' + SNO + ')' AS TEXT FROM ActFinishPerson WHERE ActFinishId = @id ";

            (DbExecuteInfo info, IEnumerable<SelectListItem> entitys) dbResult = DbaExecuteQuery<SelectListItem>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<SelectListItem>();
        }


    }
}
