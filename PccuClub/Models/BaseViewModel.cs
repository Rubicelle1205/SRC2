namespace WebPccuClub.Models
{
    public class BaseViewModel
    {
        public ColumnDataModel ColumnDataModel { get; set; }  
        
    }

    public class ColumnDataModel
    {
        public string ColumnName { get; set; }

        public string ColumnValue { get; set; }

        public bool IsDefault { get; set; }
    }
}
