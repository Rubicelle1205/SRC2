$(document).ready(function(){
    // 警示重置
    $(document).on('focus','.alert-border',function(){
        $(this).removeClass("alert-border");
    })
    $(document).on('change','.alert-border',function(){
        $(this).removeClass("alert-border");
    })
    $(document).on('click','.warning-box label, .warning-box .form-check-input',function(){
        $(this).closest(".warning-box").find(".form-check-input").removeClass("alert-border")
    })
    // unique
    function unique(list) {
        var result = [];
        $.each(list, function(i, e) {
            if ($.inArray(e, result) == -1) result.push(e);
        });
        return result;
    }

    if ($(".checkthisform").length > 0) {
        $(document).on('click',".submit-btn",function(e){
            e.preventDefault();
            var sum_arr = [];
            var this_form = Boolean($(this).parents('form'))? $(this).parents('form'): $('#'+$(this).attr('form'));
    
            
            this_form.find("[req=Y]").each(function() {
                var tooltips = $(this).attr("data-tooltip");
                var title = Boolean(tooltips) ? $.trim($(this).attr("data-tooltip")) : $.trim($(this).attr("title"));
                if ($(this).val() == "" || $(this).val() == null ) {
                    $(this).addClass("alert-border");
                    
                    sum_arr.push(title);
                }else if((this.className).indexOf("form-check-input") >= 0) {
                    var this_group = $(this).attr("name")
                    if ($(this).closest(".warning-box").find("[name="+this_group+"]:checked").length <= 0) {
                        $(this).addClass("alert-border");
                        
                        sum_arr.push(title);
                    }
                }  
            })

            if(sum_arr.length > 0){
                sum_arr = unique(sum_arr);
                sum_arr = sum_arr.join("、");
                alert(sum_arr)
                $('html, body').animate({
                    scrollTop: $('.alert-border:first').offset().top - 200
                }, 500);
    
            } else {
                // this_form.submit();
                if ((location.pathname).indexOf("login.") >= 0) {
                    location.href = "index.html"
                }else if((location.pathname).indexOf("club_edit.") >= 0) {
                    alert("儲存成功！")
                }else if((location.href).indexOf("handover_cover.") >= 0) {
                    if (confirm("確認要送出嗎？") == false) {
                        return false
                    }else {
                        location.href = "print_cover.html"
                    }
                }else if((location.href).indexOf("handover_agree.") >= 0) {
                    if (confirm("確認要送出嗎？") == false) {
                        return false
                    }else {
                        location.href = "print_agree.html"
                    }
                }else if((location.href).indexOf("handover_meeting.") >= 0) {
                    if (confirm("確認要送出嗎？") == false) {
                        return false
                    }else {
                        location.href = "print_meeting.html"
                    }
                }else if((location.href).indexOf("handover_principal.") >= 0) {
                    if (confirm("確認要送出嗎？") == false) {
                        return false
                    }else {
                        location.href = "print_principal.html"
                    }
                }else if((location.href).indexOf("handover_teacher0.") >= 0) {
                    if (confirm("確認要送出嗎？") == false) {
                        return false
                    }else {
                        location.href = "print_teacher0.html"
                    }
                }else if((location.href).indexOf("handover_teacher1.") >= 0) {
                    if (confirm("確認要送出嗎？") == false) {
                        return false
                    }else {
                        location.href = "print_teacher1.html"
                    }
                }else if((location.href).indexOf("handover_object.") >= 0) {
                    if (confirm("確認要送出嗎？") == false) {
                        return false
                    }else {
                        location.href = "print_object.html"
                    }
                }else if((location.href).indexOf("handover_budget.") >= 0) {
                    if (confirm("確認要送出嗎？") == false) {
                        return false
                    }else {
                        location.href = "print_budget.html"
                    }
                }else if((location.href).indexOf("handover_sch.") >= 0) {
                    if (confirm("確認要送出嗎？") == false) {
                        return false
                    }else {
                        location.href = "print_sch.html"
                    }
                }else if((location.href).indexOf("handover_book.") >= 0) {
                    if (confirm("確認要送出嗎？") == false) {
                        return false
                    }else {
                        location.href = "print_book.html"
                    }
                }else if((location.href).indexOf("handover_upload.") >= 0){
                    if (confirm("確認要送出嗎？") == false) {
                        return false
                    }else {
                        location.href = "handover_list.html"
                    }
                }
            }
    
        }) 
    }

    if ((location.pathname).indexOf("act.") >= 0) {
        var calendar = new VanillaCalendar('#calendar',{
            settings: {
                lang: 'define',
                iso8601: false,
                visibility: {
                    weekNumbers: true,
                },
                selection: {
                    day: false,
                },
                
            },
            locale: {
                months: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],
                weekday: ['日', '一', '二', '三', '四', '五', '六'],
            },
            actions: {
                clickWeekNumber(event, number, days, year) {
                    $("#calendar").find(".vanilla-calendar-day__btn").removeClass("weekbar");
                    for (var i = 0; i < days.length; i++) {
                        days[i].classList.add("weekbar");
                        var weekday = days[i].getAttribute("data-calendar-day");
                        weekday = weekday.replace(/-/g,"/")
                        $(".timetableWrapper .dayItem").eq(i).find(".date").html(weekday)
                    };
                    $(".timetableWrapper").removeClass("hidden");
                    timetableHeight()
                },
                clickArrow(event, year, month) {
                    changeArrow()
                },
                clickYear(event, year) {
                    setTimeout(function(){
                        changeArrow()
                    })
                },
                clickMonth(e, month) {
                    setTimeout(function(){
                        changeArrow()
                    })
                },

            },
        });
        calendar.init();



        changeArrow()
        function changeArrow(){
            $(".vanilla-calendar-week-number").wrapInner("<span class='num'></span>");
            var iconEle = document.createElement("span");
            iconEle.classList.add("arrowIcon");
            iconEle.innerHTML = '<i class="fas fa-play"></i>';
            $(".vanilla-calendar-week-number .num").before(iconEle);
        }

        
        function timetableHeight(){
            if ($(window).width() > 575) {     
                var maxHeight = 0;
        
                $(".timetableWrapper .classItem").each(function(){
                    var thisH = $(this).height();
                    if (thisH > maxHeight) { maxHeight = thisH; }
                });
        
                $(".timetableWrapper .classItem").height(maxHeight);
            }
        }

        $(document).on('click','.vanilla-calendar-day__btn',function(){
            var thisweek = $(this).attr("data-calendar-week-number");

            $(".vanilla-calendar-week-number").each(function(){
                if ($(this).find(".num").text() == thisweek) {
                    $(this).trigger("click")
                }
                
            })
        })
    }


    if ((location.pathname).indexOf("apply_add.") >= 0) {
        $(document).on('change','.act_loc_trigger',function(){
            var $val = $(".act_loc_trigger:checked").val();
            $("#act_loc_agree").find(".hideBox").each(function(){
                if ($(this).attr("data-name") == $val) {
                    $(this).addClass("show")
                    $(this).fadeIn()
                }else {
                    $(this).removeClass("show")
                    $(this).hide()

                }
            })
        })

        // $(document).on('change','.speech_ex_trigger',function(){
        //     var $val = $(".speech_ex_trigger:checked").val();
        //     $("#speech_ex_box").find(".hideBox").each(function(){
        //         if ($(this).attr("data-name") == $val) {
        //             $(this).addClass("show")
        //             $(this).fadeIn()
        //         }else {
        //             $(this).removeClass("show")
        //             $(this).hide()

        //         }
        //     })
        // })

        $(document).on('change','.speech_ex2_trigger',function(){
            var $val = $(".speech_ex2_trigger:checked").val();
            if ($val == '是') {
                $("#speech_ex_box .hideBox.hideBox2").fadeIn()
            }else {
                $("#speech_ex_box .hideBox.hideBox2").hide()
            }
        
        })

        $(document).on('click',".submit-btn",function(e){
            e.preventDefault();
            var sum_arr = [];
            var this_form = Boolean($(this).parents('form'))? $(this).parents('form'): $('#'+$(this).attr('form'));
    
            
            this_form.find("[req=Y]").each(function() {
                var tooltips = $(this).attr("data-tooltip");
                var title = Boolean(tooltips) ? $.trim($(this).attr("data-tooltip")) : $.trim($(this).attr("title"));
                if ($(this).val() == "" || $(this).val() == null ) {
                    $(this).addClass("alert-border");
                    
                    sum_arr.push(title);
                }
                else if ((this.name).indexOf("act_type") >= 0){
                    if ($("[name=act_type]:checked").val()== "" || $("[name=act_type]:checked").val() == null) {
                        $(this).addClass("alert-border");
                        sum_arr.push(title);
                    }
                }
                else if ((this.name).indexOf("act_loc") >= 0){
                    if ($("[name=act_loc]:checked").val()== "" || $("[name=act_loc]:checked").val() == null) {
                        $(this).addClass("alert-border");
                        sum_arr.push(title);
                    }else {
                        var checkedItem = $("#act_loc_agree .hideBox.show").find("[type=checkbox]:checked").length
                        if (checkedItem <= 0) {
                            sum_arr.push(title);
                            $("#act_loc_agree .hideBox.show").find(".form-check-input").addClass("alert-border")
                        }
                    }
                }
                else if ((this.name).indexOf("category") >= 0){
                    if ($("[name=category]:checked").val() == null || $("[name=category]:checked").val() == "") {
                        $(this).addClass("alert-border");
                        sum_arr.push(title);
                    }
                    // else {
                    //     if ($("[name=category]:checked").val() == '演講') {
                    //         if ($(".speech_ex2_trigger:checked").val() == "" || $(".speech_ex2_trigger:checked").val() == null) {
                    //             $(".speech_ex2_trigger").addClass("alert-border")
                    //             sum_arr.push("請確認是否為學術演講");
                    //         }else if($(".speech_ex2_trigger:checked").val() == '是'){
                    //             $("#speech_ex_box .hideBox2").find(".form-control").each(function(){
                    //                 if ($(this).val() == "" || $(this).val() == null) {
                    //                     $(this).addClass("alert-border")
                    //                     sum_arr.push("請填寫學術演講相關資訊");
                    //                 }
                    //             })
                    //         }
                            
                    //     }
                    // }
                }
                else if ((this.name).indexOf("poster") >= 0){
                    if ($("[name=poster]:checked").val()== "" || $("[name=poster]:checked").val() == null) {
                        $(this).addClass("alert-border");
                        sum_arr.push(title);
                    }
                }
                else if ((this.name).indexOf("equip_use") >= 0){
                    if ($("[name=equip_use]:checked").val()== "" || $("[name=equip_use]:checked").val() == null) {
                        $(this).addClass("alert-border");
                        sum_arr.push(title);
                    }
                }
                else if ((this.name).indexOf("sdgs") >= 0){
                    if ($("[name=sdgs]:checked").val()== "" || $("[name=sdgs]:checked").val() == null) {
                        $(this).addClass("alert-border");
                        sum_arr.push(title);
                    }
                }
               
            })
          
    
            if(sum_arr.length > 0){
                sum_arr = unique(sum_arr);
                sum_arr = sum_arr.join("、");
                alert(sum_arr)
                $('html, body').animate({
                    scrollTop: $('.alert-border:first').offset().top - 200
                }, 500);
    
            } else {
                // this_form.submit();
                location.href = "apply_add2.html"
            }
    
        }) 
    }

    if ((location.pathname).indexOf("apply_add2.") >= 0) {

        $(document).on('change','#sch_date',function(){
            var $val = $(this).val()
            
            if ($(this).val() !== "") {
                $("#usedTable_box").removeClass("hidden")
            }else {
                $("#usedTable_box").addClass("hidden")
            }

            $("#selected_date").removeClass("hidden").children("span").text($val)
        })
        
        
        $(document).on('change','.class_input_trigger',function(){
            var $val = $(".class_input_trigger:checked").val();
            $("#class_input_box").find(".hideBox").each(function(){
                if ($(this).attr("data-name") == $val) {
                    $(this).fadeIn().addClass("show");
                }else {
                    $(this).hide().removeClass("show");
                }
            })
            
        })

        var optionHtml = '<option selected="selected" value="大恩B101(112人)">大恩B101(112人)</option><option value="大恩B103(114人)">大恩B103(114人)</option><option value="大恩102(會議講堂)">大恩102(會議講堂)</option><option value="大恩103(會議講堂)">大恩103(會議講堂)</option><option value="大恩301-煮食教室(93人)">大恩301-煮食教室(93人)</option><option value="大恩302(37人)">大恩302(37人)</option><option value="大恩304(12人)">大恩304(12人)</option><option value="大恩306(12人)">大恩306(12人)</option><option value="大恩307-煮食教室(91人)">大恩307-煮食教室(91人)</option><option value="大恩308(13人)">大恩308(13人)</option><option value="大恩310-煮食教室(63人)">大恩310-煮食教室(63人)</option><option value="大恩312(64人)">大恩312(64人)</option><option value="大恩401(55人)">大恩401(55人)</option><option value="大恩402(63人)">大恩402(63人)</option><option value="大恩403-煮食教室(85人)">大恩403-煮食教室(85人)</option><option value="大恩404-煮食教室(63人)">大恩404-煮食教室(63人)</option><option value="大恩405(84人)">大恩405(84人)</option><option value="大恩406(64人)">大恩406(64人)</option><option value="大恩407(86人)">大恩407(86人)</option><option value="大恩408(66人)">大恩408(66人)</option><option value="大恩409(85人)">大恩409(85人)</option><option value="大恩411(84人)">大恩411(84人)</option><option value="大恩413(84人)">大恩413(84人)</option><option value="大恩501(57人)">大恩501(57人)</option><option value="大恩502-投影機教室(66人)">大恩502-投影機教室(66人)</option><option value="大恩503(85人)-新增投影機教室">大恩503(85人)-新增投影機教室</option><option value="大恩504-投影機教室(63人)">大恩504-投影機教室(63人)</option><option value="大恩505(84人)--新增投影機教室">大恩505(84人)--新增投影機教室</option></option><option value="大恩507(84人)--新增投影機教室">大恩507(84人)--新增投影機教室</option><option value="大恩508-投影機教室(68人)">大恩508-投影機教室(68人)</option><option value="大恩509(84人)">大恩509(84人)</option><option value="大恩511(91人)">大恩511(91人)</option><option value="大恩513(84人)">大恩513(84人)</option><option value="大恩601(54人)">大恩601(54人)</option><option value="大恩602(68人)">大恩602(68人)</option><option value="大恩603(84人)">大恩603(84人)</option><option value="大恩604(63人)">大恩604(63人)</option><option value="大恩605(88人)">大恩605(88人)</option><option value="大恩606(63人)">大恩606(63人)</option><option value="大恩607(86人)">大恩607(86人)</option><option value="大恩608(69人)">大恩608(69人)</option><option value="大恩609(84人)">大恩609(84人)</option><option value="大恩611(84人)">大恩611(84人)</option><option value="大恩613(84人)">大恩613(84人)</option>'

        $(document).on('change','.classroom1_select',function(){
            if ($(this).val() !== "") {
                $(".classroom2_select").html(optionHtml)
            }
            $(".classroom2_select").trigger("change")
        })

        $(document).on('change','.classroom2_select',function(){
            if ($(this).val() !== "") {
                $("#thisdayTable_box").add("#thisroom_box").removeClass("hidden")
            }else {
                $("#thisdayTable_box").add("#thisroom_box").addClass("hidden")
            }
            alertClass()
        })
        
        function alertClass(){
            var option = $(".classroom2_select option:selected").val()
            if (option == '大恩B103(114人)') {
                alert("此教室已有租借紀錄")
            }
        }

        $(document).on('click',".addevent-btn",function(e){
            e.preventDefault();
            var sum_arr = [];
            var this_form = Boolean($(this).parents('form'))? $(this).parents('form'): $('#'+$(this).attr('form'));
    
            
            this_form.find("[req=Y]").each(function() {
                var tooltips = $(this).attr("data-tooltip");
                var title = Boolean(tooltips) ? $.trim($(this).attr("data-tooltip")) : $.trim($(this).attr("title"));
                if ($(this).val() == "" || $(this).val() == null ) {
                    $(this).addClass("alert-border");
                    
                    sum_arr.push(title);
                }
                else if ((this.name).indexOf("loc") >= 0){
                    if ($("[name=loc]:checked").val() == null || $("[name=loc]:checked").val() == "") {
                        $(this).addClass("alert-border");
                        sum_arr.push(title);
                    }else {
                        $("#class_input_box").find(".hideBox.show").each(function(){
                            if ($(this).find(".class_info_req").val() == "" ||$(this).find(".class_info_req").val() == null) {
                                $(this).find(".class_info_req").addClass("alert-border");
                                sum_arr.push(title);
                            }
                        })
                    }
                }
            }) 
            if(sum_arr.length > 0){
                sum_arr = $.unique(sum_arr)
                sum_arr = sum_arr.join("、");
                alert(sum_arr)
                $('html, body').animate({
                    scrollTop: $('.alert-border:first').offset().top - 200
                }, 500);
    
            } else {
                var $date = $("#sch_date").val();
                var start_hour = $("#startHour option:selected").val();
                var start_min = $("#startMin option:selected").val();
                var end_hour = $("#endHour option:selected").val();
                var end_min = $("#endMin option:selected").val();

                var place 
                if ($("#class_input_box").find(".hideBox.show").attr("data-name") == '校內場地') {
                    place = $("#class_input_box").find(".hideBox.show .class_info_req.classroom2_select option:selected").val()
                }else {
                    place = $("#class_input_box").find(".hideBox.show .class_info_req").val()
                }
                 
                var eventHtml = '<div class="item"><span class="date">'+$date+'</span><span class="time">'+start_hour+':'+start_min+'~'+end_hour+':'+end_min+'</span><span class="location">'+place+'</span><button class="del-btn btn" type="button">移除 <i class="fas fa-times"></i></button></div>'
                $("#event_des").append(eventHtml)
    
            }
        }) 

        $(document).on('click','.dateItem .del-btn',function(){
            if (confirm("確認移除？") == false) {
                return false
            }else {
                $(this).closest(".item").remove()
            }
        })

        $(document).on('click',".submit-btn",function(e){
            e.preventDefault();
            var sum_arr = [];
            var this_form = Boolean($(this).parents('form'))? $(this).parents('form'): $('#'+$(this).attr('form'));

            this_form.find("[req=Y]").each(function() {
                var tooltips = $(this).attr("data-tooltip");
                var title = Boolean(tooltips) ? $.trim($(this).attr("data-tooltip")) : $.trim($(this).attr("title"));
                if ($(this).val() == "" || $(this).val() == null ) {
                    $(this).addClass("alert-border");
                    
                    sum_arr.push(title);
                }
            })
            if ($("#event_des").length > 0){
                if ($("#event_des").find(".item").length <= 0) {
                    sum_arr.push($("#event_des").attr("title"));
                }
            }
          
            if(sum_arr.length > 0){
                sum_arr = unique(sum_arr);
                sum_arr = sum_arr.join("、");
                alert(sum_arr)
                $('html, body').animate({
                    scrollTop: $('.alert-border:first').offset().top - 200
                }, 500);
    
            } else {
                // this_form.submit();
                location.href = "apply_add3.html"
            }
        }) 

        // autocomplete
        var input1 = [
            "體育館-2F-綜合球場",
            "大仁館-1F-舞蹈教室"
        ];
        var input2 = [
            "救國團-復興青年活動中心",
            "圖書館"
        ];
        $("#jq_input1").autocomplete({
            source: input1,
            appendTo: ".jq_box1"
        });
        $("#jq_input2").autocomplete({
            source: input2,
            appendTo: ".jq_box2"
        });
    }


    if ((location.pathname).indexOf("apply_add3.") >= 0) {
        $(document).on('click',".submit-btn",function(e){
            e.preventDefault();
            var sum_arr = [];
            var this_form = Boolean($(this).parents('form'))? $(this).parents('form'): $('#'+$(this).attr('form'));
            
            this_form.find("[req=Y]").each(function() {
                var tooltips = $(this).attr("data-tooltip");
                var title = Boolean(tooltips) ? $.trim($(this).attr("data-tooltip")) : $.trim($(this).attr("title"));
                if ($(this).val() == "" || $(this).val() == null ) {
                    $(this).addClass("alert-border");
                    
                    sum_arr.push(title);
                }
              
            })
          
    
            if(sum_arr.length > 0){
                sum_arr = $.unique(sum_arr)
                sum_arr = sum_arr.join("、");
                alert(sum_arr)
                $('html, body').animate({
                    scrollTop: $('.alert-border:first').offset().top - 200
                }, 500);
    
            } else {
                 location.href = "apply_add4.html"
            }
    
        }) 
    }

    if ((location.pathname).indexOf("apply_add4.") >= 0) {
        $(document).on('change','#agree',function(){
            if ($(this).is(":checked")) {
                $(".hideBox").addClass("show").fadeIn();
                $(".hideBox").find(".form-control:not(.not-req)").attr('req','Y')
            }else {
                $(".hideBox").addClass("show").hide();
                $(".hideBox").find(".form-control").val("")
                $(".hideBox").find(".form-control").attr('req','')
            }
        })

        $(document).on('click',".submit-btn",function(e){
            e.preventDefault();
            var sum_arr = [];
            var this_form = Boolean($(this).parents('form'))? $(this).parents('form'): $('#'+$(this).attr('form'));
    
            
            this_form.find("[req=Y]").each(function() {
                var tooltips = $(this).attr("data-tooltip");
                var title = Boolean(tooltips) ? $.trim($(this).attr("data-tooltip")) : $.trim($(this).attr("title"));
                if ($(this).val() == "" || $(this).val() == null ) {
                    $(this).addClass("alert-border");
                    
                    sum_arr.push(title);
                }
                else if ((this.id).indexOf("agree") >= 0){
                    if ($("#agree:checked").val()== "" || $("#agree:checked").val() == null) {
                        $(this).addClass("alert-border");
                        sum_arr.push(title);
                    }
                }
               
            })
      
          
    
            if(sum_arr.length > 0){
                sum_arr = sum_arr.join("、");
                alert(sum_arr)
                $('html, body').animate({
                    scrollTop: $('.alert-border:first').offset().top - 200
                }, 500);
    
            } else {
                // this_form.submit();
                location.href = "apply_check.html"
            }
    
        }) 

    }

    if ((location.pathname).indexOf("apply_detail.") >= 0) {
        $(document).on('click','.dateItem .del-btn',function(){
            if (confirm("確認移除？") == false) {
                return false
            }else {
                var cancel_date = $(this).closest(".item").find(".date").html();
                var cancel_time = $(this).closest(".item").find(".time").html();
                var cancel_location = $(this).closest(".item").find(".location").html();

                var cancel_html = '<div class="item">'
                cancel_html += '<span class="date">'+cancel_date+'</span>'
                cancel_html += '<span class="time">'+cancel_time+'</span>'
                cancel_html += '<span class="location">'+cancel_location+'</span>'
                cancel_html += '</div>'

                $("#cancel_wrapper .default-text").addClass("hidden");
                $("#cancel_wrapper").append(cancel_html)

                $(this).closest(".item").remove();
            }
        })
    }

    if($(".target_list").length > 0){
        $(document).on('click', '#add_input_btn', function () {
            var c = $(".target_list").find("[type='file']").length;
            var addItem = '<div class="item mb-2"><input id="File' + c + '" type="file" class="form-control w-auto" title="請上傳附件檔案" accept=".pdf,.zip,.rar,.7z,.jpg,.jpeg,.png"><button class="del-btn hidden" type="button"><i class="fas fa-times"></i></button></div>';
            var addItem2 = '<div class="item mb-2"><input type="file" class="form-control w-auto" title="請上傳附件檔案" accept=".docx,.doc,.odt,.pdf"><button class="del-btn hidden" type="button"><i class="fas fa-times"></i></button></div>'
            checkNum()
            if ($(".target_list").find("[type='file']").length <= 7) {  
                if ((location.pathname).indexOf("apply_add4.") >= 0) {
                    $(".target_list").append(addItem2);
                }else{
                    $(".target_list").append(addItem);
                }
                $(".target_list .del-btn").removeClass("hidden")
            }
        })
        $(document).on('click','.target_list .del-btn',function(){
            var num = $(".target_list").find("[type='file']").length

            if (confirm("確認清除？") == false) {
                return false
            }else{
                if (num >=2) {
                    $(this).closest(".item").remove()
                    $("#add_input_btn").prop("disabled",false)
                }

                if(num <= 2){
                    $(".target_list .del-btn").addClass("hidden")
                }else {
                    $(".target_list .del-btn").removeClass("hidden")
                }
            }
        })
  
        function checkNum(){
            var num = $(".target_list").find("[type='file']").length
            if (num >= 7) {
                $("#add_input_btn").prop("disabled",true)
            }else{
                $("#add_input_btn").prop("disabled",false)
            }
        }
    }
    
    if ((location.pathname).indexOf("club_sch.") >= 0) {
        $(document).on('click',".submit-btn",function(e){
            e.preventDefault();
            var sum_arr = [];
            var this_form = Boolean($(this).parents('form'))? $(this).parents('form'): $('#'+$(this).attr('form'));
    
            
            this_form.find("[req=Y]").each(function() {
                var tooltips = $(this).attr("data-tooltip");
                var title = Boolean(tooltips) ? $.trim($(this).attr("data-tooltip")) : $.trim($(this).attr("title"));
                if ($(this).val() == "" || $(this).val() == null ) {
                    $(this).addClass("alert-border");
                    
                    sum_arr.push(title);
                }       
            })

            if(sum_arr.length > 0){
                sum_arr = unique(sum_arr);
                sum_arr = sum_arr.join("、");
                alert(sum_arr)
                $('html, body').animate({
                    scrollTop: $('.alert-border:first').offset().top - 200
                }, 500);
    
            } else {
                // this_form.submit();

                alert("新增成功！")
                var codeVal = $(".input_code").val();
                var titleVal = $(".input_title").val();
                var timeVal = $(".input_time").val();
                var locVal = $(".input_loc").val();
                var introVal = $(".input_intro").val();
                var budVal = $(".input_bud").val();
                var typeVal = $("[name=act_type]:checked").val();

                var html = '<div class="schItem"><div class="schHeader"><div class="toolbox"><a class="tool-btn" href="club_sch_edit.html">編輯</a><a class="tool-btn" href="javascript:void(0)">刪除 <i class="fas fa-times"></i></a></div></div><div class="row"><div class="col-lg-3"><div class="form-item"><label for="">活動類別</label><div class="desc">'+codeVal+'</div></div></div><div class="col-lg-3"><div class="form-item"><label for="">活動名稱</label><div class="desc">'+titleVal+'</div></div></div><div class="col-lg-3"><div class="form-item"><label for="">活動時間</label><div class="desc">'+timeVal+'</div></div></div><div class="col-lg-3"><div class="form-item"><label for="">預定場地</label><div class="desc">'+locVal+'</div></div></div><div class="col-lg-3"><div class="form-item"><label for="">內容簡介</label><div class="desc">'+introVal+'</div></div></div><div class="col-lg-3"><div class="form-item"><label for="">經費預算</label><div class="desc">'+budVal+'</div></div></div><div class="col-lg-3"><div class="form-item"><label for="">自檢狀態項</label><div class="desc">'+typeVal+'</div></div></div></div></div>'

                $(".infoWrapper").append(html)
                
            }
    
        }) 

        $(document).on("click",".remove-btn",function(){
            if (confirm("確認刪除？") == false) {
                return false
            }else {
                $(this).closest(".schItem").remove()
            }
        })
    }

    if ((location.pathname).indexOf("club_sch_edit.") >= 0) {
        $(document).on('click',".submit-btn",function(e){
            e.preventDefault();
            var sum_arr = [];
            var this_form = Boolean($(this).parents('form'))? $(this).parents('form'): $('#'+$(this).attr('form'));
    
            
            this_form.find("[req=Y]").each(function() {
                var tooltips = $(this).attr("data-tooltip");
                var title = Boolean(tooltips) ? $.trim($(this).attr("data-tooltip")) : $.trim($(this).attr("title"));
                if ($(this).val() == "" || $(this).val() == null ) {
                    $(this).addClass("alert-border");
                    
                    sum_arr.push(title);
                }       
            })

            if(sum_arr.length > 0){
                sum_arr = unique(sum_arr);
                sum_arr = sum_arr.join("、");
                alert(sum_arr)
                $('html, body').animate({
                    scrollTop: $('.alert-border:first').offset().top - 200
                }, 500);
    
            } else {
                // this_form.submit();
                alert("儲存成功！")     
            }
    
        }) 

        $(document).on("click",".remove-btn",function(){
            if (confirm("確認刪除？") == false) {
                return false
            }else {
                alert("刪除成功！")     
                location.href = 'club_sch.html'
            }
        })
    }

    if ((location.pathname).indexOf("member_edit.") >= 0) {
        var html = '<div class="list-item"><div class="text-end"><button class="btn del-btn" type="button"><i class="fas fa-times"></i></button></div><div class="row"><div class="col-lg-3 col-md-3"><div class="form-item"><label for="">學年度<span class="req-star">*</span></label><input type="text" class="form-control" req="Y" value="111" readonly disabled></div></div><div class="col-lg-9 col-md-9"><div class="form-item"><label for="">參與期間<span class="req-star">*</span></label><div class="d-flex align-items-center"><input type="date" class="form-control" req="Y" placeholder="參與期間開始日期" title="請輸入參與期間開始日期"><span class="mx-1">至</span><input type="date" class="form-control" req="Y" placeholder="參與期間結束日期" title="請輸入參與期間結束日期"></div></div></div><div class="col-lg-3 col-md-3"><div class="form-item"><label for="">學系班級<span class="req-star">*</span></label><input type="text" class="form-control" req="Y" value="俄文3" placeholder="學系班級" title="請輸入學系班級"></div></div><div class="col-lg-3 col-md-3"><div class="form-item"><label for="">生理性別<span class="req-star">*</span></label><select name="" id="" class="form-control" req="Y" title="請選擇生理性別"><option value="" disabled>請選擇</option><option value="女" selected>女</option><option value="男">男</option><option value="其他">其他</option></select></div></div>'
        html += '<div class="col-lg-3 col-md-3"><div class="form-item"><label for="">學號<span class="req-star">*</span></label><input type="text" class="form-control" req="Y" value="A0000000" placeholder="學號" title="請輸入學號"></div></div><div class="col-lg-3 col-md-3"><div class="form-item"><label for="">姓名<span class="req-star">*</span></label><input type="text" class="form-control" req="Y" value="陳煜文" placeholder="姓名" title="請輸入姓名"></div></div><div class="col-lg-12 col-md-12"><div class="form-item"><label for="">備註</label><input type="text" class="form-control" value="" placeholder="備註"></div></div></div></div>'

        $(document).on('click','#add_btn',function(){
            $(".member-list-page").append(html)
        })

        $(document).on('click','.del-btn',function(){
            if (confirm("確認刪除？") == false) {
                return false
            }else {
                $(this).closest(".list-item").remove()
            }
        })

        $(document).on('click',".submit-btn",function(e){
            e.preventDefault();
            var sum_arr = [];
            var this_form = Boolean($(this).parents('form'))? $(this).parents('form'): $('#'+$(this).attr('form'));
    
            
            this_form.find("[req=Y]").each(function() {
                var tooltips = $(this).attr("data-tooltip");
                var title = Boolean(tooltips) ? $.trim($(this).attr("data-tooltip")) : $.trim($(this).attr("title"));
                if ($(this).val() == "" || $(this).val() == null ) {
                    $(this).addClass("alert-border");
                    
                    sum_arr.push(title);
                }       
            })

            if(sum_arr.length > 0){
                sum_arr = unique(sum_arr);
                sum_arr = sum_arr.join("、");
                alert(sum_arr)
                $('html, body').animate({
                    scrollTop: $('.alert-border:first').offset().top - 200
                }, 500);
    
            } else {
                // this_form.submit();
                alert("儲存成功！")   
                location.href = 'member_list.html'  
            }
    
        }) 
    }

    if ((location.pathname).indexOf("member_c_edit.") >= 0) {
        var html = '<div class="list-item"><div class="text-end"><button class="btn del-btn" type="button"><i class="fas fa-times"></i></button></div><div class="row"><div class="col-lg-3 col-md-3"><div class="form-item"><label for="">學年度</label><input type="text" class="form-control" req="Y" value="111" readonly></div></div><div class="col-lg-9 col-md-9"><div class="form-item"><label for="">任職期間<span class="req-star">*</span></label><div class="d-flex align-items-center"><input type="date" class="form-control" req="Y" placeholder="任職期間開始日期" title="請輸入任職期間開始日期"><span class="mx-1">至</span><input type="date" class="form-control" req="Y" placeholder="任職期間結束日期" title="請輸入任職期間結束日期"></div></div></div><div class="col-lg-3 col-md-3"><div class="form-item"><label for="">職別<span class="req-star">*</span></label><input type="text" class="form-control" req="Y" value="" placeholder="職別" title="請輸入職別"></div></div><div class="col-lg-3 col-md-3"><div class="form-item"><label for="">姓名<span class="req-star">*</span></label><input type="text" class="form-control" req="Y" value="" placeholder="姓名" title="請輸入姓名"></div></div><div class="col-lg-3 col-md-3"><div class="form-item"><label for="">學號<span class="req-star">*</span></label><input type="text" class="form-control" req="Y" value="" placeholder="學號" title="請輸入學號"></div></div><div class="col-lg-3 col-md-3"><div class="form-item"><label for="">學系年級<span class="req-star">*</span></label><input type="text" class="form-control" req="Y" value="" placeholder="學系年級" title="請輸入學系年級"></div></div><div class="col-lg-3 col-md-3"><div class="form-item"><label for="">生理性別<span class="req-star">*</span></label><select name="" id="" class="form-control" req="Y" title="請選擇生理性別"><option value="" selected disabled>請選擇</option><option value="女">女</option><option value="男">男</option><option value="其他">其他</option></select></div></div>'
        html += '<div class="col-lg-3 col-md-3"><div class="form-item"><label for="">連絡電話<span class="req-star">*</span></label><input type="tel" class="form-control" req="Y" value="" placeholder="連絡電話" title="請輸入連絡電話"></div></div><div class="col-lg-6 col-md-6"><div class="form-item"><label for="">Email<span class="req-star">*</span></label><input type="email" class="form-control" req="Y" value="" placeholder="Email" title="請輸入Email"></div></div><div class="col-lg-12 col-md-12"><div class="form-item"><label for="">其他</label><input type="text" class="form-control" value="" placeholder="其他"></div></div></div></div>'

        $(document).on('click','#add_btn',function(){
            $(".member-list-page").append(html)
        })

        $(document).on('click','.del-btn',function(){
            if (confirm("確認刪除？") == false) {
                return false
            }else {
                $(this).closest(".list-item").remove()
            }
        })

        $(document).on('click',".submit-btn",function(e){
            e.preventDefault();
            var sum_arr = [];
            var this_form = Boolean($(this).parents('form'))? $(this).parents('form'): $('#'+$(this).attr('form'));
    
            
            this_form.find("[req=Y]").each(function() {
                var tooltips = $(this).attr("data-tooltip");
                var title = Boolean(tooltips) ? $.trim($(this).attr("data-tooltip")) : $.trim($(this).attr("title"));
                if ($(this).val() == "" || $(this).val() == null ) {
                    $(this).addClass("alert-border");
                    
                    sum_arr.push(title);
                }       
            })

            if(sum_arr.length > 0){
                sum_arr = unique(sum_arr);
                sum_arr = sum_arr.join("、");
                alert(sum_arr)
                $('html, body').animate({
                    scrollTop: $('.alert-border:first').offset().top - 200
                }, 500);
    
            } else {
                // this_form.submit();
                alert("儲存成功！")   
                location.href = 'member_c_list.html'  
            }
    
        }) 
    }

    // 列印按鈕
    $(document).on('click','.print-btn',function(){
        window.print();
    })
    
    // 返回上一頁
    $('.go-back-btn').on('click', function() {
        history.back(-1)
    })

})