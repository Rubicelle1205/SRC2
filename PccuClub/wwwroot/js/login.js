$(document).ready(function(){
    //modal設定
    $(".model").on("click", function (e) {
        e.stopPropagation();
    });

    //modal關閉
    $(".modal").on("click", ".btn_close_modal", function () {
        var modal = $(this).closest('.modal');
        modal_close(modal);
    });

    //modal關閉
    $(".modal").on("click", ".modal-close", function () {
        var modal = $(this).closest('.modal');
        modal_close(modal);
    });

    //忘記密碼modal
    $(".btn-forget-psw").on('click', function (e) {
        modal_show($("#forgetPswModal"));
    });
});
//開啟modal
function modal_show(modal) {
    if (modal.hasClass('fade')) {
        modal.fadeIn(100);
        modal.addClass('in');

    } else {
        modal.show();
    }
}

//關閉modal
function modal_close(modal) {
    modal.hide().removeClass('in');
    $("body").removeAttr("style");
    $(".modal").each(function () {
        if (modal.is(":visible")) {
            modelScroll();
        }
    });
}

// 處理model scrollbar的問題
function modelScroll() {
    if (
        /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(
            navigator.userAgent
        )

    ) {
    } else {
        $("body").css({
            "overflow-y": "hidden",
            "padding-right": "16px"
        });
    }
}