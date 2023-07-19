$(document).ready(function () {

    /* DataTables start here. */

    const dataTable = $('#commentsTable').DataTable({
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
                        url: '/Admin/Comment/GetAllComments/',
                        contentType: "application/json",
                        beforeSend: function () {
                            $('#commentsTable').hide();
                            $('.spinner-border').show();
                        },
                        success: function (data) {
                            const commentResult = jQuery.parseJSON(data);
                            dataTable.clear();
                            console.log(commentResult);
                            if (!commentResult.error) {
                                const blogPostsArray = [];
                                $.each(commentResult.Comments.$values,
                                    function (index, comment) {
                                        const newComment = getJsonNetObject(comment, commentResult.Comments.$values);
                                        //let newArticle = getJsonNetObject(newComment.Article, newComment);
                                        //if (newArticle !== null) {
                                        //    blogPostsArray.push(newArticle);
                                        //}
                                        //if (newArticle === null) {
                                        //    newArticle = blogPostsArray.find((article) => {
                                        //        return article.$id === newComment.Article.$ref;
                                        //    });
                                        //}
                                        const newTableRow = dataTable.row.add([
                                            newComment.Id,
                                            newComment.BlogTitle,
                                            newComment.Content.length > 75 ? newComment.Content.substring(0, 75) : newComment.Content,
                                            `${newComment.IsActive ? "Evet" : "Hayır"}`,
                                            `${newComment.IsDeleted ? "Evet" : "Hayır"}`,
                                            `${convertToShortDate(newComment.CreatedDate)}`,
                                            newComment.CreatedByUsername,
                                            `${convertToShortDate(newComment.UpdatedDate)}`,
                                            newComment.UpdatedByUsername,
                                            getButtonsForDataTable(newComment)
                                        ]).node();
                                        const jqueryTableRow = $(newTableRow);
                                        jqueryTableRow.attr('name', `${newComment.Id}`);
                                    });
                                dataTable.draw();
                                $('.spinner-border').hide();
                                $('#commentsTable').fadeIn(1400);
                            } else {
                                toastr.error(`${commentResult.error}`, 'İşlem Başarısız!');
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            $('.spinner-border').hide();
                            $('#commentsTable').fadeIn(1000);
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

    /* Ajax POST / Deleting a Comment starts from here */

    $(document).on('click',
        '.btn-delete',
        function (event) {
            event.preventDefault();
            const id = $(this).attr('data-id');
            const tableRow = $(`[name="${id}"]`);
            let commentText = tableRow.find('td:eq(2)').text();
            commentText = commentText.length > 75 ? commentText.substring(0, 75) : commentText;
            Swal.fire({
                title: 'Silmek istediğinize emin misiniz?',
                text: `${commentText} içerikli yorum silinicektir!`,
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
                        data: { commentId: id },
                        url: '/Admin/Comment/Delete/',
                        success: function (data) {
                            const commentResult = jQuery.parseJSON(data);
                            console.log(commentResult);
                            if (commentResult.ResultStatus === 200) {
                                Swal.fire(
                                    'Silindi!',
                                    `${commentResult.Message}`,
/*                                    `${commentResult.CommentDto.Id} no'lu yorum başarıyla silinmiştir.\nTamamen silmek için veya geri almak için : \nÇöp Kutusu/Yorumlar menüsüne gidiniz.`,*/
                                    'success'
                                );

                                dataTable.row(tableRow).remove().draw();
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Başarısız İşlem!',
                                    text: `${commentResult.Message}`,
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

    /* Ajax GET / Getting the _CommentUpdatePartial as Modal Form starts from here. */

    $(function () {
        const url = '/Admin/Comment/Update/';
        const placeHolderDiv = $('#modalPlaceHolder');
        $(document).on('click',
            '.btn-update',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                $.get(url, { commentId: id }).done(function (data) {
                    placeHolderDiv.html(data);
                    placeHolderDiv.find('.modal').modal('show');
                }).fail(function (err) {
                    toastr.error(`${err.responseText}`, 'Hata!');
                });
            });

        /* Ajax POST / Updating a Comment starts from here */

        placeHolderDiv.on('click',
            '#btnUpdate',
            function (event) {
                event.preventDefault();
                const form = $('#form-comment-update');
                const actionUrl = form.attr('action');
                const dataToSend = new FormData(form.get(0));
                $.ajax({
                    url: actionUrl,
                    type: 'PUT',
                    data: dataToSend,
                    processData: false,
                    contentType: false,
                    success: function (data) {
                        const commentUpdateAjaxModel = jQuery.parseJSON(data);
                        console.log(commentUpdateAjaxModel);
                        const newFormBody = $('.modal-body', commentUpdateAjaxModel.CommentUpdatePartial);
                        placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                        const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                        if (isValid) {
                            const id = commentUpdateAjaxModel.CommentViewModel.CommentDto.Id;
                            const tableRow = $(`[name="${id}"]`);
                            placeHolderDiv.find('.modal').modal('hide');
                            dataTable.row(tableRow).data([
                                commentUpdateAjaxModel.CommentViewModel.CommentDto.Id,
                                commentUpdateAjaxModel.CommentViewModel.CommentDto.BlogTitle,
                                commentUpdateAjaxModel.CommentViewModel.CommentDto.Content.length > 75 ? commentUpdateAjaxModel.CommentViewModel.CommentDto.Content.substring(0, 75) : commentUpdateAjaxModel.CommentViewModel.CommentDto.Content,
                                `${commentUpdateAjaxModel.CommentViewModel.CommentDto.IsActive ? "Evet" : "Hayır"}`,
                                `${commentUpdateAjaxModel.CommentViewModel.CommentDto.IsDeleted ? "Evet" : "Hayır"}`,
                                `${convertToShortDate(commentUpdateAjaxModel.CommentViewModel.CommentDto.CreatedDate)}`,
                                commentUpdateAjaxModel.CommentViewModel.CommentDto.CreatedByUsername,
                                `${convertToShortDate(commentUpdateAjaxModel.CommentViewModel.CommentDto.UpdatedDate)}`,
                                commentUpdateAjaxModel.CommentViewModel.CommentDto.UpdatedByUsername,
                                getButtonsForDataTable(commentUpdateAjaxModel.CommentViewModel.CommentDto)
                            ]);
                            tableRow.attr("name", `${id}`);
                            dataTable.row(tableRow).invalidate();
                            toastr.success(`${commentUpdateAjaxModel.CommentViewModel.CommentDto.Id} no'lu yorum başarıyla güncellenmiştir`, "Başarılı İşlem!");
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

    // Get Detail Ajax Operation

    $(function () {

        const url = '/Admin/Comment/GetDetail/';
        const placeHolderDiv = $('#modalPlaceHolder');
        $(document).on('click',
            '.btn-detail',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                $.get(url, { commentId: id }).done(function (data) {
                    placeHolderDiv.html(data);
                    placeHolderDiv.find('.modal').modal('show');
                }).fail(function (err) {
                    toastr.error(`${err.responseText}`, 'Hata!');
                });
            });

    });

    /* Ajax POST / Deleting a Comment starts from here */

    $(document).on('click',
        '.btn-approve',
        function (event) {
            event.preventDefault();
            const id = $(this).attr('data-id');
            const tableRow = $(`[name="${id}"]`);
            let commentText = tableRow.find('td:eq(2)').text();
            commentText = commentText.length > 75 ? commentText.substring(0, 75) : commentText;
            Swal.fire({
                title: 'Onaylamak istediğinize emin misiniz?',
                text: `${commentText} içerikli yorum onaylanacaktır!`,
                icon: 'info',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Evet, onaylamak istiyorum.',
                cancelButtonText: 'Hayır, onaylamak istemiyorum.'
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        type: 'PUT',
                        dataType: 'json',
                        data: { commentId: id },
                        url: '/Admin/Comment/Approve/',
                        success: function (data) {
                            const commentResult = jQuery.parseJSON(data);
                            console.log(commentResult);
                            if (commentResult.ResultStatus === 200) {
                                dataTable.row(tableRow).data([
                                    commentResult.CommentDto.Id,
                                    commentResult.CommentDto.BlogTitle,
                                    commentResult.CommentDto.Content.length > 75 ? commentResult.CommentDto.Content.substring(0, 75) : commentResult.CommentDto.Content,
                                    `${commentResult.CommentDto.IsActive ? "Evet" : "Hayır"}`,
                                    `${commentResult.CommentDto.IsDeleted ? "Evet" : "Hayır"}`,
                                    `${convertToShortDate(commentResult.CommentDto.CreatedDate)}`,
                                    commentResult.CommentDto.CreatedByUsername,
                                    `${convertToShortDate(commentResult.CommentDto.UpdatedDate)}`,
                                    commentResult.CommentDto.UpdatedByUsername,
                                    getButtonsForDataTable(commentResult.CommentDto)
                                ]);
                                tableRow.attr("name", `${id}`);
                                dataTable.row(tableRow).invalidate();
                                Swal.fire(
                                    'Onaylandı!',
                                    `${commentResult.CommentDto.Id} no'lu yorum başarıyla onaylanmıştır.`,
                                    'success'
                                );

                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Başarısız İşlem!',
                                    text: `${commentResult.Message}`,
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

    function getButtonsForDataTable(comment) {
        if (!comment.IsActive) {
            return `
                                <button class="btn btn-warning btn-sm btn-approve" data-id="${comment.Id
                }"><span class="fas fa-thumbs-up"></span></button>
                                <button class="btn btn-info btn-sm btn-detail" data-id="${comment.Id
                }"><span class="fas fa-newspaper"></span></button>
                                <button class="btn btn-primary btn-sm mt-1 btn-update" data-id="${comment.Id
                }"><span class="fas fa-edit"></span></button>
                                <button class="btn btn-danger btn-sm mt-1 btn-delete" data-id="${comment.Id
                }"><span class="fas fa-minus-circle"></span></button>
                                            `;
        }
        return `<button class="btn btn-info btn-sm btn-detail" data-id="${comment.Id}"><span class="fas fa-newspaper"></span></button>
                                <button class="btn btn-primary btn-sm mt-1 btn-update" data-id="${comment.Id}"><span class="fas fa-edit"></span></button>
                                <button class="btn btn-danger btn-sm mt-1 btn-delete" data-id="${comment.Id}"><span class="fas fa-minus-circle"></span></button>`


    }

});