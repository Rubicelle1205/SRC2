var type_arr = {
    "意外事件": {
        "交通意外事件": "",
        "中毒事件": "",
        "自傷、自殺事件": "",
        "溺水事件": "",
        "運動、休閒事件": "",
        "實驗、實習及環境設施事件": "",
        "其他意外傷害事件": "",
    },
    "安全維護事件": {
        "性侵害、性騷擾或性霸凌事件": "性平事件子系統、霸凌事件子系統",
        "家庭暴力事件": "",
        "身心障礙事件": "",
        "火警": "",
        "人為破壞事件": "",
        "校園失竊事件": "",
        "糾紛事件": "",
        "校屬人員遭侵害事件": "",
        "資訊安全": "",
        "詐騙事件": "",
        "其他校園安全維護事件": "",
        "疑涉犯兒童及少年性剝削防制條例第 4 章所定之罪": "",
    },
    "暴力事件與偏差行為": {
        "疑似霸凌事件": "霸凌事件子系統",
        "霸凌事件": "霸凌事件子系統",
        "暴力偏差行為": "",
        "疑涉違法事件": "",
        "藥物濫用事件": "",
        "干擾校園安全及事務": "",
        "其他校園暴力或偏差行為": "",
    },
    "管教衝突事件": {
        "親師生衝突事件": "",
        "校務行政管教衝突事件": "",
        "其他有關管教衝突事件": "",
        "教師不當管教造成學生身心嚴重侵害之確認事件": "",
        "教師不當管教造成學生身心嚴重侵害之疑似事件": "",
        "教師不當管教造成學生身心輕微侵害事件": "",
        "其他教師不當管教學生事件(非體罰或違法處罰)": "",
        "疑似校長及教職員工對學生霸凌事件": "",
        "校長及教職員工對學生霸凌事件": "",
    },
    "兒童少年保護事件(未滿18歲)": {
        "性侵害、性騷擾或性霸凌事件": "性平事件子系統、霸凌事件子系統",
        "藥物濫用事件": "",
        "強迫引誘自殺行為": "",
        "其他兒少保護事件": "",
        "兒童及少年遭性剝削或疑似遭受性剝削事件": "",
        "家庭暴力事件": "",
    },
    "天然災害": {
        "天然災害": "",
        "其他重大災害": "",
        "環境災害": "",
    },
    "疾病事件": {
        "法定傳染病": "",
        "一般傳染病": "",
    },
    "其他事件": {
        "校務相關問題": "",
        "其他的問題": "",
    },
    "校內管制事件": {
        "校內管制事件": "",
    }
}
var type_gender_arr = {
    "大分類一": {
        "小分類1": "",
        "小分類2": "",
    },
    "大分類二": {
        "小分類A": "",
        "小分類B": "",
    }
}
var status_gender_arr = {
    "狀態一": "停用",
    "狀態二": "啟用",
    "狀態三": "啟用",
    "已受理": "啟用",
    "第一次延長": "啟用",
    "第二次延長": "啟用",
    "已結案": "啟用"
}
var unit_arr = [
    "生活輔導組",
    "課外活動組",
    "衛生保健組",
    "軍訓室職員",
    "學生諮商中心"
]
var permission_arr = {
    "前台功能": {},
    "後台功能": {
        "校安事件管理": ["case_list.html", true],
        "匯入校安事件清單": ["case_xls_mang.html", true],
        "校安事件主類別": ["type_list.html", true],
        "校安事件次類別": ["subtype_list.html", true],
        "轉介單位清單": ["unit_list.html", true],
        "轉介內容歷程": ["case_unit_list.html", true],
        "匯入轉介內容歷程": ["case_xls_mang.html", true],
        "性平事件管理": ["case_gender_list.html", false],
        "匯入性平事件清單": ["case_gender_xls_mang.html", false],
        "性平事件主類別": ["type_gender_list.html", false],
        "性平事件次類別": ["subtype_gender_list.html", false],
        "性平事件狀態維護": ["status_gender_list.html", false],
        "霸凌事件管理": ["case_bully_list.html", false],
        "匯入霸凌事件清單": ["case_bully_xls_mang.html", false],
        "霸凌事件主類別": ["type_bully_list.html", false],
        "霸凌事件次類別": ["subtype_bully_list.html", false],
        "霸凌事件狀態維護": ["status_bully_list.html", false],
        // "使用者帳號綁定": ["user_list.html", false],
        "身分權限設定": ["role_list.html", false],
        "管理員維護": ["admin_list.html", false],
        "個人資料": ["me_mang.html", false],
    },
}
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

    if ((location.pathname).indexOf("EventCaseMang") >= 0 || (location.pathname).indexOf("EventGenderMang") >= 0 || (location.pathname).indexOf("EventBullyingMang") >= 0
        || (location.pathname).indexOf("BorrowMainClassMang") >= 0 || (location.pathname).indexOf("BorrowRecordMang") >= 0 )
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

        if ($("[for=finish_date]").length > 0) {
            // 結案時間
            datetimepicker_option.format = "YYYY-MM-DD";
            $('[for=finish_date]').daterangepicker({
                // minDate: now_time(),
                autoUpdateInput: false,
                singleDatePicker: true,
                showDropdowns: true,
                timePicker: false,
                timePicker24Hour: false,
                locale: datetimepicker_option,
            });


            $('[for^=finish_date]').on('apply.daterangepicker', function (ev, picker) {
                $(this).val(picker.startDate.format('YYYY-MM-DD'));
            });
        }
    }
    


    if ((location.pathname).indexOf("admin") >= 0) {
        //###############以下為通用fun#######################//

        //避免任何場合下ENTER送出
        $(document).on('keypress', 'form', function (e) {
            var code = e.keyCode || e.which;
            if (code == 13 && !$(e.target).is('textarea,input[type="submit"],input[type="button"]')) {
                (e.preventDefault) ? e.preventDefault() : e.returnValue = false;
                return false;
            }
        });

        $('[data-toggle="tooltip"]').tooltip()

        // 下拉選單如果有 default這屬性, 代表要預選, 一頁可能有多個select..., 所以用each
        // 如: <select defaults="value">
        $("select[defaults]").each(function () {
            var defaults = $(this).attr("defaults")
            $(this).find("option").each(function () {
                if ($(this).val() === defaults) {
                    $(this).prop('selected', true);
                }
            })
        })

        //###############以下為特定頁面fun#######################//
        if ((location.pathname).indexOf("admin_mang.") >= 0 || (location.pathname).indexOf("me_mang.") >= 0) {

            // 密碼驗證
            $('[name=password]').blur(function () {
                var result = validatePasswd('[name=password]', '#passwdRules', 6, 15)
                if (!result) {
                    $(this).addClass('is-invalid');
                    $('[name=post]').prop('disabled', true);
                } else {
                    $(this).removeClass('is-invalid');
                    $('[name=post]').prop('disabled', false);
                }
            })

            // 密碼 明文、密文
            $('[id^=pwvisible]').click(function () {
                var password = $(this).parents('td').find('input[name^=password]');
                var type = password.attr("type");

                if (type === "text") {
                    $(this).html('<i class="fa-solid fa-eye-slash"></i>');
                    password.attr('type', 'password');
                } else if (type === "password") {
                    $(this).html('<i class="fa-solid fa-eye"></i>');
                    password.attr('type', 'text');
                }
            });

            // 表單重整
            $('button[type=reset]').click(function () {
                $.each($('[id^=code-warning]'), function () {
                    $(this).text('')
                });

                $.each($('.is-invalid'), function () {
                    $(this).removeClass('is-invalid')
                });
            })

        }

        if ((location.pathname).indexOf("user_mang.") >= 0) {
            // 密碼 明文、密文
            $('[id^=pwvisible]').click(function () {
                var password = $(this).parents('td').find('input[name^=password]');
                var type = password.attr("type");

                if (type === "text") {
                    $(this).html('<i class="fa-solid fa-eye-slash"></i>');
                    password.attr('type', 'password');
                } else if (type === "password") {
                    $(this).html('<i class="fa-solid fa-eye"></i>');
                    password.attr('type', 'text');
                }
            });
        }

        // 校安事件主類別
        if ($("[name=type_id]").length > 0) {
            var is_list = (location.pathname).indexOf("_list.") >= 0;
            var main_type = is_list ? "全部事件主類別" : "請選擇事件主類別";
            var main_subtype = is_list ? "全部事件次類別" : "請選擇事件次類別";

            var is_update = getUrlParam("model") === "update";
            var defaults_type = ""
            if (is_update) {
                defaults_type = "意外事件"
            } else if (is_list) {
                defaults_type = getUrlParam("type_id")
            }

            $("[name=type_id]").html(genType(type_arr, defaults_type, main_type));
            if ($("[name=subtype_id]").length > 0) {
                $(document).on("change", "[name=type_id]", function () {
                    var type = $('option:selected', this).val();
                    var defaults_subtype = is_update ? "交通意外事件" : "";
                    $("[name=subtype_id]").html(genSubtype(type_arr, type, defaults_subtype, main_subtype));
                });

                $("[name=type_id]").change();
            }
        }

        // 性平事件主類別
        if ($("[name=type_gender_id]").length > 0) {
            var is_list = (location.pathname).indexOf("_list.") >= 0;
            var main_type = is_list ? "全部事件主類別" : "請選擇事件主類別";
            var main_subtype = is_list ? "全部事件次類別" : "請選擇事件次類別";

            var is_update = getUrlParam("model") === "update";
            var defaults_type = ""
            if (is_update) {
                defaults_type = "大分類一"
            } else if (is_list) {
                defaults_type = getUrlParam("type_id")
            }

            $("[name=type_gender_id]").html(genType(type_gender_arr, defaults_type, main_type));
            if ($("[name=subtype_gender_id]").length > 0) {
                $(document).on("change", "[name=type_gender_id]", function () {
                    var type = $('option:selected', this).val();
                    var defaults_subtype = is_update ? "小分類1" : "";
                    $("[name=subtype_gender_id]").html(genSubtype(type_gender_arr, type, defaults_subtype, main_subtype));
                });

                $("[name=type_gender_id]").change();
            }
        }

        // 霸凌事件主類別
        if ($("[name=type_bully_id]").length > 0) {
            var is_list = (location.pathname).indexOf("_list.") >= 0;
            var main_type = is_list ? "全部事件主類別" : "請選擇事件主類別";
            var main_subtype = is_list ? "全部事件次類別" : "請選擇事件次類別";

            var is_update = getUrlParam("model") === "update";
            var defaults_type = ""
            if (is_update) {
                defaults_type = "大分類一"
            } else if (is_list) {
                defaults_type = getUrlParam("type_id")
            }

            $("[name=type_bully_id]").html(genType(type_gender_arr, defaults_type, main_type));
            if ($("[name=subtype_bully_id]").length > 0) {
                $(document).on("change", "[name=type_bully_id]", function () {
                    var type = $('option:selected', this).val();
                    var defaults_subtype = is_update ? "小分類1" : "";
                    $("[name=subtype_bully_id]").html(genSubtype(type_gender_arr, type, defaults_subtype, main_subtype));
                });

                $("[name=type_bully_id]").change();
            }
        }

        // 性平事件狀態、霸凌事件狀態
        if ($("[name=status]").length > 0) {
            var option = '';
            $.each(status_gender_arr, function (status, is_useful) {
                if (is_useful === "啟用") {
                    option += '<option value="' + status + '">' + status + '</option>\n';
                }
            });

            $("[name=status]").html(option);
        }

        // 轉介單位
        if ($("[name=unit_id]").length > 0) {
            var option = '<option value="">全部轉介單位</option>\n';
            $.each(unit_arr, function (key, unit) {
                option += '<option value="' + unit + '">' + unit + '</option>\n';
            });

            $("[name=unit_id]").html(option);
        }


        // 身分權限
        if ($("[name=permission_id]").length > 0) {
            var option = '<option value="">全部權限</option>\n';
            $.each(permission_arr, function (side, items) {
                $.each(items, function (page, data) {
                    option += '<option value="' + data[0] + '">' + side + '-' + page + '</option>\n';
                });
            });

            $("[name=permission_id]").html(option);
        }

        if ((location.pathname).indexOf("/role_mang.") >= 0) {
            var checkbox = '';
            $.each(permission_arr, function (side, items) {
                if (Object.keys(items).length > 0) {
                    checkbox += '<h5>' + side + '</h5>\n';
                    checkbox += '<div class="row p-2">\n';
                    $.each(items, function (title, item) {
                        var checked = item[1] ? "checked" : "";
                        checkbox += '<div class="col-md-3 custom-control custom-checkbox">\n';
                        checkbox += '<input class="custom-control-input custom-control-input-primary custom-control-input-outline" type="checkbox" id="' + side + title + item[0] + '" value="' + item[0] + '" title="' + title + '" ' + checked + '>\n';
                        checkbox += '<label class="custom-control-label" for="' + side + title + item[0] + '">' + title + '</label>\n';
                        checkbox += '</div>\n';
                    })
                    checkbox += '</div>\n';
                }
            })

            $("#permission_td").html(checkbox)

        }

        if ((location.pathname).indexOf("/type_list.") >= 0) {
            var row = '';
            $.each(type_arr, function (type) {
                row += '<tr>\n';
                row += '<td>' + type + '</td>\n';
                row += '<td></td>\n';
                row += '<td>\n';
                row += '<a href="type_mang.html?model=update" class="mx-1 btn btn-sm btn-outline-primary">編輯</a>\n';
                row += '<button type="button" class="ml-1 btn btn-sm btn-outline-danger" name="box_del">刪除\n';
                row += '</button>\n';
                row += '<a href="subtype_list.html?type_id=' + type + '" class="mx-1 btn btn-sm btn-outline-dark">編輯次類別</a>\n';
                row += '</td>\n';
                row += '</tr>\n';
            });
            $("tbody").html(row);
        }

        if ((location.pathname).indexOf("/subtype_list.") >= 0) {
            var row = '';
            $.each(type_arr, function (type, items) {
                $.each(items, function (subtype, import_sys) {
                    row += '<tr>\n';
                    row += '<td>' + type + '</td>\n';
                    row += '<td>' + subtype + '</td>\n';
                    row += '<td>' + import_sys + '</td>\n';
                    row += '<td></td>\n';
                    row += '<td>\n';
                    row += '<a href="subtype_mang.html?model=update" class="mx-1 btn btn-sm btn-outline-primary">編輯</a>\n';
                    row += '<button type="button" class="ml-1 btn btn-sm btn-outline-danger" name="box_del">刪除\n';
                    row += '</button>\n';
                    row += '</td>\n';
                    row += '</tr>\n';
                })
            });
            $("tbody").html(row);
        }

        if ((location.pathname).indexOf("/type_gender_list.") >= 0) {
            var row = '';
            $.each(type_gender_arr, function (type) {
                row += '<tr>\n';
                row += '<td>' + type + '</td>\n';
                row += '<td></td>\n';
                row += '<td>\n';
                row += '<a href="type_gender_mang.html?model=update" class="mx-1 btn btn-sm btn-outline-primary">編輯</a>\n';
                row += '<button type="button" class="ml-1 btn btn-sm btn-outline-danger" name="box_del">刪除\n';
                row += '</button>\n';
                row += '<a href="subtype_gender_list.html?type_id=' + type + '" class="mx-1 btn btn-sm btn-outline-dark">編輯次類別</a>\n';
                row += '</td>\n';
                row += '</tr>\n';
            });
            $("tbody").html(row);
        }

        if ((location.pathname).indexOf("/subtype_gender_list.") >= 0) {
            var row = '';
            $.each(type_gender_arr, function (type, items) {
                $.each(items, function (subtype) {
                    row += '<tr>\n';
                    row += '<td>' + type + '</td>\n';
                    row += '<td>' + subtype + '</td>\n';
                    row += '<td></td>\n';
                    row += '<td>\n';
                    row += '<a href="subtype_gender_mang.html?model=update" class="mx-1 btn btn-sm btn-outline-primary">編輯</a>\n';
                    row += '<button type="button" class="ml-1 btn btn-sm btn-outline-danger" name="box_del">刪除\n';
                    row += '</button>\n';
                    row += '</td>\n';
                    row += '</tr>\n';
                })
            });
            $("tbody").html(row);
        }

        if ((location.pathname).indexOf("/type_bully_list.") >= 0) {
            var row = '';
            $.each(type_gender_arr, function (type) {
                row += '<tr>\n';
                row += '<td>' + type + '</td>\n';
                row += '<td></td>\n';
                row += '<td>\n';
                row += '<a href="type_bully_mang.html?model=update" class="mx-1 btn btn-sm btn-outline-primary">編輯</a>\n';
                row += '<button type="button" class="ml-1 btn btn-sm btn-outline-danger" name="box_del">刪除\n';
                row += '</button>\n';
                row += '<a href="subtype_bully_list.html?type_id=' + type + '" class="mx-1 btn btn-sm btn-outline-dark">編輯次類別</a>\n';
                row += '</td>\n';
                row += '</tr>\n';
            });
            $("tbody").html(row);
        }


        if ((location.pathname).indexOf("/subtype_bully_list.") >= 0) {
            var row = '';
            $.each(type_gender_arr, function (type, items) {
                $.each(items, function (subtype) {
                    row += '<tr>\n';
                    row += '<td>' + type + '</td>\n';
                    row += '<td>' + subtype + '</td>\n';
                    row += '<td></td>\n';
                    row += '<td>\n';
                    row += '<a href="subtype_bully_mang.html?model=update" class="mx-1 btn btn-sm btn-outline-primary">編輯</a>\n';
                    row += '<button type="button" class="ml-1 btn btn-sm btn-outline-danger" name="box_del">刪除\n';
                    row += '</button>\n';
                    row += '</td>\n';
                    row += '</tr>\n';
                })
            });
            $("tbody").html(row);
        }

        if ((location.pathname).indexOf("/status_gender_list.") >= 0) {
            // 性平事件狀態不可被刪除
            var row = '';
            $.each(status_gender_arr, function (status, is_useful) {
                var useful_class = (is_useful === "停用") ? "text-danger" : "text-success";
                row += '<tr>\n';
                row += '<td>' + status + '</td>\n';
                row += '<td class="' + useful_class + '">' + is_useful + '</td>\n';
                row += '<td></td>\n';
                row += '<td>\n';
                row += '<a href="status_gender_mang.html?model=update" class="mx-1 btn btn-sm btn-outline-primary">編輯</a>\n';
                // row += '<button type="button" class="ml-1 btn btn-sm btn-outline-danger" name="box_del">刪除\n';
                row += '</button>\n';
                row += '</td>\n';
                row += '</tr>\n';
            });
            $("tbody").html(row);
        }

        if ((location.pathname).indexOf("/status_bully_list.") >= 0) {
            // 霸凌事件狀態不可被刪除
            var row = '';
            $.each(status_gender_arr, function (status, is_useful) {
                var useful_class = (is_useful === "停用") ? "text-danger" : "text-success";
                row += '<tr>\n';
                row += '<td>' + status + '</td>\n';
                row += '<td class="' + useful_class + '">' + is_useful + '</td>\n';
                row += '<td></td>\n';
                row += '<td>\n';
                row += '<a href="status_bully_mang.html?model=update" class="mx-1 btn btn-sm btn-outline-primary">編輯</a>\n';
                // row += '<button type="button" class="ml-1 btn btn-sm btn-outline-danger" name="box_del">刪除\n';
                row += '</button>\n';
                row += '</td>\n';
                row += '</tr>\n';
            });
            $("tbody").html(row);
        }

        if ((location.pathname).indexOf("case_list.") >= 0) {
            // 下載各單位空白轉介紀錄表
            if ($('[name=download_excel]').length > 0) {
                $('[name=download_excel]').click(function (e) {
                    e.preventDefault();
                    window.open('../uploads/excels/轉介單位紀錄表_生活輔導組.xlsx');
                    window.open('../uploads/excels/轉介單位紀錄表_衛生保健組.xlsx');
                })
            }
        }

        if ($("[name=happen_time],[name=notify_time],[name=accept_time],[name=finish_time]").length > 0) {
            // 事件時間、知悉時間、受理時間、結案時間
            $('[name=happen_time],[name=notify_time],[name=accept_time],[name=finish_time]').daterangepicker({
                // minDate: now_time(),
                autoUpdateInput: false,
                singleDatePicker: true,
                showDropdowns: true,
                timePicker: true,
                timePicker24Hour: true,
                locale: datetimepicker_option,
            });

            $('[name=happen_time],[name=notify_time],[name=accept_time],[name=finish_time]').on('apply.daterangepicker', function (ev, picker) {
                $(this).val(picker.startDate.format('YYYY-MM-DD HH:mm'));
            });
        }

        if ($("[name=now_time]").length > 0) {
            // 事件時間、知悉時間
            $('[name=now_time]').daterangepicker({
                minDate: getISODateTime(now_time(), "yyyy-MM-dd HH:mm"),
            });
        }

        if ((location.pathname).indexOf("case_") >= 0) {
            if ($("#peo_table").length > 0) {
                var len = $("#peo_table tbody tr").length;

                $("[name=add_row]").click(function () {
                    len += 1
                    var new_row = addRow(len);
                    $("#peo_table tbody").append(new_row);
                })

                $("[name=clean_all]").click(function () {
                    var event_arr = [];
                    event_arr["success"] = function () {
                        len = 1;
                        var new_row = addRow(len);
                        $("#peo_table tbody").html(new_row);
                    }
                    showConfirm(event_arr);
                })

                $(document).on("click", "[name=delete_row]", function () {
                    var this_tr = $(this).closest("tr");
                    var event_arr = [];
                    event_arr["success"] = function () {
                        this_tr.remove(); // 模擬刪除
                    }
                    showConfirm(event_arr);
                })

                //var gender_arr = ["男", "女", "其他"]; // 性別
                //var status_arr = ["死亡", "重傷", "輕傷", "失蹤", "正常", "疾病"]; // 狀態
                //var title_arr = ["學生", "教師", "教官", "職員", "校外人士", "其他", "校長(園長)", "實驗學生(短期進修)", "廠商", "教保員(助理教保員)"]; // 職稱
                //var unit_arr = ["本校一、二級行政單位。", "本校一、二級學術單位。", "校外人員。"]; // 所屬單位
                //var location_arr = ["醫院", "學校", "警局", "在家中", "安置中", "其他", "不明", "集中檢疫所"]; // 目前位置
                //var char_arr = ["受害者", "行為人/肇事者", "關係人", "其他(無法定義者)", "朋友", "協調者", "疾患人員", "工友", "互為行為人(疑似合意)"]; // 角色

                //function addRow(num) {
                //    var tr = '<tr>\n';
                //    tr += '<th scope="col">' + num + '</th>\n';
                //    tr += '<td>\n';
                //    tr += genSelectOptions("peo_gender" + num, "性別", gender_arr);
                //    tr += '</td>\n';
                //    tr += '<td><input type="text" class="form-control form-control-sm" name="peo_name' + num + '" placeholder="請輸入姓名"></td>\n';
                //    tr += '<td>\n';
                //    tr += genSelectOptions("peo_status" + num, "狀態", status_arr);
                //    tr += '</td>\n';
                //    tr += '<td>\n';
                //    tr += genSelectOptions("peo_title" + num, "職稱", title_arr);
                //    tr += '</td>\n';
                //    tr += '<td>\n';
                //    tr += genSelectOptions("peo_unit" + num, "所屬單位", unit_arr);
                //    tr += '</td>\n';
                //    tr += '<td><input type="text" class="form-control form-control-sm" name="peo_num' + num + '" placeholder="請輸入學號"></td>\n';
                //    tr += '<td>\n';
                //    tr += '<select class="form-control form-control-sm bd_y" name="peo_year' + num + '">\n';
                //    tr += '<option value="" selected>請選擇出生年</option>\n';
                //    tr += genBirthYear();
                //    tr += '</select>\n';
                //    tr += '</td>\n';
                //    tr += '<td>\n';
                //    tr += genSelectOptions("peo_location" + num, "目前位置", location_arr);
                //    tr += '</td>\n';
                //    tr += '<td>\n';
                //    tr += genSelectOptions("peo_char" + num, "角色", char_arr);
                //    tr += '</td>\n';
                //    tr += '<td>\n';
                //    tr += '<button type="button" class="btn btn-sm btn-outline-danger" name="delete_row"><i class="fa-solid fa-x"></i>\n';
                //    tr += '</button>\n';
                //    tr += '</td>\n';
                //    tr += '</tr>\n';

                //    return tr;
                //}

                //if ($(".bd_y").length > 0) {
                //    // 年
                //    var year_defaults = $('.bd_y').attr('defaults'); // 選擇的項目
                //    var option = genBirthYear(year_defaults);
                //    $('.bd_y').append(option);
                //}

                //function genBirthYear(defaults) {
                //    var str = "";
                //    var y = parseInt(getISODateTime(now_time(), "yyyy")) + 1 - 1911;
                //    for (var i = 0; i <= 100; i++) {
                //        y = y - 1;
                //        var selected = '';
                //        if (y == defaults) {
                //            selected = 'selected';
                //        }

                //        str += '<option value="' + y + '" ' + selected + '>民國' + y + '年</option>';
                //    }

                //    return str;
                //}

                //function genSelectOptions(name = "", title = "", arr = []) {
                //    var str = ''
                //    str += '<select class="form-control form-control-sm" name="' + name + '">\n';
                //    str += '<option value="" selected>請選擇' + title + '</option>\n';
                //    $.each(arr, function (i, item) {
                //        str += '<option value="' + item + '">' + item + '</option>\n';
                //    });
                //    str += '</select>\n';

                //    return str
                //}
            }

            // 是否結案。若是，則須填寫結案時間
            if ($("[name=is_finish]").length > 0) {
                $("[name=is_finish]").change(function () {
                    var val = $("option:selected", this).val();
                    if (val === "是") {
                        $("[name=finish_time]").attr("req", "Y").parents('div:first').removeClass("hide").find('.required').text('*');
                    } else {
                        $("[name=finish_time]").removeAttr("req").parents('div:first').addClass("hide").find('.required').text('');
                    }
                }).change();
            }


            // 是否受理。若是，則須填寫結案時間
            if ($("[name=is_accept]").length > 0) {
                $("[name=is_accept]").change(function () {
                    var val = $("option:selected", this).val();
                    if (val === "是") {
                        $("[name=accept_time]").attr("req", "Y").parents('div:first').removeClass("hide").find('.required').text('*');
                    } else {
                        $("[name=accept_time]").removeAttr("req").parents('div:first').addClass("hide").find('.required').text('');
                    }
                }).change();
            }

            var now_format = getISODateTime(now_time(), "yyyy-MM-dd HH:mm:ss")
            $("[name=now_time]").val(now_format);

            // 新增事件紀錄
            $("#add_history").click(function () {
                var time = $("[name=now_time]").val().replace("T", " ");
                var status = $("[name=status]").val();
                var history = $("[name=history]").val();
                if (history !== "") {
                    var list = '<li class="list-group-item p-1">';
                    list += getISODateTime(time, "yyyy/MM/dd HH:mm");
                    list += '<span class="badge badge-primary mx-1">' + status + '</span> ' + history + '\n';
                    list += '</li>\n';

                    $(".list-group").append(list);

                    // 新增成功後，重整記錄時間+清空紀錄內容
                    now_format = getISODateTime(now_time(), "yyyy-MM-dd HH:mm:ss")
                    $("[name=now_time]").val(now_format);
                    $("[name=history]").val("");

                    showAlert("新增成功!")
                } else {
                    showAlert("請輸入本次紀錄內容", "red")
                }
            })

            // 清除當前輸入的事件紀錄內容
            $("#clean_history").click(function () {
                var history = $("[name=history]").val();
                if (history !== "") {
                    var event_arr = [];
                    event_arr["success"] = function () {
                        $("[name=history]").val("");

                        // 清除內容後，重整記錄時間
                        now_format = getISODateTime(now_time(), "yyyy-MM-dd HH:mm:ss")
                        $("[name=now_time]").val(now_format);
                    }
                    showConfirm(event_arr, "確定要清空內容嗎?", "清空內容無法復原");
                }
            })
        }

        //###############以下為新增及編輯頁面 fun#######################//
        if ((location.pathname).indexOf("_mang") >= 0) {

            //如果送出後，因為有必填沒填寫而被返回上一頁時，預覽圖會消失，所以要讓預覽圖再出現
            setTimeout(function () {
                $("textarea").each(function () {
                    var b64 = $(this).val();
                    if (b64.indexOf("base64") >= 0 && b64.indexOf("data:image") >= 0) {
                        $(this).prev().css('background-image', 'url("' + b64 + '")')
                    }
                })
            }, 500);


            // 刪除圖片, 刪除附件用
            $(document).on("click", "[file_sql]", function () {
                if (confirm('確定刪除?')) {
                    // 找到元素中的update sql語法
                    // 圖片刪除僅會將資料庫欄位更新為空
                    var cmd = $(this).attr("file_sql")
                    var temp = gettoken_value();
                    var value = temp.value;
                    var token = temp.token;
                    document.location.href = "sp_command.php?cmd=" + cmd + "&value=" + value + "&token=" + token + "";
                }
            });

            // 檢查必填
            $("button[name=post]").click(function (e) {
                var sum_arr = [];
                var $btn = $(this);
                var link = $btn.data("link");
                $btn.prev('button:reset').prop('disabled', true);
                $btn.prop('disabled', true).append(" <span class=\"spinner-grow spinner-grow-sm mx-1\" role=\"status\" aria-hidden=\"true\"></span>");
                var this_form = Boolean($btn.parents('form')) ? $btn.parents('form') : $('#' + $btn.attr('form'));
                this_form.find(".is-invalid").each(function () {
                    $(this).removeClass('is-invalid');
                })

                // 取得所有包含rep(必填)的元素
                this_form.find("[req]:not([readonly],[disabled])").each(function () {
                    var title = $.trim($(this).attr("data-title"));
                    // var model = $('[name=model]').val();
                    var model = getUrlParam("model") !== "" ? getUrlParam("model") : "add";
                    var name = $(this).attr('name');

                    if ($(this).attr('type') === 'file') {
                        var files = $(this).prop('files');
                        $(this).addClass('is-invalid');
                        if (model === "add") {
                            // 新增模式 如果files長度為0
                            if (files.length <= 0) {
                                sum_arr.push("請檢查「" + title + "」是否已上傳?");
                            }
                        } else if (model === "update") {
                            // 編輯模式 如果data('file')為空且files長度為0
                            if (aes_decrypt($(this).data('file')) === "" && files.length <= 0) {
                                sum_arr.push("請檢查「" + title + "」是否已上傳?");
                            }
                        }
                    } else if ($(this).attr('type') === "radio") {
                        var value = $('input:radio[name=' + $(this).attr("name") + ']:checked').val();
                        if (value === "" || value === undefined) {
                            sum_arr.push("請勾選一項「" + title + "」");
                        }
                    } else if ($(this).attr('type') === "checkbox") {
                        var checked = $("input[type=checkbox][name=" + $(this).attr("name") + "]:checked").length;
                        if (!checked) {
                            sum_arr.push("請勾選一項「" + title + "」");
                        }
                    } else {
                        if ($(this).val() === "") {
                            sum_arr.push("請檢查「" + title + "」是否填寫?");
                            $(this).addClass('is-invalid');
                        } else {
                            if ($(this).attr('type') === 'email' && !validateEmail($(this).val())) {
                                sum_arr.push("電子信箱格式錯誤");
                                $(this).addClass('is-invalid');
                            } else if ($(this).attr('name') === 'mobile' && !ValidateMobile($(this).val())) {
                                sum_arr.push("手機格式錯誤");
                                $(this).addClass('is-invalid');
                            } else if ($(this).attr('name') === 'dob' && !ValidateYYYYMMDD($(this).val())) {
                                sum_arr.push("生日格式錯誤");
                                $(this).addClass('is-invalid');
                            }
                        }
                    }


                    // 判斷Checkbox多選
                    if ($(this).hasClass('hide')) {
                        var arr = Object.keys($.parseJSON($(this).val()));
                        if (arr.length === 0) {
                            sum_arr.push("請至少勾選一項「" + title + "」");
                            $(this).addClass('is-invalid');
                        }
                    }
                })

                // 密碼確認
                if ($("#password1").val() !== $("#password2").val() && $("#password1").val() !== "" && $("#password2").val() !== "") {
                    sum_arr.push("兩次輸入的密碼不同");
                    $("#password1, #password2").addClass('is-invalid');
                }

                $.unique(sum_arr);
                if (sum_arr.length > 0) {
                    $btn.prop('disabled', false);
                    $btn.prev('button:reset').prop('disabled', false);
                    $btn.children("span").remove();
                    this_form.find('[data-title="' + sum_arr[0] + '"]').focus();
                    sum_arr = sum_arr.join("<br/>");
                    showAlert(sum_arr, 'red');
                    (e.preventDefault) ? e.preventDefault() : e.returnValue = false;
                    return false
                } else {
                    document.location.href = link;
                    // this_form.submit();
                }
            })

            /**
             * This will fix the preview image does not reset the input[type=reset] clicks.
             */
            $('form').on('reset', function (e) {
                var model = $('[name=model]').val();
                if (model === "add") {
                    $('.upload_cover').each(function () {
                        $(this).css('background-image', 'url(../uploads/others/nophoto.png)');
                    })
                    $('.filesupload[data-file]').each(function () {
                        $(this).parents('td:first').find('#filename').addClass('text-muted').text("尚未選擇檔案...");
                    })
                } else if (model === "update") {
                    $('input.file[type=file]').each(function () {
                        var file = aes_decrypt($(this).data('file'));
                        $(this).parents('td').find('.upload_cover').css('background-image', 'url(../uploads/others/' + file + ')');

                        if ($(this).parents('td').find('.btn-remove').length > 0 && $(this).parents('td').find('.btn-preview').length > 0) {
                            $(this).parents('td').find('.btn-remove').removeClass('hide');
                            $(this).parents('td').find('.btn-preview').removeClass('hide');
                        }
                    })

                    $('.filesupload[data-file]').each(function () {
                        $(this).parents('td:first').find('#filename').removeClass('text-muted').addClass('text-danger').html("<i class=\"fa-solid fa-circle-exclamation\"></i> 若重新上傳將會覆蓋原始檔案...");
                        $(this).parents('td:first').find('[name=file_area]').removeClass('hide')
                    })
                }
            });

            $('input.file[type=file]').change(function () {
                var model = $('[name=model]').val();
                if (model === "update") {
                    $(this).parents('td').find('.btn-remove').addClass('hide');
                    $(this).parents('td').find('.btn-preview').addClass('hide');
                }
            })

            // 顯示檔案名稱
            $('.filesupload[data-file]').each(function () {
                var fileInput = $(this);
                var file_data = aes_decrypt(fileInput.data('file'))
                var text = fileInput.parents('td:first').find('#filename')
                if (file_data !== "") {
                    text.addClass('text-danger').html("<i class=\"fa-solid fa-circle-exclamation\"></i> 若重新上傳將會覆蓋原始檔案...");
                } else {
                    text.addClass('text-muted').text("尚未選擇檔案...");
                }
                fileInput.on('change', function () {
                    var filename = $(this).val();
                    text.removeClass('text-danger').removeClass('text-muted')
                    if (/^\s*$/.test(filename)) {
                        if (file_data !== "") {
                            text.addClass('text-danger').html("<i class=\"fa-solid fa-circle-exclamation\"></i> 若重新上傳將會覆蓋原始檔案...");
                        } else {
                            text.addClass('text-muted').text("尚未選擇檔案...");
                        }
                    } else {
                        text.addClass('text-muted').text(filename.replace("C:\\fakepath\\", ""));
                    }
                });
            })

            // 多檔上傳
            if ($(".target_list").length > 0) {
                $(document).on('click', '#add_input_btn', function () {

                    var num = checkNum();
                    var addItem = '<div class="d-block mb-1 item">';
                    addItem += '<label class="btn btn-outline-primary mb-0">';
                    addItem += '<input style="display:none;" req class="filesupload" data-title="附件" type="file" name="file' + num + '" value="" accept=".doc,.docx,.odt,.pdf" data-file="">';
                    addItem += '<i class="fa-solid fa-cloud-arrow-up"></i> 上傳檔案';
                    addItem += '</label>';
                    addItem += '<span class="text-sm mx-2 text-muted" id="filename">尚未選擇檔案...</span>';
                    addItem += '<button class="del-btn btn btn-sm btn-outline-secondary" type="button"><i class="fas fa-times"></i> 移除</button>';
                    addItem += '</div>';
                    if ($(".target_list").find("[type='file']").length <= 7) {
                        $(".target_list").append(addItem);
                        $(".target_list .del-btn").removeClass("hidden")
                    }
                })
                $(document).on('click', '.target_list .del-btn', function () {
                    var num = $(".target_list").find("[type='file']").length

                    if (confirm("確認清除？") == false) {
                        return false
                    } else {
                        if (num >= 2) {
                            $(this).closest(".item").remove()
                            $("#add_input_btn").prop("disabled", false)
                        }

                        if (num <= 2) {
                            $(".target_list .del-btn").addClass("hidden")
                        } else {
                            $(".target_list .del-btn").removeClass("hidden")
                        }
                    }
                })

                function checkNum() {
                    var num = $(".target_list").find("[type='file']").length
                    if (num >= 7) {
                        $("#add_input_btn").prop("disabled", true)
                    } else {
                        $("#add_input_btn").prop("disabled", false)
                    }
                    return num;
                }
            }

        }

        //###############以下為列表頁面 fun#######################//
        if ((location.pathname).indexOf("_list.") >= 0) {

            // 列表批次選取
            $("input[name=box_toggle]").click(function (event) {
                if (this.checked) {
                    $("input[name=box_list]").each(function () { //loop through each checkbox
                        $(this).prop('checked', true); //check
                    });
                } else {
                    $("input[name=box_list]").each(function () { //loop through each checkbox
                        $(this).prop('checked', false); //uncheck
                    });
                }
            });

            // 列表批次刪除
            $("button[name=box_del]").click(function (event) {
                var this_tr = $(this).parents("tr");
                var event_arr = [];
                event_arr["success"] = function () {
                    this_tr.remove(); // 模擬刪除
                    showAlert("刪除成功");
                }
                showConfirm(event_arr);
            });

            // 列表上方的篩選DOM
            // 按下搜尋按鈕時，取得有search_ref的所有元素，將所有值組合成url param，並導向
            $("[name=search_button]").click(function (event) {
                var str = $('[search_ref]').map(function () {
                    return $(this).attr("name") + "=" + $(this).val();
                    // 拼成網址參數字串
                }).get().join("&");
                document.location.href = "?" + str;
            });

            // 列表清除篩選條件
            $("[name=clear_filter]").click(function (event) {
                var current = window.location.href;
                current = current.split('?')[0];
                document.location.href = current;
            })

            // 自動搜尋
            // $('[search_ref]').each(function () {
            //     $(this).change(function () {
            //         $("[name=search_button]").trigger('click');
            //     })
            // })

            // 列表excel匯出
            if ($('[name=export_excel]').length > 0) {
                $("[name=export_excel]").click(function (event) {
                    var filename = $(this).data("file");
                    var event_arr = []
                    event_arr["success"] = function () {
                        if (filename !== undefined) {
                            window.open('../uploads/excels/' + filename);
                        } else {
                            showAlert("此時將匯出符合篩選條件之結果Excel")
                        }
                    }
                    showConfirm(event_arr, "提醒", "確定要匯出嗎?")
                })
            }

            // 列表excel匯入按鈕警示
            if ($('[name=import_excel]').length > 0) {
                $('[name=import_excel]').click(function () {
                    var link = $(this).data("link");
                    var event_arr = []
                    event_arr["success"] = function () {
                        document.location.href = link;
                    }
                    showConfirm(event_arr, "提醒", "匯入時，請使用系統提供之格式")
                })
            }
        }

    }
})

