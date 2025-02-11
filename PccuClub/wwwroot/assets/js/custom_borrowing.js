// 呈現之數據是否包含[已提出借用但是尚未審核成功]者?
var resource_arr = [
    {
        "id": "1",
        "name": "MA", // 名稱
        "type": "社團器材", // 類別
        "total_real": "12", // 實際庫存：不顯示於前台，表示[實際可借用總數量]+[保留數量]
        "total": "8", // 已上架庫存：顯示於前台，表示[實際可借用總數量]
        "single_limit": "2", // 單次可借用數量上限：預約可選擇之數量不得高於此上限，0=沒限制
        "remark": "", // 備註
        "disabled": "0", // 是否啟用：0啟用、1停用
        "sort": "0",　// 排序
        "created_at": "1494990675", // 建立時間
        "updated_at": "1523021756", // 更新時間
        "current_available": "4", // 當日剩餘可借用數量
        "safety_count": "5", // 安全庫存上限：當預約當下庫存(current_available)已低於安全庫存時，0=沒限制
        "safety_note": "可能無法順利借用" // 安全庫存提示：當預約當下庫存(current_available)已低於安全庫存時，顯示後台設定之提示訊息
    }, {
        "id": "2",
        "name": "喇叭",
        "type": "社團器材",
        "total_real": "10",
        "total": "8",
        "single_limit": "2",
        "remark": "含麥克風*1、電源組 (請自備麥克風電池)",
        "disabled": "0",
        "sort": "0",
        "created_at": "1494990970",
        "updated_at": "1681889991",
        "current_available": "6",
        "safety_count": "2",
        "safety_note": ""
    }, {
        "id": "3",
        "name": "喇叭腳架",
        "type": "社團器材",
        "total_real": "12",
        "total": "12",
        "single_limit": "2",
        "remark": "",
        "disabled": "0",
        "sort": "0",
        "created_at": "1494991194",
        "updated_at": "1683515290",
        "current_available": "10",
        "safety_count": "2",
        "safety_note": ""
    }, {
        "id": "5",
        "name": "3.5-3.5",
        "type": "社團器材",
        "total_real": "5",
        "total": "2",
        "single_limit": "2",
        "remark": "需100人以上活動方可借用",
        "disabled": "0",
        "sort": "0",
        "created_at": "1494991674",
        "updated_at": "1494991674",
        "current_available": "0",
        "safety_count": "2",
        "safety_note": ""
    }, {
        "id": "6",
        "name": "大恩館",
        "type": "海報欄位",
        "total_real": "20",
        "total": "15",
        "single_limit": "1",
        "remark": "",
        "disabled": "0",
        "sort": "0",
        "created_at": "1494991797",
        "updated_at": "1494991824",
        "current_available": "5",
        "safety_count": "6",
        "safety_note": ""
    }, {
        "id": "8",
        "name": "大賢館",
        "type": "海報欄位",
        "total_real": "20",
        "total": "14",
        "single_limit": "2",
        "remark": "",
        "disabled": "0",
        "sort": "0",
        "created_at": "1494992089",
        "updated_at": "1499145138",
        "current_available": "10",
        "safety_count": "0",
        "safety_note": ""
    }, {
        "id": "10",
        "name": "拐杖",
        "type": "醫療用品",
        "total_real": "230",
        "total": "200",
        "single_limit": "0",
        "remark": "",
        "disabled": "0",
        "sort": "0",
        "created_at": "1494992452",
        "updated_at": "1581041388",
        "current_available": "98",
        "safety_count": "30",
        "safety_note": ""
    }, {
        "id": "12",
        "name": "急救箱",
        "type": "醫療用品",
        "total_real": "60",
        "total": "40",
        "single_limit": "3",
        "remark": "",
        "disabled": "0",
        "sort": "0",
        "created_at": "1494992672",
        "updated_at": "1494992672",
        "current_available": "30",
        "safety_count": "10",
        "safety_note": ""
    }, {
        "id": "14",
        "name": "文學院-領巾",
        "type": "學位服",
        "total_real": "350",
        "total": "250",
        "single_limit": "0",
        "remark": "",
        "disabled": "0",
        "sort": "0",
        "created_at": "1494992802",
        "updated_at": "1494994537",
        "current_available": "250",
        "safety_count": "100",
        "safety_note": ""
    }, {
        "id": "15",
        "name": "文學院-披風",
        "type": "學位服",
        "total_real": "350",
        "total": "250",
        "single_limit": "0",
        "remark": "",
        "disabled": "0",
        "sort": "0",
        "created_at": "1494992843",
        "updated_at": "1494992843",
        "current_available": "250",
        "safety_count": "100",
        "safety_note": ""
    }, {
        "id": "16",
        "name": "文學院-學士帽",
        "type": "學位服",
        "total_real": "350",
        "total": "250",
        "single_limit": "0",
        "remark": "",
        "disabled": "0",
        "sort": "0",
        "created_at": "1496387557",
        "updated_at": "1496387557",
        "current_available": "250",
        "safety_count": "100",
        "safety_note": ""
    }, {
        "id": "19",
        "name": "理學院-領巾",
        "type": "學位服",
        "total_real": "500",
        "total": "480",
        "single_limit": "1",
        "remark": "",
        "disabled": "0",
        "sort": "18",
        "created_at": "1581041047",
        "updated_at": "1590125850",
        "current_available": "480",
        "safety_count": "100",
        "safety_note": ""
    }
];
var people_arr = [
    {
        "id": 1,
        "type": "學生社團",
        "name": "勞動暨人力資源學系系學會",
        "host": "王力（總務）",
        "reason": "宿營練習",
        "resources": [
            {
                "id": "1",
                "resource_id": "1", // 器材id
                "resource_name": "MA", // 器材名稱
                "resource_type": "社團器材", // 器材類別
                "total": "2", // 借用數量
                "created_at": "1494990675",
                "updated_at": "1523021756",
            }
        ],
    },
    {
        "id": 2,
        "type": "學生社團",
        "name": "西洋占星術研究社",
        "host": "李興如",
        "reason": "士林大富翁",
        "resources": [
            {
                "id": "1",
                "resource_id": "8",
                "resource_name": "大賢館",
                "resource_type": "海報欄位",
                "total": "1",
                "created_at": "1494992089",
                "updated_at": "1499145138",
            },
            {
                "id": "2",
                "resource_id": "6",
                "resource_name": "大恩館",
                "resource_type": "海報欄位",
                "total": "1",
                "created_at": "1494991797",
                "updated_at": "1494991824",
                "current_available": "5"
            }
        ],
    },
    {
        "id": 3,
        "type": "學生社團",
        "name": "美術系學會",
        "host": "柯如營",
        "reason": "寒假育樂營",
        "resources": [
            {
                "id": "1",
                "resource_id": "1",
                "resource_name": "MA", // 器材名稱
                "resource_type": "社團器材", // 器材類別
                "total": "1",
                "created_at": "1494990675",
                "updated_at": "1523021756",
            }, {
                "id": "2",
                "resource_id": "2",
                "resource_name": "喇叭",
                "resource_type": "社團器材",
                "total": "2",
                "created_at": "1494990970",
                "updated_at": "1681889991",
            }, {
                "id": "3",
                "resource_id": "3",
                "resource_name": "喇叭腳架",
                "resource_type": "社團器材",
                "total": "2",
                "created_at": "1494991194",
                "updated_at": "1683515290",
                "current_available": "10"
            }, {
                "id": "4",
                "resource_id": "5",
                "resource_name": "3.5-3.5",
                "resource_type": "社團器材",
                "total": "2",
                "created_at": "1494991674",
                "updated_at": "1494991674",
            }
        ],
    },
    {
        "id": 4,
        "type": "應屆畢業生",
        "name": "跆拳道社",
        "host": "柯如營",
        "reason": "寒假育樂營",
        "resources": [
            {
                "id": "1",
                "resource_id": "19",
                "resource_name": "理學院-領巾",
                "resource_type": "學位服",
                "total": "50",
                "created_at": "1581041047",
                "updated_at": "1590125850",
            }
        ]

    }
];
var startDate = "";
var endDate = "";
$(document).ready(function () {
    // 警示重置
    $(document).on('focus', '.alert-border', function () {
        $(this).removeClass("alert-border");
    })
    $(document).on('change', '.alert-border', function () {
        $(this).removeClass("alert-border");
    })
    $(document).on('click', '.warning-box label, .warning-box .form-check-input', function () {
        $(this).closest(".warning-box").find(".form-check-input").removeClass("alert-border")
    })

    // unique
    function unique(list) {
        var result = [];
        $.each(list, function (i, e) {
            if ($.inArray(e, result) == -1) result.push(e);
        });
        return result;
    }

    if ($(".checkthisform").length > 0) {
        $(document).on('click', ".submit-btn", function (e) {
            e.preventDefault();
            var sum_arr = [];
            var this_form = Boolean($(this).parents('form')) ? $(this).parents('form') : $('#' + $(this).attr('form'));


            this_form.find("[req=Y]").each(function () {
                var tooltips = $(this).attr("data-tooltip");
                var title = Boolean(tooltips) ? $.trim($(this).attr("data-tooltip")) : $.trim($(this).attr("title"));
                if ($(this).val() == "" || $(this).val() == null) {
                    $(this).addClass("alert-border");

                    sum_arr.push(title);
                } else if ((this.className).indexOf("form-check-input") >= 0) {
                    var this_group = $(this).attr("name")
                    if ($(this).closest(".warning-box").find("[name=" + this_group + "]:checked").length <= 0) {
                        $(this).addClass("alert-border");

                        sum_arr.push(title);
                    }
                }
            })

            if ($("#event_des").length > 0) {
                if ($("#event_des").find(".item").length <= 0) {
                    $("#resource_id").addClass("alert-border");
                    sum_arr.push($("#event_des").attr("title"));
                }
            }

            if (sum_arr.length > 0) {
                sum_arr = unique(sum_arr);
                sum_arr = sum_arr.join("、");
                alert(sum_arr)
                $('html, body').animate({
                    scrollTop: $('.alert-border:first').offset().top - 200
                }, 500);

            } else {
                // this_form.submit();
                if ((location.href).indexOf("record_add.") >= 0) {
                    if (confirm("確認要送出嗎？") == false) {
                        return false
                    } else {
                        location.href = "record.html"
                    }
                }
            }

        })
    }

    if ($("#calendar").length > 0) {
        var today = getISODateTime(now_time(), "yyyy-MM-dd");
        $(".dateTitle").text(today);
        $("#ConditionModel_SDate").val(today);

        var calendar = new VanillaCalendar('#calendar', {
            settings: {
                lang: 'define',
                iso8601: false,
                selected: {
                    dates: [today],
                },
            },
            locale: {
                months: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],
                weekday: ['日', '一', '二', '三', '四', '五', '六'],
            },
            actions: {
                clickDay(event, self) {
                    if (self !== undefined) {
                        var date = self[0];
                        $("#ConditionModel_SDate").val(date);
                        GoSearch();
                        //getBorrowList(date);
                        //getPeopleList(date);
                    }
                },
            },
        });

        calendar.init();
        //getBorrowList(today);
        //getPeopleList(today);

        // 資源借用狀況
        function getBorrowList(date = "2024-01-15", type = "") {
            var newDate = new Date(date);  // 僅作展示使用
            var weekday = newDate.getDay(); // 僅作展示使用
            var random = Math.floor(Math.random() * 5); // 僅作展示使用
            var standard = (random > 0) ? weekday / random : 1; // 僅作展示使用

            var str = '';
            // 篩選資源類別
            var filtered = resource_arr;
            if (type !== "") {
                filtered = $.grep(resource_arr, function (item) {
                    return item.type === type;
                })
            }

            $.each(filtered, function (index, item) {
                var assume = Math.round(parseInt(item.current_available) * standard); // 僅作展示使用
                var available = assume > parseInt(item.total) ? parseInt(item.total) : assume; // 僅作展示使用
                var zero_class = available <= 0 ? "text-danger" : "";
                str += '<div class="listItem">\n';
                str += '<div class="item item-sm">' + item.type + '</div>\n';
                str += '<div class="item">' + item.name;
                if (item.remark !== "") {
                    str += ' <i class="fa-solid fa-circle-exclamation" data-bs-target="tooltip" title="' + item.remark + '"></i>';
                }
                str += '</div>\n';
                str += '<div class="item ' + zero_class + '">' + available + '</div>\n';
                str += '<div class="item">' + parseInt(item.total) + '</div>\n';
                str += '<div class="item"><a href="resource_detail.html">說明</a></div>\n';
                str += '</div>\n';
            });

            $(".dateTitle").text(date);
            $(".memberlist > .listBody").html(str)
        }

        // 本日借用者(展示使用)
        function getPeopleList(date = "2024-01-16") {
            var random = Math.floor(Math.random() * 4); // 僅作展示使用
            var str = '';

            const filtered = getRandomElementsFromArray(people_arr, random);

            if (filtered.length > 0) {
                $.each(filtered, function (index, item) {
                    str += '<li class="list-group-item d-flex justify-content-between align-items-center">\n';
                    str += '<span class="fs-16 title-2">' + item.name + '</span>\n';
                    str += '<a class="btn btn-sm btn-primary" href="javascript:void(0)" onclick="showDialog(this)" data-id="' + item.id + '" data-date="' + date + '">檢視資訊</a>\n';
                    str += '</li>\n';
                });
            } else {
                str += '<span class="text-muted">本日無借用者</span>';
            }

            $(".borrowlist > .list-group").html(str)
        }

        // 篩選資源類別
        $("#resource_type").change(function () {
            var val = $("option:selected", this).val();
            var selectedDates = calendar.selectedDates; // 當前日曆點選的日期

            //getBorrowList(selectedDates, val);

            
        });

        // 資源備註
        $(document).on("click", "[data-bs-target=\"tooltip\"]", function () {
            var title = $(this).attr("title");
            alert(title);
        })
    }

    if ((location.pathname).indexOf("record_add.") >= 0) {
        $("#start_date").change(function () {
            var start_date = new Date($(this).val());
            var end_date = $("#end_date").val();
            if (end_date !== "") {
                end_date = new Date(end_date);
                var diff = DateDiff(start_date, end_date)
                if (diff < 0) {
                    alert("領取日期不得大於歸還日期");
                    $(this).val(startDate);
                    return false;
                }
            }

            startDate = $(this).val();
        });

        $("#end_date").change(function (e) {
            var start_date = $("#start_date").val();
            var end_date = new Date($(this).val());
            if (start_date !== "") {
                start_date = new Date(start_date);
                var diff = DateDiff(start_date, end_date)
                if (diff < 0) {
                    alert("領取日期不得大於歸還日期");
                    $(this).val(endDate);
                    return false;
                }
            }

            endDate = $(this).val();
        });

        genResourceOption();

        function genResourceOption() {
            var resourceOption = '';
            $.each(resource_arr, function (index, item) {
                var disabled = parseInt(item.current_available) === 0 ? "disabled" : ""; // 若無可借用庫存則不可選擇
                resourceOption += '<option value="' + item.id + '" data-available="' + item.current_available + '" ' + disabled + ' data-once="' + item.single_limit + '">(' + item.type + ') ' + item.name + '</option>';
            });

            $("#resource_id").append(resourceOption);
        }

        // 借用資源
        $(document).on("change", "#resource_id", function () {
            var available = parseInt($("option:selected", this).data("available"));
            var once = parseInt($("option:selected", this).data("once"));
            var resourceNumOption = '<option value="">請選擇借用數量</option>';
            for (let i = 1; i < available + 1; i++) {
                var disabled = (once > 0 && once < i) ? "disabled" : ""; // 資源單次借用上限數量 (0=無限制)
                resourceNumOption += '<option value="' + i + '" ' + disabled + '>' + i + '</option>';
            }

            $("#resource_num").html(resourceNumOption);
        });

        // 新增器材
        $(".addevent-btn").click(function () {
            var sum_arr = [];
            $(this).closest(".infoItem").find(".form-control").each(function () {
                var val = $("option:selected", this).val();
                if (val === "") {
                    var title = $(this).attr("title");
                    $(this).addClass("alert-border");
                    sum_arr.push(title);
                }
            })

            if (sum_arr.length > 0) {
                sum_arr = unique(sum_arr);
                sum_arr = sum_arr.join("、");
                alert(sum_arr)
                $('html, body').animate({
                    scrollTop: $('.alert-border:first').offset().top - 200
                }, 500);
                return false;
            } else {
                var resource_id = $("#resource_id option:selected").val(); // 借用資源id
                var resource_num = parseInt($("#resource_num option:selected").val()); // 借用資源數量
                var index = resource_arr.findIndex(function (e) {
                    return e.id === resource_id;
                });

                var resource = resource_arr[index];
                var resource_name = "(" + resource.type + ") " + resource.name; // 借用資源名稱
                var resource_available = parseInt(resource.current_available); // 資源上架庫存
                var resource_once = parseInt(resource.single_limit); // 資源單次借用上限數量
                var resource_safety = parseInt(resource.safety_count); // 資源安全庫存數量
                var resource_safety_note = resource.safety_note; // 資源安全庫存提示

                if (resource_once > 0 && resource_num > resource_once) {
                    alert("「" + resource_name + "」的單次借用上限數量為：" + resource_once);
                    return false;
                }

                // 當預約當下庫存已低於安全庫存，應顯示後台設定之提示訊息。但不影響送出
                if (resource_safety > resource_available) {
                    var yes = confirm("安全庫存提示!確定要借用嗎?\n\n" + resource_safety_note);
                    if (!yes) {
                        return false;
                    }
                }

                var is_exist = $("[data-resource-id='" + resource_id + "']").length;
                if (is_exist > 0) {
                    var update_check = confirm("是否要更新「" + resource_name + "」的借用數量為：" + resource_num + "？");
                    if (update_check) {
                        $("[data-resource-id='" + resource_id + "']>.location").html('<i class="fas fa-x"></i> ' + resource_num);
                    }
                } else {
                    var str = '<div class="item" data-resource-id="' + resource_id + '">\n';
                    str += '<span class="time w-auto">' + resource_name + '</span>\n';
                    str += '<span class="location"><i class="fas fa-x"></i> ' + resource_num + '</span>\n';
                    str += '<button class="del-btn btn" type="button">移除器材 <i class="fas fa-times"></i></button>\n';
                    str += '</div>\n';

                    $(".dateItem").append(str);
                    genResourceOption();
                }

                // 更新器材暫存數量
                // var index = resource_arr.findIndex(function (e) {
                //     return e.id == resource_id;
                // });
                // resource_arr[index].current_available = resource_arr[index].current_available - resource_num
            }
        });

        // 移除器材
        $(document).on('click', '#event_des .del-btn', function () {
            if (confirm("確認刪除？") == false) {
                return false
            } else {
                $(this).closest(".item").remove();
            }
        })

        // 佐證資料(多檔上傳)
        if ($(".target_list").length > 0) {
            $(document).on('click', '#add_input_btn', function () {
                var addItem = '<div class="item mb-2"><input type="file" class="form-control w-auto" title="請上傳佐證資料" accept=".pdf,.jpg,.jpeg,.png,.zip,.rar,.7z"><button class="del-btn hidden" type="button"><i class="fas fa-times"></i></button></div>';
                checkNum()
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
            }
        }
    }

    // 列印按鈕
    $(document).on('click', '.print-btn', function () {
        window.print();
    })

    // 返回上一頁
    $('.go-back-btn').on('click', function () {
        history.back()
    })

})

