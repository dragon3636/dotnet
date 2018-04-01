//hàm chưa chay đc
var category = {
    init:function(){
        category.registryEvent();
    },
    registryEvent: function(){
        $('.btn-delete').off('click').on('click', function (e) {
            
            var id = this.data('id');
            $.ajax({
                url: "/Admin/Category",
                type: "post",
                success:function(resport){
                    location.reload();
                },
                data: { id: id }

            })
        })
    }
}
category.init()