function showAlert(content = '', type = '') {
    $.alert({
        title: '提醒您',
        content: content,
        type: type
    });
}

function showConfirm(event_arr = [], title = "確定刪除?", content = "刪除資料無法復原!") {
    var success = function () {
    };
    var fail = function () {
    };

    if (typeof (event_arr["success"]) != "undefined" && $.isFunction(event_arr["success"])) {
        success = event_arr["success"];
    }
    if (typeof (event_arr["fail"]) != "undefined" && $.isFunction(event_arr["fail"])) {
        fail = event_arr["fail"];
    }

    $.confirm({
        title: title,
        content: content,
        type: 'red',
        buttons: {
            confirm: {
                text: '確定',
                action: success
            },
            cancel: {
                text: '取消',
                action: fail
            },
        }
    });
}

function genType(arr = type_arr, defaults = "", main = "請選擇校安事件主類別") {
    var option = (main !== "") ? '<option value="">' + main + '</option>\n' : '';
    $.each(arr, function (type) {
        if (type === defaults) {
            option += '<option value="' + type + '" selected>' + type + '</option>\n';
        } else {
            option += '<option value="' + type + '">' + type + '</option>\n';
        }
    });

    return option;
}

function genSubtype(arr = type_arr, type = "意外事件", defaults = "", main = "請選擇校安事件次類別") {
    var option = '<option value="">' + main + '</option>\n';
    if (arr[type] !== undefined) {
        $.each(arr[type], function (subtype) {
            if (subtype === defaults) {
                option += '<option value="' + subtype + '" selected>' + subtype + '</option>\n';
            } else {
                option += '<option value="' + subtype + '">' + subtype + '</option>\n';
            }
        });
    }
    return option;
}

