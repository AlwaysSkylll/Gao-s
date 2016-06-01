//显示遮罩层
function showOverlay() {
    $('.overlay').height($(window).height());
    $('.overlay').width($(window).width());
    $('.overlay').fadeTo(200, 0.5);
}
//隐藏遮罩层
function hideOverlay() {
    $('.overlay').fadeOut();
}
//遮罩背景点击
function bg_click() {
    $('.addr-list').fadeOut();
    $('.new-addr').fadeOut();
    setTimeout("hideOverlay()", 200);
}
//显示loading遮罩层
function showLoading() {
    $('.loading').height($(window).height());
    $('.loading').width($(window).width());
    $('.loading').fadeTo(200, 0.5);
};
//隐藏loading遮罩层
function hideLoading() {
    $('.loading').fadeOut();
}
//当前地址点击
function addr_blank_click() {
    $('.addr-list').css('display', 'block');
    showOverlay();
    setTimeout("$('.addr-list').fadeIn()", 200);
}
//新建地址/编辑地址弹出层
function addr_new_show() {
    $('.addr-list').fadeOut();
    setTimeout("$('.new-addr').fadeIn()", 200);
}
//提交新建地址
function addr_btn(num) {
    //num=0时适用地址管理显示全部地址信息,num=1时适用在订单页面地址选择copy，
    $provin = $('#s_province').val();
    $city = $('#s_city').val();
    $county = $('#s_county').val();
    $detail = $('#s_detail').val();
    $tel = $('#s_tel').val();
    $name = $('#s_name').val();
    if ($provin != "省份" && $city != "地级市" && $county != "市、县级市" && $detail != "" && $tel != "" && $name != "") {
        $.post("NewAddress", {
            "Name": $name,
            "Phone": $tel,
            "Detail": $provin + $city + $county + $detail
        },
            function (data) {
                addr_load(num);
                $('.new-addr').fadeOut();
                if(num==0)
                    hideOverlay();
                setTimeout("$('.addr-list').fadeIn()", 200);
                $('.new-addr ')[0].reset();
            });
    }
    else
        alert("您的输入有误");
}
//加载地址列表
function addr_load(num) {
   
    $.getJSON("GetAddress", function (data) {
        var $addrList = $('.addr-list ul');
        $addrList.html("");
        //num=0时适用地址管理显示全部地址信息
        if (num == 0) {
            for (var i = 0; i < data.length; i++) {
                $addrList.append('<li><div><span class="addrId">' + data[i].AddressId + '</span><span>收件人：</span><span>' + data[i].Name + '</span></div><div><span>收货地址：</span><span>' + data[i].Detail + '</span></div><div><span>联系方式：</span><span>' + data[i].Phone + '</span></div><a class="delete" href="javascript:;">删除</a><a class="modify" href="javascript:;">修改</a><hr/></li>');
            }
            $('.delete').on('click', function () {
                var thisrow = $(this).parent();
                //alert(thisrow.find('span').eq(0).html());
                $(this).parent().fadeOut();
                delete_address(thisrow.find('span').eq(0).html());
            });
        }
        //num=1时适用在订单页面地址选择copy
        if (num == 1) {
            for (var i = 0; i < data.length; i++) {
                $addrList.append('<li><span>' + data[i].Phone + '</span>&nbsp<span>' + data[i].Name + '</span><span>' + data[i].Detail + '</span><span class="addrId">' + data[i].AddressId + '</span><a class="delete" href="javascript:;">删除</a><a class="modify" href="javascript:;">修改</a></li>');
            }
            $addrList.find('span').each(function () {
                $(this).on('click', function () {
                    $('.addr-show').html("");
                    $('.addr-show').append('<ul><li><span class="addrId">' + $(this).parent().find('span').eq(3).html() + '</span><span>收件人：</span><span>' + $(this).parent().find('span').eq(1).html() + '</span></li><li><span>收货地址：</span><span>' + $(this).parent().find('span').eq(2).html() + '</span></li><li><span>联系方式：</span><span>' + $(this).parent().find('span').eq(0).html() + '</span></li></ul>');
                    bg_click();
                });
            });
            $('.delete').on('click', function () {
                var thisindex = $(this).index();
                var thisrow = $(this).parent();
                //alert(thisrow.find('span').eq(thisindex - 1).html());
                $(this).parent().fadeOut();
                delete_address(thisrow.find('span').eq(thisindex - 1).html());
            });
        }
        
    });
}
//删除指定id地址信息
function delete_address(addrid) {
    $.post("DeleteAddress", {"addrid":addrid});
}
//取消订单
function cancel_order() {
    window.location.href = 'Index';
}