//function showDialog(ele) {
//    var data = $(ele).data();
//    var index = people_arr.findIndex(function (e) {
//        return e.id == data.id;
//    });

//    var arr = people_arr[index];

//    var content = '<ul class="list-group">\n';
//    content += '<li class="list-group-item d-flex justify-content-between align-items-center">\n';
//    content += '<span class="fs-16">提出申請日</span>\n';
//    content += '<span class="fs-16">' + GetDateStr(-5, data.date) + '</span>\n';
//    content += '</li>\n';

//    content += '<li class="list-group-item d-flex justify-content-between align-items-center">\n';
//    content += '<span class="fs-16">領取日期</span>\n';
//    content += '<span class="fs-16">' + data.date + '</span>\n';
//    content += '</li>\n';

//    content += '<li class="list-group-item d-flex justify-content-between align-items-center">\n';
//    content += '<span class="fs-16">歸還日期</span>\n';
//    content += '<span class="fs-16">' + GetDateStr(+3, data.date) + '</span>\n';
//    content += '</li>\n';

//    content += '<li class="list-group-item d-flex justify-content-between align-items-center">\n';
//    content += '<span class="fs-16">申請單位類型</span>\n';
//    content += '<span class="fs-16">' + arr.type + '</span>\n';
//    content += '</li>\n';

