jQuery(document).ready(function ($) {
    //导航标签当前样式
    //var $index = $('#index');
    //var $about = $('#about');
    //var $contact = $('#contact');
    //var $clear = $('#main-nav li a');
    //$index.on('click', function (event) {
    //    $clear.removeClass("current");
    //    $index.addClass("current");
    //});
    //$about.on('click', function (event) {
    //    $clear.removeClass("current");
    //    $about.addClass("current");
    //});
    //$contact.on('click', function (event) {
    //    $clear.removeClass("current");
    //    $contact.addClass("current");
    //});

    //注册表单验证
    //$('#btnregister').on('click', function () {
    //    $('#register').attr('disabled',true);
    //});
    var phone_erro = 1;
    var name_erro = 1;
    var pwd_erro = 1;
    var rpwd_erro = 1;
    var pwd_match = false;
    //判断注册表单整体验证结果 
    function result() {
        if (phone_erro == 0 && name_erro == 0 && pwd_match == true)
            return true;
        else
            return false;
    };

    //手机号码数字限制
    $('#rphone').keypress(function () {
        var keyCode = event.keyCode;
        if ((keyCode >= 48 && keyCode <= 57)) {
            event.returnValue = true;
        }
        else {
            event.returnValue = false;
        }
    });

    //注册表单验证规则
        
        //空项判断
    function IsNull(obj) {
        if (obj.val() == '' || $(this).val() == null) {
            obj.next().remove();
            obj.parent().append("<div class='alert alert-danger'>这是必填项</div>");
        }
    };


    $('#rphone').on('blur', function () {
        if (IsNull($(this))()) { }
        else if ($(this).val().length <= 10 && $(this).val().length >= 1) {
            $(this).next().remove();
            $(this).parent().append("<div  class='alert alert-danger'>手机号码长度过短</div>");
        }
        else if ($(this).val().length == 11) {

            $(this).next().remove();
            phone_erro = 0;
            $(this).parent().append("<div class='alert alert-success'>填写正确</div>");
        }
    });
    $('#rname').on('blur', function () {
        if (IsNull($(this))()) { }
        else {
            $(this).next().remove();
            $(this).parent().append("<div name='error' class='alert alert-success'>填写正确</div>");
            name_erro = 0;
        }
    });
    
    function pwd_compare() {
        //两次密码检测
        if ($('#rpwd').val() === $('#rrpwd').val() && pwd_erro == 0 && rpwd_erro == 0)
            return true;
        else {
            $('#rrpwd').next().remove();
            $('#rrpwd').parent().append("<div class='alert alert-danger'>两次密码不相同</div>");
            return false;
        }
    };
    $('#rpwd').on('blur', function () {
        if (IsNull($(this))()) { }
        else if ($(this).val().length >= 0 && $(this).val().length<6) {
            $(this).next().remove();
            $(this).parent().append("<div class='alert alert-danger'>密码长度不能小于6位</div>");
        }
        else {
            $(this).next().remove();
            $(this).parent().append("<div name='error' class='alert alert-success'>填写正确</div>");
            pwd_erro = 0;
            
        }
        pwd_match = pwd_compare();
    });
    $('#rrpwd').on('blur', function () {
        if ($(this).val().length >= 1 && $(this).val().length < 6) {
            $(this).next().remove();
            $(this).parent().append("<div class='alert alert-danger'>两次密码不相同</div>");
        }
        else if ($(this).val()=='') { }
        else {
            $(this).next().remove();
            $(this).parent().append("<div name='error' class='alert alert-success'>填写正确</div>");
            rpwd_erro = 0;
        }
        pwd_match = pwd_compare();
    });
    //注册按钮触发表单提交
    $('#register').on('click', function () {
        $('#temp').focus();
        if (result())
        {
            $('#register_form').submit();
        }
        IsNull($('#rphone'))();
        alert("1");
        IsNull($('#rname'))();
        alert("2"); 
        IsNull($('#rpwd'))();
        alert("3");
    });



});
