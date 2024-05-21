

$(document).ready(function () {
    $(".btnLog ").click(function (e) {

        var user_tendn = $("#user_tendn").val();
        var user_pas = $("#user_pas").val();
        var sttError = 1;
        if (user_tendn == null || user_tendn == '') {
            $("#user_tendn").focus();
            $(".item_message .message").html('Vui lòng nhập tên đăng nhập.');
        } 
         else if (user_pas == null || user_pas == '') {
            $("#user_pas").focus();
            $(".item_message .message").html('Vui lòng nhập mật khẩu.');
        } else { sttError = 0; }
        if (sttError == 1) { e.preventDefault(); }
    });
});