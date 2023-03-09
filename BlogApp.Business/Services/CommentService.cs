using AutoMapper;
using BlogApp.Core.DTOs.Abstract;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using BlogApp.Core.UnitOfWork;
using System.Reflection.Metadata;

namespace BlogApp.Business.Services
{
    public class CommentService : Service<Comment>, ICommentService
    {
        public CommentService(IGenericRepository<Comment> repository, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork, mapper)
        {
        }

        public async Task<CustomResponse<CommentDto>> AddAsync(CommentCreateDto commentCreateDto, string userId)
        {
            //if (userId != null)
            //{
            var blog = await UnitOfWork.Blogs.GetAsync(x => x.Id == commentCreateDto.BlogId);
            if (blog == null) return CustomResponse<CommentDto>.Fail(400, "Yorum yapabileceğiniz bir blog bulunamadı!");

            var user = await UnitOfWork.Users.GetAsync(x => x.Id == int.Parse(userId));
            if (user == null) return CustomResponse<CommentDto>.Fail(400, "Kullanıcı bilgileri getirilemedi!");

            var comment = Mapper.Map<Comment>(commentCreateDto);
            comment.Email = user.Email;
            comment.IsActive = false;
            await UnitOfWork.Comments.AddAsync(comment);

            blog.CommentCount += 1;
            UnitOfWork.Blogs.Update(blog);

            await UnitOfWork.CommitAsync();

            return CustomResponse<CommentDto>.Success(200, Mapper.Map<CommentDto>(comment));
            //}
            //return CustomResponse<CommentDto>.Fail(400, "Yorum yapmak için üye olun veya giriş yapın!");
        }

        public async Task<CustomResponse<CommentDto>> UpdateAsync(CommentUpdateDto commentUpdateDto)
        {
            var oldComment = await UnitOfWork.Comments.GetAsync(x => x.Id == commentUpdateDto.Id, x => x.Blog);
            if (oldComment == null) return CustomResponse<CommentDto>.Fail(400, "Güncelleyeceğiniz yorum bulunamadı!");

            var updatedComment = Mapper.Map<CommentUpdateDto, Comment>(commentUpdateDto, oldComment);

            UnitOfWork.Comments.Update(updatedComment);
            await UnitOfWork.CommitAsync();

            return CustomResponse<CommentDto>.Success(200, Mapper.Map<CommentDto>(updatedComment));
        }

        public async Task<CustomResponse<CommentDto>> DeleteAsync(int commentId)
        {
            var comment = await UnitOfWork.Comments.GetAsync(x => x.Id == commentId, x => x.Blog);
            if (comment != null)
            {
                comment.IsActive = false;
                comment.IsDeleted = true;

                UnitOfWork.Comments.Update(comment);

                var blog = comment.Blog;
                blog.CommentCount -= 1;
                UnitOfWork.Blogs.Update(blog);

                await UnitOfWork.CommitAsync();

                return CustomResponse<CommentDto>.Success(200, Mapper.Map<CommentDto>(comment));
            }
            return CustomResponse<CommentDto>.Fail(404, $"{commentId} numaralı yorum bulunamadı!");
        }

        public async Task<CustomResponse<NoContent>> HardDeleteAsync(int commentId)
        {
            var comment = await UnitOfWork.Comments.GetAsync(x => x.Id == commentId, x => x.Blog);
            if (comment != null)
            {
                if (comment.IsDeleted)
                {
                    UnitOfWork.Comments.Remove(comment);
                    await UnitOfWork.CommitAsync();
                    return CustomResponse<NoContent>.Success(204);
                }

                var blog = comment.Blog;
                UnitOfWork.Comments.Remove(comment);
                blog.CommentCount = await UnitOfWork.Comments.CountAsync(x => x.BlogId == blog.Id && !x.IsDeleted);
                UnitOfWork.Blogs.Update(blog);
                await UnitOfWork.CommitAsync();

                return CustomResponse<NoContent>.Success(204);
            }
            return CustomResponse<NoContent>.Fail(404, "Bir yorum bulunamadı!");
        }

        public async Task<CustomResponse<int>> CountTotalAsync()
        {
            var commentsCount = await UnitOfWork.Comments.CountAsync();

            return commentsCount > -1 ? CustomResponse<int>.Success(200, commentsCount) : CustomResponse<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {commentsCount}");
        }

        public async Task<CustomResponse<int>> CountByNonDeletedAsync()
        {
            var commentsCount = await UnitOfWork.Comments.CountAsync(x => !x.IsDeleted);

            return commentsCount > -1 ? CustomResponse<int>.Success(200, commentsCount) : CustomResponse<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {commentsCount}");
        }