// 檢查密碼複雜度
function validatePasswd(passwdSelector, rulesSelector, min, max) {
    min = (typeof min !== 'undefined') ? min : 6; // 預設最小長度
    max = (typeof max !== 'undefined') ? max : 15; // 預設最大長度

    var val = $(passwdSelector).val(); // 密碼值
    var regex = {
        isValidLength: new RegExp('^.{' + min + ',' + max + '}$'), // 介於長度
        // isNonWhiteSpace: new RegExp('^\S*$'), // 不包含空白
        // isContainsUppercase: new RegExp('^(?=.*[A-Z]).*$'), // 至少包含一大寫英文字母
        // isContainsLowercase: new RegExp('^(?=.*[a-z]).*$'), // 至少包含一小寫英文字母
        isContainsUpperOrLowercase: new RegExp('^(?=.*[A-Za-z]).*$'), // 至少包含一大寫或小寫英文字母
        isContainsNumber: new RegExp('^(?=.*[0-9]).*$'), // 至少包含一數字
        // isContainsSymbol: new RegExp('^(?=.*[!#$@^%&?]).*$'), // 至少包含一特殊符號，如!#$@^%&?
        isPassedOverall: new RegExp('^(?=.*[A-Za-z])(?=.*[0-9])[A-Za-z0-9]{' + min + ',' + max + '}$')
    }

    var warning = 0; // 計算錯誤數量

    Object.keys(regex).forEach(function (item) {
        $(rulesSelector).find('#' + item).removeClass('text-success');
        $(rulesSelector).find('#' + item).removeClass('text-danger');
        if (val !== "" && val !== undefined) {
            if (!regex[item].test(val)) {
                // 不符合規則，將其文字加上.text-danger
                $(rulesSelector).find('#' + item).removeClass('text-success');
                $(rulesSelector).find('#' + item).addClass('text-danger');
                warning++
            } else {
                // 符合規則，將其文字加上.text-success
                $(rulesSelector).find('#' + item).removeClass('text-danger');
                $(rulesSelector).find('#' + item).addClass('text-success');
            }
        }
    });

    return warning <= 0;
}

