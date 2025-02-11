var datetimepicker_option = {
    format: "YYYY-MM-DD HH:mm",
    applyLabel: "確定",
    cancelLabel: "取消",
    fromLabel: "開始日期",
    toLabel: "結束日期",
    customRangeLabel: "自訂日期區間",
    daysOfWeek: ["日", "一", "二", "三", "四", "五", "六"],
    monthNames: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"],
    firstDay: 1,
}

$(function () {

    if ((location.pathname).indexOf("HolisticPassportMang") >= 0)
    {
        if ($("[for=finish_time]").length > 0) {
            // 結案時間
            datetimepicker_option.format = "YYYY-MM-DD HH:mm";
            $('[for=finish_time]').daterangepicker({
                // minDate: now_time(),
                autoUpdateInput: false,
                singleDatePicker: true,
                showDropdowns: true,
                timePicker: true,
                timePicker24Hour: true,
                locale: datetimepicker_option,
            });
        }

        if ($("[for=finish_time2]").length > 0) {
            // 結案時間
            datetimepicker_option.format = "YYYY-MM-DD HH:mm";
            $('[for=finish_time2]').daterangepicker({
                // minDate: now_time(),
                autoUpdateInput: false,
                singleDatePicker: true,
                showDropdowns: true,
                timePicker: true,
                timePicker24Hour: true,
                locale: datetimepicker_option,
            });


            $('[for^=finish_time]').on('apply.daterangepicker', function (ev, picker) {
                $(this).val(picker.startDate.format('YYYY-MM-DD HH:mm'));
            });
        }
    }
    
})
