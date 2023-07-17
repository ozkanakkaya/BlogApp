$(document).ready(function () {
    /* DataTables start here. */
    const dataTable = $('#blogPostsTable').DataTable({
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
                text: 'Yenile',
                className: 'btn btn-warning',
                action: function (e, dt, node, config) {
                    $.ajax({
                        type: 'GET',
                        url: '/Admin/Blog/GetAllBlogs/',
                        contentType: "application/json",
                        beforeSend: function () {
                            $('#blogPostsTable').hide();
                            $('.spinner-border').show();
                        },
                        success: function (data) {
                            const result = jQuery.parseJSON(data);
                            dataTable.clear();
                            //console.log(result);
                            if (!result.error) {
                                let categoriesArray = [];
                                $.each(result.$values,
                                    function (index, blogPost) {
                                        const newBlogPost = getJsonNetObject(blogPost, result.$values);
                                        let newCategories = getJsonNetObject(newBlogPost.Categories, newBlogPost);
                                        if (newCategories !== null) {
                                            categoriesArray.push(newCategories);
                                        }
                                        if (newCategories === null) {
                                            newCategories = categoriesArray.find((category) => {
                                                return category.$id === newBlogPost.Categories.$ref;
                                            });
                                        }
                                        //console.log(newBlogPost);
                                        //console.log(newBlogPost.Id);
                                        //console.log(newCategories.$values.map(category => category.Name).join(', '));
                                        const newTableRow = dataTable.row.add([
                                            newBlogPost.Id,
                                            newCategories.$values.map(category => category.Name).join(', '),
                                            newBlogPost.Title,
                                            `<img src="/img/${newBlogPost.ImageUrl}" alt="${newBlogPost.Title}" class="my-image-table" />`,
                                            newBlogPost.ViewCount,
                                            newBlogPost.CommentCount,
                                            `${newBlogPost.IsActive ? "Evet" : "Hayır"}`,
                                            `${newBlogPost.IsDeleted ? "Evet" : "Hayır"}`,
                                            `${convertToShortDate(newBlogPost.CreatedDate)}`,
                                            newBlogPost.CreatedByUsername,
                                            `${convertToShortDate(newBlogPost.UpdatedDate)}`,
                                            newBlogPost.UpdatedByUsername,
                                            `
                                <a class="btn btn-primary btn-sm btn-update" href="/Admin/Blog/Update?blogId=${newBlogPost.Id}"><span class="fas fa-edit"></span></a>
                                <button class="btn btn-danger btn-sm btn-delete" data-id="${newBlogPost.Id}"><span class="fas fa-minus-circle"></span></button>
                                            `
                                        ]).node();
                                        const jqueryTableRow = $(newTableRow);
                                        jqueryTableRow.attr('name', `${newBlogPost.Id}`);
                                    });
                                dataTable.draw();
                                $('.spinner-border').hide();
                                $('#blogPostsTable').fadeIn(1400);
                            } else {
                                toastr.error(`${result.error}`, 'İşlem Başarısız!');
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            $('.spinner-border').hide();
                            $('#blogPostsTable').fadeIn(1000);
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
    /* Ajax POST / Deleting a User starts here. */

    //Trumbowyg

    //Trumbowyg

    $(document).on('click', '.btn-delete', function (event) { //.btn-delete butona atadığımız class ı kullanarak o objeyi yakalıyoruz. BU class bu işlem için butona eklendi
        event.preventDefault(); //Butonun kendi bir işlevi varsa bunu deaktif ediyoruz.
        const id = $(this).attr('data-id');
        const tableRow = $(`[name="${id}"]`);
        const blogTitle = tableRow.find('td:eq(2)').text(); //<td> ler içerisinden 3. td yi seçmiş olduk.
        Swal.fire({
            title: 'Silmek istediğinize emin misiniz?',
            text: `${blogTitle} başlıklı makale silinecektir!`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Evet, silmek istiyorum!',
            cancelButtonText: 'Hayır, silmek istemiyorum.'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: 'PUT',
                    dataType: 'json',
                    data: { blogId: id },
                    url: '/Admin/Blog/Delete/',
                    success: function (data) {
                        const blogViewModel = jQuery.parseJSON(data);
                        if (blogViewModel.ResultStatus === 200) {
                            Swal.fire(
                                'Silindi!',
                                `${blogViewModel.Message}`,
                                "success"
                            );

                            dataTable.row(tableRow).remove().draw();
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Başarısız işlem.',
                                text: `${blogViewModel.Message}`
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