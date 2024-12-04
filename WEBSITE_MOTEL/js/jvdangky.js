// Kiểm tra s là email hay không
function isEmail(s) {
    const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(s).toLowerCase());
}

// Kiểm tra ngày sinh có hợp lệ không
function isValidDate(dateString) {
    const re = /^\d{4}-\d{2}-\d{2}$/;
    if (!re.test(dateString)) return false;
    const date = new Date(dateString);
    return date instanceof Date && !isNaN(date);
}


// Kiểm tra số điện thoại
function isValidPhone(phone) {
    const re = /^0\d{9,10}$/; // Số điện thoại Việt Nam (bắt đầu bằng 0 và có 10-11 chữ số)
    return re.test(phone);
}

// Kiểm tra số CCCD
function isValidCCCD(cccd) {
    const re = /^\d{12}$/; // CCCD Việt Nam phải có 12 chữ số
    return re.test(cccd);
}

// Kiểm tra mật khẩu mạnh
function isStrongPassword(password) {
    const re = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/; // Ít nhất 8 ký tự, bao gồm chữ và số
    return re.test(password);
}

$(document).ready(function () {
    // Kiểm tra tên đăng nhập khi nhập
    $("#user_tendn").on('input', function () {
        var user_tendn = $(this).val();

        if (user_tendn) {
            $.ajax({
                url: '/User/CheckUsername',
                type: 'GET',
                data: { tendn: user_tendn },
                success: function (response) {
                    if (!response.available) {
                        $(".item_message .message").html(response.message).css('color', 'red');
                        $("#user_tendn").data('valid', false); // Set valid flag to false if unavailable
                    } else {
                        $(".item_message .message").html('');
                        $("#user_tendn").data('valid', true); // Set valid flag to true if available
                    }
                },
                error: function () {
                    $(".item_message .message").html('Đã xảy ra lỗi khi kiểm tra tên đăng nhập.').css('color', 'red');
                    $("#user_tendn").data('valid', false); // Set valid flag to false if there's an error
                }
            });
        } else {
            $(".item_message .message").html('');
            $("#user_tendn").data('valid', false);
        }
    });

    $("#user_sdt").on('input', function () {
        var user_sdt = $(this).val();

        if (user_sdt) {
            $.ajax({
                url: '/User/CheckPhone',
                type: 'GET',
                data: { sdt: user_sdt },
                success: function (response) {
                    if (!response.available) {
                        $(".item_message .message").html(response.message).css('color', 'red');
                        $("#user_sdt").data('valid', false); // Set valid flag to false if unavailable
                    } else {
                        $(".item_message .message").html('');
                        $("#user_sdt").data('valid', true); // Set valid flag to true if available
                    }
                },
                error: function () {
                    $(".item_message .message").html('Đã xảy ra lỗi khi kiểm tra số điện thoại.').css('color', 'red');
                    $("#user_sdt").data('valid', false);
                }
            });
        } else {
            $(".item_message .message").html('');
            $("#user_sdt").data('valid', false);
        }
    });

    $("input[name='phanquyen']").change(function () {
        if ($('#chutro').is(':checked')) {
            $(".cccd-file").show();
        } else {
            $(".cccd-file").hide();
        }
    });
    // Kiểm tra tất cả các trường khi bấm nút Đăng ký
    $(".btnRe").click(function (e) {
        $(".item_message .message").html(''); // Clear previous messages

        var user_name = $("#user_name").val();
        var user_email = $("#user_email").val();
        var user_tendn = $("#user_tendn").val();
        var use_phone = $("#user_sdt").val();
        var user_cccd = $("#user_cccd").val();
        var user_ns = $("#user_ns").val();
        var user_dc = $("#user_dc").val();
        var user_pass = $("#user_pass").val();
        var user_re_pass = $("#user_re_pass").val();
        var phanquyen = $('input[name="phanquyen"]:checked').val();
        var checkEmail = isEmail(user_email);

        // Validations
        if (!user_name) {
            $("#user_name").focus();
            return showError('Vui lòng nhập họ tên.');
        }
        if (!user_email) {
            $("#user_email").focus();
            return showError('Vui lòng nhập email.');
        }
        if (!checkEmail) {
            $("#user_email").focus();
            return showError('Vui lòng nhập đúng email.');
        }
        if (!use_phone) {
            $("#user_sdt").focus();
            return showError('Vui lòng nhập số điện thoại.');
        }
        if (!isValidPhone(use_phone)) {
            $("#user_sdt").focus();
            return showError('Số điện thoại không hợp lệ. Số điện thoại phải bắt đầu bằng 0 và có 10-11 chữ số.');
        }
        if ($("#user_sdt").data('valid') === false) {
            $("#user_sdt").focus();
            return showError('Số điện thoại đã tồn tại.');
        }
        if (!user_cccd) {
            $("#user_cccd").focus();
            return showError('Vui lòng nhập số căn cước công dân.');
        }
        if (!isValidCCCD(user_cccd)) {
            $("#user_cccd").focus();
            return showError('Số CCCD không hợp lệ. CCCD phải có 12 chữ số.');
        }
        if (!user_dc) {
            $("#user_dc").focus();
            return showError('Vui lòng nhập địa chỉ.');
        }
        if (!user_ns) {
            $("#user_ns").focus();
            return showError('Vui lòng điền ngày sinh.');
        }
        if (!isValidDate(user_ns)) {
            $("#user_ns").focus();
            return showError('Ngày sinh không hợp lệ. Vui lòng nhập theo định dạng MM-DD-YYYY.');
        }
        if (!user_tendn) {
            $("#user_tendn").focus();
            return showError('Vui lòng nhập tên đăng nhập.');
        }
        if ($("#user_tendn").data('valid') === false) {
            $("#user_tendn").focus();
            return showError('Tên đăng nhập đã tồn tại.');
        }
        if (!user_pass) {
            $("#user_pass").focus();
            return showError('Vui lòng nhập mật khẩu.');
        }
        if (!isStrongPassword(user_pass)) {
            $("#user_pass").focus();
            return showError('Mật khẩu phải có ít nhất 8 ký tự và bao gồm cả chữ cái và số.');
        }
        if (!user_re_pass) {
            $("#user_re_pass").focus();
            return showError('Vui lòng nhập lại mật khẩu.');
        }
        if (user_pass !== user_re_pass) {
            $("#user_re_pass").focus();
            return showError('Nhập lại mật khẩu không trùng khớp.');
        }
        if (!phanquyen) {
            return showError('Vui lòng chọn hình thức đăng ký.');
        }
        if (phanquyen === "2") {
            if ($("#user_card_photo").get(0).files.length === 0 || $("#user_card_photo_2").get(0).files.length === 0) {
                return showError('Vui lòng tải lên hình ảnh mặt trước và mặt sau của CCCD.');
            }
        }

        function showError(message) {
            $(".item_message .message").html(message);
            e.preventDefault(); // Prevent form submission
            return;
        }
    });

});
