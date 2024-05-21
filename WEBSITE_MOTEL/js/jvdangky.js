// //kiem tra s la email hay khong
function isEmail(s) {
    const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(s).toLowerCase());
}
$(document).ready(function () {
    $(".btnRe").click(function (e) {
        var user_name = $("#user_name").val();
        var user_tendn = $("#user_tendn").val();
        var use_phone = $("#use_phone").val();
        var user_email = $("#user_email").val();
        var user_pass = $("#user_pass").val();
        var user_re_pass = $("#user_re_pass").val();
        var checkEmail = isEmail(user_email);
        var sttError = 1;
        if (user_name == null || user_name == '') {
            $("#user_name").focus();
            $(".item_message .message").html('Vui lòng nhập họ và tên.');
        } else if (user_tendn == null || user_tendn == '') {
            $("#user_tendn").focus();
            $(".item_message .message").html('Vui lòng nhập tên đăng nhập.');
        }
       /* else if (user_tendn != null) {
            $("#user_tendn").focus();
            $(".item_message .message").html('Tên đăng nhập đã tồn tại.');
        }*/ else if (use_phone == null || use_phone == '') {
            $("#use_phone").focus();
            $(".item_message .message").html('Vui lòng nhập số điện thoại liên lạc.');
        }
        else if (user_email == null || user_email == '') {
            $("#user_email").focus();
            $(".item_message .message").html('Vui lòng nhập email.');
        } else if (!checkEmail) {
            $("#user_email").focus();
            $(".item_message .message").html('Vui lòng nhập đúng email.');
        } else if (user_pass == null || user_pass == '') {
            $("#user_pass").focus();
            $(".item_message .message").html('Vui lòng nhập mật khẩu.');
        } else if (user_re_pass == null || user_re_pass == '') {
            $("#user_pass").focus();
            $(".item_message .message").html('Vui lòng nhập lại mật khẩu.');
        } else if (user_pass != user_re_pass) {
            $("#user_re_pass").focus();
            $(".item_message .message").html('Nhập lại mật khẩu không trùng khớp.');
        } else { sttError = 0; }
        if (sttError == 1) { e.preventDefault(); }
    });
});