        public async Task<CustomResponse<CommentDto>> ApproveAsync(int commentId)
        {
            var comment = await UnitOfWork.Comments.GetAsync(x => x.Id == commentId, x => x.Blog);
            if (comment != null)
            {
                var blog = comment.Blog;
                comment.IsActive = true;

                UnitOfWork.Comments.Update(comment);

                blog.CommentCount = await UnitOfWork.Comments.CountAsync(x => x.BlogId == blog.Id && !x.IsDeleted);
                UnitOfWork.Blogs.Update(blog);
                await UnitOfWork.CommitAsync();

                return CustomResponse<CommentDto>.Success(200, Mapper.Map<CommentDto>(comment));
            }
            return CustomResponse<CommentDto>.Fail(404, "Bir yorum bulunamadı!");
        }

        public async Task<CustomResponse<CommentDto>> UndoDeleteAsync(int categoryId)
        {
            var comment = await UnitOfWork.Comments.GetAsync(x => x.Id == categoryId, x => x.Blog);

            if (comment != null)
            {
                var blog = comment.Blog;
                comment.IsActive = true;
                comment.IsDeleted = false;

                UnitOfWork.Comments.Update(comment);

                blog.CommentCount += 1;

                UnitOfWork.Blogs.Update(blog);

                await UnitOfWork.CommitAsync();

                return CustomResponse<CommentDto>.Success(200, Mapper.Map<CommentDto>(comment));
            }
            return CustomResponse<CommentDto>.Fail(404, "Bir yorum bulunamadı!");
        }

        public async Task<CustomResponse<CommentDto>> GetCommentByIdAsync(int commentId)
        {
            var comment = await UnitOfWork.Comments.GetAsync(x => x.Id == commentId, x => x.Blog);

            if (comment != null)
            {
                return CustomResponse<CommentDto>.Success(200, Mapper.Map<CommentDto>(comment));
            }
            return CustomResponse<CommentDto>.Fail(404, "Bir yorum bilgisi bulunamadı!");
        }

        public async Task<CustomResponse<CommentUpdateDto>> GetCommentUpdateDtoAsync(int commentId)
        {
            var result = await UnitOfWork.Comments.AnyAsync(x => x.Id == commentId);
            if (result)
            {
                var comment = await UnitOfWork.Comments.GetAsync(x => x.Id == commentId);

                return CustomResponse<CommentUpdateDto>.Success(200, Mapper.Map<CommentUpdateDto>(comment));
            }
            return CustomResponse<CommentUpdateDto>.Fail(404, "Bir yorum bilgisi bulunamadı!");
        }

        public async Task<CustomResponse<CommentListDto>> GetAllCommentsAsync()
        {
            var comments = await UnitOfWork.Comments.GetAllAsync(null, x => x.Blog);

            if (comments.Any())
            {
                return CustomResponse<CommentListDto>.Success(200, new CommentListDto
                {
                    Comments = Mapper.Map<IList<CommentDto>>(comments)
                });
            }
            return CustomResponse<CommentListDto>.Fail(404, "Bir yorum bulunamadı!");
        }

        public async Task<CustomResponse<CommentListDto>> GetAllByDeletedAsync()
        {
            var comments = await UnitOfWork.Comments.GetAllAsync(x => x.IsDeleted, x => x.Blog);

            if (comments.Any())
            {
                var commentsDto = comments.Select(comment => Mapper.Map<CommentListDto>(comment)).ToList();

                return CustomResponse<CommentListDto>.Success(200, new CommentListDto
                {
                    Comments = Mapper.Map<IList<CommentDto>>(comments)
                });
            }
            return CustomResponse<CommentListDto>.Fail(404, "Silinmiş bir yorum bulunamadı!");
        }

        public async Task<CustomResponse<CommentListDto>> GetAllByNonDeletedAsync()//Aktif ve pasif tüm yorumlar
        {
            var comments = await UnitOfWork.Comments.GetAllAsync(x => !x.IsDeleted, x => x.Blog);

            if (comments.Any())
            {
                return CustomResponse<CommentListDto>.Success(200, new CommentListDto
                {
                    Comments = Mapper.Map<IList<CommentDto>>(comments)
                });
            }
            return CustomResponse<CommentListDto>.Fail(404, "Bir yorum bulunamadı!");
        }

        public async Task<CustomResponse<CommentListDto>> GetAllByActiveAsync()
        {
            var comments = await UnitOfWork.Comments.GetAllAsync(x => x.IsActive && !x.IsDeleted, x => x.Blog);

            if (comments.Any())
            {
                return CustomResponse<CommentListDto>.Success(200, new CommentListDto
                {
                    Comments= Mapper.Map<IList<CommentDto>>(comments)
                });
            }
            return CustomResponse<CommentListDto>.Fail(404, "Bir yorum bulunamadı!");
        }
    }
}