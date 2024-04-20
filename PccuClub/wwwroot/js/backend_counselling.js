var weekTitleArr = {1: '一', 2: '二', 3: '三', 4: '四', 5: '五', 6: '六', 7: '日'};
var monthTitleArr = {
    1: "1月",
    2: "2月",
    3: "3月",
    4: "4月",
    5: "5月",
    6: "6月",
    7: "7月",
    8: "8月",
    9: "9月",
    10: "10月",
    11: "11月",
    12: "12月",
}
var datetimepicker_option = {
    format: "YYYY-MM-DD",
    applyLabel: "確定",
    cancelLabel: "取消",
    fromLabel: "開始日期",
    toLabel: "結束日期",
    customRangeLabel: "自訂日期區間",
    daysOfWeek: ["日", "一", "二", "三", "四", "五", "六"],
    monthNames: ["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"],
    firstDay: 1,
}
var status_arr = {
    "狀態一": "停用",
    "狀態二": "啟用",
    "狀態三": "啟用",
    "已受理": "啟用",
    "第一次延長": "啟用",
    "第二次延長": "啟用",
    "已結案": "啟用"
}
var question_arr = {
    "自我傷害": true,
    "自殺": false,
    "傷害他人": false,
    "以上皆無": false,
}
var national_arr = [
    {
        "id": 1,
        "title": "本國籍",
        "checked": false,
        "sub_items": [],
    },
    {
        "id": 2,
        "title": "外國籍",
        "checked": true,
        "sub_items": [
            {
                "sub_id": 1,
                "title": "",
                "type": "text",
                "value": "日本",
            }
        ],
    },
    {
        "id": 3,
        "title": "大陸籍",
        "checked": false,
        "sub_items": [
            {
                "sub_id": 1,
                "title": "一般生",
                "type": "radio",
                "value": false,
            },
            {
                "sub_id": 2,
                "title": "閩江學院生",
                "type": "radio",
                "value": false,
            },
            {
                "sub_id": 3,
                "title": "交換學生",
                "type": "radio",
                "value": false,
            }
        ],
    }
]
var psychologist_arr = [
    {
        "id": 1,
        "title": "心理師A",
        "avaliable_time": {
            1: [],
            2: [8, 9, 10, 11, 13, 14, 15, 16, 17, 19, 20, 21, 22],
            3: [8, 9, 10, 11, 13, 14, 15, 16, 17, 19, 20, 21, 22],
            4: [8, 9, 10, 11, 13, 14, 15, 16, 17, 19, 20, 21, 22],
            5: [8, 9, 10, 11, 13, 14, 15, 16, 17, 19], // , 20, 21, 22
        }
    },
    {
        "id": 2,
        "title": "心理師B",
        "avaliable_time": {
            1: [13, 14, 15, 16, 17, 19, 20, 21, 22],
            2: [13, 14, 15, 16, 17, 19, 20, 21, 22],
            3: [13, 14, 15, 16, 17, 19, 20, 21, 22],
            4: [13, 14, 15, 16, 17, 19, 20, 21, 22],
            5: [13, 14, 15, 16, 17],
        }
    },
    {
        "id": 3,
        "title": "心理師C",
        "avaliable_time": {
            1: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            2: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            3: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            4: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            5: [8, 9, 10, 11, 13, 14, 15, 16, 17],
        }
    }
];
var room_arr = [
    {
        "id": 1,
        "title": "諮商室A",
        "avaliable_time": {
            1: [],
            2: [8, 9, 10, 11, 13, 14, 15, 16, 17, 19, 20, 21, 22],
            3: [8, 9, 10, 11, 13, 14, 15, 16, 17, 19, 20, 21, 22],
            4: [8, 9, 10, 11, 13, 14, 15, 16, 17, 19, 20, 21, 22],
            5: [8, 9, 10, 11, 13, 14, 15, 16, 17, 19], // , 20, 21, 22
        }
    },
    {
        "id": 2,
        "title": "諮商室B",
        "avaliable_time": {
            1: [13, 14, 15, 16, 17, 19, 20, 21, 22],
            2: [13, 14, 15, 16, 17, 19, 20, 21, 22],
            3: [13, 14, 15, 16, 17, 19, 20, 21, 22],
            4: [13, 14, 15, 16, 17, 19, 20, 21, 22],
            5: [13, 14, 15, 16, 17],
        }
    },
    {
        "id": 3,
        "title": "諮商室C",
        "avaliable_time": {
            1: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            2: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            3: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            4: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            5: [8, 9, 10, 11, 13, 14, 15, 16, 17],
        }
    },
    {
        "id": 9,
        "title": "諮商室D",
        "avaliable_time": {
            1: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            2: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            3: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            4: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            5: [8, 9, 10, 11, 13, 14, 15, 16, 17],
        }
    },
    {
        "id": 10,
        "title": "諮商室E",
        "avaliable_time": {
            1: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            2: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            3: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            4: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            5: [8, 9, 10, 11, 13, 14, 15, 16, 17],
        }
    },
    {
        "id": 11,
        "title": "諮商室F",
        "avaliable_time": {
            1: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            2: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            3: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            4: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            5: [8, 9, 10, 11, 13, 14, 15, 16, 17],
        }
    },
    {
        "id": 12,
        "title": "諮商室G",
        "avaliable_time": {
            1: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            2: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            3: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            4: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            5: [8, 9, 10, 11, 13, 14, 15, 16, 17],
        }
    },
    {
        "id": 4,
        "title": "心測室",
        "avaliable_time": {
            1: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            2: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            3: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            4: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            5: [8, 9, 10, 11, 13, 14, 15, 16, 17],
        }
    },
    {
        "id": 5,
        "title": "初談室",
        "avaliable_time": {
            1: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            2: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            3: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            4: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            5: [8, 9, 10, 11, 13, 14, 15, 16, 17],
        }
    },
    {
        "id": 6,
        "title": "多功能教室",
        "avaliable_time": {
            1: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            2: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            3: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            4: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            5: [8, 9, 10, 11, 13, 14, 15, 16, 17],
        }
    },
    {
        "id": 7,
        "title": "會議室",
        "avaliable_time": {
            1: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            2: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            3: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            4: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            5: [8, 9, 10, 11, 13, 14, 15, 16, 17],
        }
    },
    {
        "id": 8,
        "title": "團體室",
        "avaliable_time": {
            1: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            2: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            3: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            4: [8, 9, 10, 11, 13, 14, 15, 16, 17],
            5: [8, 9, 10, 11, 13, 14, 15, 16, 17],
        }
    }
]
var permission_arr = {
    "前台功能": {},
    "後台功能": {
        "初談預約管理": ["case_list.html", true],
        "派案管理": ["counselling_list.html", true],
        "諮商空間維護": ["room_list.html", true],
        "心理師維護": ["psychologist_list.html", false],
        "設定": ["setting_mang.html", false],
        // "使用者帳號綁定": ["user_list.html", false],
        "身分權限設定": ["role_list.html", false],
        "管理員維護": ["admin_list.html", false],
        "個人資料": ["me_mang.html", false],
    },
}


