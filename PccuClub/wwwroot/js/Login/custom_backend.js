$(function () {

    if ($("select").length > 0) {
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
    }

    if ($(".checkthisform").length > 0) {

        $(".submit-btn").click(function (e) {
            e.preventDefault();
            var sum_arr = [];
            var this_form = Boolean($(this).parents('form')) ? $(this).parents('form') : $('#' + $(this).attr('form'));

            this_form.find("[req=Y]").each(function () {
                var tooltips = $(this).attr("data-tooltip");
                var title = Boolean(tooltips) ? $.trim($(this).attr("data-tooltip")) : $.trim($(this).attr("title"));

                if ($(this).val() == "" || $(this).val() == null) {
                    $(this).closest(".warning-box").find(".alert-text").addClass("show");

                    sum_arr.push(title);
                } else {
                    if ((this.id).indexOf("email") >= 0) {
                        if (validateEmail($(this).val()) == false) {
                            $(this).closest(".warning-box").find(".alert-text").addClass("show");
                            sum_arr.push(title);
                        }
                    }
                }
            })
            if ($("#password1").val() !== $("#password2").val() && $("#password1").val() !== "" && $("#password2").val() !== "") {
                sum_arr.push("兩次輸入的密碼不同");
            }

            if ($("#password1").val() !== "" && !ValidatePasswd2($("#password1").val())) {
                sum_arr.push("密碼需最少四碼以上");
            }


            if ((location.pathname).indexOf("timeform") >= 0) {
                var selected = $(".timeTarget").find(".time-item").length
                if (selected <= 0) {
                    sum_arr.push("請選擇時段")
                }
            }

            if (sum_arr.length > 0) {
                sum_arr = $.unique(sum_arr); //消除重複的陣列元素
                sum_arr = sum_arr.join("、");
                alert("－" + sum_arr);
            } else {
                if ((location.pathname).indexOf("timeform") >= 0) {
                    $("#confirm").modal("show");
                } else {
                    this_form.submit();
                }
            }
        })
    }

    if ($(".hamburger-box").length > 0) {
        $(document).on('click', '#hamburger', function () {
            $(".hamburger-box").toggleClass("active");
        })
        $(".drop-item > a").click(function () {
            $(this).siblings("ul").slideToggle();
            $(this).toggleClass('arrow-change');
        })
    }

    if ((location.pathname).indexOf("index.") >= 0 || (location.pathname).slice(-1) === "/") {

        var id = getUrlParam("id");
        var calendarEl = document.getElementById('calendar');
        var calendar = new FullCalendar.Calendar(calendarEl, {
            locale: 'zh-tw',
            timeZone: 'Asia/Taipei',
            initialView: 'timeGridWeek',
            views: {
                timeGrid: {
                    dayHeaderContent: function (arg) {
                        var week = document.createElement('span');
                        var date = document.createElement('span');
                        week.innerHTML = arg.text.slice(6, -1);
                        date.innerHTML = arg.text.slice(0, 5);
                        return {
                            domNodes: [
                                date,
                                week
                            ]
                        };
                    }
                },
            },
            firstDay: 1,
            businessHours: true,
            allDaySlot: false,
            slotMinWidth: 60,
            eventMinHeight: 10,
            expandRows: true,
            contentHeight: 'auto',
            customButtons: {
                goBtn: {
                    text: '前往租借',
                    click: function () {
                        var selected = $('select[id=classroom]').val();
                        document.location.href = 'classform.php?id=' + selected
                    }
                },
            },
            headerToolbar: {
                left: 'prev,today,next',
                right: 'goBtn',
            },
            aspectRatio: 0.6,
            slotMinTime: '08:00:00',
            slotMaxTime: '22:00:00',
            buttonText: {
                today: '本週',
                month: '月',
                week: '週',
                day: '日',
            },
            businessHours: false,
            dayHeaderFormat: { //上方
                weekday: 'long',
                month: '2-digit',
                day: '2-digit',
                omitCommas: true
            },
            slotLabelFormat: { //左側
                hour: '2-digit',
                minute: '2-digit',
                hour12: false,
                meridiem: false, // lower level of text
            },
            eventTimeFormat: { // like '14:30:00'
                hour: '2-digit',
                minute: '2-digit',
                hour12: false,
                meridiem: false
            },
            slotDuration: '01:00',
            events: {
                url: 'api/get_calendar.php',
                method: 'POST',
                extraParams: {
                    "model": "show",
                    "room_id": id
                }
            },
            eventContent: function (eventInfo) {
                var innerHtml;
                if (eventInfo.event.extendedProps.category) {
                    innerHtml = "<div class='fc-event-main-frame'>\n"
                    innerHtml += "<div class='fc-event-title-container'><div class='fc-event-title fc-sticky'>" + eventInfo.event.title + "</div></div>\n";
                    innerHtml += "<div class='fc-event-category'>" + eventInfo.event.extendedProps.category + "</div>\n";
                    innerHtml += "<div class='fc-event-applicant'>" + eventInfo.event.extendedProps.applicant + "</div>\n"
                    innerHtml += "</div>\n"

                    return createElement = {
                        html: innerHtml
                    }
                }

            },
            selectable: false,
            selectOverlap: false,
            slotEventOverlap: false,
            datesSet: function (dateInfo) {
                addSelect(); // 加上下拉式選單
                $("button").attr("title", ""); // 移除翻譯不完整的tooltip title
            }
        })

        calendar.setOption('slotLabelFormat', function (data) {
            var start = moment(data.date).format("HH:mm");
            var end = moment(data.date).add(60, 'minutes').format("HH:mm");
            return {html: '<span class="topTime">' + start + '</span><span class="bottomTime">' + end + '</span>'}
        });

        calendar.render();

        $("#calendar").find("table").addClass("no_autoresize")

        function addSelect() {
            var id = getUrlParam("id");
            if ($("select[id=classroom]").length < 1) {
                var selectHTML = "";
                /* request */
                var filters_arr = ["orders>=0"];
                var orderby_arr = {"orders": "ASC", "pub_date": "DESC"}

                /* query */
                var obj = {
                    "filters": JSON.stringify(filters_arr),
                    "orderby": JSON.stringify(orderby_arr),
                }

                var event_arr = [];
                event_arr['success'] = function (resp) {
                    if (resp.state) {
                        if (resp.data.items.length > 0) {
                            selectHTML += "<label>選擇場地：</label>\n"
                            selectHTML += "<select class='form-control' id='classroom'>\n";
                            $.each(resp.data.items, function (key, item) {
                                var selected = "";
                                if (id !== "" && aes_decrypt(id) === item.id) {
                                    selected = "selected";
                                } else if (key === 0) {
                                    selected = "selected";
                                }
                                selectHTML += "<option " + selected + " value='" + aes_encrypt(item.id) + "'>教室" + item.title + "</option>\n";
                            })
                            selectHTML += "</select>\n";
                        }
                    }

                    // depending on where on the bar you want it, use prependTo or appendTo, and alter the array number
                    $(selectHTML).appendTo($(".fc-header-toolbar .fc-toolbar-chunk")[0]);

                    $("#classroom").on('change', function () {
                        var selected = $(this).val();
                        document.location.href = "index.php?id=" + selected
                    });
                }

                ajax_pub_adv("api/get_room.php", obj, event_arr, {"async": true})
            }
        }

    }

    if ((location.pathname).indexOf("timeform") >= 0) {

        $('[name=time_target]').val('[]');
        var mounted = false;
        var arr = [];
        var prev = getUrlParam('prev');
        var prev_arr = JSON.parse(aes_decrypt(prev));
        var calendarEl = document.getElementById('calendar');

        var calendar = new FullCalendar.Calendar(calendarEl, {
            locale: 'zh-tw',
            timeZone: 'Asia/Taipei',
            initialView: 'timeGridWeek',
            views: {
                timeGrid: {
                    dayHeaderContent: function (arg) {
                        var week = document.createElement('span');
                        var date = document.createElement('span');
                        week.innerHTML = arg.text.slice(6, -1);
                        date.innerHTML = arg.text.slice(0, 5);
                        return {
                            domNodes: [
                                date,
                                week
                            ]
                        };
                    }
                },
            },
            firstDay: 1,
            allDaySlot: false,
            slotMinWidth: 60,
            eventMinHeight: 30,
            expandRows: true,
            contentHeight: 'auto',
            longPressDelay: 300,
            eventLongPressDelay: 300,
            selectLongPressDelay: 300,
            headerToolbar: {
                right: 'prev,today,next',
                left: '',
            },
            aspectRatio: 0.6,
            slotMinTime: '08:00:00',
            slotMaxTime: '22:00:00',
            buttonText: {
                today: '本週',
                month: '月',
                week: '週',
                day: '日',
            },
            businessHours: false,
            dayHeaderFormat: { //上方
                weekday: 'long',
                month: '2-digit',
                day: '2-digit',
                omitCommas: true
            },
            slotLabelFormat: { //左側
                hour: '2-digit',
                minute: '2-digit',
                hour12: false,
                meridiem: false, // lower level of text
            },
            eventTimeFormat: { // like '14:30:00'
                hour: '2-digit',
                minute: '2-digit',
                hour12: false,
                meridiem: false
            },
            slotDuration: '01:00',
            datesSet: function () {
                $("button").attr("title", ""); // 移除翻譯不完整的tooltip title
            },
            eventsSet: function (events) { // 在datesSet後執行一次、eventDidMount後再執行一次
                /*
                https://stackoverflow.com/questions/69580283/fix-duplicate-events-in-fullcalendar-in-react
                https://github.com/fullcalendar/fullcalendar/issues/4775
                從server上抓取資料並炫染後，fullcalendar又會自己產生新的events，變成畫面上會有重複event的問題。
                這是因為我們給定的id跟fullcalendar產生的id不同，所以可以用排除的。
                */
                var lookup = events.reduce(function (a, e) {
                    a[e.id] = ++a[e.id] || 0;
                    return a;
                }, {});

                var duplicatedEvents = events.filter(function (e) {
                    return lookup[e.id];
                })

                if (duplicatedEvents.length) {
                    duplicatedEvents.splice(-1, 1) // keep the last one
                    duplicatedEvents.map(function (evt) {
                        evt.remove()
                    }) // remove the others, may be more than one
                }
            },
            events: { // 先從api取得目前有的event、並顯示 (add暫存/edit已預約)
                url: 'api/get_calendar.php',
                method: 'POST',
                extraParams: {
                    "model": "add",
                    "room_id": aes_encrypt(prev_arr.room_id)
                },
                success: function (data) {
                    if (!mounted) {
                        var itemHtml = '';
                        var len = data.length;
                        $.each(data, function (key, item) {
                            if (item.model === "add") { // 暫存的
                                var start = moment.utc(item.start).format('HH:mm');
                                var end = moment.utc(item.end).format('HH:mm');
                                var obj = {
                                    "genid": parseInt(item.id),
                                    "start_date": moment.utc(item.start).format('YYYY/MM/DD HH:mm:ss'),
                                    "end_date": moment.utc(item.end).format('YYYY/MM/DD HH:mm:ss'),
                                }

                                arr = addArr(arr, item.id, obj)

                                // 右方時間訊息
                                itemHtml += addTimeItem(item.id, item.date, start, end);
                            }
                            if (len === key + 1) {
                                mounted = true;
                            }
                        })
                        $('[name=time_target]').val(JSON.stringify(arr));
                        $(".timeTarget").html(itemHtml);
                    }
                }
            },
            eventContent: function (eventInfo) {
                // console.log("eventContent", eventInfo, click)
                var innerHtml = "";
                if (eventInfo.event.extendedProps.category) {
                    innerHtml += "<div id='" + eventInfo.event.id + "' class='fc-event-main-frame'>"
                    innerHtml += "<div class='fc-event-title-container'><div class='fc-event-title fc-sticky'>" + eventInfo.event.title + "</div></div>";
                    innerHtml += "<div class='fc-event-category'>" + eventInfo.event.extendedProps.category + "</div>";
                    innerHtml += "<div class='fc-event-applicant'>" + eventInfo.event.extendedProps.applicant + "</div>"
                    innerHtml += "</div>"

                    return createElement = {
                        html:
                        innerHtml
                    }
                }
            },
            eventDidMount: function (eventInfo) {
                // console.log("eventDidMount", eventInfo, click)
                $('.del-btn').click(function () { // 當點擊刪除X按鈕時
                    var item = $(this).closest(".time-item");
                    var id = item.data("id"); // event id
                    var event = calendar.getEventById(id);
                    if (event.extendedProps.model !== undefined && event.extendedProps.model !== "edit") {
                        // 選擇的時間和今天
                        var chosenday = moment.utc(event.start).format('YYYY-MM-DD HH:mm');
                        var now = getISODateTime(now_time(), "yyyy-MM-dd HH:mm")

                        if (chosenday < now) {
                            return false
                        } else {
                            var delObj = {
                                "id": id,
                                "room_id": aes_encrypt(prev_arr.room_id),
                                "model": "delbyid"
                            }
                            var event_arr = [];
                            event_arr['success'] = function (resp) {
                                if (resp.state) {
                                    event.remove();
                                    item.remove();

                                    $(".timeTarget").find(".time-item").each(function () {
                                        var timeItem = $(this);
                                        var dataId = timeItem.data("id");
                                        if (parseInt(dataId) === parseInt(id)) {
                                            timeItem.remove();
                                        }
                                    })

                                    arr = filterArr(arr, id);
                                    $('[name=time_target]').val(JSON.stringify(arr));
                                } else {
                                    return false;
                                }
                            }

                            ajax_pub_adv("api/post_event.php", delObj, event_arr, {"async": true})
                        }
                    } else {
                        return false;
                    }

                })
            },
            selectable: true,
            selectOverlap: false,
            slotEventOverlap: false,
            select: function (selectionInfo) {
                // https://stackoverflow.com/questions/29491230/fullcalendar-day-doubleclick-callback
                var doubleClick = false;
                prevTime = typeof currentTime === 'undefined' || currentTime === null ? new Date().getTime() - 1000000 : currentTime;
                currentTime = new Date().getTime();
                // 如果500毫秒內點擊2次就算是雙擊
                if (currentTime - prevTime < 500) {
                    doubleClick = true;
                }

                // 選擇的時間和今天
                var chosenday = moment.utc(selectionInfo.start).format('YYYY-MM-DD HH:mm');
                var now = getISODateTime(now_time(), "yyyy-MM-dd HH:mm")

                // If the selection spans multiple day 不能跨日
                var diff = DateDiff(moment.utc(selectionInfo.start).format('YYYY-MM-DD'), moment.utc(selectionInfo.end).format('YYYY-MM-DD'), 'd')
                if (diff >= 1) {
                    return false
                } else {
                    if (chosenday < now) {
                        return false
                    } else {
                        if (!doubleClick) {
                            var geneId = moment.utc(selectionInfo.start).format('YYYYMMDDHHmm')

                            var date = moment.utc(selectionInfo.start).format('YYYY/MM/DD');
                            var start = moment.utc(selectionInfo.start).format('HH:mm');
                            var end = moment.utc(selectionInfo.end).format('HH:mm');

                            // 右方時間訊息
                            var itemHtml = addTimeItem(geneId, date, start, end);

                            // 新增時段需要的時間格式
                            var dateSelect = moment.utc(selectionInfo.start).format('YYYY-MM-DD') + 'T' + moment.utc(selectionInfo.start).format('HH:mm:ss');
                            var dateSelectEnd = moment.utc(selectionInfo.end).format('YYYY-MM-DD') + 'T' + moment.utc(selectionInfo.end).format('HH:mm:ss');

                            var title = $('[name=title]').val();
                            var types_option = $('[name=types_option]').val();
                            var user = $('[name=user]').val();
                            var addObj = {
                                "id": geneId,
                                "room_id": aes_encrypt(prev_arr.room_id),
                                "title": title,
                                "category": types_option,
                                "applicant": user,
                                "date": date,
                                "start": dateSelect,
                                "end": dateSelectEnd,
                                "model": "add"
                            }

                            var event_arr = [];
                            event_arr['success'] = function (resp) {
                                if (resp.state) {
                                    // 將所有選擇的時間組合成json
                                    arr.push({
                                        "genid": geneId,
                                        "start_date": moment.utc(selectionInfo.start).format('YYYY/MM/DD HH:mm:ss'),
                                        "end_date": moment.utc(selectionInfo.end).format('YYYY/MM/DD HH:mm:ss'),
                                    })
                                    $(".timeTarget").find(".time-item").each(function () {
                                        var timeItem = $(this);
                                        var id = timeItem.data("id");
                                        var date1 = $.trim(timeItem.find('.date i')[0].nextSibling.nodeValue);
                                        var times = $.trim(timeItem.find('.time i')[0].nextSibling.nodeValue);
                                        var split = times.split("-");
                                        var start_date = date1 + " " + split[0] + ":00";
                                        var end_date = date1 + " " + split[1] + ":00";
                                        var obj = {
                                            "genid": id,
                                            "start_date": moment.utc(start_date).format('YYYY/MM/DD HH:mm:ss'),
                                            "end_date": moment.utc(end_date).format('YYYY/MM/DD HH:mm:ss'),
                                        }

                                        arr = addArr(arr, id, obj);
                                    })

                                    $(".timeTarget").append(itemHtml);
                                    $('[name=time_target]').val(JSON.stringify(arr));
                                    calendar.addEvent(addObj);
                                } else {
                                    return false;
                                }
                            }

                            ajax_pub_adv("api/post_event.php", addObj, event_arr, {"async": true})
                        }
                    }
                }
            },
            eventClick: function (info) {
                if (info.event.extendedProps.model !== "edit") {
                    // 選擇的時間和今天
                    var chosenday = moment.utc(info.event.start).format('YYYY-MM-DD HH:mm');
                    var now = getISODateTime(now_time(), "yyyy-MM-dd HH:mm")

                    if (chosenday < now) {
                        return false
                    } else {

                        id = info.event.id;
                        var delObj = {
                            "id": id,
                            "room_id": aes_encrypt(prev_arr.room_id),
                            "model": "delbyid"
                        }
                        var event_arr = [];
                        event_arr['success'] = function (resp) {
                            if (resp.state) {
                                var event = calendar.getEventById(id);
                                event.remove();
                                arr = removeArr(arr, id);
                                $('[name=time_target]').val(JSON.stringify(arr));
                                $(".timeTarget").find(".time-item").each(function () {
                                    var timeItem = $(this);
                                    var dataId = timeItem.data("id");
                                    if (parseInt(dataId) === parseInt(id)) {
                                        timeItem.remove();
                                    }
                                })
                            } else {
                                return false;
                            }
                        }

                        ajax_pub_adv("api/post_event.php", delObj, event_arr, {"async": true})
                    }
                } else {
                    return false;
                }
            }
        });

        // 左側時間顯示
        calendar.setOption('slotLabelFormat', function (data) {
            var start = moment(data.date).format("HH:mm");
            var end = moment(data.date).add(60, 'minutes').format("HH:mm");
            return {html: '<span class="topTime">' + start + '</span><span class="bottomTime">' + end + '</span>'}
        });

        calendar.render();

        $("#calendar").find("table").addClass("no_autoresize")

        if ($(window).width() > 1025) {
            $(document).on({
                mouseenter: function () {
                    var cellWidth = $('th.fc-col-header-cell').outerWidth();
                    var cellHeight = $('.fc-timegrid-slot.fc-timegrid-slot-lane').outerHeight();
                    var columnCount = $('table.fc-col-header th.fc-col-header-cell').children().length;
                    if (!$(this).html()) {
                        for (var i = 0; i < columnCount; i++) {
                            $(this).append('<span class="temp-cell" style="height:100%;width:' + (cellWidth) + 'px"></span>');
                        }
                    }
                    $(this).children('.temp-cell').each(function () {
                        $(this).hover(function () {
                            // pokud vubec nema datum ve sloupci, neukazuj
                            coldate = $('table.fc-col-header th.fc-col-header-cell').eq($(this).index()).data('date');
                            if (coldate) {

                                // hodina
                                var dtime = $(this).parent().data('time').slice(0, -3).split(":");

                                coldate_split = coldate.split("-");
                                d = new Date(coldate_split[0], coldate_split[1] - 1, coldate_split[2], dtime[0], dtime[1]);
                                now = new Date();

                                // pokud je to novejsi
                                if (d > now) {
                                    $(this).html('<span class="current-time"><i class="fas fa-plus"></i></span>');
                                }
                            }
                        }, function () {
                            $(this).html('');
                        });
                    });
                },
                mouseleave: function () {
                    $(this).children('.temp-cell').remove();
                }
            }, 'td.fc-timegrid-slot.fc-timegrid-slot-lane');

        }

        // 確認租借modal
        if ($("#finalBtn").length > 0) {
            $(document).on('click', '#finalBtn', function () {
                var form = $('form').eq(0);
                form.submit();
            })
        }
    }

})


