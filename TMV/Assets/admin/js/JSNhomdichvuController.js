﻿var config = {
    page: 1,
    filter: ''
};
var NhomdichvuController = {
    init: function () {
        NhomdichvuController.registerEvent();
    },
    registerEvent: function () {

        //delete
        $('.btn-delete').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);

            var id = btn.data('id');

            if (confirm("Bạn thực sự muốn xóa nhóm dịch vụ này?")) {
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Nhomdichvu/Delete',
                    data: { id: id },
                    dataType: 'json',
                    success: function (response) {
                        if (response) {
                            $('#row_' + id).remove();
                        }
                    },
                    error: function (response) {
                        alert(response.message);
                    }
                });
            };
        });
    },
};
NhomdichvuController.init();