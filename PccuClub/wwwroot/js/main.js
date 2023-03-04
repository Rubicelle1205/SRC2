$(document).ready(function () {
    //modal設定
    $(".model").on("click", function (e) {
        e.stopPropagation();
    });

    //modal關閉
    $("div").on("click", ".modal .btn_close_modal", function () {
        var modal = $(this).closest('.modal');
        modal_close(modal);
    });

    //modal關閉
    $("div").on("click", ".modal .modal-close", function () {
        var modal = $(this).closest('.modal');
        modal_close(modal);
    });

    //date time月曆設定 (Bootstrap Material DatePicker)
    $('.date_time_range').bootstrapMaterialDatePicker({
        format: 'YYYY/MM/DD HH:mm',
        lang: 'zh-tw',
        cancelText: '取消',
        okText: '確定',
        clearButton: true,
        clearText: '清除',
        switchOnClick: true,
        nowButton: false,
    });
    //date月曆設定 (Bootstrap Material DatePicker)
    $('.date_range').bootstrapMaterialDatePicker({
        format: 'YYYY/MM/DD',
        time: false,
        lang: 'zh-tw',
        cancelText: '取消',
        okText: '確定',
        clearButton: true,
        clearText: '清除',
        switchOnClick: true,
        nowButton: false,
    });

    ////手機版menu開啟
    $(".mobile-menu-btn").on('click', function (e) {
        $(this).addClass('active');
        $("body").addClass("mobile-menu-open");
        $(".mobile-menu-backdrop").addClass('active');
        $(".mobile-menu").animate({ 'marginLeft': 0 }, 'fast');
    });

    //手機版menu關閉
    $('.btn-close-menu').on('click', function (e) {
        $('.mobile-menu-btn').removeClass('active');
        $("body").removeClass("mobile-menu-open");
        $(".mobile-menu").animate({ 'marginLeft': -300 }, 'fast');
        $(".mobile-menu-backdrop").removeClass('active');

    });

    // 頁籤tab
    $(".tab").on("click", function (e) {
        //e.preventDefault();
        var robj = getRelatedObjByTab($(this));
        robj.siblingTabs.children().removeClass("active");
        robj.siblingTabDivs.removeClass("active");
        robj.thisTab.children().addClass("active");
        robj.thisTabDiv.addClass("active");
    });

    // 點擊更多按鈕
    $(document).on("click", ".btn-setting", function (e) {
        e.preventDefault();
        e.stopPropagation();
        $(this).toggleClass('active');
        var isActived = $(this).hasClass('active');
        if (isActived) {
            $(".setting-menu").hide();
            $(this).next($(".setting-menu")).show();
            $(".setting-btn").removeClass('active');
            $(this).addClass('active');
        } else {
            $(this).next($(".setting-menu")).hide();
        }
    });

    //點擊更多按鈕外的區域關閉dropdown
    $("body,.model").click(function () {
        $(".setting-menu").hide();
        $(".btn-setting").removeClass('active');
    });

    //小群組內的check-all全選
    $(".checkbox-col-all").on('click', '.check-all', function (e) {
        if ($(this).prop('checked')) {
            $(this).closest('.checkbox-col-all').find('.check-one').each(function (e) {
                $(this).prop('checked', true).change();
            });
        } else {
            $(this).closest('.checkbox-col-all').find('.check-one').each(function (e) {
                $(this).prop('checked', false).change();
            });
        }
    });

    //表格的check-all全選
    $("table").on('click', '.check-all', function (e) {
        if ($(this).prop('checked')) {
            $(this).closest('table').find('.check-one').each(function (e) {
                $(this).prop('checked', true).change();
            });
        } else {
            $(this).closest('table').find('.check-one').each(function (e) {
                $(this).prop('checked', false).change();
            });
        }
    });

    //使用者權限管理 -> 角色管理 -> 新增modal
    $(".btn_add_role").on('click', function (e) {
        modal_show($("#addRoleModal"));
        modelScroll();
    });

    //使用者權限管理 -> 帳號管理 -> 新增modal
    $(".btn_add_account").on('click', function (e) {
        modal_show($("#addAccountModal"));
        modelScroll();
    });

    //疫情研析報告 -> 新興或猖獗害蟲疫情 -> 意見補充modal
    $(".btn_edit_literature").on('click', function (e) {
        e.preventDefault();
        modal_show($("#editLiteratureModal"));
        modelScroll();
    });

    //文獻管理與瀏覽 -> 文獻管理 -> 編輯(資料庫)modal
    $(".btn_add_opinion").on('click', function (e) {
        e.preventDefault();
        modal_show($("#addOpinionModal"));
        modelScroll();
    });

    //文獻管理-編輯modal(資料庫)檔案上傳
    const fileInput = document.getElementById('fileInput');
    const fileTextBox = document.getElementById('fileTextBox');
    if (fileInput) {
        fileInput.addEventListener('change', function () {
            fileTextBox.value = this.files[0].name;
        });
    }

    //文獻管理與瀏覽 -> 文獻管理 -> 檢視modal
    $(".btn_view_literature").on('click', function (e) {
        e.preventDefault();
        modal_show($("#viewLiteratureModal"));
        modelScroll();
    });

    //文獻管理與瀏覽 -> 文獻管理 -> 歷程modal
    $(".btn_history").on('click', function (e) {
        e.preventDefault();
        modal_show($("#historyModal"));
        modelScroll();
    });

    //文獻管理與瀏覽 -> 文獻管理 -> 編輯(上傳)modal
    $(".btn_literature_upload").on('click', function (e) {
        e.preventDefault();
        modal_show($("#uploadLiteratureModal"));
        modelScroll();
    });

    //文獻管理-編輯modal(上傳)檔案上傳
    const UploadfileInput = document.getElementById('UploadfileInput');
    const UploadfileTextBox = document.getElementById('UploadfileTextBox');
    if (UploadfileInput) {
        UploadfileInput.addEventListener('change', function () {
            UploadfileTextBox.value = this.files[0].name;
        });
    }

    //疫情預警通知 -> 預警設定 -> 新增modal
    $(".btn_add_warning").on('click', function (e) {
        modal_show($("#addWarningModal"));
        modelScroll();
    });

    //關鍵字查詢重設按鈕
    $('.keywordReset').on('click', function (e,) {
        var TagAreaIdentity = $(this).attr('data-id');
        if (TagAreaIdentity == null)
            TagAreaIdentity = "Default";
        CleanKeywordTagArea(TagAreaIdentity);
        $('.keywordSelector.' + TagAreaIdentity).val('');
        $('.keywordSelector.' + TagAreaIdentity).trigger('change');
        $.ajax({
            method: 'POST',
            url: './Base/CleanKeywordTempData',
            data: { 'TagAreaIdentity': TagAreaIdentity },
            error: function (error) {
                alert(error.responseJSON.error);
            }
        });
    })

    //首頁tooltip顯示
    $(".tooltip").on('click', function (e) {
        $(this).toggleClass('active');
    });

    //tab 被hover show hide tooltip
    $(".btn-tab").hover(function (e) {
        var tooltip = $(this).next('.tab-tooltip');
        if (tooltip) {
            if (tooltip.text() != "") {
                tooltip.show();
                var right = ($(window).width() - ($(this).offset().left));
                if (right < 230) {
                    console.log(right);
                    tooltip.addClass('right-edge');
                }
            }
        }
    }, function (e) {
        var tooltip = $(this).next('.tab-tooltip');
        if (tooltip) {
            if (tooltip.text() != "") {
                tooltip.hide();
                var right = ($(window).width() - ($(this).offset().left));
                if (right < 230) {
                    tooltip.removeClass('right-edge');
                }
            }
        }
    });

    //點擊上傳按鈕開啟跳窗
    var uploadSuccess = true;
    $(".btn_upload").on('click', function () {
        modal_show($("#uploadModal"));
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

// 給定tab jQueryObject，回傳此tab相關物件
// 物件包含屬性:同層的tab、同層的tab對應的div、此tab、此tab對應的div (皆為jQueryObject)
function getRelatedObjByTab($theTab) {
    var lv = $theTab.parent();
    var $siblingTabDivs = lv.next().children(".tab-pane");
    var _robj =
    {
        siblingTabs: lv.children(".tab"), //同層的tab
        siblingTabDivs: $siblingTabDivs, //同層的tab對應的div
        thisTab: $theTab, //此tab
        thisTabDiv: $siblingTabDivs.eq($theTab.index()) //此tab對應的div
    };
    return _robj;
}

// 清除關鍵字查詢

function CleanKeywordTagArea(TagAreaIdentity) {
    $('#KeywordTagArea' + TagAreaIdentity).html('');
}

// Ajax Call back function
function SolventoAjax(options) {
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