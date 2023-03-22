$(document).ready(function () {//sayfa y�klendi�inde �al���r
    $(function () {
        $(document).on('click',
            '#btnSave',
            function (event) {
                event.preventDefault();//y�klemeyi engeller
                const form = $('#form-comment-add');//veriler al�n�r
                const actionUrl = form.attr('action');
                const dataToSend = form.serialize();
                $.post(actionUrl, dataToSend).done(function (data) {
                    const commentCreateAjaxModel = jQuery.parseJSON(data);
                    console.log(commentCreateAjaxModel);
                    const newFormBody = $('.form-card', commentCreateAjaxModel.CommentCreatePartial);
                    const cardBody = $('.form-card');
                    cardBody.replaceWith(newFormBody);
                    const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                    if (isValid)
                    {
                        const newSingleComment =
                        `<div class="media">
                            <img src="/img/${commentCreateAjaxModel.CommentDto.ImageUrl}" alt="" class="img-fluid image-blog" />
                            <div class="media-body">

                                <h5 class="mt-0">
                                    ${commentCreateAjaxModel.CommentDto.CreatedByUsername} <span style="font-size: small;">(${commentCreateAjaxModel.CommentDto.Firstname} ${commentCreateAjaxModel.CommentDto.Lastname})</span>
                                </h5>

                                <p>${commentCreateAjaxModel.CommentDto.Content}</p>
                            </div>
                        </div>`;
                        const newSingleCommentObject = $(newSingleComment);
                        newSingleCommentObject.hide();
                        $('#comments').append(newSingleCommentObject);
                        newSingleCommentObject.fadeIn(3000);
                        toastr.success(
                            `Say�n ${commentCreateAjaxModel.CommentDto.CreatedByUsername} yorumunuz ba�ar�yla eklenmi�tir. Yorumunuz onayland�ktan sonra herkese g�r�n�r olacakt�r.`);
                        $('#btnSave').prop('disabled', true);
                        setTimeout(function () {
                            $('#btnSave').prop('disabled', false);
                        }, 15000);
                    }
                    else
                    {
                        let summaryText = "";
                        $('#validation-summary > ul > li').each(function () {
                            let text = $(this).text();
                            summaryText += `*${text}\n`;
                        });
                        toastr.warning(summaryText);
                    }
                }).fail(function (error) {
                    console.error(error);
                });
            });
    });
});