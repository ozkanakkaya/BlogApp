using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using BlogApp.Core.UnitOfWork;

namespace BlogApp.Business.Services
{
    public class CommentService : Service<Comment>, ICommentService
    {
        public CommentService(IGenericRepository<Comment> repository, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork, mapper)
        {
        }

        public async Task<CustomResponseDto<CommentDto>> AddAsync(CommentCreateDto commentCreateDto, string userId)
        {
            //if (userId != null)
            //{
            var blog = await UnitOfWork.Blogs.GetAsync(x => x.Id == commentCreateDto.BlogId);
            if (blog == null) return CustomResponseDto<CommentDto>.Fail(200, "Yorum yapabileceğiniz bir blog bulunamadı!");

            var user = await UnitOfWork.Users.GetAsync(x => x.Id == int.Parse(userId));
            if (user == null) return CustomResponseDto<CommentDto>.Fail(200, "Kullanıcı bilgileri getirilemedi!");

            var comment = Mapper.Map<Comment>(commentCreateDto);
            comment.Email = user.Email;
            comment.UserId = user.Id;
            comment.Firstname = user.Firstname;
            comment.Lastname = user.Lastname;
            comment.ImageUrl = user.ImageUrl;
            comment.IsActive = false;
            await UnitOfWork.Comments.AddAsync(comment);

            blog.CommentCount += 1;
            UnitOfWork.Blogs.Update(blog);

            await UnitOfWork.CommitAsync();

            return CustomResponseDto<CommentDto>.Success(200, Mapper.Map<CommentDto>(comment));
            //}
            //return CustomResponse<CommentDto>.Fail(400, "Yorum yapmak için üye olun veya giriş yapın!");
        }

        public async Task<CustomResponseDto<CommentDto>> UpdateAsync(CommentUpdateDto commentUpdateDto)
        {
            var oldComment = await UnitOfWork.Comments.GetAsync(x => x.Id == commentUpdateDto.Id, x => x.Blog);
            if (oldComment == null) return CustomResponseDto<CommentDto>.Fail(400, "Güncelleyeceğiniz yorum bulunamadı!");

            var updatedComment = Mapper.Map<CommentUpdateDto, Comment>(commentUpdateDto, oldComment);

            UnitOfWork.Comments.Update(updatedComment);
            await UnitOfWork.CommitAsync();

            return CustomResponseDto<CommentDto>.Success(200, Mapper.Map<CommentDto>(updatedComment));
        }

        public async Task<CustomResponseDto<CommentDto>> DeleteAsync(int commentId)
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

