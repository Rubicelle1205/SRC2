$(function () {
    
    if ((location.pathname).indexOf("Personal") >= 0 || (location.pathname).indexOf("AdminMang") >= 0) {
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
        if ((location.pathname).indexOf("Personal") >= 0 || (location.pathname).indexOf("AdminMang") >= 0) {
            // 密碼驗證
            $('[id$=Password]').blur(function () {
                var result = validatePasswd('[id$=Password]', '#passwdRules', 6, 15)
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
                var password = $(this).parents('td').find('input[id$=Password]');
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
                var password = $(this).parents('td').find('input[id$=Password]');
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

        //###############以下為新增及編輯頁面 fun#######################//
        if ((location.pathname).indexOf("_mang.") >= 0) {

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
                var ckeditorObj = CKEDITOR.instances;
                var ckeditorArr = Object.keys(ckeditorObj);
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
                    var model = $('[name=model]').val();
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
                    } else if (ckeditorArr.length > 0 && (ckeditorArr.indexOf(name) !== -1)) {
                        var value = "";
                        if (ckeditorArr.indexOf(name) !== -1) {
                            value = ckeditorObj[name].getData();
                        } else if (ckeditorArr.indexOf("ckeditor") !== -1) {
                            value = ckeditorObj.ckeditor.getData();
                        }
                        if (value === "") {
                            sum_arr.push("請檢查「" + title + "」是否填寫?");
                            $(this).addClass('is-invalid');
                        }
                    } else if ($(this).attr('type') === "radio") {
                        var value = $('input:radio[name=' + $(this).attr("name") + ']:checked').val();
                        if (value === "" || value === undefined) {
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
                            } else if ($(this).attr('name') === 'types_option1' && $(this).val() === '其他' && $('[name=other0]').val() === '') {
                                var title = $.trim($('[name=other0]').attr("data-title"));
                                sum_arr.push("請檢查「" + title + "」是否填寫?");
                                $(this).addClass('is-invalid');
                            }
                        }
                    }


                    // 判斷
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

                if (sum_arr.length > 0) {
                    $btn.prop('disabled', false);
                    $btn.prev('button:reset').prop('disabled', false);
                    $btn.children("span").remove();
                    this_form.find('[data-title="' + sum_arr[0] + '"]').focus();
                    sum_arr = sum_arr.join("\n");
                    alert(sum_arr);
                    (e.preventDefault) ? e.preventDefault() : e.returnValue = false;
                    return false
                } else {
                    document.location.href = link;
                    // this_form.submit();
                }
            })

            /**
             * This will fix the CKEDITOR not handling the input[type=reset] clicks.
             */
            if (typeof CKEDITOR != 'undefined') {
                $('form').on('reset', function (e) {
                    if ($(CKEDITOR.instances).length) {
                        for (var key in CKEDITOR.instances) {
                            var instance = CKEDITOR.instances[key];
                            if ($(instance.element.$).closest('form').attr('name') == $(e.target).attr('name')) {
                                instance.setData(instance.element.$.defaultValue);
                            }
                        }
                    }
                });

                /* The "instanceCreated" event is fired for every editor instance created. */
                CKEDITOR.on('instanceCreated', function (event, data) {
                    var editor = event.editor,
                        element = editor.element;
                    editor.name = $(element).attr('name');
                });
            }

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
                // 找到所有被勾選的id，並合併成,字串
                var id = $('input[name=box_list]:checked').map(function () {
                    return $(this).val();
                }).get().join(",");

                if (id !== "") {
                    // alert("相關資料也會統一刪除")
                    // 為了避免使用者無腦刪除，會希望他們輸入字樣，知道自己在做刪除動作
                    var ans = prompt('請輸入 確定刪除 四個字');

                    if ($.trim(ans) === "確定刪除") {
                        $("input[name=box_list]").each(function () { //loop through each checkbox
                            $(this).parents("tr").remove(); // 模擬刪除
                            $(this).prop('checked', false); //uncheck
                        });

                        if ($("input[name=box_toggle]").prop("checked")) {
                            $("input[name=box_toggle]").prop("checked", false); // 全選按鈕
                        }

                        alert("刪除成功");
                    }
                } else {
                    alert("請先勾選欲刪除的項目")
                }
            });

            // 列表儲存排序
            $("button[name=save]").click(function (event) {
                var orders = $(this).parents("tr").find("[name=orders]").val();
                var id = $(this).parents("tr").find("[name=box_list]").val();

                alert("異動成功")
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
            $('[search_ref]').each(function () {
                $(this).change(function () {
                    $("[name=search_button]").trigger('click');
                })
            })

            // 列表excel匯出
            if ($('[name=export_excel]').length > 0) {
                $("[name=export_excel]").click(function (event) {
                    var yes = confirm("確定要匯出嗎？");
                    if (yes) {
                        alert("此時將匯出符合搜尋結果的excel資料");
                    } else {
                        return false;
                    }
                })
            }

            // 列表excel匯入按鈕警示
            if ($('[name=import_excel]').length > 0) {
                $('[name=import_excel]').click(function () {
                    var yes = confirm("匯入時，請使用系統提供之格式");
                    var link = $(this).data("link");
                    if (yes) {
                        document.location.href = link;
                    } else {
                        return false;
                    }
                })
            }
        }

    }
})


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