$(document).ready(function () {
    //DataTable
    $('#blogsTable').DataTable({
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
        },
        "order": [[4, "desc"]] //Tablonun 4. indeksine göre (0'dan itibaren 4) ve azalan şekilde sıralanmasını ayarladık.
        //Bu şekilde 4.indis olan Tarih alanını en yeniden en eskiye göre sıralayacaktır.
    });
    //DataTable

    //Chart.js
    $.get('/Admin/Blog/GetAllByViewCount/?isAscending=false&takeSize=10',
        function (data) {
            const blogResult = jQuery.parseJSON(data);

            let viewCountContext = $('#viewCountChart');//Kanvas'ı seçtik. (Yani grafik html etiketini burada seçtik.)
            let viewCountChart = new Chart(viewCountContext,
                {
                    type: 'bar',
                    data: {
                        labels: blogResult.$values.map(blog => blog.Title.length > 30 ? blog.Title.substring(0, 30)+`...` : blog.Title),//Foreach döngüsü,
                        datasets: [
                            {
                                label: 'Okunma Sayısı',
                                data: blogResult.$values.map(blog => blog.ViewCount),
                                backgroundColor: '#fb3640', /* ['#00ADB5', '#EEEEEE', '#EAFFD0', '#C3BEF0', '#A0E4CB']*/
                                hoverBorderWith: 4,
                                hoverBorderColor: 'black'
                            },
                            {
                                label: 'Yorum Sayısı',
                                data: blogResult.$values.map(blog => blog.CommentCount),
                                backgroundColor: '#fdca40',
                                hoverBorderWith: 4,
                                hoverBorderColor: 'black'
                            }
                        ]
                    },
                    options: {
                        plugins: {
                            legend: {
                                labels: {
                                    font: {
                                        size: 18
                                    }
                                }
                            }
                        }
                    }
                });
        });
    }) 

  
    //Chart.js