$(function () {

    if ((location.pathname).indexOf("ConsultationPsyMang") >= 0) {
        //###############以下為通用fun#######################//

        //避免任何場合下ENTER送出
        $(document).on('keypress', 'form', function (e) {
            var code = e.keyCode || e.which;
            if (code == 13 && !$(e.target).is('textarea,input[type="submit"],input[type="button"]')) {
                (e.preventDefault) ? e.preventDefault() : e.returnValue = false;
                return false;
            }
        });

        // $('[data-toggle="tooltip"]').tooltip()
        $(document).tooltip({selector: '[data-toggle="tooltip"]'});

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

        // 心理師
        if ($("[name=psychologist_id]").length > 0) {
            var is_list = (location.pathname).indexOf("_list.") >= 0;
            var main_type = is_list ? "全部負責心理師" : "請選擇負責心理師";

            var is_update = getUrlParam("model") === "update";
            var defaults_type = ""
            if (is_update) {
                defaults_type = "心理師B"
            } else if (is_list) {
                defaults_type = getUrlParam("psychologist_id")
            }

            $("[name=psychologist_id]").html(genType(psychologist_arr, defaults_type, main_type));
        }

        // 諮商空間
        if ($("[name=room_id]").length > 0) {
            var is_list = (location.pathname).indexOf("_list.") >= 0;
            var main_type = is_list ? "全部負責諮商空間" : "請選擇諮商空間";

            var is_update = getUrlParam("model") === "update";
            var defaults_type = ""
            if (is_update) {
                defaults_type = "諮商室A"
            } else if (is_list) {
                defaults_type = getUrlParam("room_id")
            }

            $("[name=room_id]").html(genType(room_arr, defaults_type, main_type));
        }

        // 諮詢問題類別
        if ($("#question_id").length > 0) {
            var checkbox = '';
            $.each(question_arr, function (ques, check) {
                var checked = check ? "checked" : "";
                checkbox += '<div class="icheck-primary d-inline col-md-2">';
                checkbox += '<input type="checkbox" id="checkbox' + ques + '" name="box_list" ' + checked + ' value="' + ques + '" disabled>';
                checkbox += '<label for="checkbox' + ques + '">' + ques + '</label>';
                checkbox += '</div>\n';
            })

            $("#question_id").html(checkbox)
        }

        // 初談預約-國籍
        if ($("#national_id").length > 0) {
            var checkbox = '';
            $.each(national_arr, function (index, item) {
                var checked = item.checked ? "checked" : "";
                checkbox += '<div class="icheck-primary col-md-12 mt-2">';
                checkbox += '<input type="radio" id="radio' + item.id + '" name="national" ' + checked + ' value="' + item.title + '" >'; // disabled
                checkbox += '<label for="radio' + item.id + '">' + item.title + '</label>';
                if (item.sub_items.length > 0) {
                    $.each(item.sub_items, function (index, sub_item) {
                        var checked = (sub_item.type === "radio" && sub_item.value) ? "checked" : "";
                        if (sub_item.type === "radio") {
                            if (index === 0) {
                                checkbox += '<div class="row">';
                            }
                            checkbox += '<div class="icheck-secondary d-inline col-md-3">';
                        }
                        checkbox += '<input class="form-control" type="' + sub_item.type + '" id="sub_item' + item.id + sub_item.sub_id + '" name="sub_national" ' + checked + ' value="' + sub_item.value + '">'; // disabled
                        if (sub_item.type === "radio") {
                            checkbox += '<label class="text-secondary" for="sub_item' + item.id + sub_item.sub_id + '">' + sub_item.title + '</label>';
                            checkbox += '</div>\n';
                            if (index + 1 === item.sub_items.length) {
                                checkbox += '</div>';
                            }
                        }
                    })
                }
                checkbox += '</div>\n';
            })

            $("#national_id").html(checkbox)
        }

        if ($("[name=finish_time]").length > 0) {
            // 結案時間
            datetimepicker_option.format = "YYYY-MM-DD HH:mm";
            $('[name=finish_time]').daterangepicker({
                // minDate: now_time(),
                autoUpdateInput: false,
                singleDatePicker: true,
                showDropdowns: true,
                timePicker: true,
                timePicker24Hour: true,
                locale: datetimepicker_option,
            });

            $('[name=finish_time]').on('apply.daterangepicker', function (ev, picker) {
                $(this).val(picker.startDate.format('YYYY-MM-DD HH:mm'));
            });
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

        if ((location.pathname).indexOf("/ConsultationPsyMang") >= 0) {

            if ($('#view-calendar').length > 0) {
                // 有排案的[日期]：若有資料則點擊日期時 hasEvent=true
                var initData = [
                    {
                        date: getISODateTime(now_time(), "yyyy-MM-dd"),
                        classname: "", // td樣式
                        markup: "<span class='badge rounded-pill bg-success'>[day]</span>", // html
                        orders: [
                            {
                                id: 1,
                                room_id: "room1",
                                room_title: "諮商室A",
                                psychologist_title: "心理師A",
                                student_number: "B852145",
                                start_time: "11:00",
                                end_time: "12:00",
                            },
                            {
                                id: 2,
                                room_id: "room1",
                                room_title: "諮商室A",
                                psychologist_title: "心理師B",
                                student_number: "B451348",
                                start_time: "08:00",
                                end_time: "09:00",
                            },
                            {
                                id: 5,
                                room_id: "room3",
                                room_title: "諮商室C",
                                psychologist_title: "心理師B",
                                student_number: "B988456",
                                start_time: "09:00",
                                end_time: "10:00",
                            }
                        ]
                    }, {
                        date: GetDateStr(+1),
                        classname: "",
                        markup: "<span class='badge rounded-pill bg-success'>[day]</span>",
                        orders: [
                            {
                                id: 3,
                                room_id: "room1",
                                room_title: "諮商室A",
                                psychologist_title: "心理師A",
                                student_number: "B123456",
                                start_time: "11:00",
                                end_time: "12:00",
                            },
                            {
                                id: 4,
                                room_id: "room3",
                                room_title: "諮商室C",
                                psychologist_title: "心理師C",
                                student_number: "B789456",
                                start_time: "15:00",
                                end_time: "17:00",
                            }
                        ]
                    }, {
                        date: GetDateStr(-15),
                        classname: "", // td樣式
                        markup: "<span class='badge rounded-pill bg-success'>[day]</span>", // html
                        orders: [
                            {
                                id: 6,
                                room_id: "room3",
                                room_title: "諮商室C",
                                psychologist_title: "心理師A",
                                student_number: "B852145",
                                start_time: "08:00",
                                end_time: "10:00",
                            },
                            {
                                id: 7,
                                room_id: "room2",
                                room_title: "諮商室B",
                                psychologist_title: "心理師A",
                                student_number: "B451348",
                                start_time: "11:00",
                                end_time: "12:00",
                            },
                            {
                                id: 8,
                                room_id: "room1",
                                room_title: "諮商室A",
                                psychologist_title: "心理師C",
                                student_number: "B988456",
                                start_time: "09:00",
                                end_time: "10:00",
                            }
                        ]
                    }
                ]


                // 目前排案狀態參考
                var $calendar = $('#view-calendar');

                /* 套件範例：https://calendar.wrick17.com/ */
                // function selectDate(date) {
                //     $calendar.updateCalendarOptions({
                //         date: date
                //     });
                // }
                //
                // var options = {
                //     prevButton: '上月',
                //     nextButton: '下月',
                //     todayButtonContent: '今天',
                //     monthYearOrder: 'ym',
                //     monthYearSeparator: '年',
                //     showYearDropdown: true,
                //     startOnMonday: true,
                //     monthMap: monthTitleArr,
                //     alternateDayMap: weekTitleArr,
                //     onClickDate: selectDate,
                //     disable: function (date) {
                //         var weekday = date.getDay();
                //         return weekday === 0 || weekday === 6;
                //     },
                //
                // }
                // var calendar = $calendar.calendar(options);

                /* 套件範例: https://www.zabuto.com/dev/calendar/demo/#basic */
                $calendar.zabuto_calendar({
                    language: 'cn', // 本地化語言
                    header_format: '[year]年 [month]', // header顯示格式
                    classname: 'table table-bordered lightgrey-weekends clickable',
                    today_markup: '<span class="bg-gradient-blue">[day]</span>', // [今天]樣式
                    navigation_markup: { // 月份前後切換樣式
                        prev: '<i class="fas fa-chevron-circle-left"></i>',
                        next: '<i class="fas fa-chevron-circle-right"></i>'
                    },
                    events: initData,
                    // ajax:"get_data.php", // 從api取得events
                    // show_days: true, // 是否顯示[星期幾]
                    // week_starts: 'monday', // 每周從星期幾開始
                });

                // 切換月份
                $calendar.on('zabuto:calendar:goto', function (e) {
                    // console.log('zabuto:calendar:goto' + ' year=' + e.year + ' month=' + e.month)
                });

                // 點擊日期：列出有哪些排案紀錄
                $calendar.on('zabuto:calendar:day', function (e) {
                    var weekday = e.date.getDay(); // 星期幾
                    var str = '<h5><b>' + e.value + '（' + weekTitleArr[weekday] + '）</b></h5>\n';
                    if (e.eventdata !== null) {
                        var events = e.eventdata.events[0].orders;

                        // 按照[教室]整合
                        var room_data = {}
                        $.each(events, function (index, item) {
                            if (room_data[item.room_id] === undefined) {
                                room_data[item.room_id] = {
                                    room_title: item.room_title,
                                    room_events: []
                                }
                            }

                            room_data[item.room_id].room_events.push(item)
                        });

                        // 按照[開始時間]排序
                        $.each(room_data, function (room_id, item) {
                            str += '<span class="badge badge-warning mx-1">' + item.room_title + '</span> ';
                            str += '<ul class="list-group list-group-flush text-sm">';
                            item.room_events
                                .sort(function (a, b) {
                                    var a_hour = parseInt(a.start_time.substr(0, 2));
                                    var b_hour = parseInt(b.start_time.substr(0, 2));
                                    return a_hour - b_hour;
                                })
                                .forEach(function (event) {
                                    str += '<li class="list-group-item p-1">';
                                    str += event.start_time + '~' + event.end_time;
                                    str += '<span class="mx-1">' + event.psychologist_title + '</span> ';
                                    str += '<span class="mx-1">' + event.student_number + '</span> ';
                                    str += '</li>';
                                });
                            str += '</ul>';
                        })

                    } else {
                        str += '<span class="text-muted">無排案紀錄</span>';
                    }

                    $("#choose-day-events").html(str);
                });

                $(".zabuto-calendar__day--today").click(); // 自動觸發[今天]日期
            }

            // 諮商日期
            $('input[name=counselling_date]').daterangepicker({
                minDate: getISODateTime(now_time(), "yyyy-MM-dd"),
                autoUpdateInput: false,
                singleDatePicker: true,
                showDropdowns: true,
                // timePicker: true,
                // timePicker24Hour: true,
                locale: datetimepicker_option,
                isInvalidDate: function (date) {
                    var weekday = date.weekday();
                    return weekday === 0 || weekday === 6;
                }
            });

            $('input[name=counselling_date]').on('apply.daterangepicker', function (ev, picker) {
                $(this).val(picker.startDate.format('YYYY-MM-DD'));
            });

            // 諮商時間-開始時間
            $("[name=start_time]").timepicker({
                timeFormat: 'H:i',
                step: 60,
                minTime: '8:00',
                maxTime: '21:00',
            });
            $("[name=start_time]").on("change", function (e) {
                e.preventDefault();
                var this_hour = parseInt(e.target.value.substr(0, 2));
                var end_hour = parseInt($("[name=end_time]").val().substr(0, 2));
                if (isNaN(this_hour)) {
                    $(this).timepicker('setTime', '');
                    return false;
                }

                if (this_hour < 8) { // 不得小於minTime
                    this_hour = 8;
                } else if (this_hour > 21) { // 不得大於maxTime
                    this_hour = 21;
                }

                $(this).timepicker('setTime', this_hour + ':00');
                if (this_hour >= end_hour) { // 如果開始時間大於等於結束時間, 就將結束時間=開始時間往後推延1小時
                    var next_hour = this_hour + 1;
                    $("[name=end_time]").timepicker('setTime', next_hour.toString() + ':00')
                }
            });

            // 諮商時間-結束時間
            $("[name=end_time]").timepicker({
                timeFormat: 'H:i',
                step: 60,
                minTime: '9:00',
                maxTime: '22:00',
            });
            $("[name=end_time]").on("change", function (e) {
                e.preventDefault();
                var this_hour = parseInt(e.target.value.substr(0, 2));
                var start_hour = parseInt($("[name=start_time]").val().substr(0, 2));
                if (isNaN(this_hour)) {
                    $(this).timepicker('setTime', '');
                    return false;
                }

                if (this_hour > 22) { // 不得大於maxTime
                    this_hour = 22;
                } else if (this_hour < 9) { // 不得小於minTime
                    this_hour = 9;
                }

                $(this).timepicker('setTime', this_hour + ':00');
                if (this_hour <= start_hour) { // 如果結束時間小於等於結束時間, 就將開始時間=結束時間往前推延1小時
                    var prev_hour = this_hour - 1;
                    $("[name=start_time]").timepicker('setTime', prev_hour.toString() + ':00')
                }
            });

            // 檢查學號是否正確(是否為在校生)
            // $("[name=school_number]").change(function () {
            //     var val = $(this).val();
            //     var soap = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            //         "<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
            //         "<soap:Body>" +
            //         "<isMyStudent xmlns=\"http://tempuri.org/\">" +
            //         "<sToken>zxcv!@#$\\][p</sToken>" +
            //         "<stdno>" + val + "</stdno>" +
            //         "</isMyStudent>" +
            //         "</soap:Body>" +
            //         "</soap:Envelope>";
            //
            //     var settings = {
            //         "url": "https://ap2.pccu.edu.tw:8888/stdmservice/service.asmx",
            //         "method": "POST",
            //         "timeout": 0,
            //         "headers": {
            //             "Content-Type": "text/xml; charset=utf-8",
            //             "SOAPAction": "\"http://tempuri.org/isMyStudent\""
            //         },
            //         "data": soap,
            //     };
            //
            //     $.ajax(settings).done(function (response) {
            //         console.log(response);
            //     });
            // })


            // 心理師-可預約時間(上班時間)
            $("[name=psychologist_id]").change(function () {
                var val = $("option:selected", this).val();
                var avaliable_time = psychologist_arr[val - 1].avaliable_time;
                var str = avaliableTime(avaliable_time);
                $("#psychologist_avaliable_time").html(str);
            });

            // 諮商空間-可預約時間
            $("[name=room_id]").change(function () {
                var val = $("option:selected", this).val();
                var avaliable_time = room_arr[val - 1].avaliable_time;
                var str = avaliableTime(avaliable_time);
                $("#room_avaliable_time").html(str);
            });

            function avaliableTime(data) {
                var arr = {};
                var str = '<h6><u>可預約時間參考</u></h6>\n<ul class="list-group list-group-flush text-sm">';
                $.each(data, function (day, times) {
                    arr[day] = [];
                    var tmp = [];
                    var len = times.length;
                    str += '<li class="list-group-item p-1">';
                    str += '<span class="badge badge-dark mx-1">' + weekTitleArr[day] + '</span> ';

                    if (len > 0) {
                        $.each(times, function (key, time) {
                            if (tmp[0] === undefined) {
                                tmp[0] = time;
                            }

                            if (key + 1 <= len) {
                                var next_time = times[key + 1];
                                if (time + 1 !== next_time) {
                                    if (tmp[1] === undefined) {
                                        tmp[1] = time;
                                        str += tmp[0] + '~' + (tmp[1] + 1) + '點；';
                                        arr[day].push(tmp);
                                        tmp = [];
                                    }
                                }
                            }
                        })
                    } else {
                        str += '無可預約時間';
                    }
                    str += '</li>';
                });
                str += '</ul>';

                return str;
            }
        }

        if ((location.pathname).indexOf("/room_mang") >= 0 || (location.pathname).indexOf("/ConsultationPsyMang") >= 0) {
            var model = getUrlParam("model") !== "" ? getUrlParam("model") : "add";

            /* 套件範例: https://duoani.github.io/jquery.scheduler/example/ */
            // 設定本地化文字
            $.fn.scheduler.locales["language"] = {
                AM: "上午",
                PM: "下午",
                TIME_TITLE: "時間",
                WEEK_TITLE: "",
                WEEK_DAYS: Object.values(weekTitleArr),
                DRAG_TIP: '可移動鼠標選擇時段',
                RESET: '清空選擇'
            }

            if ($('#schedule-table').length > 0) {
                // 初始資料
                var initData = {
                    1: [8, 9, 10, 11, 13, 14, 15, 16, 17, 19, 20, 21, 22],
                    2: [8, 9, 10, 11, 13, 14, 15, 16, 17, 19, 20, 21, 22],
                    3: [8, 9, 10, 11, 13, 14, 15, 16, 17, 19, 20, 21, 22],
                    4: [8, 9, 10, 11, 13, 14, 15, 16, 17, 19, 20, 21, 22],
                    5: [8, 9, 10, 11, 13, 14, 15, 16, 17, 19, 20, 21, 22],
                };

                if (model === "add") {
                    initData = {}
                }

                $('#schedule-table').scheduler({
                    data: initData, // 初始資料
                    locale: 'language', // 本地化文字
                    footer: true, // 是否顯示footer
                    onSelect: function (newData) {
                        // console.log(newData); // 列出當前選擇的所有時段(格式同initData)
                    }
                });

                // var parseStr = $.fn.scheduler.util.parse(str); // 取得當前選擇的所有時段(格式同initData)
            }

            //if ($('#psychologist-schedule-table').length > 0) {
            //    // 初始資料
            //    var initData = {
            //        1: [8, 9, 10, 11, 19, 20, 21, 22],
            //        2: [8, 9, 10, 11, 15, 16, 17, 19, 20, 21, 22],
            //        3: [8, 9, 10, 11, 15, 16, 17, 19, 20, 21, 22],
            //        4: [8, 9, 10, 11, 15, 16, 17],
            //        5: [13, 14, 15, 16, 17, 19, 20, 21, 22],
            //    };

            //    $('#psychologist-schedule-table').scheduler({
            //        data: initData, // 初始資料
            //        locale: 'language', // 本地化文字
            //        footer: true, // 是否顯示footer
            //        onSelect: function (newData) {
            //            // console.log(newData); // 列出當前選擇的所有時段(格式同initData)
            //        }
            //    });

            //    // var parseStr = $.fn.scheduler.util.parse(str); // 取得當前選擇的所有時段(格式同initData)
            //}

            /* 套件範例: https://github.com/artsy/day-schedule-selector */
            // $("#day-schedule").dayScheduleSelector({
            //     days: [1, 2, 3, 4, 5], // 0=星期天 ~ 6=星期六
            //     stringDays: ['星期一', '星期二', '星期三', '星期四', '星期五', '星期六', '星期日'], // 欄位文字
            //     interval: 60, // 時間區間
            //     startTime: '08:00', // 開始時間
            //     endTime: '22:00', // 結束時間
            // });
            //
            // var selected = {
            //     1: [['09:00', '12:00'], ['13:00', '17:00'], ['19:00', '21:00']],
            //     2: [['09:00', '12:00'], ['13:00', '17:00'], ['19:00', '21:00']],
            //     3: [['09:00', '12:00'], ['13:00', '17:00'], ['19:00', '21:00']],
            //     4: [['09:00', '12:00'], ['13:00', '17:00'], ['19:00', '21:00']],
            //     5: [['09:00', '12:00'], ['13:00', '17:00']],
            //     // 6: [],
            //     // 0: [],
            // }
            //
            // $("#day-schedule").on('change.artsy.dayScheduleSelector', function (e, selected) {
            //     console.log(e, selected);
            // })
            // $("#day-schedule").on('selected.artsy.dayScheduleSelector', function (e, selected) {
            //     console.log(e, selected);
            // })
            //
            // // 初始化資料
            // $("#day-schedule").data('artsy.dayScheduleSelector').deserialize(selected);


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

        // 是否結案/完成。若[是]，則須填寫結案時間
        if ($("[name=is_finish]").length > 0) {
            $("[name=is_finish]").change(function () {
                var val = $("option:selected", this).val();
                console.log(val)
                if (val === "是") {
                    $("[name=finish_time]").attr("req", "Y").parents('div:first').removeClass("hide").find('.required').text('*');
                } else {
                    $("[name=finish_time]").removeAttr("req").parents('div:first').addClass("hide").find('.required').text('');
                }
            }).change();
        }

        if ((location.pathname).indexOf("case_") >= 0) {
            // 是否受理/接案。若[是]，則須填寫結案時間
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

            // 可初談時段
            var week_arr = ["mon", "tue", "wed", "thu", "fri"];
            var time_arr = [9, 10, 11, 12, 13, 14, 15];
            var checked_arr = ["mon-9", "mon-10", "mon-11", "mon-12", "mon-13", "mon-14", "wed-9", "wed-10", "wed-11", "wed-12", "fri-9", "fri-10", "fri-11", "fri-12"];
            var time_table = "";
            $.each(time_arr, function (index, time) {
                time_table += '<tr>\n';
                time_table += '<th>' + time + '-' + (time + 1);
                // time_table += ' <input type="checkbox" name="toggle-all-horizontal" disabled data-toggle="tooltip" data-title="全選每天' + time + '-' + (time + 1) + '"></th>\n';
                $.each(week_arr, function (index, week) {
                    var val = week + '-' + time;
                    var checked = checked_arr.indexOf(val) >= 0 ? "checked" : "";
                    time_table += '<td>' + (checked_arr.indexOf(val) >= 0 ? "<i class=\"fa-solid fa-check text-success\"></i>" : "") + '</td>\n';
                    // time_table += '<td><input type="checkbox" value="' + val + '" name="time[]" ' + checked + '></td>\n';
                })
                time_table += '</tr>';
            })

            $("#time-table").html(time_table);

            // 選擇星期N所有時段
            $("[name=toggle-all-vertical]").change(function () {
                var is_checked = $(this).prop("checked");
                var val = $(this).val();
                $('[value^=' + val + ']').each(function () {
                    $(this).prop("checked", is_checked);
                })
            })

            // 選擇每天的N時段
            $("[name=toggle-all-horizontal]").change(function () {
                var is_checked = $(this).prop("checked");
                $(this).closest("tr").find("[type=checkbox]").each(function () {
                    $(this).prop("checked", is_checked);
                })
            })

            $("[name=toggle-all]").change(function () {
                var is_checked = $(this).prop("checked");
                $(this).closest("tbody").find("td>[type=checkbox]").each(function () {
                    $(this).prop("checked", is_checked);
                })
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
                        $(this).css('background-image', 'url(../../uploads/others/nophoto.png)');
                    })
                    $('.filesupload[data-file]').each(function () {
                        $(this).parents('td:first').find('#filename').addClass('text-muted').text("尚未選擇檔案...");
                    })
                } else if (model === "update") {
                    $('input.file[type=file]').each(function () {
                        var file = aes_decrypt($(this).data('file'));
                        $(this).parents('td').find('.upload_cover').css('background-image', 'url(../../uploads/others/' + file + ')');

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
                            window.open('../../uploads/excels/' + filename);
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

function genType(arr = psychologist_arr, defaults = "", main = "請選擇校安事件主類別") {
    var option = (main !== "") ? '<option value="">' + main + '</option>\n' : '';
    $.each(arr, function (index, item) {
        if (item.title === defaults) {
            option += '<option value="' + item.id + '" selected>' + item.title + '</option>\n';
        } else {
            option += '<option value="' + item.id + '">' + item.title + '</option>\n';
        }
    });

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

//N天後或N天前, GetDateStr(+-5) 或 特定日期 GetDateStr(+-5,"2016-05-05");
function GetDateStr(AddDayCount, date) {
    var arr = (date) ? date.split("-") : [];
    var dd = (date) ? new Date(arr[0], arr[1] - 1, arr[2]) : new Date();
    dd.setDate(dd.getDate() + AddDayCount);//获取AddDayCount天后的日期
    //console.log(datetime_to_unix("2016-01-01 12:00:00").getDate())
    var y = dd.getFullYear();

    var m = (dd.getMonth() + 1 < 10) ? "0" + (dd.getMonth() + 1) : dd.getMonth() + 1;
    var d = (dd.getDate() < 10) ? "0" + dd.getDate() : dd.getDate();
    return y + "-" + m + "-" + d;
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