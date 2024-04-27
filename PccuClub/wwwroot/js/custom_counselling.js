$(document).ready(function () {
    //// 警示重置
    //$(document).on('focus', '.alert-border', function () {
    //    $(this).removeClass("alert-border");
    //})
    //$(document).on('change', '.alert-border', function () {
    //    $(this).removeClass("alert-border");
    //})
    //$(document).on('click', '.warning-box label, .warning-box .form-check-input', function () {
    //    $(this).closest(".warning-box").find(".form-check-input").removeClass("alert-border")
    //})


    // 國籍
    $(document).on('change', '.national_trigger', function () {
        var $val = $(".national_trigger:checked").val();
        $("#national_agree").find(".hideBox").each(function () {
            if ($(this).attr("data-name") == $val) {
                $(this).addClass("show")
                $(this).fadeIn()
            } else {
                $(this).removeClass("show")
                $(this).hide()
            }
        })
    })

    // 過去2週曾出現這些想法或計劃
    $(document).on('change', '.mindset_trigger', function () {
        var is_checked = $(this).prop("checked");
        var $val = $(this).val();
        if (is_checked && $val === "04") {
            $(".mindset_trigger:not([value='04'])").each(function () {
                $(this).prop("checked", !is_checked); // 取消勾選其他選項
            })
        } else {
            $(".mindset_trigger[value='04']").prop("checked", false); // 取消勾選[以上皆無]
        }
    })

    // 可初談時段
    var week_arr = ["mon", "tue", "wed", "thu", "fri"];
    var time_arr = [9, 10, 11, 12, 13, 14, 15];
    var time_table = "";
    $.each(time_arr, function (index, time) {
        time_table += '<tr class="text-center">\n';
        time_table += '<th>' + time + '-' + (time + 1);
        time_table += '<input type="checkbox" name="toggle-all-horizontal" class="form-check-input mx-2" data-bs-toggle="tooltip" data-bs-title="每天' + time + '-' + (time + 1) + '"></th>\n';
        $.each(week_arr, function (index, week) {
            var val = week + '-' + time;
            time_table += '<td><input type="checkbox" value="' + val + '" name="chktime" class="form-check-input"></td>\n';
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
        $(this).closest("tr").find(":checkbox").each(function () {
            $(this).prop("checked", is_checked);
        })
    })

    $("[name=toggle-all]").change(function () {
        var is_checked = $(this).prop("checked");
        $(this).closest("table").find(":checkbox").each(function () { // td>
            $(this).prop("checked", is_checked);
        })
    })

    // 返回上一頁
    $('.go-back-btn').on('click', function () {
        history.back()
    })
})