//驗證EMAIL
function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email.toLowerCase());
}

//驗證數字，如果有傳floats這參數，代表浮點數也可以過
function ValidateNumber(pnumber, floats) {
    var re = (floats) ? /^[+\-]?\d+(.\d+)?$/ : /^\d+$/;
    return re.test(pnumber);
}

//驗證手機格式
function ValidateMobile(pnumber) {
    var re = /^[09]{2}[0-9]{8}$/;
    return re.test(pnumber);
}

//驗證日期
function ValidateYYYYMMDD(str) {
    var date_regex = /^(19|20)\d{2}\-(0[1-9]|1[0-2])\-(0[1-9]|1\d|2\d|3[01])$/;
    return (date_regex.test(str))
}

function aes_encrypt(str, key) {
    if (typeof (key) == "undefined") {
        key = "3883136338831363";
    }
    var key = CryptoJS.enc.Utf8.parse(key);
    var encryptedData = CryptoJS.AES.encrypt(pintech_trim(str), key, {
        mode: CryptoJS.mode.ECB,
        padding: CryptoJS.pad.Pkcs7
    });
    //encryptedData = encryptedData.toString().replaceAll('+', '-');
    //encryptedData = encryptedData.replaceAll('/', '_');
    //encryptedData = encryptedData.replaceAll('=', '');

    encryptedData = encryptedData.toString().replace(/\+/g, '-');
    encryptedData = encryptedData.replace(/\//g, '_');
    encryptedData = encryptedData.replace(/=/g, '');

    return encryptedData;
}

function aes_decrypt(str, key) {
    if (typeof (key) == "undefined") {
        key = "3883136338831363";
    }
    //str = str.replaceAll('_', '/');
    //str = str.replaceAll('-', '+');
    str = str.replace(/_/g, '/');
    str = str.replace(/-/g, '+');
    var mod = (str.length) % 4;
    if (mod) {
        str += '===='.substr(mod);
    }
    var key = CryptoJS.enc.Utf8.parse(key);
    var decryptData = CryptoJS.AES.decrypt(pintech_trim(str), key, {
        mode: CryptoJS.mode.ECB,
        padding: CryptoJS.pad.Pkcs7
    });
    var decryptedStr = decryptData.toString(CryptoJS.enc.Utf8);
    return decryptedStr.toString();
}

function pintech_trim(str, option) {
    var emoji = false;
    if (typeof (option) !== "undefined") {
        if (typeof (option["emoji"]) !== "undefined") {
            if (option["emoji"] == true) {
                emoji = true;
            }
        }
    }

    if (emoji == false) {
        return $.trim(str).replace(/([\uE000-\uF8FF]|\uD83C[\uDF00-\uDFFF]|\uD83D[\uDC00-\uDDFF]|\uFE0F)/g, '');
    } else {
        return $.trim(str);
    }
}

//var d = new Date("October 13, 2014 11:13:00");
//var d = new Date();
//var n = d.getTime();
//console.log(now_time(d))
//返回yyyy-MM-dd HH:mm:ss，也可以傳入new Date()格式，如果不傳入就是今天時間
function now_time(d) {
    Number.prototype.padLeft = function (base, chr) {
        var len = (String(base || 10).length - String(this).length) + 1;
        return len > 0 ? new Array(len).join(chr || '0') + this : this;
    }
    var d = (d) ? d : new Date();
    dformat = [d.getFullYear(), (d.getMonth() + 1).padLeft(), d.getDate().padLeft()].join('-') + ' ' + [d.getHours().padLeft(), d.getMinutes().padLeft(), d.getSeconds().padLeft()].join(':');
    return dformat;
}

//使用方式 getISODateTime("這邊要輸入yyyy-MM-dd HH:mm:ss完整格式", "yyyy-MM-dd")
function getISODateTime(date, format) {
    if (date.split(" ").length == 1) {
        var tmp_datetime = date.replace(/:/g, '-');
        tmp_datetime = tmp_datetime.replace(/ /g, '-');
        var arr = tmp_datetime.split("-");
        date = new Date(Date.UTC(arr[0], arr[1] - 1, arr[2], 00, 00, 00));
    }

    if (!date) return;
    if (!format) format = "yyyy-MM-dd";
    switch (typeof date) {
        case "string":
            date = new Date(date.replace(/\-/g, "/"));
            break;
        case "number":
            date = new Date(date);
            break;
    }

    if (!date instanceof Date) return;
    var dict = {
        "yyyy": date.getFullYear(),
        "M": date.getMonth() + 1,
        "d": date.getDate(),
        "H": date.getHours(),
        "m": date.getMinutes(),
        "s": date.getSeconds(),
        "MM": ("" + (date.getMonth() + 101)).substr(1),
        "dd": ("" + (date.getDate() + 100)).substr(1),
        "HH": ("" + (date.getHours() + 100)).substr(1),
        "mm": ("" + (date.getMinutes() + 100)).substr(1),
        "ss": ("" + (date.getSeconds() + 100)).substr(1)
    };

    return format.replace(/(yyyy|MM?|dd?|HH?|ss?|mm?)/g, function () {
        return dict[arguments[0]];
    });
}

/*
getUrlParam() //列出全部參數
getUrlParam("id") //列出id的值
getUrlParam("id","網址") //從網址裡面拉參數
*/
function getUrlParam(name, url) {
    if (typeof (name) != "undefined") {
        var reg = RegExp('[?&]' + name.replace(/([[\]])/, '\\$1') + '=([^&#]*)');
        var r = (url) ? (url.match(reg) || ['', ''])[1] : decodeURI((window.location.href.match(reg) || ['', ''])[1]);
        return r;
    } else {
        var r = {};
        location.search.replace(/[?&;]+([^=]+)=([^&;]*)/gi, function (s, k, v) {
            r[k] = decodeURI(v)
        })
        return r;
    }
}

/*
CryptoJS v3.1.2
code.google.com/p/crypto-js
(c) 2009-2013 by Jeff Mott. All rights reserved.
code.google.com/p/crypto-js/wiki/License
*/
var CryptoJS = CryptoJS || function (u, p) {
    var d = {}, l = d.lib = {}, s = function () {
        }, t = l.Base = {
            extend: function (a) {
                s.prototype = this;
                var c = new s;
                a && c.mixIn(a);
                c.hasOwnProperty("init") || (c.init = function () {
                    c.$super.init.apply(this, arguments)
                });
                c.init.prototype = c;
                c.$super = this;
                return c
            }, create: function () {
                var a = this.extend();
                a.init.apply(a, arguments);
                return a
            }, init: function () {
            }, mixIn: function (a) {
                for (var c in a) a.hasOwnProperty(c) && (this[c] = a[c]);
                a.hasOwnProperty("toString") && (this.toString = a.toString)
            }, clone: function () {
                return this.init.prototype.extend(this)
            }
        },
        r = l.WordArray = t.extend({
            init: function (a, c) {
                a = this.words = a || [];
                this.sigBytes = c != p ? c : 4 * a.length
            }, toString: function (a) {
                return (a || v).stringify(this)
            }, concat: function (a) {
                var c = this.words, e = a.words, j = this.sigBytes;
                a = a.sigBytes;
                this.clamp();
                if (j % 4) for (var k = 0; k < a; k++) c[j + k >>> 2] |= (e[k >>> 2] >>> 24 - 8 * (k % 4) & 255) << 24 - 8 * ((j + k) % 4); else if (65535 < e.length) for (k = 0; k < a; k += 4) c[j + k >>> 2] = e[k >>> 2]; else c.push.apply(c, e);
                this.sigBytes += a;
                return this
            }, clamp: function () {
                var a = this.words, c = this.sigBytes;
                a[c >>> 2] &= 4294967295 <<
                    32 - 8 * (c % 4);
                a.length = u.ceil(c / 4)
            }, clone: function () {
                var a = t.clone.call(this);
                a.words = this.words.slice(0);
                return a
            }, random: function (a) {
                for (var c = [], e = 0; e < a; e += 4) c.push(4294967296 * u.random() | 0);
                return new r.init(c, a)
            }
        }), w = d.enc = {}, v = w.Hex = {
            stringify: function (a) {
                var c = a.words;
                a = a.sigBytes;
                for (var e = [], j = 0; j < a; j++) {
                    var k = c[j >>> 2] >>> 24 - 8 * (j % 4) & 255;
                    e.push((k >>> 4).toString(16));
                    e.push((k & 15).toString(16))
                }
                return e.join("")
            }, parse: function (a) {
                for (var c = a.length, e = [], j = 0; j < c; j += 2) e[j >>> 3] |= parseInt(a.substr(j,
                    2), 16) << 24 - 4 * (j % 8);
                return new r.init(e, c / 2)
            }
        }, b = w.Latin1 = {
            stringify: function (a) {
                var c = a.words;
                a = a.sigBytes;
                for (var e = [], j = 0; j < a; j++) e.push(String.fromCharCode(c[j >>> 2] >>> 24 - 8 * (j % 4) & 255));
                return e.join("")
            }, parse: function (a) {
                for (var c = a.length, e = [], j = 0; j < c; j++) e[j >>> 2] |= (a.charCodeAt(j) & 255) << 24 - 8 * (j % 4);
                return new r.init(e, c)
            }
        }, x = w.Utf8 = {
            stringify: function (a) {
                try {
                    return decodeURIComponent(escape(b.stringify(a)))
                } catch (c) {
                    throw Error("Malformed UTF-8 data");
                }
            }, parse: function (a) {
                return b.parse(unescape(encodeURIComponent(a)))
            }
        },
        q = l.BufferedBlockAlgorithm = t.extend({
            reset: function () {
                this._data = new r.init;
                this._nDataBytes = 0
            }, _append: function (a) {
                "string" == typeof a && (a = x.parse(a));
                this._data.concat(a);
                this._nDataBytes += a.sigBytes
            }, _process: function (a) {
                var c = this._data, e = c.words, j = c.sigBytes, k = this.blockSize, b = j / (4 * k),
                    b = a ? u.ceil(b) : u.max((b | 0) - this._minBufferSize, 0);
                a = b * k;
                j = u.min(4 * a, j);
                if (a) {
                    for (var q = 0; q < a; q += k) this._doProcessBlock(e, q);
                    q = e.splice(0, a);
                    c.sigBytes -= j
                }
                return new r.init(q, j)
            }, clone: function () {
                var a = t.clone.call(this);
                a._data = this._data.clone();
                return a
            }, _minBufferSize: 0
        });
    l.Hasher = q.extend({
        cfg: t.extend(), init: function (a) {
            this.cfg = this.cfg.extend(a);
            this.reset()
        }, reset: function () {
            q.reset.call(this);
            this._doReset()
        }, update: function (a) {
            this._append(a);
            this._process();
            return this
        }, finalize: function (a) {
            a && this._append(a);
            return this._doFinalize()
        }, blockSize: 16, _createHelper: function (a) {
            return function (b, e) {
                return (new a.init(e)).finalize(b)
            }
        }, _createHmacHelper: function (a) {
            return function (b, e) {
                return (new n.HMAC.init(a,
                    e)).finalize(b)
            }
        }
    });
    var n = d.algo = {};
    return d
}(Math);
(function () {
    var u = CryptoJS, p = u.lib.WordArray;
    u.enc.Base64 = {
        stringify: function (d) {
            var l = d.words, p = d.sigBytes, t = this._map;
            d.clamp();
            d = [];
            for (var r = 0; r < p; r += 3) for (var w = (l[r >>> 2] >>> 24 - 8 * (r % 4) & 255) << 16 | (l[r + 1 >>> 2] >>> 24 - 8 * ((r + 1) % 4) & 255) << 8 | l[r + 2 >>> 2] >>> 24 - 8 * ((r + 2) % 4) & 255, v = 0; 4 > v && r + 0.75 * v < p; v++) d.push(t.charAt(w >>> 6 * (3 - v) & 63));
            if (l = t.charAt(64)) for (; d.length % 4;) d.push(l);
            return d.join("")
        }, parse: function (d) {
            var l = d.length, s = this._map, t = s.charAt(64);
            t && (t = d.indexOf(t), -1 != t && (l = t));
            for (var t = [], r = 0, w = 0; w <
            l; w++) if (w % 4) {
                var v = s.indexOf(d.charAt(w - 1)) << 2 * (w % 4), b = s.indexOf(d.charAt(w)) >>> 6 - 2 * (w % 4);
                t[r >>> 2] |= (v | b) << 24 - 8 * (r % 4);
                r++
            }
            return p.create(t, r)
        }, _map: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/="
    }
})();
(function (u) {
    function p(b, n, a, c, e, j, k) {
        b = b + (n & a | ~n & c) + e + k;
        return (b << j | b >>> 32 - j) + n
    }

    function d(b, n, a, c, e, j, k) {
        b = b + (n & c | a & ~c) + e + k;
        return (b << j | b >>> 32 - j) + n
    }

    function l(b, n, a, c, e, j, k) {
        b = b + (n ^ a ^ c) + e + k;
        return (b << j | b >>> 32 - j) + n
    }

    function s(b, n, a, c, e, j, k) {
        b = b + (a ^ (n | ~c)) + e + k;
        return (b << j | b >>> 32 - j) + n
    }

    for (var t = CryptoJS, r = t.lib, w = r.WordArray, v = r.Hasher, r = t.algo, b = [], x = 0; 64 > x; x++) b[x] = 4294967296 * u.abs(u.sin(x + 1)) | 0;
    r = r.MD5 = v.extend({
        _doReset: function () {
            this._hash = new w.init([1732584193, 4023233417, 2562383102, 271733878])
        },
        _doProcessBlock: function (q, n) {
            for (var a = 0; 16 > a; a++) {
                var c = n + a, e = q[c];
                q[c] = (e << 8 | e >>> 24) & 16711935 | (e << 24 | e >>> 8) & 4278255360
            }
            var a = this._hash.words, c = q[n + 0], e = q[n + 1], j = q[n + 2], k = q[n + 3], z = q[n + 4],
                r = q[n + 5], t = q[n + 6], w = q[n + 7], v = q[n + 8], A = q[n + 9], B = q[n + 10], C = q[n + 11],
                u = q[n + 12], D = q[n + 13], E = q[n + 14], x = q[n + 15], f = a[0], m = a[1], g = a[2], h = a[3],
                f = p(f, m, g, h, c, 7, b[0]), h = p(h, f, m, g, e, 12, b[1]), g = p(g, h, f, m, j, 17, b[2]),
                m = p(m, g, h, f, k, 22, b[3]), f = p(f, m, g, h, z, 7, b[4]), h = p(h, f, m, g, r, 12, b[5]),
                g = p(g, h, f, m, t, 17, b[6]), m = p(m, g, h, f, w, 22, b[7]),
                f = p(f, m, g, h, v, 7, b[8]), h = p(h, f, m, g, A, 12, b[9]), g = p(g, h, f, m, B, 17, b[10]),
                m = p(m, g, h, f, C, 22, b[11]), f = p(f, m, g, h, u, 7, b[12]), h = p(h, f, m, g, D, 12, b[13]),
                g = p(g, h, f, m, E, 17, b[14]), m = p(m, g, h, f, x, 22, b[15]), f = d(f, m, g, h, e, 5, b[16]),
                h = d(h, f, m, g, t, 9, b[17]), g = d(g, h, f, m, C, 14, b[18]), m = d(m, g, h, f, c, 20, b[19]),
                f = d(f, m, g, h, r, 5, b[20]), h = d(h, f, m, g, B, 9, b[21]), g = d(g, h, f, m, x, 14, b[22]),
                m = d(m, g, h, f, z, 20, b[23]), f = d(f, m, g, h, A, 5, b[24]), h = d(h, f, m, g, E, 9, b[25]),
                g = d(g, h, f, m, k, 14, b[26]), m = d(m, g, h, f, v, 20, b[27]), f = d(f, m, g, h, D, 5, b[28]),
                h = d(h, f,
                    m, g, j, 9, b[29]), g = d(g, h, f, m, w, 14, b[30]), m = d(m, g, h, f, u, 20, b[31]),
                f = l(f, m, g, h, r, 4, b[32]), h = l(h, f, m, g, v, 11, b[33]), g = l(g, h, f, m, C, 16, b[34]),
                m = l(m, g, h, f, E, 23, b[35]), f = l(f, m, g, h, e, 4, b[36]), h = l(h, f, m, g, z, 11, b[37]),
                g = l(g, h, f, m, w, 16, b[38]), m = l(m, g, h, f, B, 23, b[39]), f = l(f, m, g, h, D, 4, b[40]),
                h = l(h, f, m, g, c, 11, b[41]), g = l(g, h, f, m, k, 16, b[42]), m = l(m, g, h, f, t, 23, b[43]),
                f = l(f, m, g, h, A, 4, b[44]), h = l(h, f, m, g, u, 11, b[45]), g = l(g, h, f, m, x, 16, b[46]),
                m = l(m, g, h, f, j, 23, b[47]), f = s(f, m, g, h, c, 6, b[48]), h = s(h, f, m, g, w, 10, b[49]),
                g = s(g, h, f, m,
                    E, 15, b[50]), m = s(m, g, h, f, r, 21, b[51]), f = s(f, m, g, h, u, 6, b[52]),
                h = s(h, f, m, g, k, 10, b[53]), g = s(g, h, f, m, B, 15, b[54]), m = s(m, g, h, f, e, 21, b[55]),
                f = s(f, m, g, h, v, 6, b[56]), h = s(h, f, m, g, x, 10, b[57]), g = s(g, h, f, m, t, 15, b[58]),
                m = s(m, g, h, f, D, 21, b[59]), f = s(f, m, g, h, z, 6, b[60]), h = s(h, f, m, g, C, 10, b[61]),
                g = s(g, h, f, m, j, 15, b[62]), m = s(m, g, h, f, A, 21, b[63]);
            a[0] = a[0] + f | 0;
            a[1] = a[1] + m | 0;
            a[2] = a[2] + g | 0;
            a[3] = a[3] + h | 0
        }, _doFinalize: function () {
            var b = this._data, n = b.words, a = 8 * this._nDataBytes, c = 8 * b.sigBytes;
            n[c >>> 5] |= 128 << 24 - c % 32;
            var e = u.floor(a /
                4294967296);
            n[(c + 64 >>> 9 << 4) + 15] = (e << 8 | e >>> 24) & 16711935 | (e << 24 | e >>> 8) & 4278255360;
            n[(c + 64 >>> 9 << 4) + 14] = (a << 8 | a >>> 24) & 16711935 | (a << 24 | a >>> 8) & 4278255360;
            b.sigBytes = 4 * (n.length + 1);
            this._process();
            b = this._hash;
            n = b.words;
            for (a = 0; 4 > a; a++) c = n[a], n[a] = (c << 8 | c >>> 24) & 16711935 | (c << 24 | c >>> 8) & 4278255360;
            return b
        }, clone: function () {
            var b = v.clone.call(this);
            b._hash = this._hash.clone();
            return b
        }
    });
    t.MD5 = v._createHelper(r);
    t.HmacMD5 = v._createHmacHelper(r)
})(Math);
(function () {
    var u = CryptoJS, p = u.lib, d = p.Base, l = p.WordArray, p = u.algo, s = p.EvpKDF = d.extend({
        cfg: d.extend({keySize: 4, hasher: p.MD5, iterations: 1}), init: function (d) {
            this.cfg = this.cfg.extend(d)
        }, compute: function (d, r) {
            for (var p = this.cfg, s = p.hasher.create(), b = l.create(), u = b.words, q = p.keySize, p = p.iterations; u.length < q;) {
                n && s.update(n);
                var n = s.update(d).finalize(r);
                s.reset();
                for (var a = 1; a < p; a++) n = s.finalize(n), s.reset();
                b.concat(n)
            }
            b.sigBytes = 4 * q;
            return b
        }
    });
    u.EvpKDF = function (d, l, p) {
        return s.create(p).compute(d,
            l)
    }
})();
CryptoJS.lib.Cipher || function (u) {
    var p = CryptoJS, d = p.lib, l = d.Base, s = d.WordArray, t = d.BufferedBlockAlgorithm, r = p.enc.Base64,
        w = p.algo.EvpKDF, v = d.Cipher = t.extend({
            cfg: l.extend(), createEncryptor: function (e, a) {
                return this.create(this._ENC_XFORM_MODE, e, a)
            }, createDecryptor: function (e, a) {
                return this.create(this._DEC_XFORM_MODE, e, a)
            }, init: function (e, a, b) {
                this.cfg = this.cfg.extend(b);
                this._xformMode = e;
                this._key = a;
                this.reset()
            }, reset: function () {
                t.reset.call(this);
                this._doReset()
            }, process: function (e) {
                this._append(e);
                return this._process()
            },
            finalize: function (e) {
                e && this._append(e);
                return this._doFinalize()
            }, keySize: 4, ivSize: 4, _ENC_XFORM_MODE: 1, _DEC_XFORM_MODE: 2, _createHelper: function (e) {
                return {
                    encrypt: function (b, k, d) {
                        return ("string" == typeof k ? c : a).encrypt(e, b, k, d)
                    }, decrypt: function (b, k, d) {
                        return ("string" == typeof k ? c : a).decrypt(e, b, k, d)
                    }
                }
            }
        });
    d.StreamCipher = v.extend({
        _doFinalize: function () {
            return this._process(!0)
        }, blockSize: 1
    });
    var b = p.mode = {}, x = function (e, a, b) {
        var c = this._iv;
        c ? this._iv = u : c = this._prevBlock;
        for (var d = 0; d < b; d++) e[a + d] ^=
            c[d]
    }, q = (d.BlockCipherMode = l.extend({
        createEncryptor: function (e, a) {
            return this.Encryptor.create(e, a)
        }, createDecryptor: function (e, a) {
            return this.Decryptor.create(e, a)
        }, init: function (e, a) {
            this._cipher = e;
            this._iv = a
        }
    })).extend();
    q.Encryptor = q.extend({
        processBlock: function (e, a) {
            var b = this._cipher, c = b.blockSize;
            x.call(this, e, a, c);
            b.encryptBlock(e, a);
            this._prevBlock = e.slice(a, a + c)
        }
    });
    q.Decryptor = q.extend({
        processBlock: function (e, a) {
            var b = this._cipher, c = b.blockSize, d = e.slice(a, a + c);
            b.decryptBlock(e, a);
            x.call(this,
                e, a, c);
            this._prevBlock = d
        }
    });
    b = b.CBC = q;
    q = (p.pad = {}).Pkcs7 = {
        pad: function (a, b) {
            for (var c = 4 * b, c = c - a.sigBytes % c, d = c << 24 | c << 16 | c << 8 | c, l = [], n = 0; n < c; n += 4) l.push(d);
            c = s.create(l, c);
            a.concat(c)
        }, unpad: function (a) {
            a.sigBytes -= a.words[a.sigBytes - 1 >>> 2] & 255
        }
    };
    d.BlockCipher = v.extend({
        cfg: v.cfg.extend({mode: b, padding: q}), reset: function () {
            v.reset.call(this);
            var a = this.cfg, b = a.iv, a = a.mode;
            if (this._xformMode == this._ENC_XFORM_MODE) var c = a.createEncryptor; else c = a.createDecryptor, this._minBufferSize = 1;
            this._mode = c.call(a,
                this, b && b.words)
        }, _doProcessBlock: function (a, b) {
            this._mode.processBlock(a, b)
        }, _doFinalize: function () {
            var a = this.cfg.padding;
            if (this._xformMode == this._ENC_XFORM_MODE) {
                a.pad(this._data, this.blockSize);
                var b = this._process(!0)
            } else b = this._process(!0), a.unpad(b);
            return b
        }, blockSize: 4
    });
    var n = d.CipherParams = l.extend({
        init: function (a) {
            this.mixIn(a)
        }, toString: function (a) {
            return (a || this.formatter).stringify(this)
        }
    }), b = (p.format = {}).OpenSSL = {
        stringify: function (a) {
            var b = a.ciphertext;
            a = a.salt;
            return (a ? s.create([1398893684,
                1701076831]).concat(a).concat(b) : b).toString(r)
        }, parse: function (a) {
            a = r.parse(a);
            var b = a.words;
            if (1398893684 == b[0] && 1701076831 == b[1]) {
                var c = s.create(b.slice(2, 4));
                b.splice(0, 4);
                a.sigBytes -= 16
            }
            return n.create({ciphertext: a, salt: c})
        }
    }, a = d.SerializableCipher = l.extend({
        cfg: l.extend({format: b}), encrypt: function (a, b, c, d) {
            d = this.cfg.extend(d);
            var l = a.createEncryptor(c, d);
            b = l.finalize(b);
            l = l.cfg;
            return n.create({
                ciphertext: b,
                key: c,
                iv: l.iv,
                algorithm: a,
                mode: l.mode,
                padding: l.padding,
                blockSize: a.blockSize,
                formatter: d.format
            })
        },
        decrypt: function (a, b, c, d) {
            d = this.cfg.extend(d);
            b = this._parse(b, d.format);
            return a.createDecryptor(c, d).finalize(b.ciphertext)
        }, _parse: function (a, b) {
            return "string" == typeof a ? b.parse(a, this) : a
        }
    }), p = (p.kdf = {}).OpenSSL = {
        execute: function (a, b, c, d) {
            d || (d = s.random(8));
            a = w.create({keySize: b + c}).compute(a, d);
            c = s.create(a.words.slice(b), 4 * c);
            a.sigBytes = 4 * b;
            return n.create({key: a, iv: c, salt: d})
        }
    }, c = d.PasswordBasedCipher = a.extend({
        cfg: a.cfg.extend({kdf: p}), encrypt: function (b, c, d, l) {
            l = this.cfg.extend(l);
            d = l.kdf.execute(d,
                b.keySize, b.ivSize);
            l.iv = d.iv;
            b = a.encrypt.call(this, b, c, d.key, l);
            b.mixIn(d);
            return b
        }, decrypt: function (b, c, d, l) {
            l = this.cfg.extend(l);
            c = this._parse(c, l.format);
            d = l.kdf.execute(d, b.keySize, b.ivSize, c.salt);
            l.iv = d.iv;
            return a.decrypt.call(this, b, c, d.key, l)
        }
    })
}();
(function () {
    for (var u = CryptoJS, p = u.lib.BlockCipher, d = u.algo, l = [], s = [], t = [], r = [], w = [], v = [], b = [], x = [], q = [], n = [], a = [], c = 0; 256 > c; c++) a[c] = 128 > c ? c << 1 : c << 1 ^ 283;
    for (var e = 0, j = 0, c = 0; 256 > c; c++) {
        var k = j ^ j << 1 ^ j << 2 ^ j << 3 ^ j << 4, k = k >>> 8 ^ k & 255 ^ 99;
        l[e] = k;
        s[k] = e;
        var z = a[e], F = a[z], G = a[F], y = 257 * a[k] ^ 16843008 * k;
        t[e] = y << 24 | y >>> 8;
        r[e] = y << 16 | y >>> 16;
        w[e] = y << 8 | y >>> 24;
        v[e] = y;
        y = 16843009 * G ^ 65537 * F ^ 257 * z ^ 16843008 * e;
        b[k] = y << 24 | y >>> 8;
        x[k] = y << 16 | y >>> 16;
        q[k] = y << 8 | y >>> 24;
        n[k] = y;
        e ? (e = z ^ a[a[a[G ^ z]]], j ^= a[a[j]]) : e = j = 1
    }
    var H = [0, 1, 2, 4, 8,
        16, 32, 64, 128, 27, 54], d = d.AES = p.extend({
        _doReset: function () {
            for (var a = this._key, c = a.words, d = a.sigBytes / 4, a = 4 * ((this._nRounds = d + 6) + 1), e = this._keySchedule = [], j = 0; j < a; j++) if (j < d) e[j] = c[j]; else {
                var k = e[j - 1];
                j % d ? 6 < d && 4 == j % d && (k = l[k >>> 24] << 24 | l[k >>> 16 & 255] << 16 | l[k >>> 8 & 255] << 8 | l[k & 255]) : (k = k << 8 | k >>> 24, k = l[k >>> 24] << 24 | l[k >>> 16 & 255] << 16 | l[k >>> 8 & 255] << 8 | l[k & 255], k ^= H[j / d | 0] << 24);
                e[j] = e[j - d] ^ k
            }
            c = this._invKeySchedule = [];
            for (d = 0; d < a; d++) j = a - d, k = d % 4 ? e[j] : e[j - 4], c[d] = 4 > d || 4 >= j ? k : b[l[k >>> 24]] ^ x[l[k >>> 16 & 255]] ^ q[l[k >>>
            8 & 255]] ^ n[l[k & 255]]
        }, encryptBlock: function (a, b) {
            this._doCryptBlock(a, b, this._keySchedule, t, r, w, v, l)
        }, decryptBlock: function (a, c) {
            var d = a[c + 1];
            a[c + 1] = a[c + 3];
            a[c + 3] = d;
            this._doCryptBlock(a, c, this._invKeySchedule, b, x, q, n, s);
            d = a[c + 1];
            a[c + 1] = a[c + 3];
            a[c + 3] = d
        }, _doCryptBlock: function (a, b, c, d, e, j, l, f) {
            for (var m = this._nRounds, g = a[b] ^ c[0], h = a[b + 1] ^ c[1], k = a[b + 2] ^ c[2], n = a[b + 3] ^ c[3], p = 4, r = 1; r < m; r++) var q = d[g >>> 24] ^ e[h >>> 16 & 255] ^ j[k >>> 8 & 255] ^ l[n & 255] ^ c[p++], s = d[h >>> 24] ^ e[k >>> 16 & 255] ^ j[n >>> 8 & 255] ^ l[g & 255] ^ c[p++], t =
                d[k >>> 24] ^ e[n >>> 16 & 255] ^ j[g >>> 8 & 255] ^ l[h & 255] ^ c[p++], n = d[n >>> 24] ^ e[g >>> 16 & 255] ^ j[h >>> 8 & 255] ^ l[k & 255] ^ c[p++], g = q, h = s, k = t;
            q = (f[g >>> 24] << 24 | f[h >>> 16 & 255] << 16 | f[k >>> 8 & 255] << 8 | f[n & 255]) ^ c[p++];
            s = (f[h >>> 24] << 24 | f[k >>> 16 & 255] << 16 | f[n >>> 8 & 255] << 8 | f[g & 255]) ^ c[p++];
            t = (f[k >>> 24] << 24 | f[n >>> 16 & 255] << 16 | f[g >>> 8 & 255] << 8 | f[h & 255]) ^ c[p++];
            n = (f[n >>> 24] << 24 | f[g >>> 16 & 255] << 16 | f[h >>> 8 & 255] << 8 | f[k & 255]) ^ c[p++];
            a[b] = q;
            a[b + 1] = s;
            a[b + 2] = t;
            a[b + 3] = n
        }, keySize: 8
    });
    u.AES = p._createHelper(d)
})();

CryptoJS.pad.ZeroPadding = {
    pad: function (data, blockSize) {
        // Shortcut
        var blockSizeBytes = blockSize * 4;

        // Pad
        data.clamp();
        data.sigBytes += blockSizeBytes - ((data.sigBytes % blockSizeBytes) || blockSizeBytes);
    },

    unpad: function (data) {
        // Shortcut
        var dataWords = data.words;

        // Unpad
        var i = data.sigBytes - 1;
        while (!((dataWords[i >>> 2] >>> (24 - (i % 4) * 8)) & 0xff)) {
            i--;
        }
        data.sigBytes = i + 1;
    }
};

CryptoJS.mode.ECB = function () {
    var a = CryptoJS.lib.BlockCipherMode.extend();
    a.Encryptor = a.extend({
        processBlock: function (a, b) {
            this._cipher.encryptBlock(a, b)
        }
    });
    a.Decryptor = a.extend({
        processBlock: function (a, b) {
            this._cipher.decryptBlock(a, b)
        }
    });
    return a
}();