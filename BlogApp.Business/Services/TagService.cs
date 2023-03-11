using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using BlogApp.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Business.Services
{
    public class TagService : Service<Tag>, ITagService
    {
        public TagService(IGenericRepository<Tag> repository, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork, mapper)
        {
        }

        public async Task<CustomResponse<TagDto>> DeleteAsync(int tagId)
        {
            var tag = await UnitOfWork.Tags.GetAsync(x => x.Id == tagId);
            if (tag != null)
            {
                tag.IsActive = false;
                tag.IsDeleted = true;

                UnitOfWork.Tags.Update(tag);
                await UnitOfWork.CommitAsync();

                return CustomResponse<TagDto>.Success(200, Mapper.Map<TagDto>(tag));
            }
            return CustomResponse<TagDto>.Fail(404, $"{tagId} numaralı etiket bulunamadı!");
        }

        public async Task<CustomResponse<TagDto>> UndoDeleteAsync(int tagId)//Admin-Arşiv-Users
        {
            var result = await UnitOfWork.Tags.AnyAsync(x => x.Id == tagId);
            if (result)
            {
                var tag = await UnitOfWork.Tags.GetAsync(x => x.Id == tagId);
                tag.IsDeleted = false;
                tag.IsActive = true;
                UnitOfWork.Tags.Update(tag);
                await UnitOfWork.CommitAsync();
                return CustomResponse<TagDto>.Success(200, Mapper.Map<TagDto>(tag));
            }
            return CustomResponse<TagDto>.Fail(404, "Bir etiket bulunamadı!");
        }

        public async Task<CustomResponse<NoContent>> HardDeleteAsync(int tagId)
        {
            var result = await UnitOfWork.Tags.AnyAsync(x => x.Id == tagId);
            if (result)
            {
                var tag = UnitOfWork.Tags.Where(x => x.Id == tagId);

                UnitOfWork.Tags.RemoveRange(tag);
                await UnitOfWork.CommitAsync();

                return CustomResponse<NoContent>.Success(204);
            }
            return CustomResponse<NoContent>.Fail(404, "Bir etiket bulunamadı!");
        }

        public async Task<CustomResponse<TagListDto>> GetAllByNonDeletedAsync()//Aktif ve pasif tüm tagler
        {
            var tags = await UnitOfWork.Tags.GetAllAsync(x => !x.IsDeleted);

            if (tags.Any())
            {
                return CustomResponse<TagListDto>.Success(200, new TagListDto
                {
                    Tags = tags
                });
            }
            return CustomResponse<TagListDto>.Fail(404, "Bir etiket bulunamadı!");
        }

        public async Task<CustomResponse<TagListDto>> GetAllByActiveAsync()
        {
            var tags = await UnitOfWork.Tags.GetAllAsync(x => x.IsActive && !x.IsDeleted);

            if (tags.Any())
            {
                return CustomResponse<TagListDto>.Success(200, new TagListDto
                {
                    Tags = tags
                });
            }
            return CustomResponse<TagListDto>.Fail(404, "Bir etiket bulunamadı!");
        }

        public async Task<CustomResponse<TagListDto>> GetAllTagsAsync()
        {
            var tags = await UnitOfWork.Tags.GetAllAsync(null);

            if (tags.Any())
            {
                return CustomResponse<TagListDto>.Success(200, new TagListDto
                {
                    Tags = tags
                });
            }
            return CustomResponse<TagListDto>.Fail(404, "Bir etiket bulunamadı!");
        }

        public async Task<CustomResponse<TagUpdateDto>> GetTagUpdateDtoAsync(int tagId)
        {
            var result = await UnitOfWork.Tags.AnyAsync(x => x.Id == tagId);
            if (result)
            {
                var tag = await UnitOfWork.Tags.GetAsync(x => x.Id == tagId);

                return CustomResponse<TagUpdateDto>.Success(200, Mapper.Map<TagUpdateDto>(tag));
            }
            return CustomResponse<TagUpdateDto>.Fail(404, "Bir etiket bulunamadı!");
        }

        public async Task<CustomResponse<NoContent>> UpdateAsync(TagUpdateDto tagUpdateDto)
        {
            var oldTag = await UnitOfWork.Tags.Where(x => x.Id == tagUpdateDto.Id).SingleOrDefaultAsync();
            var updatedTag = Mapper.Map<TagUpdateDto, Tag>(tagUpdateDto, oldTag);

            UnitOfWork.Tags.Update(updatedTag);
            await UnitOfWork.CommitAsync();

            return CustomResponse<NoContent>.Success(204);
        }

        public async Task<CustomResponse<int>> CountTotalAsync()
        {
            var tagsCount = await UnitOfWork.Tags.CountAsync();

            return tagsCount > -1 ? CustomResponse<int>.Success(200, tagsCount) : CustomResponse<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {tagsCount}");
        }

        public async Task<CustomResponse<int>> CountByNonDeletedAsync()
        {
            var tagsCount = await UnitOfWork.Tags.CountAsync(x => !x.IsDeleted);

            return tagsCount > -1 ? CustomResponse<int>.Success(200, tagsCount) : CustomResponse<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {tagsCount}");
        }

        public async Task<CustomResponse<TagListDto>> GetAllByDeletedAsync()//Admin-Arşiv
        {
            var tags = await UnitOfWork.Tags.GetAllAsync(x => x.IsDeleted);

            if (tags.Any())
            {
                var tagsDto = tags.Select(tag => Mapper.Map<TagListDto>(tag)).ToList();

                return CustomResponse<TagListDto>.Success(200, new TagListDto
                {
                    Tags = tags
                });
            }
            return CustomResponse<TagListDto>.Fail(404, "Silinmiş bir etiket bulunamadı!");
        }
    }
}
