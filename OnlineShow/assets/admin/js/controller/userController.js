var user = {
    init:function (){
        user.registerEvent()
    },
    registerEvent: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            $.ajax(
                {
                    url:"/Admin/User/ChangStatus",
                    type: "POST",
                    dataType: "json",
                    data: { id:id },
                    success: function (response) {
                        console.log(response);
                        if (response.status == true) {
                            $(this).text('Kích hoạt');
                           
                        }
                        else {
                            $(this).text('Khóa');
                        }
                    }
                })
            
        });
    }
}
user.init();