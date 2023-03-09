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
    public class CategoryService : Service<Category>, ICategoryService
    {
        public CategoryService(IGenericRepository<Category> repository, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork, mapper)
        {
        }
        public async Task<CustomResponse<CategoryCreateDto>> AddAsync(CategoryCreateDto categoryCreateDto)
        {
            var hasCategory = await UnitOfWork.Categories.AnyAsync(x => x.Name == categoryCreateDto.Name);

            if (!hasCategory)
            {
                var category = Mapper.Map<Category>(categoryCreateDto);

                await UnitOfWork.Categories.AddAsync(category);
                await UnitOfWork.CommitAsync();

                return CustomResponse<CategoryCreateDto>.Success(201, categoryCreateDto);
            }
            return CustomResponse<CategoryCreateDto>.Fail(400, "Bu isimde bir kategori adı zaten mevcut!");
        }

        public async Task<CustomResponse<NoContent>> DeleteAsync(int categoryId)
        {
            var category = await UnitOfWork.Categories.GetAsync(x => x.Id == categoryId);
            if (category != null)
            {
                category.IsActive = false;
                category.IsDeleted = true;

                UnitOfWork.Categories.Update(category);
                await UnitOfWork.CommitAsync();

                return CustomResponse<NoContent>.Success(204);
            }
            return CustomResponse<NoContent>.Fail(404, $"{categoryId} numaralı kategori bulunamadı!");
        }

        public async Task<CustomResponse<NoContent>> UndoDeleteAsync(int categoryId)//Admin-Arşiv-Users
        {
            var result = await UnitOfWork.Categories.AnyAsync(x => x.Id == categoryId);
            if (result)
            {
                var category = await UnitOfWork.Categories.GetAsync(x => x.Id == categoryId);
                category.IsDeleted = false;
                category.IsActive = true;
                UnitOfWork.Categories.Update(category);
                await UnitOfWork.CommitAsync();
                return CustomResponse<NoContent>.Success(204);
            }
            return CustomResponse<NoContent>.Fail(404, "Bir kategori bulunamadı!");
        }

        public async Task<CustomResponse<NoContent>> HardDeleteAsync(int categoryId)//Admin-Arşiv-Users
        {
            var result = await UnitOfWork.Categories.AnyAsync(x => x.Id == categoryId);
            if (result)
            {
                var category = UnitOfWork.Categories.Where(x => x.Id == categoryId);

                UnitOfWork.Categories.RemoveRange(category);
                await UnitOfWork.CommitAsync();

                return CustomResponse<NoContent>.Success(204);
            }
            return CustomResponse<NoContent>.Fail(404, "Bir kategori bulunamadı!");
        }

        public async Task<CustomResponse<CategoryListDto>> GetAllByNonDeletedAsync()//Aktif ve pasif tüm kategoriler
        {
            var categories = await UnitOfWork.Categories.GetAllAsync(x => !x.IsDeleted);

            if (categories.Any())
            {
                //var categoryListDto = categories.Select(category => Mapper.Map<CategoryListDto>(category));

                return CustomResponse<CategoryListDto>.Success(200, new CategoryListDto
                {
                    Categories = categories
                });
            }
            return CustomResponse<CategoryListDto>.Fail(404, "Bir kategori bulunamadı!");
        }

        public async Task<CustomResponse<CategoryListDto>> GetAllByActiveAsync()
        {
            var categories = await UnitOfWork.Categories.GetAllAsync(x => x.IsActive && !x.IsDeleted);

            if (categories.Any())
            {
                return CustomResponse<CategoryListDto>.Success(200, new CategoryListDto
                {
                    Categories = categories
                });
            }
            return CustomResponse<CategoryListDto>.Fail(404, "Bir kategori bulunamadı!");
        }

        public async Task<CustomResponse<CategoryListDto>> GetAllCategoriesAsync()
        {
            var categories = await UnitOfWork.Categories.GetAllAsync(null);

            if (categories.Any())
            {
                return CustomResponse<CategoryListDto>.Success(200, new CategoryListDto
                {
                    Categories = categories
                });
            }
            return CustomResponse<CategoryListDto>.Fail(404, "Bir kategori bulunamadı!");
        }

        public async Task<CustomResponse<CategoryUpdateDto>> GetCategoryUpdateDtoAsync(int categoryId)
        {
            var result = await UnitOfWork.Categories.AnyAsync(x => x.Id == categoryId);
            if (result)
            {
                var category = await UnitOfWork.Categories.GetAsync(x => x.Id == categoryId);

                return CustomResponse<CategoryUpdateDto>.Success(200, Mapper.Map<CategoryUpdateDto>(category));
            }
            return CustomResponse<CategoryUpdateDto>.Fail(404, "Bir kategori bulunamadı!");
        }

        public async Task<CustomResponse<NoContent>> UpdateAsync(CategoryUpdateDto categoryUpdateDto)
        {
            var oldCategory = await UnitOfWork.Categories.Where(x => x.Id == categoryUpdateDto.Id).SingleOrDefaultAsync();
            var updatedCategory = Mapper.Map<CategoryUpdateDto, Category>(categoryUpdateDto, oldCategory);

            UnitOfWork.Categories.Update(updatedCategory);
            await UnitOfWork.CommitAsync();

            return CustomResponse<NoContent>.Success(204);
        }

        public async Task<CustomResponse<int>> CountTotalAsync()
        {
            var categoriesCount = await UnitOfWork.Categories.CountAsync();

            return categoriesCount > -1 ? CustomResponse<int>.Success(200, categoriesCount) : CustomResponse<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {categoriesCount}");
        }

        public async Task<CustomResponse<int>> CountByNonDeletedAsync()
        {
            var categoriesCount = await UnitOfWork.Categories.CountAsync(x => !x.IsDeleted);

            return categoriesCount > -1 ? CustomResponse<int>.Success(200, categoriesCount) : CustomResponse<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {categoriesCount}");
        }

        public async Task<CustomResponse<CategoryListDto>> GetAllByDeletedAsync()//Admin-Arşiv
        {
            var categories = await UnitOfWork.Categories.GetAllAsync(x => x.IsDeleted);

            if (categories.Any())
            {
                var categoriesDto = categories.Select(category => Mapper.Map<CategoryListDto>(category)).ToList();

                return CustomResponse<CategoryListDto>.Success(200, new CategoryListDto
                {
                    Categories = categories
                });
            }
            return CustomResponse<CategoryListDto>.Fail(404, "Silinmiş bir kullanıcı bulunamadı!");
        }
    }
}
