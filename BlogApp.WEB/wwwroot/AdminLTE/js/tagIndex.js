$(document).ready(function () {

    /* DataTables start here. */

    const dataTable = $('#tagsTable').DataTable({
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
                        url: '/Admin/Tag/GetAllTags/',
                        contentType: "application/json",
                        beforeSend: function () {
                            $('#tagsTable').hide();
                            $('.spinner-border').show();
                        },
                        success: function (data) {
                            const tagResult = jQuery.parseJSON(data);
                            dataTable.clear();
                            console.log(tagResult);
                            if (!tagResult.error) {
                                $.each(tagResult.Tags.$values,
                                    function (index, tag) {
                                        const newTableRow = dataTable.row.add([
                                            tag.Id,
                                            tag.Name,
                                            tag.Description,
                                            tag.IsActive ? "Evet" : "Hayır",
                                            tag.IsDeleted ? "Evet" : "Hayır",
                                            convertToShortDate(tag.CreatedDate),
                                            tag.CreatedByUsername,
                                            convertToShortDate(tag.UpdatedDate),
                                            tag.UpdatedByUsername,
                                            `
                                <button class="btn btn-primary btn-sm btn-update" data-id="${tag.Id}"><span class="fas fa-edit"></span></button>
                                <button class="btn btn-danger btn-sm btn-delete" data-id="${tag.Id}"><span class="fas fa-minus-circle"></span></button>
                                            `
                                        ]).node();
                                        const jqueryTableRow = $(newTableRow);
                                        jqueryTableRow.attr('name', `${tag.Id}`);
                                    });
                                dataTable.draw();
                                $('.spinner-border').hide();
                                $('#tagsTable').fadeIn(1400);
                            } else {
                                toastr.error(`${tagResult.error}`, 'İşlem Başarısız!');
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            $('.spinner-border').hide();
                            $('#tagsTable').fadeIn(1000);
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




    $(document).on('click',
        '.btn-delete',
        function (event) {
            event.preventDefault();
            const id = $(this).attr('data-id');
            const tableRow = $(`[name="${id}"]`);
            const tagName = tableRow.find('td:eq(1)').text();
            Swal.fire({
                title: 'Silmek istediğinize emin misiniz?',
                text: `${tagName} adlı etiket silinicektir!`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Evet, silmek istiyorum.',
                cancelButtonText: 'Hayır, silmek istemiyorum.'
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        type: 'PUT',
                        dataType: 'json',
                        data: { tagId: id },
                        url: '/Admin/Tag/Delete/',
                        success: function (data) {
                            const result = jQuery.parseJSON(data);
                            if (result.ResultStatus === 200) {
                                Swal.fire(
                                    'Silindi!',
                                    `${result.Message}`,
                                    'success'
                                );
                                dataTable.row(tableRow).remove().draw();
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Başarısız İşlem!',
                                    text: `${result.Message}`,
                                });
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            toastr.error(`${err.responseText}`, "Hata!")
                        }
                    });
                }
            });
        });

    /* Ajax GET / Getting the _CategoryUpdatePartial as Modal Form starts from here. */

    $(function () {
        const url = '/Admin/Tag/Update/';
        const placeHolderDiv = $('#modalPlaceHolder');
        $(document).on('click',
            '.btn-update',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                $.get(url, { tagId: id }).done(function (data) {
                    placeHolderDiv.html(data);
                    placeHolderDiv.find('.modal').modal('show');
                }).fail(function (err) {
                    toastr.error(`${err.responseText}`, 'Hata!');
                });
            });

        /* Ajax POST / Updating a Category starts from here */

        placeHolderDiv.on('click',
            '#btnUpdate',
            function (event) {
                event.preventDefault();
                const form = $('#form-tag-update');
                const actionUrl = form.attr('action');
                const dataToSend = new FormData(form.get(0));
                $.ajax({
                    url: actionUrl,
                    type: 'PUT',
                    data: dataToSend,
                    processData: false,
                    contentType: false,
                    success: function (data) {
                        const tagUpdateAjaxModel = jQuery.parseJSON(data);
                        console.log(tagUpdateAjaxModel);
                        const newFormBody = $('.modal-body', tagUpdateAjaxModel.TagUpdatePartial);
                        placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                        const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                        if (isValid) {
                            const id = tagUpdateAjaxModel.TagViewModel.TagDto.Id;
                            const tableRow = $(`[name="${id}"]`);
                            placeHolderDiv.find('.modal').modal('hide');
                            dataTable.row(tableRow).data([
                                tagUpdateAjaxModel.TagViewModel.TagDto.Id,
                                tagUpdateAjaxModel.TagViewModel.TagDto.Name,
                                tagUpdateAjaxModel.TagViewModel.TagDto.Description,
                                `${tagUpdateAjaxModel.TagViewModel.TagDto.IsActive ? "Evet" : "Hayır"}`,
                                `${tagUpdateAjaxModel.TagViewModel.TagDto.IsDeleted ? "Evet" : "Hayır"}`,
                                `${convertToShortDate(tagUpdateAjaxModel.TagViewModel.TagDto.CreatedDate)}`,
                                tagUpdateAjaxModel.TagViewModel.TagDto.CreatedByUsername,
                                `${convertToShortDate(tagUpdateAjaxModel.TagViewModel.TagDto.UpdatedDate)}`,
                                tagUpdateAjaxModel.TagViewModel.TagDto.UpdatedByUsername,
                                `
                                <button class="btn btn-primary btn-sm btn-update" data-id="${tagUpdateAjaxModel
                                    .TagViewModel.TagDto.Id}"><span class="fas fa-edit"></span></button>
                                <button class="btn btn-danger btn-sm btn-delete" data-id="${tagUpdateAjaxModel
                                    .TagViewModel.TagDto.Id}"><span class="fas fa-minus-circle"></span></button>
                                            `
                            ]);
                            tableRow.attr("name", `${id}`);
                            dataTable.row(tableRow).invalidate();
                            toastr.success(`${tagUpdateAjaxModel.TagViewModel.TagDto.Id} no'lu etiket başarıyla güncellenmiştir`, "Başarılı İşlem!");
                        } else {
                            let summaryText = "";
                            $('#validation-summary > ul > li').each(function () {
                                let text = $(this).text();
                                summaryText = `*${text}\n`;
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
});

$(document).on('mouseover', '.btn-update', function () {
    this.title = 'Güncelle';
});

$(document).on('mouseover', '.btn-delete', function () {
    this.title = 'Sil';
});