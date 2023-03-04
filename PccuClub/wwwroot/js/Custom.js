// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


var LoadingDom = "<div class='loadingSpinner'><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div></div>";


$(function () {

    SysPageListPostBack();
});

function SysFormLoadingStart() { }
function SysFormLoadingEnd() { }


$(document).bind("ajaxSend", function () {


}).bind("ajaxSuccess", function () {

    SysPageListPostBack();

}).bind("ajaxError", function (event, xhr, settings, error) {


}).bind("ajaxComplete", function () {
    //$(".sysMmodal").fadeIn(100);
    //$(".sysMmodal").addClass('in');
});

// Alert Message
function ShowAlertMsgDialog() {

    $AlertMsg = $('.AlertMsgDialog:first');

    if ($AlertMsg.length > 0 && $AlertMsg.html() != "") {
        Swal.fire({
            title: $AlertMsg.attr('title'),
            html: $AlertMsg.html().replace('/\n/g', '<br>'),
            confirmButtonText: '確定',
            confirmButtonColor: '#8CD4F5',
        })
            .then(function () {
                $('.AlertMsgDialog:first').remove();
            })
            .then(function () {
                if ($('.AlertMsgDialog').length > 0) {
                    ShowAlertMsgDialog();
                }
            });
    }
}

// 取得 Url Parameter
function getUrlParams(Url) {
    if (Url && (i = Url.indexOf('?')) >= 0) {
        const queryString = Url.substring(i + 1);
        if (queryString) {
            let params = {};
            let vars = queryString.split('&');
            for (var i = 0; i < vars.length; i++) {
                var pair = vars[i].split('=');
                params[pair[0]] = decodeURIComponent(pair[1]);
            }
            return params;
        }
    }
    return {};
}

// 分頁模組PostBack
function SysPageListPostBack() {
    $('.PagePostBack').find('.pagination-container > .pagination > li > a').off('click').click(function () {
        let Url = $(this).attr('href');
        let param = getUrlParams(Url);
        let $ParentForm = $(this).parents('form:first');
        if (Url && $ParentForm.length == 1) {
            $.each(param, function (key, value) {
                $ParentForm.find("input[type='hidden'][name='" + key + "']").remove();
                $('<input>').attr({ type: 'hidden', id: key, name: key, value: value }).appendTo($ParentForm);
            });
            $ParentForm.submit();
            return false;
        }
    });
}


// submit And OpenWin
function SysOpenWin(Btn) {

    let $ParForm = $(Btn).parents('form:first');
    let OpenWinUrl = $ParForm.attr('action');
    let $target = $(event.target);
    if (!$target.is("button")) {
        $target = $target.parents("button:first");
    }

    let AjaxData = $ParForm.serializeArray();
    AjaxData[AjaxData.length] = { name: $target.attr('name'), value: $target.val() };

    $.ajax({
        url: OpenWinUrl,
        method: 'Post',
        data: AjaxData,
        cache: false,
        beforeSend: function (xhr) {

        },
        success: function (result) {
           
            $resultView = $(result);
            $('#MainContent').html($resultView);
            $(".sysModal").each(function (index) {
                if ($(this).hasClass('fade')) {
                    $(this).fadeIn(100);
                    $(this).addClass('in');

                } else {
                    $(this).show();
                }
            });
        },
        complete: function (msg) {

        }
    });
}
