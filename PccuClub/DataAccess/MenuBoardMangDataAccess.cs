using DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using NPOI.POIFS.Crypt;
using PccuClub.WebAuth;
using System.Data;
using System.Text.Encodings.Web;
using WebPccuClub.Global;
using WebPccuClub.Global.Extension;
using WebPccuClub.Models;

namespace WebPccuClub.DataAccess
{
    
    public class MenuBoardMangDataAccess : BaseAccess
    {

        /// <summary> 查詢結果 </summary>

        public List<MenuBoardMangResultModel> GetSearchResult(MenuBoardMangConditionModel model, string SystemCode)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@Header", model?.Header);
            parameters.Add("@ShortDesc", model?.ShortDesc);
            parameters.Add("@MenuBoardCode", SystemCode);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT MenuBoardId, MenuBoardCode, Header, ShortDesc, IconPath, IsEnable, Creator, Created, LastModifier, LastModified
FROM MenuBoardMang
WHERE 1 = 1
AND MenuBoardCode = @MenuBoardCode 
AND (@Header IS NULL OR Header LIKE '%' + @Header + '%') 
AND (@ShortDesc IS NULL OR ShortDesc LIKE '%' + @ShortDesc + '%') ";


            (DbExecuteInfo info, IEnumerable<MenuBoardMangResultModel> entitys) dbResult = DbaExecuteQuery<MenuBoardMangResultModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList();

            return new List<MenuBoardMangResultModel>();
        }

        /// <summary>
        /// 取得編輯資料
        /// </summary>
        /// <param name="submitBtn"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public MenuBoardMangEditModel GetEditData(string Ser)
        {
            string CommandText = string.Empty;
            DataSet ds = new DataSet();

            DBAParameter parameters = new DBAParameter();

            parameters.Add("@MenuBoardId", Ser);

            #region 參數設定
            #endregion

            CommandText = $@"
SELECT MenuBoardId, MenuBoardCode, Header, ShortDesc, IconPath, IsEnable, Creator, Created, LastModifier, LastModified
FROM MenuBoardMang
WHERE 1 = 1
AND (MenuBoardId = @MenuBoardId) ";


            (DbExecuteInfo info, IEnumerable<MenuBoardMangEditModel> entitys) dbResult = DbaExecuteQuery<MenuBoardMangEditModel>(CommandText, parameters, true, DBAccessException);

            if (dbResult.info.isSuccess && dbResult.entitys.Count() > 0)
                return dbResult.entitys.ToList().FirstOrDefault();

            return null;
        }

        /// <summary> 修改資料 </summary>
        public DbExecuteInfo UpdateData(MenuBoardMangViewModel vm, UserInfo LoginUser)
        {
            DbExecuteInfo ExecuteResult = new DbExecuteInfo();
            DBAParameter parameters = new DBAParameter();

            #region 參數設定
            parameters.Add("@MenuBoardId", vm.EditModel.MenuBoardId);
            parameters.Add("@Header", vm.EditModel.Header);
            parameters.Add("@ShortDesc", vm.EditModel.ShortDesc);
            parameters.Add("@LoginId", LoginUser.LoginId);

            if (!string.IsNullOrEmpty(vm.EditModel.IconPath))
                parameters.Add("@IconPath", vm.EditModel.IconPath);

            #endregion 參數設定

            string CommendText = $@"UPDATE MenuBoardMang 
                                       SET Header = @Header, ShortDesc = @ShortDesc,  %IconPath% LastModifier = @LoginId, LastModified = GETDATE()
                                     WHERE MenuBoardId = @MenuBoardId";

            if(vm.EditModel.RemoveIcon)
                CommendText = CommendText.Replace("%IconPath%", "IconPath = '', ");

            if (!string.IsNullOrEmpty(vm.EditModel.IconPath))
                CommendText = CommendText.Replace("%IconPath%", "IconPath = @IconPath,");
            else
                CommendText = CommendText.Replace("%IconPath%", string.Empty);

            ExecuteResult = DbaExecuteNonQuery(CommendText, parameters, false, DBAccessException);

            return ExecuteResult;
        }
    }
}
