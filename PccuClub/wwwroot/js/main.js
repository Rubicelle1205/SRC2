// Ajax Call back function
function SendAjax(options) {
    var settings = $.extend(true, {
        url: "",
        type: 'POST',
        data: null,
        success: null,
        beforeSend: function () {
            $.blockUI({
                message: "<i class='fa fa-spinner fa-pulse orange' style='font-size:600%'></i>",
                //borderWidth:'0px' 和透明背景
                css: { borderWidth: '0px', backgroundColor: 'transparent' },
            });
        },
        complete: function () {
            $.unblockUI();
        },
        error: function () {
            alert("系統發生錯誤，請稍後再嘗試。")
            setTimeout(function () {
                $.unblockUI();
            }, 2000)
        },
        global: true,
        async: true,
        headers: { token: (window.UserAuth != null) ? window.UserAuth.token : "" },
        timeout: 300000
    }, options || {});

    console.log(settings);
    return $.ajax(settings);
}