$(document).ready(function () {

    /* DataTables start here. */

    const dataTable = $('#deletedBlogPostsTable').DataTable({
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
                        url: '/Admin/Blog/GetAllDeletedBlogPosts/',
                        contentType: "application/json",
                        beforeSend: function () {
                            $('#deletedBlogPostsTable').hide();
                            $('.spinner-border').show();
                        },
                        success: function (data) {
                            const blogPostListModel = jQuery.parseJSON(data);
                            dataTable.clear();
                            //console.log(blogPostListModel);
                            if (blogPostListModel.ResultStatus === 200) {
                                let categoriesArray = [];
                                $.each(blogPostListModel.BlogListDto.$values,
                                    function (index, bloPost) {
                                        const newBlogPost = getJsonNetObject(bloPost, blogPostListModel.BlogListDto);
                                        let newCategories = getJsonNetObject(newBlogPost.Categories, newBlogPost);
                                        if (newCategories !== null) {
                                            categoriesArray.push(newCategories);
                                        }
                                        if (newCategories === null) {
                                            newCategories = categoriesArray.find((category) => {
                                                return category.$id === newBlogPost.Categories.$ref;
                                            });
                                        }
                                        const newTableRow = dataTable.row.add([
                                            newBlogPost.Id,
                                            newCategories.$values.map(category => category.Name).join(', '),
                                            newBlogPost.Title,
                                            `<img src="/img/${newBlogPost.ImageUrl}" alt="${newBlogPost.Title}" class="my-image-table" />`,
                                            newBlogPost.ViewCount,
                                            newBlogPost.CommentCount,
                                            `${convertToShortDate(newBlogPost.CreatedDate)}`,
                                            newBlogPost.CreatedByUsername,
                                            `${convertToShortDate(newBlogPost.UpdatedDate)}`,
                                            newBlogPost.UpdatedByUsername,
                                            `
                                <button class="btn btn-primary btn-sm btn-undo" data-id="${newBlogPost.Id}"><span class="fas fa-undo"></span></button>
                                <button class="btn btn-danger btn-sm btn-delete" data-id="${newBlogPost.Id}"><span class="fas fa-minus-circle"></span></button>
                                            `
                                        ]).node();
                                        const jqueryTableRow = $(newTableRow);
                                        jqueryTableRow.attr('name', `${newBlogPost.Id}`);
                                    });
                                dataTable.draw();
                                $('.spinner-border').hide();
                                $('#deletedBlogPostsTable').fadeIn(1400);
                            } else {
                                toastr.error(`${blogPostListModel.Message}`, 'İşlem Başarısız!');
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            $('.spinner-border').hide();
                            $('#deletedBlogPostsTable').fadeIn(1000);
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

    /* Ajax POST / HardDeleting a Article starts from here */

    $(document).on('click',
        '.btn-delete',
        function (event) {
            event.preventDefault();
            const id = $(this).attr('data-id');
            const tableRow = $(`[name="${id}"]`);
            const blogPostTitle = tableRow.find('td:eq(2)').text();
            Swal.fire({
                title: 'Kalıcı olarak silmek istediğinize emin misiniz?',
                text: `'${blogPostTitle}' başlıklı blog yazısı kalıcı olarak silinicektir!`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Evet, kalıcı olarak silmek istiyorum.',
                cancelButtonText: 'Hayır, istemiyorum.'
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        type: 'DELETE',
                        dataType: 'json',
                        data: { blogId: id },
                        url: '/Admin/Blog/HardDelete/',
                        success: function (data) {
                            const blogPostResult = jQuery.parseJSON(data);
                            if (blogPostResult.ResultStatus === 200) {
                                Swal.fire(
                                    'Silindi!',
                                    `${blogPostResult.Message}`,
                                    'success'
                                );

                                dataTable.row(tableRow).remove().draw();
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Başarısız İşlem!',
                                    text: `${blogPostResult.Message}`,
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

    /* Ajax POST / HardDeleting a Article ends here */

    /* Ajax POST / UndoDeleting a Article starts from here */

    $(document).on('click',
        '.btn-undo',
        function (event) {
            event.preventDefault();
            const id = $(this).attr('data-id');
            const tableRow = $(`[name="${id}"]`);
            let blogPostTitle = tableRow.find('td:eq(2)').text();
            Swal.fire({
                title: 'Arşivden geri getirmek istediğinize emin misiniz?',
                text: `'${blogPostTitle}' başlıklı makale arşivden geri getirilecektir!`,
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
                        data: { blogId: id },
                        url: '/Admin/Blog/UndoDelete/',
                        success: function (data) {
                            const blogPostResult = jQuery.parseJSON(data);
                            console.log(blogPostResult);
                            if (blogPostResult.ResultStatus === 200) {
                                Swal.fire(
                                    'Arşivden Geri Getirildi!',
                                    `${blogPostResult.Message}`,
                                    'success'
                                );

                                dataTable.row(tableRow).remove().draw();
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Başarısız İşlem!',
                                    text: `${blogPostResult.Message}`,
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
/* Ajax POST / UndoDeleting a Article ends here */

});