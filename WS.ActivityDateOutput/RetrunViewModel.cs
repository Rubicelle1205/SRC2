
namespace WebPccuClub.Models
{
    public class ReturnViewModel
    {
        public ReturnViewModel() {
            ErrorCode = 0;
            ErrorMsg = "";
        }

        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }
    }
}
