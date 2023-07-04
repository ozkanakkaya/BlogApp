$(document).ready(function () {

    /* DataTables start here. */

    const dataTable = $('#usersTable').DataTable({
        dom:
            "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
            {
                text: 'Ekle',
                attr: {
                    id: "btnAdd",
                },
                className: 'btn btn-success',
                action: function (e, dt, node, config) {
                }
            },
            {
                text: 'Aktif Kullanıcılar',
                className: 'btn btn-warning btn-users',
                action: function (e, dt, node, config) {
                    $.ajax({
                        type: 'GET',
                        url: '/Admin/User/GetAllByActive/',
                        contentType: "application/json",
                        beforeSend: function () {
                            $('#usersTable').hide();
                            $('.spinner-border').show();
                        },
                        success: function (data) {
                            const response = jQuery.parseJSON(data);
                            dataTable.clear();
                            console.log(response);
                            if (!response.error) {
                                $.each(response.$values,
                                    function (index, user) {
                                        const newTableRow = dataTable.row.add([
                                            user.Id,
                                            user.Username,
                                            user.Email,
                                            user.Firstname,
                                            user.Lastname,
                                            `<img src="/img/${user.ImageUrl}" alt="${user.Username}" class="my-image-table" />`,
                                            `
                                <button class="btn btn-info btn-sm btn-detail" data-id="${user.Id}"><span class="fas fa-newspaper"></span></button>
                                <button class="btn btn-warning btn-sm btn-assign" data-id="${user.Id}"><span class="fas fa-user-shield"></span></button>
                                <button class="btn btn-primary btn-sm btn-update" data-id="${user.Id}"><span class="fas fa-edit"></span></button>
                                <button class="btn btn-danger btn-sm btn-delete" data-id="${user.Id}"><span class="fas fa-minus-circle"></span></button>
                                            `
                                        ]).node();
                                        const jqueryTableRow = $(newTableRow);
                                        jqueryTableRow.attr('name', `${user.Id}`);
                                    });
                                dataTable.draw();
                                $('.spinner-border').hide();
                                $('#usersTable').fadeIn(1400);
                            } else {
                                toastr.warning(`${response.error}`, 'Kayıt Bulunamadı!');
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            $('.spinner-border').hide();
                            $('#usersTable').fadeIn(1000);
                            toastr.error(`${err.responseText}`, 'Hata!');
                        }
                    });
                }
            },
            {
                text: 'Onay Bekleyenler',
                className: 'btn btn-secondary btn-users',
                action: function (e, dt, node, config) {
                    $.ajax({
                        type: 'GET',
                        url: '/Admin/User/GetAllByInactive/',
                        contentType: "application/json",
                        beforeSend: function () {
                            $('#usersTable').hide();
                            $('.spinner-border').show();
                        },
                        success: function (data) {
                            const response = jQuery.parseJSON(data);
                            dataTable.clear();
                            console.log(response);
                            if (!response.error) {
                                $.each(response.$values,
                                    function (index, user) {
                                        const newTableRow = dataTable.row.add([
                                            user.Id,
                                            user.Username,
                                            user.Email,
                                            user.Firstname,
                                            user.Lastname,
                                            `<img src="/img/${user.ImageUrl}" alt="${user.Username}" class="my-image-table" />`,
                                            `
                                <button class="btn btn-success btn-sm btn-activate" data-id="${user.Id}"><span class="fas fa-check"></span></button>
                                <button class="btn btn-info btn-sm btn-detail" data-id="${user.Id}"><span class="fas fa-newspaper"></span></button>
                                <button class="btn btn-warning btn-sm btn-assign" data-id="${user.Id}"><span class="fas fa-user-shield"></span></button>
                                <button class="btn btn-primary btn-sm btn-update" data-id="${user.Id}"><span class="fas fa-edit"></span></button>
                                <button class="btn btn-danger btn-sm btn-delete" data-id="${user.Id}"><span class="fas fa-minus-circle"></span></button>
                                            `
                                        ]).node();
                                        const jqueryTableRow = $(newTableRow);
                                        jqueryTableRow.attr('name', `${user.Id}`);
                                    });
                                dataTable.draw();
                                $('.spinner-border').hide();
                                $('#usersTable').fadeIn(1400);
                            } else {
                                toastr.warning(`${response.error}`, 'Kayıt Bulunamadı!');
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            $('.spinner-border').hide();
                            $('#usersTable').fadeIn(1000);
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

    //fo Table Name
    $('.btn-users').on('click', function () {
        var buttonText = $(this).text();
        $('.card-header').html('<i class="fas fa-table mr-1"></i>' + buttonText);
    });

    /* Ajax GET / Getting the _UserAddPartial as Modal Form starts from here. */

    $(function () {
        const url = '/Admin/User/Add/';
        const placeHolderDiv = $('#modalPlaceHolder');
        $('#btnAdd').click(function () {
            $.get(url).done(function (data) {
                placeHolderDiv.html(data);
                placeHolderDiv.find(".modal").modal('show');
            });
        });

        /* Ajax GET / Getting the _UserAddPartial as Modal Form ends here. */

        /* Ajax POST / Posting the FormData as UserAddDto starts from here. */

        placeHolderDiv.on('click',
            '#btnSave',
            function (event) {
                event.preventDefault();
                const form = $('#form-user-add');
                const actionUrl = form.attr('action');
                const dataToSend = new FormData(form.get(0));
                $.ajax({
                    url: actionUrl,
                    type: 'POST',
                    data: dataToSend,
                    processData: false,
                    contentType: false,
                    success: function (data) {
                        console.log(data);
                        const userAddAjaxModel = jQuery.parseJSON(data);
                        console.log(userAddAjaxModel);
                        const newFormBody = $('.modal-body', userAddAjaxModel.UserAddPartial);
                        placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                        const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                        if (isValid) {
                            placeHolderDiv.find('.modal').modal('hide');
                            const newTableRow = dataTable.row.add([
                                userAddAjaxModel.UserViewModel.UserDto.Id,
                                userAddAjaxModel.UserViewModel.UserDto.Username,
                                userAddAjaxModel.UserViewModel.UserDto.Email,
                                userAddAjaxModel.UserViewModel.UserDto.Firstname,
                                userAddAjaxModel.UserViewModel.UserDto.Lastname,
                                `<img src="/img/${userAddAjaxModel.UserViewModel.UserDto.ImageUrl}" alt="${userAddAjaxModel.UserViewModel.UserDto.Username}" class="my-image-table" />`,
                                `
                                <button class="btn btn-info btn-sm btn-detail" data-id="${userAddAjaxModel.UserViewModel.UserDto.Id}"><span class="fas fa-newspaper"></span></button>
                                <button class="btn btn-warning btn-sm btn-assign" data-id="${userAddAjaxModel.UserViewModel.UserDto.Id}"><span class="fas fa-user-shield"></span></button>
                                <button class="btn btn-primary btn-sm btn-update" data-id="${userAddAjaxModel.UserViewModel.UserDto.Id}"><span class="fas fa-edit"></span></button>
                                <button class="btn btn-danger btn-sm btn-delete" data-id="${userAddAjaxModel.UserViewModel.UserDto.Id}"><span class="fas fa-minus-circle"></span></button>
                            `
                            ]).node();
                            const jqueryTableRow = $(newTableRow);
                            jqueryTableRow.attr('name', `${userAddAjaxModel.UserViewModel.UserDto.Id}`);
                            dataTable.row(newTableRow).draw();
                            toastr.success(`${userAddAjaxModel.UserViewModel.Message}`, 'Başarılı İşlem!');
                        } else {
                            let summaryText = "";
                            $('#validation-summary > ul > li').each(function () {
                                let text = $(this).text();
                                summaryText += `*${text}<br/>`;
                            });
                            toastr.error(summaryText);
                        }
                    },
                    error: function (err) {
                        console.log(err);
                        toastr.error(`${err.responseText}`, 'Hata!');
                    }
                });
            });
    });

    /* Ajax POST / Posting the FormData as UserAddDto ends here. */

    /* Ajax POST / Deleting a User starts from here */

    $(document).on('click',
        '.btn-delete',
        function (event) {
            event.preventDefault();
            const id = $(this).attr('data-id');
            const tableRow = $(`[name="${id}"]`);
            const userName = tableRow.find('td:eq(1)').text();
            Swal.fire({
                title: 'Silmek istediğinize emin misiniz?',
                text: `${userName} adlı kullanıcı silinicektir!`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Evet, silmek istiyorum.',
                cancelButtonText: 'Hayır, silmek istemiyorum.'
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        type: 'POST',
                        dataType: 'json',
                        data: { userId: id },
                        url: '/Admin/User/Delete/',
                        success: function (data) {
                            const deletedUserModel = jQuery.parseJSON(data);
                            if (deletedUserModel.ResultStatus === 200) {
                                Swal.fire(
                                    'Silindi!',
                                    `${deletedUserModel.Message}`,
/*                                    `${deletedUserModel.UserDto.Username} adlı kullanıcı başarıyla silinmiştir.`,*/
                                    'success'
                                );

                                dataTable.row(tableRow).remove().draw();
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Başarısız İşlem!',
                                    text: `${deletedUserModel.Message}`,
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

    /* Ajax GET / Getting the _UserUpdatePartial as Modal Form starts from here. */

    $(function () {
        const url = '/Admin/User/Update/';
        const placeHolderDiv = $('#modalPlaceHolder');
        $(document).on('click',
            '.btn-update',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                $.get(url, { userId: id }).done(function (data) {
                    placeHolderDiv.html(data);
                    placeHolderDiv.find('.modal').modal('show');
                }).fail(function (err) {
                    toastr.error(`${err.responseText}`, 'Hata!');
                });
            });

        /* Ajax POST / Updating a User starts from here */

        placeHolderDiv.on('click',
            '#btnUpdate',
            function (event) {
                event.preventDefault();
                const form = $('#form-user-update');
                const actionUrl = form.attr('action');
                const dataToSend = new FormData(form.get(0));
                $.ajax({
                    url: actionUrl,
                    type: 'POST',
                    data: dataToSend,
                    processData: false,
                    contentType: false,
                    success: function (data) {
                        const userUpdateAjaxModel = jQuery.parseJSON(data);
                        console.log(userUpdateAjaxModel);
                        const newFormBody = $('.modal-body', userUpdateAjaxModel.UserUpdatePartial);
                        placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                        const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                        if (isValid) {
                            const id = userUpdateAjaxModel.UserViewModel.UserDto.Id;
                            const tableRow = $(`[name="${id}"]`);
                            placeHolderDiv.find('.modal').modal('hide');
                            dataTable.row(tableRow).data([
                                userUpdateAjaxModel.UserViewModel.UserDto.Id,
                                userUpdateAjaxModel.UserViewModel.UserDto.Username,
                                userUpdateAjaxModel.UserViewModel.UserDto.Email,
                                userUpdateAjaxModel.UserViewModel.UserDto.Firstname,
                                userUpdateAjaxModel.UserViewModel.UserDto.Lastname,
                                `<img src="/img/${userUpdateAjaxModel.UserViewModel.UserDto.ImageUrl}" alt="${userUpdateAjaxModel.UserViewModel.UserDto.Username}" class="my-image-table" />`,
                                `
                                <button class="btn btn-info btn-sm btn-detail" data-id="${userUpdateAjaxModel.UserViewModel.UserDto.Id}"><span class="fas fa-newspaper"></span></button>
                                <button class="btn btn-warning btn-sm btn-assign" data-id="${userUpdateAjaxModel.UserViewModel.UserDto.Id}"><span class="fas fa-user-shield"></span></button>
                                <button class="btn btn-primary btn-sm btn-update" data-id="${userUpdateAjaxModel.UserViewModel.UserDto.Id}"><span class="fas fa-edit"></span></button>
                                <button class="btn btn-danger btn-sm btn-delete" data-id="${userUpdateAjaxModel.UserViewModel.UserDto.Id}"><span class="fas fa-minus-circle"></span></button>
                            `
                            ]);
                            tableRow.attr("name", `${id}`);
                            dataTable.row(tableRow).invalidate();
                            toastr.success(`${userUpdateAjaxModel.UserViewModel.Message}`, "Başarılı İşlem!");
                        } else {
                            let summaryText = "";
                            $('#validation-summary > ul > li').each(function () {
                                let text = $(this).text();
                                summaryText += `*${text}<br/>`;
                            });
                            toastr.error(summaryText);
                        }
                    },
                    error: function (error) {
                        console.log(error);
                        toastr.error(`${err.responseText}`, 'Hata!');
                    }
                });
            });

    });
    // Get Detail Ajax Operation

    $(function () {

        const url = '/Admin/User/GetDetail/';
        const placeHolderDiv = $('#modalPlaceHolder');
        $(document).on('click',
            '.btn-detail',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                $.get(url, { userId: id }).done(function (data) {
                    placeHolderDiv.html(data);
                    placeHolderDiv.find('.modal').modal('show');
                }).fail(function (err) {
                    toastr.error(`${err.responseText}`, 'Hata!');
                });
            });

    });

    $(function () {
        const url = '/Admin/Role/Assign/';
        const placeHolderDiv = $('#modalPlaceHolder');
        $(document).on('click',
            '.btn-assign',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                $.get(url, { userId: id }).done(function (data) {
                    placeHolderDiv.html(data);
                    placeHolderDiv.find('.modal').modal('show');
                }).fail(function (err) {
                    console.log(err);
                    toastr.error(`${err.responseText}`, 'Hata!');
                });
            });

        /* Ajax PUT / Updating a RoleAssign starts from here */

        placeHolderDiv.on('click',
            '#btnAssign',
            function (event) {
                event.preventDefault();
                const form = $('#form-role-assign');
                const actionUrl = form.attr('action');
                const dataToSend = new FormData(form.get(0));
                $.ajax({
                    url: actionUrl,
                    type: 'PUT',
                    data: dataToSend,
                    processData: false,
                    contentType: false,
                    success: function (data) {
                        const userRoleAssignAjaxModel = jQuery.parseJSON(data);
                        console.log(userRoleAssignAjaxModel);
                        const newFormBody = $('.modal-body', userRoleAssignAjaxModel.RoleAssignPartial);
                        placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                        const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                        if (isValid) {
                            const id = userRoleAssignAjaxModel.UserViewModel.UserDto.Id;
                            const tableRow = $(`[name="${id}"]`);
                            placeHolderDiv.find('.modal').modal('hide');
                            toastr.success(`${userRoleAssignAjaxModel.UserViewModel.Message}`, "Başarılı İşlem!");
                        } else {
                            let summaryText = "";
                            $('#validation-summary > ul > li').each(function () {
                                let text = $(this).text();
                                summaryText += `*${text}<br/>`;
                            });
                            toastr.warning(summaryText);
                        }
                    },
                    error: function (error) {
                        console.log(error);
                        toastr.error(`${err.responseText}`, 'Hata!');
                    }
                });
            });

    });

    /* Ajax POST / Updating a Activate starts from here */

    $(document).on('click',
        '.btn-activate',
        function (event) {
            event.preventDefault();
            const id = $(this).attr('data-id');
            const tableRow = $(`[name="${id}"]`);
            const userName = tableRow.find('td:eq(1)').text();
            Swal.fire({
                title: 'Kullanıcıyı aktif etmek istediğinize emin misiniz?',
                text: `${userName} adlı kullanıcı aktif edilecektir!`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Evet, aktif etmek istiyorum.',
                cancelButtonText: 'Hayır, aktif etmek istemiyorum.'
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        type: 'PUT',
                        dataType: 'json',
                        data: { userId: id },
                        url: '/Admin/User/ActivateUser/',
                        success: function (data) {
                            const response = jQuery.parseJSON(data);
                            if (!response.error) {
                                Swal.fire(
                                    'Aktif Edildi!',
                                    `${response.Username} adlı kullanıcı başarıyla aktif edilmiştir.`,
                                    'success'
                                );

                                dataTable.row(tableRow).remove().draw();
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Başarısız İşlem!',
                                    text: `${response.error}`,
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
});
$(document).on('mouseover', '.btn-detail', function () {
    this.title = 'Detay';
});

$(document).on('mouseover', '.btn-assign', function () {
    this.title = 'Rol Atama';
});

$(document).on('mouseover', '.btn-update', function () {
    this.title = 'Güncelle';
});

$(document).on('mouseover', '.btn-delete', function () {
    this.title = 'Sil';
});

$(document).on('mouseover', '.btn-activate', function () {
    this.title = 'Aktif Et';
});