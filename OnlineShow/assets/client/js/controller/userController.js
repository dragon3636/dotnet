var user = {
    init: function () {
        user.loadProvince();
        user.registerEvent()
    },
    registerEvent: function () {
        $('#ddlprovince').off('channge').on('change', function () {
            var id = $(this).val();
            if (id != '') {
                user.loadDistrict(parseInt(id));
            } else {
                $('#ddlprovince').html('');
            }
        })
    },
    loadProvince: function () {

        $.ajax({
            url: '/User/LoadProvince',
            type: 'POST',
            datatype: 'JSON',
            success: function (response) {
                if (response.status == true) {
                    var html = '<option value="">--Chọn tỉnh thành--</option>';
                    var data = response.data;
                    $.each(data, function (i, item) {
                        html += '<option value ="' + item.ID + '">' + item.Name + '</option>';
                    });
                    $('#ddlprovince').html(html);

                }
            }
        })
    },
    loadDistrict: function (id) {

        $.ajax({
            url: '/User/LoadDistrict',
            type: 'POST',
            datatype: 'JSON',
            data: { provinceID: id },
            success: function (response) {
                if (response.status == true) {
                    var html = '<option value="">--Chọn Quận/Huyện- Xã/Phường--</option>';
                    var data = response.data;
                    $.each(data, function (i, item) {
                        html += '<option value ="' + item.ID + '">' + item.Name + '</option>';
                    });
                    $('#ddldistrict').html(html);

                }
            }
        })
    }

}
user.init();
