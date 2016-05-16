jQuery(document).ready(function ($) {

    init();//初始化
    function init() {
        registerValidation();//注册表单初始化
        loginValidation();//登陆表单初始化
        goodlist();
        if (!autholization()) {      //用户验证
            $('.cd-cart-items').html("");
        }
        else
            getCartAjax();//获得当前用户购物车
        
        $('#btnquit').click(quitAutholization);//退出当前用户
    }

    //商品列表
    function goodlist() {
        var productCustomization = $('.cd-customization'),
            cart = $('.cart'),
            animating = false;
        initCustomization(productCustomization);

        $('body').on('click', function (event) {
            //if user clicks outside the .cd-gallery list items - remove the .hover class and close the open ul.size/ul.color list elements
            if ($(event.target).is('body') || $(event.target).is('.cd-gallery')) {
                deactivateCustomization();
            }
        });

        function initCustomization(items) {
            items.each(function () {
                var actual = $(this),
                    addToCartBtn = actual.find('.add-to-cart'),
                    touchSettings = actual.next('.cd-customization-trigger');

                //绑定addtocartClick事件
                addToCartBtn.on('click', function () {
                    if (!animating) {
                        //animate if not already animating
                        animating = true;
                        resetCustomization(addToCartBtn);
                        addToCartBtn.addClass('is-added').find('path').eq(0).animate({
                            //加入购物车完成
                            'stroke-dashoffset': 0
                        }, 300, function () {
                            setTimeout(function () {

                                var cartItemId = addToCartBtn.parent().parent().parent().find('span').eq(0).text();
                                $.ajax({
                                    url: "/Store/AddToCart",
                                    type: "post",
                                    data: { "cartItemId": cartItemId },
                                    success: function (data) {
                                        if (data == "0")
                                            alert("log in first");
                                        else
                                            getCartAjax();
                                        //var item = eval("(" + data + ")");
                                        //if (!item.hasOwnProperty("num")) {
                                        //    if (data == "0") {
                                        //        alert("log in first");
                                        //    }
                                        //    else {
                                        //        var str = data.split('|');
                                        //        var Id = str[0];
                                        //        var totalPrice = str[1];
                                        //        $('.cd-cart-total span').html("$" + parseFloat(totalPrice).toFixed(2));
                                        //        var text = parseInt($('.cd-cart-items #' + Id + 'good .cd-qty').text());
                                        //        text++;
                                        //        $('.cd-cart-items #' + Id + 'good .cd-qty').text(text + 'x');
                                        //        updateCart();
                                        //    }
                                        //}
                                        //else {
                                        //    var itemName = item.product;
                                        //    var itemNum = item.num;
                                        //    var itemPrice = item.price;
                                        //    var itemId = item.productId;
                                        //    var totalPrice = item.totalPrice.toFixed(2);
                                        //    var $productli = $('.cd-cart-items').append("<li id='" + itemId + "good'><span class='cd-qty'>" + itemNum + "x</span> <span>" + itemName + "</span><div class='cd-price'>$" + itemPrice + "</div><a href='javascript:;' class='cd-item-remove cd-img-replace' >Remove</a></li>");
                                        //    $('.cd-cart-total span').html("$" + totalPrice);
                                        //    removeCartItem($("#" + itemId + "good"));//移除购物车内商品
                                        //    updateCart();
                                        //}

                                    }
                                })


                                addToCartBtn.removeClass('is-added').find('em').on('webkitTransitionEnd otransitionend oTransitionEnd msTransitionEnd transitionend', function () {
                                    //wait for the end of the transition to reset the check icon
                                    addToCartBtn.find('path').eq(0).css('stroke-dashoffset', '19.79');
                                    animating = false;
                                });
                                if ($('.no-csstransitions').length > 0) {
                                    // check if browser doesn't support css transitions
                                    addToCartBtn.find('path').eq(0).css('stroke-dashoffset', '19.79');
                                    animating = false;
                                }
                                addToCartBtn.find('path').eq(0).css('stroke-dashoffset', '19.79');
                                animating = false;
                            }, 600);
                        });
                    }
                });

                //移动端 检测按钮
                touchSettings.on('click', function (event) {
                    event.preventDefault();
                    resetCustomization(addToCartBtn);
                });
            });
        }
        //获得当前用户购物车
        function getCartAjax() {
            $('.cd-cart-items').html("");
            $.ajax({
                type: "POST",
                url: "Store/GetCart",
                success: function (data) {
                    if (data != '0') {
                        var item = eval("(" + data + ")");
                        var totalNum = 0;
                        var totalPrice = item[0].totalPrice;
                        for (var i = 0; i < item.length; i++) {
                            var itemName = item[i].product;
                            var itemNum = item[i].num;
                            totalNum += itemNum;
                            var itemId = item[i].productId;
                            var itemPrice = item[i].price;
                            var $productli = $('.cd-cart-items').append("<li id='" + itemId + "good'><span class='cd-qty'>" + itemNum + "x</span> <span>" + itemName + "</span><div class='cd-price'>$" + itemPrice + "</div><a href='javascript:;' class='cd-item-remove cd-img-replace'>Remove</a></li>");
                            $('.cd-cart-total span').html("$" + (totalPrice).toFixed(2));
                        }
                        (!$('.cart').hasClass('items-added')) && $('.cart').addClass('items-added');
                        var cartItems = $('.cart').find('span');
                        cartItems.text(totalNum);
                    }
                    else {
                        $('.cd-cart-items').html("");
                    }
                    removeCartItem();//移除购物车内商品
                }
            })
        }

        function updateSlider(actual, index) {
            var slider = actual.parent('.cd-customization').prev('a').children('.cd-slider-wrapper'),
                slides = slider.children('li');

            slides.eq(index).removeClass('move-left').addClass('selected').prevAll().removeClass('selected').addClass('move-left').end().nextAll().removeClass('selected move-left');
        }

        function resetCustomization(selectOptions) {
            //重置移动端添加按钮
            selectOptions.siblings('[data-type="select"]').removeClass('is-open').end().parents('.cd-single-item').addClass('hover').parent('li').siblings('li').find('.cd-single-item').removeClass('hover').end().find('[data-type="select"]').removeClass('is-open');
        }

        function deactivateCustomization() {
            productCustomization.parent('.cd-single-item').removeClass('hover').end().find('[data-type="select"]').removeClass('is-open');
        }

        function updateCart() {
            //显示购物车气泡 商品数量
            (!cart.hasClass('items-added')) && cart.addClass('items-added');
            var cartItems = cart.find('span'),
                text = parseInt(cartItems.text()) + 1;
            cartItems.text(text);
        }
    }

    //获得当前用户购物车
    function getCartAjax()
    {
        $.ajax({
            type: "POST",
            url: "Store/GetCart",
            success: function (data) {
                if (data != '0') {
                    var item = eval("(" + data + ")");
                    var totalNum = 0;
                    var totalPrice = item[0].totalPrice;
                    for (var i = 0; i < item.length; i++) {
                        var itemName = item[i].product;
                        var itemNum = item[i].num;
                        totalNum += itemNum;
                        var itemId = item[i].productId;
                        var itemPrice = item[i].price;
                        var $productli = $('.cd-cart-items').append("<li id='" + itemId + "good'><span class='cd-qty'>" + itemNum + "x</span> <span>" + itemName + "</span><div class='cd-price'>$" + itemPrice + "</div><a href='javascript:;' class='cd-item-remove cd-img-replace'>Remove</a></li>");
                        $('.cd-cart-total span').html("$" + (totalPrice).toFixed(2));
                    }
                    (!$('.cart').hasClass('items-added')) && $('.cart').addClass('items-added');
                    var cartItems = $('.cart').find('span');
                    cartItems.text(totalNum);
                }
                else {
                    $('.cd-cart-items').html("");
                }
                removeCartItem();//移除购物车内商品
            }
        })
    }

    //移除购物车内商品
    function removeCartItem() {
        var items = $('.cd-cart-items li');
        items.each(function () {
            $(this).find('.cd-item-remove').eq(0).on('click', function () {
                $(this).parent().fadeOut("slow");
                var itemId = parseInt($(this).parent().attr("id"));
                $.ajax({
                    type: "POST",
                    url: "Store/RemoveCartItem",
                    data: {
                        "cartItemId": itemId,
                    },
                    success: function (data) {
                        var priceCount = data.split('|')[0];
                        var numCount = data.split('|')[1];
                        var $priceCount = $('.cd-cart-total>p>span');
                        var cartItems = $('.cart').find('span');
                        $priceCount.text("$" + priceCount);
                        cartItems.text(numCount);
                        if(numCount==0)
                            $('.cart').removeClass('items-added');
                    }
                })
            })
        })
    }

    //推出当前用户
    function quitAutholization() {
        $.ajax({
            type: "post",
            url: "../UserLog/LogQuit",
            data: {},
            success: function (data) {
                window.location.reload();
            }
        });
    }

    //用户登陆验证,即session
    function autholization() {
        var $userName = $('#usershow');
        var $btnQuit = $('#btnquit');
        var $btnRegisterLi = $('#btnregister').parent().parent();
        var $btnLoginLi = $('#btnlogin').parent().parent();
        if ($userName.html() == "") {
            $btnRegisterLi.css('display', 'inline-block');
            $btnLoginLi.css('display', 'inline-block');
            $userName.css('display', 'none');
            $btnQuit.css('display', 'none');
            return false;
        }
        else {
            $btnRegisterLi.css('display', 'none');
            $btnLoginLi.css('display', 'none');
            $userName.css('display', 'inline-block');
            $btnQuit.css('display', 'inline-block');
            return true;
        }
    }

    //空项判断正则表达
    var parten = /\s+/;
    function IsNull(obj) {
        if (obj.val() == '' || obj.val() == null) {
            obj.next().remove();
            obj.parent().append("<div class='alert alert-danger'>这一项不能为空</div>");
            return true;
        }
        if (parten.test(obj.val())) {
            obj.next().remove();
            obj.parent().append("<div class='alert alert-danger'>不允许有空格</div>");
            return true;
        }
        else
            return false;
    };

    //注册表单初始化
    function registerValidation() {
        var phone_erro = 1;
        var name_erro = 1;
        var pwd_erro = 1;
        var repwd_erro = 1;
        var pwd_match = false;
        var $phone = $('#rphone');
        var $name = $('#rname');
        var $pwd = $('#rpwd');
        var $repwd = $('#rrpwd'); 
        //表单清空
        function formEmpty() {
            $phone.val('');
            $phone.next().remove();
            $name.val('');
            $name.next().remove();
            $pwd.val('');
            $pwd.next().remove();
            $repwd.val('');
            $repwd.next().remove();
        };
        //激活表单
        $('#btnregister').on('click', function () {
            formEmpty();     
        });

        //判断注册表单整体验证结果 
        function result() {
            if (phone_erro == 0 && name_erro == 0 && pwd_match == true)
                return true;
            else
                return false;
        };
        //手机号码数字限制
        $phone.keypress(function () {
            var keyCode = event.keyCode;
            if ((keyCode >= 48 && keyCode <= 57)) {
                event.returnValue = true;
            }
            else {
                event.returnValue = false;
            }
        });
        $('#lphone').keypress(function () {
            var keyCode = event.keyCode;
            if ((keyCode >= 48 && keyCode <= 57)) {
                event.returnValue = true;
            }
            else {
                event.returnValue = false;
            }
        });
        //手机号码ajax验证
        $phone.on('blur', function () {
            if (!IsNull($(this))) {
                if ($(this).val().length <= 10 && $(this).val().length >= 1) {
                    $(this).next().remove();
                    $(this).parent().append("<div  class='alert alert-danger'>手机号码长度过短</div>");
                    phone_erro = 1;
                }
                else if ($(this).val().length == 11) {

                    $.ajax({
                        type: "POST",
                        url: "../UserRegister/IsExist",
                        data: { "phonenum": $phone.val() },
                        success: function (data) {
                            if (data == "1") {
                                $phone.next().remove();
                                phone_erro = 0;
                                $phone.parent().append("<div class='alert alert-success'>填写正确</div>");
                            }
                            if (data == "0") {
                                $phone.next().remove();
                                $phone.parent().append("<div class='alert alert-danger'>账号已存在</div>");
                                phone_erro = 1;
                            }
                        }
                    })
                }
            }
        });
        //姓名非空
        $name.on('blur', function () {
            if (!IsNull($(this))) {
                $(this).next().remove();
                $(this).parent().append("<div  class='alert alert-success'>填写正确</div>");
                name_erro = 0;
            }
        });
        //两次密码检测
        function pwd_compare() {
            if (($pwd.val() == $repwd.val()) && pwd_erro == 0 && repwd_erro == 0) {
                if ($repwd.val().length >= 6) {
                    repwd_erro = 0;
                    return true;
                }  
            }
            if ($pwd.val() != $repwd.val()) {
                if ($repwd.val() != "") {
                    $repwd.next().remove();
                    $repwd.parent().append("<div class='alert alert-danger'>两次密码不相同</div>");
                    return false;
                }
            }
        };
        $pwd.on('blur', function () {
            if (!IsNull($(this))) {
                if ($(this).val().length >= 1 && $(this).val().length < 6) {
                    $(this).next().remove();
                    $(this).parent().append("<div class='alert alert-danger'>密码长度需要大于6位小于等于16位</div>");
                    pwd_erro = 1;
                }
                else {
                    $(this).next().remove();
                    $(this).parent().append("<div  class='alert alert-success'>填写正确</div>");
                    pwd_erro = 0;
                } 
            }
            pwd_match = pwd_compare();
        });
        $repwd.on('blur', function () {
            if ($repwd.val().length >= 6) {
                if ($pwd.val() == $repwd.val()) {
                    repwd_erro = 0;
                }
            }
            pwd_match = pwd_compare();
        });
        //注册按钮触发表单提交
        $('#register').on('click', function () {
            $('#temp1').focus();

            if (IsNull($phone))
                phone_erro = 1;
            if (IsNull($name))
                name_erro = 1;
            if (IsNull($pwd))
                pwd_erro = 1;
            if (IsNull($repwd))
                repwd_erro = 1;
            if (result()) {
                $('#register_form').submit();
                //location.href = "../../Store/Index";
            }
        });
    }
    
    //登陆表单初始化
    function loginValidation() {
        var $phone2 = $('#lphone');
        var $pwd2 = $('#pwd');
        var pwd2_erro = 1;
        var phone2_erro = 1;
        //登陆表单清空
        $('#btnlogin').on('click', function () {
            $phone2.val('');
            $phone2.next().remove();
            $pwd2.val('');
            $pwd2.next().remove();
        });

        //登陆表单验证
        function result2() {
            if (pwd2_erro == 0 && phone2_erro == 0)
                return true;
            else
                return false;
        };
        $phone2.on('blur', function () {
            if (!IsNull($(this))) {
                if ($(this).val().length < 11) {
                    $(this).next().remove();
                    $(this).parent().append("<div  class='alert alert-danger'>请填写正确的手机号</div>");
                    phone2_erro = 1;
                }
                else if ($(this).val().length == 11) {
                    $(this).next().remove();
                    phone2_erro = 0;
                }
            }
        });
        $pwd2.on('blur', function () {
            if (!IsNull($(this))) {
                $(this).next().remove();
                pwd2_erro = 0;
            }
        });
        //按钮触发账号密码验证
        $('#login').on('click', function () {
            $('#temp2').focus();
            if (IsNull($phone2))
                phone2_erro = 1;
            else
                phone2_erro = 0;
            if (IsNull($pwd2))
                pwd2_erro = 1;
            else
                pwd2_erro = 0;
            if (result2()) {
                $.ajax({
                    type: "POST",
                    url: "/UserLog/LogValidation",
                    data: {
                        "phonenum": $phone2.val(),
                        "password": $pwd2.val()
                    },
                    success: function (data) {
                        if (data == "1") {
                            window.location.reload();
                        }
                        if (data == "0") {
                            alert("账号密码不正确,请重新输入");
                            //$phone.next().remove();
                            //$phone.parent().append("<div class='alert alert-danger'>账号已存在</div>");
                            //phone_erro = 1;
                        }
                    }
                })
            }
        });
    }

       
});
