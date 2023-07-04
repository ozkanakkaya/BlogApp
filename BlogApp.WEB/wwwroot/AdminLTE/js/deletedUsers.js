$(document).ready(function () {

    /* DataTables start here. */

    const dataTable = $('#deletedUsersTable').DataTable({
        dom:
            "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
            {
                text: 'Yenile',
                className: 'btn btn-warning',
                action: function (e, dt, node, config) {
                    $.ajax({
                        type: 'GET',
                        url: '/Admin/User/GetAllDeletedUsers/',
                        contentType: "application/json",
                        beforeSend: function () {
                            $('#deletedUsersTable').hide();
                            $('.spinner-border').show();
                        },
                        success: function (data) {
                            const deletedUsers = jQuery.parseJSON(data);
                            dataTable.clear();
                            console.log(deletedUsers);
                            if (deletedUsers.ResultStatus === 200) {
                                $.each(deletedUsers.UserListDto,
                                    function (index, user) {
                                        const newTableRow = dataTable.row.add([
                                            user.Id,
                                            user.Username,
                                            user.Email,
                                            user.Firstname,
                                            user.Lastname,
                                            `<img src="/img/${user.ImageUrl}" alt="${user.Username}" class="my-image-table" />`,
                                            `
                                <button class="btn btn-warning btn-sm btn-undo" data-id="${user.Id}"><span class="fas fa-undo"></span></button>
                                <button class="btn btn-danger btn-sm btn-delete" data-id="${user.Id}"><span class="fas fa-minus-circle"></span></button>
                            `
                                        ]).node();
                                        const jqueryTableRow = $(newTableRow);
                                        jqueryTableRow.attr('name', `${user.Id}`);
                                    });
                                dataTable.draw();
                                $('.spinner-border').hide();
                                $('#deletedUsersTable').fadeIn(1400);
                            } else {
                                toastr.warning(`${deletedUsers.Message}`, 'Kayıt Bulunamadı!');
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            $('.spinner-border').hide();
                            $('#deletedUsersTable').fadeIn(1000);
                            toastr.error(`${err.responseText}`, 'Hata!');
                        }
                    });
                }
            }
        ],
        language: {
            "sDecimal": ",",
            "sEmptyTable": "Tabloda herhangi bir veri mevcut değil",
            "sInfo": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
            "sInfoEmpty": "Kayıt yok",
            "sInfoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "Sayfada _MENU_ kayıt göster",
            "sLoadingRecords": "Yükleniyor...",
            "sProcessing": "İşleniyor...",
            "sSearch": "Ara:",
            "sZeroRecords": "Eşleşen kayıt bulunamadı",
            "oPaginate": {
                "sFirst": "İlk",
                "sLast": "Son",
                "sNext": "Sonraki",
                "sPrevious": "Önceki"
            },
            "oAria": {
                "sSortAscending": ": artan sütun sıralamasını aktifleştir",
                "sSortDescending": ": azalan sütun sıralamasını aktifleştir"
            },
            "select": {
                "rows": {
                    "_": "%d kayıt seçildi",
                    "0": "",
                    "1": "1 kayıt seçildi"
                }
            }
        }
    });

    /* DataTables end here */

    /* UndoDelete */

    $(document).on('click',
        '.btn-undo',
        function (event) {
            event.preventDefault();
            const id = $(this).attr('data-id');
            const tableRow = $(`[name="${id}"]`);
            let username = tableRow.find('td:eq(1)').text();
            Swal.fire({
                title: 'Arşivden geri getirmek istediğinize emin misiniz?',
                text: `${username} adlı kullanıcı arşivden geri getirilecektir.`,
                icon: 'question',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Evet, arşivden geri getirmek istiyorum.',
                cancelButtonText: 'Hayır, istemiyorum.'
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        type: 'PUT',
                        dataType: 'json',
                        data: { userId: id },
                        url: '/Admin/User/UndoDelete/',
                        success: function (data) {
                            const undoDeletedUsersResult = jQuery.parseJSON(data);
                            console.log(undoDeletedUsersResult);
                            if (undoDeletedUsersResult.ResultStatus === 200) {
                                Swal.fire(
                                    'Arşivden Geri Getirildi!',
                                    `${undoDeletedUsersResult.Message}`,
                                    'success'
                                );

                                dataTable.row(tableRow).remove().draw();
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Başarısız İşlem!',
                                    text: `${undoDeletedUsersResult.Message}`,
                                });
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            toastr.error(`${err.responseText}`, "Hata!");
                        }
                    });
                }
            });
        });
    /* UndoDelete */



    /* Hard Delete */

    $(document).on('click',
        '.btn-delete',
        function (event) {
            event.preventDefault();
            const id = $(this).attr('data-id');
            const tableRow = $(`[name="${id}"]`);
            let username = tableRow.find('td:eq(1)').text();
            Swal.fire({
                title: 'Kalıcı olarak silmek istediğinize emin misiniz?',
                text: `${username} adlı kullanıcı kalıcı olarak silinecektir!`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Evet, kalıcı olarak silmek istiyorum.',
                cancelButtonText: 'Hayır, istemiyorum.'
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        type: 'POST',
                        dataType: 'json',
                        data: { userId: id },
                        url: '/Admin/User/HardDelete/',
                        success: function (data) {
                            const hardDeleteResult = jQuery.parseJSON(data);
                            console.log(hardDeleteResult);
                            if (hardDeleteResult.ResultStatus === 200) {
                                Swal.fire(
                                    'Kalıcı olarak silindi!',
                                    `${hardDeleteResult.Message}`,
                                    'success'
                                );

                                dataTable.row(tableRow).remove().draw();
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Başarısız İşlem!',
                                    text: `${hardDeleteResult.Message}`,
                                });
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            toastr.error(`${err.responseText}`, "Hata!");
                        }
                    });
                }
            });
        });
    /* HardDelete */

});

$(document).on('mouseover', '.btn-undo', function () {
    this.title = 'Geri Al';
});

$(document).on('mouseover', '.btn-delete', function () {
    this.title = 'Tamamen Sil';
});