function ValidatePasswd2(str) {
    var re = /^.{4,}$/;
    return re.test(str);
}

function uniqArr(arr) {
    return arr.filter(function (item, index) {
        var _item = JSON.stringify(item);
        return index === arr.findIndex(obj => {
            return JSON.stringify(obj) === _item;
        });
    })
}

function addTimeItem(id, date, start, end) {
    var str = '';

    str += '<div data-id="' + id + '" class="time-item">';
    str += '<div class="left-box">\n';
    str += '<div class="date"><i class="far fa-calendar-alt"></i> ' + date + '</div>\n';
    str += '<div class="time"><i class="far fa-clock"></i> ' + start + '-' + end + '</div>\n';
    str += '</div>\n';
    str += '<div class="right-box"><button class="del-btn" type="button"><i class="fas fa-times"></i></button></div>\n';
    str += '</div>\n';

    return str;
}

function dateFormatter(date, format = "") {
    return moment.utc(date).format(format);
}

function addArr(arr, id, obj) {
    var newArr = arr;
    var result = $.map(arr, function (item, index) {
        return parseInt(item.genid);
    }).indexOf(parseInt(id));

    if (result < 0) {
        newArr.push(obj)
    }

    return newArr;
}

function removeArr(arr, id) {
    var newArr = arr;
    var result = $.map(arr, function (item, index) {
        return parseInt(item.genid);
    }).indexOf(parseInt(id));

    if (result >= 0) {
        newArr.splice(result, 1)
    }

    return newArr;
}

function filterArr(arr, id) {
    return $.grep(arr, function (v) {
        return parseInt(v.genid) !== parseInt(id);
    });
}