//    content += '<li class="list-group-item d-flex justify-content-between align-items-center">\n';
//    content += '<span class="fs-16">申請單位</span>\n';
//    content += '<span class="fs-16">' + arr.name + '</span>\n';
//    content += '</li>\n';

//    content += '<li class="list-group-item d-flex justify-content-between align-items-center">\n';
//    content += '<span class="fs-16">申請人員</span>\n';
//    content += '<span class="fs-16">' + arr.host + '</span>\n';
//    content += '</li>\n';

//    content += '<li class="list-group-item d-flex justify-content-between align-items-center">\n';
//    content += '<span class="fs-16">申請目的</span>\n';
//    content += '<span class="fs-16">' + arr.reason + '</span>\n';
//    content += '</li>\n';
//    content += '</ul>\n';

//    if (arr.resources.length > 0) {
//        content += '<div class="font-weight-bold py-2"><i class="fas fa-tag"></i> 借用資源清單</div>\n\n';
//        content += '<ul class="list-group">\n';
//        $.each(arr.resources, function (i, item) {
//            content += '<li class="list-group-item d-flex justify-content-between align-items-center">\n';
//            content += '<span class="fs-16">(' + item.resource_type + ') ' + item.resource_name + '</span>\n';
//            content += '<span class="fs-16"><i class="fas fa-x"></i> ' + item.total + '</span>\n';
//            content += '</li>\n';
//        })
//        content += '</ul>\n';
//    }


    //$.dialog({
    //    title: '檢視借用資源',
    //    backgroundDismiss: true,
    //    content: content
    //    // ajax
    //    // content: function () {
    //    //     var self = this;
    //    //     return $.ajax({
    //    //         url: 'bower.json',
    //    //         dataType: 'json',
    //    //         method: 'get'
    //    //     }).done(function (response) {
    //    //         self.setContent('Description: ' + response.description);
    //    //         self.setContentAppend('<br>Version: ' + response.version);
    //    //         self.setTitle(response.name);
    //    //     }).fail(function () {
    //    //         self.setContent('Something went wrong.');
    //    //     });
    //    // }
    //});
//}

// 亂數取得陣列中的index
function getRandomIndexes(array, num) {
    const indexes = [];

    // 計算原始資料數
    const arrayLength = array.length;

    // 避免取得資料數量 num > 原始資料數量時造成的 Error
    const count = num < array.length ? num : array.length;

    while (indexes.length < count) {
        const randomIndex = Math.floor(Math.random() * arrayLength);
        if (!indexes.includes(randomIndex)) {
            indexes.push(randomIndex);
        }
    }

    return indexes;
}

// 對應陣列index取得資料
function getRandomElementsFromArray(array, count) {
    const randomIndexes = getRandomIndexes(array, count);
    const randomElements = randomIndexes.map(index => array[index]);
    return randomElements;
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

// 天數差
function DateDiff(startTime, endTime) {
    const diffTime = endTime - startTime;
    return Math.ceil(diffTime / (1000 * 60 * 60 * 24));
}