using WebPccuClub.Global;

namespace WebPccuClub.Models
{
    public class ReturnViewModel
    {
        public ReturnViewModel() {
            ErrorCode = (int)DBActionChineseName.成功;
            ErrorMsg = "";
        }

        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }
    }
}