                return CustomResponseDto<CommentDto>.Success(200, Mapper.Map<CommentDto>(comment));
            }
            return CustomResponseDto<CommentDto>.Fail(200, $"{commentId} numaralı yorum bulunamadı!");
        }

        public async Task<CustomResponseDto<NoContent>> HardDeleteAsync(int commentId)
        {
            var comment = await UnitOfWork.Comments.GetAsync(x => x.Id == commentId, x => x.Blog);
            if (comment != null)
            {
                if (comment.IsDeleted)
                {
                    UnitOfWork.Comments.Remove(comment);
                    await UnitOfWork.CommitAsync();
                    return CustomResponseDto<NoContent>.Success(200);
                }

                var blog = comment.Blog;
                UnitOfWork.Comments.Remove(comment);
                blog.CommentCount = await UnitOfWork.Comments.CountAsync(x => x.BlogId == blog.Id && !x.IsDeleted);
                UnitOfWork.Blogs.Update(blog);
                await UnitOfWork.CommitAsync();

                return CustomResponseDto<NoContent>.Success(200);
            }
            return CustomResponseDto<NoContent>.Fail(200, "Bir yorum bulunamadı!");
        }

        public async Task<CustomResponseDto<int>> CountTotalAsync()
        {
            var commentsCount = await UnitOfWork.Comments.CountAsync();

            return commentsCount > -1 ? CustomResponseDto<int>.Success(200, commentsCount) : CustomResponseDto<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {commentsCount}");
        }

        public async Task<CustomResponseDto<int>> CountByNonDeletedAsync()
        {
            var commentsCount = await UnitOfWork.Comments.CountAsync(x => !x.IsDeleted);

            return commentsCount > -1 ? CustomResponseDto<int>.Success(200, commentsCount) : CustomResponseDto<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {commentsCount}");
        }

        public async Task<CustomResponseDto<CommentDto>> ApproveAsync(int commentId)
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

                return CustomResponseDto<CommentDto>.Success(200, Mapper.Map<CommentDto>(comment));
            }
            return CustomResponseDto<CommentDto>.Fail(200, "Bir yorum bulunamadı!");
        }

        public async Task<CustomResponseDto<CommentDto>> UndoDeleteAsync(int categoryId)
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

                return CustomResponseDto<CommentDto>.Success(200, Mapper.Map<CommentDto>(comment));
            }
            return CustomResponseDto<CommentDto>.Fail(200, "Bir yorum bulunamadı!");
        }

        public async Task<CustomResponseDto<CommentDto>> GetCommentByIdAsync(int commentId)
        {
            var comment = await UnitOfWork.Comments.GetAsync(x => x.Id == commentId, x => x.Blog);

            if (comment != null)
            {
                return CustomResponseDto<CommentDto>.Success(200, Mapper.Map<CommentDto>(comment));
            }
            return CustomResponseDto<CommentDto>.Fail(200, "Bir yorum bilgisi bulunamadı!");
        }

        public async Task<CustomResponseDto<CommentListDto>> GetAllCommentsByUserIdAsync(int userId)
        {
            var comments = await UnitOfWork.Comments.GetAllAsync(x => x.UserId == userId, x => x.Blog);

            if (comments != null)
            {
                return CustomResponseDto<CommentListDto>.Success(200, new CommentListDto
                {
                    Comments = Mapper.Map<IList<CommentDto>>(comments)
                });
            }
            return CustomResponseDto<CommentListDto>.Fail(200, "Bir yorum bilgisi bulunamadı!");
        }

        public async Task<CustomResponseDto<CommentUpdateDto>> GetCommentUpdateDtoAsync(int commentId)
        {
            var result = await UnitOfWork.Comments.AnyAsync(x => x.Id == commentId);
            if (result)
            {
                var comment = await UnitOfWork.Comments.GetAsync(x => x.Id == commentId);

                return CustomResponseDto<CommentUpdateDto>.Success(200, Mapper.Map<CommentUpdateDto>(comment));
            }
            return CustomResponseDto<CommentUpdateDto>.Fail(200, "Bir yorum bilgisi bulunamadı!");
        }

        public async Task<CustomResponseDto<CommentListDto>> GetAllCommentsAsync()
        {
            var comments = await UnitOfWork.Comments.GetAllAsync(null, x => x.Blog);

            if (comments.Any())
            {
                return CustomResponseDto<CommentListDto>.Success(200, new CommentListDto
                {
                    Comments = Mapper.Map<IList<CommentDto>>(comments)
                });
            }
            return CustomResponseDto<CommentListDto>.Fail(200, "Bir yorum bulunamadı!");
        }

        public async Task<CustomResponseDto<CommentListDto>> GetAllByDeletedAsync()
        {
            var comments = await UnitOfWork.Comments.GetAllAsync(x => x.IsDeleted, x => x.Blog);

            if (comments.Any())
            {
                var commentsDto = comments.Select(comment => Mapper.Map<CommentListDto>(comment)).ToList();

                return CustomResponseDto<CommentListDto>.Success(200, new CommentListDto
                {
                    Comments = Mapper.Map<IList<CommentDto>>(comments)
                });
            }
            return CustomResponseDto<CommentListDto>.Fail(200, "Silinmiş bir yorum bulunamadı!");
        }

        public async Task<CustomResponseDto<CommentListDto>> GetAllByNonDeletedAsync()//Aktif ve pasif tüm yorumlar
        {
            var comments = await UnitOfWork.Comments.GetAllAsync(x => !x.IsDeleted, x => x.Blog);

            if (comments.Any())
            {
                return CustomResponseDto<CommentListDto>.Success(200, new CommentListDto
                {
                    Comments = Mapper.Map<IList<CommentDto>>(comments)
                });
            }
            return CustomResponseDto<CommentListDto>.Fail(200, "Bir yorum bulunamadı!");
        }

        public async Task<CustomResponseDto<CommentListDto>> GetAllByActiveAsync()
        {
            var comments = await UnitOfWork.Comments.GetAllAsync(x => x.IsActive && !x.IsDeleted, x => x.Blog);

            if (comments.Any())
            {
                return CustomResponseDto<CommentListDto>.Success(200, new CommentListDto
                {
                    Comments = Mapper.Map<IList<CommentDto>>(comments)
                });
            }
            return CustomResponseDto<CommentListDto>.Fail(200, "Bir yorum bulunamadı!");
        }
    }
}