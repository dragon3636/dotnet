var feedback = {
    init: function () {
        feedback.registerEvents()
    },
    registerEvents: function () {
        $('#btnSend').off('click').on('click', function () {
            var name = $('#txtName').val();
            var mobile = $('#txtMobile').val();
            var address = $('#txtAddress').val();
            var email = $('#txtEmail').val();
            var content = $('#txtContent').val();
            $.ajax({
                url: '/Contact/Send',
                type: 'post',
                datatype: 'json',
                data: {
                    name: name,
                    mobile: mobile,
                    address: address,
                    email: email,
                    content:content
                },
                success: function(res){
                    if (res==true) {
                        window.alert('Gữi thành công ')
                    }
            }
            })
        });
    }

}
feedback.init();