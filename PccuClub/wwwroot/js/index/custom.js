$(document).ready(function(){
    if ($(".checkthisform").length > 0) {
        
        $(".submit-btn").click(function(e){
            e.preventDefault();
            var sum_arr = [];
            var this_form = Boolean($(this).parents('form'))? $(this).parents('form'): $('#'+$(this).attr('form'));
            
            this_form.find("[req=Y]").each(function() {
                var tooltips = $(this).attr("data-tooltip");
                var title = Boolean(tooltips) ? $.trim($(this).attr("data-tooltip")) : $.trim($(this).attr("title"));

                if ($(this).val() == "" || $(this).val() == null ) {
                    $(this).closest(".warning-box").find(".alert-text").addClass("show");
                    
                    sum_arr.push(title);
                }
                else {
                    if ((this.id).indexOf("email") >= 0) {
                        if (validateEmail($(this).val()) == false) {
                                $(this).closest(".warning-box").find(".alert-text").addClass("show");
                                sum_arr.push(title);
                        }
                    }
                }
            })
            if($("#password1").val()!== $("#password2").val() && $("#password1").val()!=="" && $("#password2").val()!==""){     
                sum_arr.push("兩次輸入的密碼不同");
            }
            if ((location.pathname).indexOf("timeform") >= 0) {
                var selected = $(".timeTarget").find(".time-item").length
                if (selected <= 0) {
                    sum_arr.push("請選擇時段")
                }
            }
    
            if(sum_arr.length > 0){
                sum_arr = $.unique(sum_arr); //消除重複的陣列元素
                sum_arr = sum_arr.join("、");
                alert("－" + sum_arr);
            } else {
                // this_form.submit();
                if ((location.pathname).indexOf("login") >= 0) {
                    location.href = 'index.html'
                }
                else if((location.pathname).indexOf("edit") >= 0){
                    alert("密碼變更成功")
                }
                else if ((location.pathname).indexOf("classform") >= 0){
                    location.href = 'timeform.html'
                }
                else if ((location.pathname).indexOf("timeform") >= 0){
                    // alert("租借成功！")
                    $("#confirm").modal("show");
                    // location.href = 'index.html'
                }
            }
    
        }) 
    }

    if ($("#finalBtn").length > 0) {
        $(document).on('click','#finalBtn',function(){
            alert("租借成功！")
            location.href = 'index.html'
        })
    }


    if ($(".hamburger-box").length > 0) {
        $(document).on('click','#hamburger',function(){
            $(".hamburger-box").toggleClass("active");
        })
        $(".drop-item > a").click(function(){
            $(this).siblings("ul").slideToggle();
            $(this).toggleClass('arrow-change');
        })
    }


    if ((location.pathname).indexOf("index.") >= 0 || (location.pathname).slice(-1) == "/") {
        var calendarEl = document.getElementById('calendar');

        var calendar = new FullCalendar.Calendar(calendarEl, {
            locale: 'zh-tw',	 
            timeZone: 'Asia/Taipei',
            initialView: 'timeGridWeek',
            views: {
                timeGrid: {     
                    dayHeaderContent : function (arg) {
                        var week = document.createElement('span');
                        var date = document.createElement('span');
                        week.innerHTML = arg.text.slice(6, -1);
                        date.innerHTML = arg.text.slice(0, 5);
                        return { domNodes: [
                            date,
                            week
                        ]};
                    }
                },
            },
            firstDay: 1,
            businessHours: true,
            allDaySlot:false,
            slotMinWidth:60,
            eventMinHeight:10,
            expandRows:true,
            contentHeight: 'auto',
            customButtons: {
                goBtn: {
                    text: '前往租借',
                        click: function() {
                            location.href = 'classform.html'
                        }
                },
            },
            headerToolbar: {
                left:'prev,today,next',
                right: 'goBtn',
            },
            aspectRatio: 0.6,
            slotMinTime:'08:00:00',
            slotMaxTime:'22:00:00',
            buttonText:{
                today:    '本週',
                month:    '月',
                week:     '週',
                day:      '日',
            },
            businessHours:false,
            dayHeaderFormat:{ //上方
                weekday: 'long', 
                month: '2-digit', 
                day: '2-digit', 
                omitCommas: true 
            },
            slotLabelFormat: { //左側
                hour: '2-digit',
                minute: '2-digit',
                hour12: false,
                meridiem: false , // lower level of text
            },
            eventTimeFormat: { // like '14:30:00'
                hour: '2-digit',
                minute: '2-digit',
                hour12: false,
                meridiem: false
            },
            slotDuration:'01:00',
            events: [
                {
                  title: "水處理薄膜技術課程(考試)",
                  category: "化工四",
                  applicant: "林松池教授",
                  start: "2022-11-15T08:00:00",
                  end: "2022-11-15T09:00:00",
                },
                {
                  title: "水處理薄膜技術課程(考試)",
                  category: "化工四",
                  applicant: "林松池教授",
                  start: "2022-11-18T08:00:00",
                  end: "2022-11-18T09:00:00",
                },
                {
                  title: "學安室",
                  category: "化工一",
                  applicant: "吳文仁",
                  start: "2022-11-15T12:00:00",
                  end: "2022-11-15T13:00:00",
                },
                {
                  title: "學安室",
                  category: "化工一",
                  applicant: "吳文仁",
                  start: "2022-11-15T13:00:00",
                  end: "2022-11-15T14:00:00",
                },
                {
                  title: "學安室",
                  category: "化工一",
                  applicant: "吳文仁",
                  start: "2022-11-16T11:00:00",
                  end: "2022-11-16T12:00:00",
                }
            ],
            eventContent: function(eventInfo) { 
                var innerHtml;
                if (eventInfo.event.extendedProps.category) {
                    var start = moment.utc(eventInfo.event.start).format('HH:mm');
                    var end = moment.utc(eventInfo.event.end).format('HH:mm');
                
                    innerHtml = "<div class='fc-event-main-frame'>"
                    // innerHtml += "<div class='fc-event-time'>" + start +"-"+ end +"</div>";
                    innerHtml += "<div class='fc-event-title-container'><div class='fc-event-title fc-sticky'>" + eventInfo.event.title+"</div></div>";
                    innerHtml += "<div class='fc-event-category'>" + eventInfo.event.extendedProps.category+"</div>";
                    innerHtml += "<div class='fc-event-applicant'>"+eventInfo.event.extendedProps.applicant+"</div>"
                    innerHtml += "</div>"

                    return createElement = { html: 
                        innerHtml
                     }
                }
         
            },
            selectable:false,
            selectOverlap:false,
            datesSet(dateInfo) {  
                //加上下拉式選單
                addSelect();            

                // 移除翻譯不完整的tooltip title
                $("button").attr("title","")
            }
            
        });
        
        calendar.setOption('slotLabelFormat', function (data) {
            var start = moment(data.date).format("HH:mm");
            var end = moment(data.date).add(60, 'minutes').format("HH:mm");

            
            return {html: '<span class="topTime">'+start+'</span><span class="bottomTime">'+end+'</span>'}
        });

        calendar.render();
        
        // var num
        // $("#calendar").find(".fc-timegrid-slot-label").each(function(i){
        //     num = (i+1)
        //     $(this).find(".topTime").before('<span class="topNum">第'+num+'節</span>')
        // })
        
        
        $("#calendar").find("table").addClass("no_autoresize")

        function addSelect() {
            if($("select[id=classroom]").length < 1) {
                var selectHTML =
                    "<label>選擇場地：</label>"+
                    "<select class='form-control' id='classroom'>" +
                        "<option selected value='C104'>教室C104</option>" +
                        "<option value='C202'>教室C202</option>" +
                        "<option value='C203'>教室C203</option>" +
                        "<option value='C204'>教室C204</option>" +
                        "<option value='C205'>教室C205</option>" +
                        "<option value='C206'>教室C206</option>" +
                        "<option value='C301'>教室C301</option>" +
                        "<option value='C305'>教室C305</option>" +
                        "<option value='C308'>教室C308</option>" +
                    "</select>";
                    
                // depending on where on the bar you want it, use prependTo or appendTo, and alter the array number
                $(selectHTML).appendTo($(".fc-header-toolbar .fc-toolbar-chunk")[0]);
                
                $("#classroom").on('change', function() {
                    location.href = "index202.html"
                    
                });
            }
        }
        
    }

    if ((location.pathname).indexOf("index202.") >= 0) {
        var calendarEl = document.getElementById('calendar');

        var calendar = new FullCalendar.Calendar(calendarEl, {
            locale: 'zh-tw',	 
            timeZone: 'Asia/Taipei',
            initialView: 'timeGridWeek',
            views: {
                timeGrid: {     
                    dayHeaderContent : function (arg) {
                        var week = document.createElement('span');
                        var date = document.createElement('span');
                        week.innerHTML = arg.text.slice(6, -1);
                        date.innerHTML = arg.text.slice(0, 5);
                        return { domNodes: [
                            date,
                            week
                        ]};
                    }
                },
            },
            firstDay: 1,
            businessHours: true,
            allDaySlot:false,
            slotMinWidth:60,
            eventMinHeight:10,
            expandRows:true,
            contentHeight: 'auto',
            customButtons: {
                goBtn: {
                    text: '前往租借',
                        click: function() {
                            location.href = 'classform.html'
                        }
                },
            },
            headerToolbar: {
                left:'prev,today,next',
                right: 'goBtn',
            },
            aspectRatio: 0.6,
            slotMinTime:'08:00:00',
            slotMaxTime:'22:00:00',
            buttonText:{
                today:    '本週',
                month:    '月',
                week:     '週',
                day:      '日',
            },
            businessHours:false,
            dayHeaderFormat:{ //上方
                weekday: 'long', 
                month: '2-digit', 
                day: '2-digit', 
                omitCommas: true 
            },
            slotLabelFormat: { //左側
                hour: '2-digit',
                minute: '2-digit',
                hour12: false,
                meridiem: false , // lower level of text
            },
            eventTimeFormat: { // like '14:30:00'
                hour: '2-digit',
                minute: '2-digit',
                hour12: false,
                meridiem: false
            },
            slotDuration:'01:00',
            events: [
                {
                  title: "楊宏達實驗室meeting",
                  category: "碩二",
                  applicant: "楊宏達教授",
                  start: "2022-11-15T08:00:00",
                  end: "2022-11-15T09:00:00",
                },
                {
                  title: "楊宏達實驗室meeting",
                  category: "碩二",
                  applicant: "楊宏達教授",
                  start: "2022-11-18T08:00:00",
                  end: "2022-11-18T09:00:00",
                },
          
            ],
            eventContent: function(eventInfo) { 
                var innerHtml;
                if (eventInfo.event.extendedProps.category) {
                    var start = moment.utc(eventInfo.event.start).format('HH:mm');
                    var end = moment.utc(eventInfo.event.end).format('HH:mm');
                
                    innerHtml = "<div class='fc-event-main-frame'>"
                    // innerHtml += "<div class='fc-event-time'>" + start +"-"+ end +"</div>";
                    innerHtml += "<div class='fc-event-title-container'><div class='fc-event-title fc-sticky'>" + eventInfo.event.title+"</div></div>";
                    innerHtml += "<div class='fc-event-category'>" + eventInfo.event.extendedProps.category+"</div>";
                    innerHtml += "<div class='fc-event-applicant'>"+eventInfo.event.extendedProps.applicant+"</div>"
                    innerHtml += "</div>"

                    return createElement = { html: 
                        innerHtml
                     }
                }
            },
            selectable:false,
            selectOverlap:false,
            datesSet(dateInfo) {  
                //加上下拉式選單
                addSelect();            

                // 移除翻譯不完整的tooltip title
                $("button").attr("title","")
            }
            
        });
        
        calendar.setOption('slotLabelFormat', function (data) {
            var start = moment(data.date).format("HH:mm");
            var end = moment(data.date).add(60, 'minutes').format("HH:mm");

            
            return {html: '<span class="topTime">'+start+'</span><span class="bottomTime">'+end+'</span>'}
        });

        calendar.render();
        
        // var num
        // $("#calendar").find(".fc-timegrid-slot-label").each(function(i){
        //     num = (i+1)
        //     $(this).find(".topTime").before('<span class="topNum">第'+num+'節</span>')
        // })
        
        
        $("#calendar").find("table").addClass("no_autoresize")

        function addSelect() {
            if($("select[id=classroom]").length < 1) {
                var selectHTML =
                    "<label>選擇場地：</label>"+
                    "<select class='form-control' id='classroom'>" +
                        "<option value='C104'>教室C104</option>" +
                        "<option selected value='C202'>教室C202</option>" +
                        "<option value='C203'>教室C203</option>" +
                        "<option value='C204'>教室C204</option>" +
                        "<option value='C205'>教室C205</option>" +
                        "<option value='C206'>教室C206</option>" +
                        "<option value='C301'>教室C301</option>" +
                        "<option value='C305'>教室C305</option>" +
                        "<option value='C308'>教室C308</option>" +
                    "</select>";
                    
                // depending on where on the bar you want it, use prependTo or appendTo, and alter the array number
                $(selectHTML).appendTo($(".fc-header-toolbar .fc-toolbar-chunk")[0]);
                
                $("#classroom").on('change', function() {
                    location.href = "index.html"
                    
                });
            }
        }
        
    }
    
    if ((location.pathname).indexOf("timeform") >= 0) {
        var calendarEl = document.getElementById('calendar');

        var calendar = new FullCalendar.Calendar(calendarEl, {
            locale: 'zh-tw',	 
            timeZone: 'Asia/Taipei',
            initialView: 'timeGridWeek',
            views: {
                timeGrid: {     
                    dayHeaderContent : function (arg) {
                        var week = document.createElement('span');
                        var date = document.createElement('span');
                        week.innerHTML = arg.text.slice(6, -1);
                        date.innerHTML = arg.text.slice(0, 5);
                        return { domNodes: [
                            date,
                            week
                        ]};
                    }
                },
            },
            firstDay: 1,
            businessHours: true,
            allDaySlot:false,
            slotMinWidth:60,
            eventMinHeight:30,
            expandRows:true,
            contentHeight: 'auto',
            longPressDelay: 300,
            eventLongPressDelay:300,
            selectLongPressDelay:300,
            headerToolbar: {
                right:'prev,today,next',
                left: '',
            },
            aspectRatio: 0.6,
            slotMinTime:'08:00:00',
            slotMaxTime:'22:00:00',
            buttonText:{
                today:    '本週',
                month:    '月',
                week:     '週',
                day:      '日',
            },
            businessHours:false,
            dayHeaderFormat:{ //上方
                weekday: 'long', 
                month: '2-digit', 
                day: '2-digit', 
                omitCommas: true 
            },
            slotLabelFormat: { //左側
                hour: '2-digit',
                minute: '2-digit',
                hour12: false,
                meridiem: false , // lower level of text
            },
            eventTimeFormat: { // like '14:30:00'
                hour: '2-digit',
                minute: '2-digit',
                hour12: false,
                meridiem: false
            },
            slotDuration:'01:00',
            
            events: [
                {
                    id: "20221115T080000",
                    title: "水處理薄膜技術課程(考試)",
                    category: "化工四",
                    applicant: "林松池教授",
                    start: "2022-11-15T08:00:00",
                    end: "2022-11-15T09:00:00",
                },
                {
                    id: "20221118T080000",
                    title: "水處理薄膜技術課程(考試)",
                    category: "化工四",
                    applicant: "林松池教授",
                    start: "2022-11-18T08:00:00",
                    end: "2022-11-18T09:00:00",
                },
                {
                    id: "20221115T120000",
                    title: "學安室",
                    category: "化工一",
                    applicant: "吳文仁",
                    start: "2022-11-15T12:00:00",
                    end: "2022-11-15T13:00:00",
                },
                {
                    id: "20221115T130000",
                    title: "學安室",
                    category: "化工一",
                    applicant: "吳文仁",
                    start: "2022-11-15T13:00:00",
                    end: "2022-11-15T14:00:00",
                },
                {
                    id: "20221116T110000",
                    title: "學安室",
                    category: "化工一",
                    applicant: "吳文仁",
                    start: "2022-11-16T11:00:00",
                    end: "2022-11-16T12:00:00",
                }
            ],
            eventDidMount: function(eventInfo) {
                // console.log(eventInfo.event.extendedProps.category);
                // console.log(eventInfo.event.extendedProps.applicant);
            },
            eventContent: function(eventInfo) { 
                var innerHtml;
                if (eventInfo.event.extendedProps.category) {
                    var start = moment.utc(eventInfo.event.start).format('HH:mm');
                    var end = moment.utc(eventInfo.event.end).format('HH:mm');
                

                    innerHtml = "<div id="+eventInfo.event.id+" class='fc-event-main-frame'>"
                    // innerHtml += "<div class='fc-event-time'>" + start +"-"+ end +"</div>";
                    innerHtml += "<div class='fc-event-title-container'><div class='fc-event-title fc-sticky'>" + eventInfo.event.title+"</div></div>";
                    innerHtml += "<div class='fc-event-category'>" + eventInfo.event.extendedProps.category+"</div>";
                    innerHtml += "<div class='fc-event-applicant'>"+eventInfo.event.extendedProps.applicant+"</div>"
                    innerHtml += "</div>"


                    return createElement = { html: 
                        innerHtml
                     }
                }
         
            },
            selectable:true,
            selectOverlap:false,
            select:function(selectionInfo){
                var geneId = moment.utc(selectionInfo.start).format('YYYYMMDDHHmm')
                
                var date = moment.utc(selectionInfo.start).format('YYYY/MM/DD');
                var start = moment.utc(selectionInfo.start).format('HH:mm');
                var end = moment.utc(selectionInfo.end).format('HH:mm');

                // 選擇的時間和今天
                var chosenday = moment.utc(selectionInfo.start).format('YYYY-MM-DD HH:mm');
                var now = getISODateTime(now_time(), "yyyy-MM-dd HH:mm")

                // 右方時間訊息
                var itemHtml = '<div data-id="'+geneId+'" class="time-item"><div class="left-box"><div class="date"><i class="far fa-calendar-alt"></i> '+date+'</div><div class="time"><i class="far fa-clock"></i> '+start+'-'+end+'</div></div><div class="right-box"><button class="del-btn" type="button"><i class="fas fa-times"></i></button></div></div>'

                // 新增時段需要的時間格式
                var dateSelect = moment.utc(selectionInfo.start).format('YYYY-MM-DD')+'T'+moment.utc(selectionInfo.start).format('HH:mm:ss');
                var dateSelectEnd = moment.utc(selectionInfo.end).format('YYYY-MM-DD')+'T'+moment.utc(selectionInfo.end).format('HH:mm:ss'); 

                // calendar.fullCalendar('unselect');
                
                if(chosenday < now) {
                    return false
                } else {
                    $(".timeTarget").append(itemHtml);

                    calendar.addEvent({
                        id: geneId,
                        title: "學安室",
                        category: "化工一",
                        applicant: "吳文仁",
                        start: dateSelect,
                        end: dateSelectEnd
                        
                    });
                }
             
            },
            datesSet(dateInfo) {  
                //加上下拉式選單
                addSelect();            

                // 移除翻譯不完整的tooltip title
                $("button").attr("title","")
            },
            eventClick: function(info) {
                // 選擇的時間和今天
                var chosenday = moment.utc(info.event.start).format('YYYY-MM-DD HH:mm');
                var now = getISODateTime(now_time(), "yyyy-MM-dd HH:mm")

                if(chosenday < now) {
                    alert("!")
                    return false
                }else{

                    id = info.event.id;
                    var event = calendar.getEventById(id);
                    event.remove();
    
                    $(".timeTarget").find("[data-id]").each(function(){
                        if ($(this).data("id") == id) {
                            $(this).remove()
                        }
                    })
                }
                
            }
          
        });
        
        // 左側時間顯示
        calendar.setOption('slotLabelFormat', function (data) {
            var start = moment(data.date).format("HH:mm");
            var end = moment(data.date).add(60, 'minutes').format("HH:mm");
            return {html: '<span class="topTime">'+start+'</span><span class="bottomTime">'+end+'</span>'}
        });

        calendar.render();
        
        // var num
        // $("#calendar").find(".fc-timegrid-slot-label").each(function(i){
        //     num = (i+1)
        //     $(this).find(".topTime").before('<span class="topNum">第'+num+'節</span>')
        // })
        
        
        $("#calendar").find("table").addClass("no_autoresize")

        function addSelect() {
            if($(".classTitle").length < 1) {
                var selectHTML = "<h4 class='classTitle text-6 mb-0'>教室 C104</h4>";
                    
                // depending on where on the bar you want it, use prependTo or appendTo, and alter the array number
                $(selectHTML).appendTo($(".fc-header-toolbar .fc-toolbar-chunk")[0]);
            }
        }


        if ($(window).width() > 1025) {
            $(document).on({
                mouseenter: function() {
                    var cellWidth = $('th.fc-col-header-cell').outerWidth();
                    var cellHeight = $('.fc-timegrid-slot.fc-timegrid-slot-lane').outerHeight();
                    var columnCount = $('table.fc-col-header th.fc-col-header-cell').children().length;
                    if (!$(this).html()) {
                        for (var i = 0; i < columnCount; i++) {
                            $(this).append('<span class="temp-cell" style="height:100%;width:' + (cellWidth) + 'px"></span>');
                        }
                    }
                    $(this).children('.temp-cell').each(function() {
                        $(this).hover(function() {
                            // pokud vubec nema datum ve sloupci, neukazuj
                            coldate = $('table.fc-col-header th.fc-col-header-cell').eq($(this).index()).data('date');
                            if (coldate) {
            
                                // hodina
                                var dtime = $(this).parent().data('time').slice(0, -3).split(":");
                                
                                coldate_split = coldate.split("-");
                                d = new Date(coldate_split[0], coldate_split[1]-1, coldate_split[2], dtime[0], dtime[1]);
                                now = new Date();
            
                                // pokud je to novejsi
                                if (d > now) {
                                    $(this).html('<span class="current-time"><i class="fas fa-plus"></i></span>');
                                }
                            }  
                        }, function() {
                            $(this).html('');
                        });
                    });
                },
                mouseleave: function() {
                    $(this).children('.temp-cell').remove();
                }
            }, 'td.fc-timegrid-slot.fc-timegrid-slot-lane');
        
        }
        $(document).on('click','.del-btn',function(){
            var thisId = $(this).closest(".time-item").data("id")
            $("#"+thisId).trigger("click")
            $(this).closest(".time-item").remove();
    
        })
    }

})