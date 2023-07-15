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
        public async Task<CustomResponseDto<CategoryDto>> AddAsync(CategoryCreateDto categoryCreateDto)
        {
            var hasCategory = await UnitOfWork.Categories.AnyAsync(x => x.Name == categoryCreateDto.Name);

            if (!hasCategory)
            {
                var category = Mapper.Map<Category>(categoryCreateDto);

                await UnitOfWork.Categories.AddAsync(category);
                await UnitOfWork.CommitAsync();

                return CustomResponseDto<CategoryDto>.Success(200, Mapper.Map<CategoryDto>(category));
            }
            return CustomResponseDto<CategoryDto>.Fail(200, "Bu isimde bir kategori adı zaten mevcut!");
        }

        public async Task<CustomResponseDto<CategoryDto>> DeleteAsync(int categoryId)
        {
            var category = await UnitOfWork.Categories.GetAsync(x => x.Id == categoryId);
            if (category != null)
            {
                category.IsActive = false;
                category.IsDeleted = true;

                UnitOfWork.Categories.Update(category);
                await UnitOfWork.CommitAsync();

                return CustomResponseDto<CategoryDto>.Success(200, Mapper.Map<CategoryDto>(category));
            }
            return CustomResponseDto<CategoryDto>.Fail(200, $"{categoryId} numaralı kategori bulunamadı!");
        }

        public async Task<CustomResponseDto<CategoryDto>> UndoDeleteAsync(int categoryId)//Admin-Arşiv-Users
        {
            var result = await UnitOfWork.Categories.AnyAsync(x => x.Id == categoryId);
            if (result)
            {
                var category = await UnitOfWork.Categories.GetAsync(x => x.Id == categoryId);
                category.IsDeleted = false;
                category.IsActive = true;
                UnitOfWork.Categories.Update(category);
                await UnitOfWork.CommitAsync();
                return CustomResponseDto<CategoryDto>.Success(200, Mapper.Map<CategoryDto>(category));
            }
            return CustomResponseDto<CategoryDto>.Fail(200, "Bir kategori bulunamadı!");
        }

        public async Task<CustomResponseDto<CategoryDto>> HardDeleteAsync(int categoryId)//Admin-Arşiv-Users
        {
            var result = await UnitOfWork.Categories.AnyAsync(x => x.Id == categoryId);
            if (result)
            {
                var category = UnitOfWork.Categories.Where(x => x.Id == categoryId);
                var categoryDto = await category.Select(x => Mapper.Map<CategoryDto>(x)).FirstOrDefaultAsync();

                UnitOfWork.Categories.RemoveRange(category);
                await UnitOfWork.CommitAsync();

                return CustomResponseDto<CategoryDto>.Success(200, categoryDto);
            }
            return CustomResponseDto<CategoryDto>.Fail(200, "Bir kategori bulunamadı!");
        }

        public async Task<CustomResponseDto<CategoryListDto>> GetAllByNonDeletedAsync()//Aktif ve pasif tüm kategoriler
        {
            var categories = await UnitOfWork.Categories.GetAllAsync(x => !x.IsDeleted);

            if (categories.Any())
            {
                var categoriesDto = categories.Select(category => Mapper.Map<CategoryDto>(category)).ToList();

                return CustomResponseDto<CategoryListDto>.Success(200, new CategoryListDto
                {
                    Categories = categoriesDto
                });
            }
            return CustomResponseDto<CategoryListDto>.Fail(200, "Bir kategori bulunamadı!");
        }

        public async Task<CustomResponseDto<CategoryListDto>> GetAllByActiveAsync()
        {
            var categories = await UnitOfWork.Categories.GetAllAsync(x => x.IsActive && !x.IsDeleted);

            if (categories.Any())
            {
                var categoriesDto = categories.Select(category => Mapper.Map<CategoryDto>(category)).ToList();

                return CustomResponseDto<CategoryListDto>.Success(200, new CategoryListDto
                {
                    Categories = categoriesDto
                });
            }
            return CustomResponseDto<CategoryListDto>.Fail(200, "Bir kategori bulunamadı!");
        }

        public async Task<CustomResponseDto<CategoryListDto>> GetAllCategoriesAsync()
        {
            var categories = await UnitOfWork.Categories.GetAllAsync(null);

            if (categories.Any())
            {
                var categoriesDto = categories.Select(category => Mapper.Map<CategoryDto>(category)).ToList();

                return CustomResponseDto<CategoryListDto>.Success(200, new CategoryListDto
                {
                    Categories = categoriesDto
                });
            }
            return CustomResponseDto<CategoryListDto>.Fail(200, "Bir kategori bulunamadı!");
        }

        public async Task<CustomResponseDto<CategoryUpdateDto>> GetCategoryUpdateDtoAsync(int categoryId)
        {
            var result = await UnitOfWork.Categories.AnyAsync(x => x.Id == categoryId);
            if (result)
            {
                var category = await UnitOfWork.Categories.GetAsync(x => x.Id == categoryId);

                return CustomResponseDto<CategoryUpdateDto>.Success(200, Mapper.Map<CategoryUpdateDto>(category));
            }
            return CustomResponseDto<CategoryUpdateDto>.Fail(200, "Bir kategori bulunamadı!");
        }

        public async Task<CustomResponseDto<CategoryDto>> UpdateAsync(CategoryUpdateDto categoryUpdateDto)
        {
            var oldCategory = await UnitOfWork.Categories.Where(x => x.Id == categoryUpdateDto.Id).SingleOrDefaultAsync();
            var updatedCategory = Mapper.Map<CategoryUpdateDto, Category>(categoryUpdateDto, oldCategory);

            UnitOfWork.Categories.Update(updatedCategory);
            await UnitOfWork.CommitAsync();

            return CustomResponseDto<CategoryDto>.Success(200, Mapper.Map<CategoryDto>(updatedCategory));
        }

        public async Task<CustomResponseDto<int>> CountTotalAsync()
        {
            var categoriesCount = await UnitOfWork.Categories.CountAsync();

            return categoriesCount > -1 ? CustomResponseDto<int>.Success(200, categoriesCount) : CustomResponseDto<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {categoriesCount}");
        }

        public async Task<CustomResponseDto<int>> CountByNonDeletedAsync()
        {
            var categoriesCount = await UnitOfWork.Categories.CountAsync(x => !x.IsDeleted);

            return categoriesCount > -1 ? CustomResponseDto<int>.Success(200, categoriesCount) : CustomResponseDto<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {categoriesCount}");
        }

        public async Task<CustomResponseDto<CategoryListDto>> GetAllByDeletedAsync()//Admin-Arşiv
        {
            var categories = await UnitOfWork.Categories.GetAllAsync(x => x.IsDeleted);

            if (categories.Any())
            {
                var categoriesDto = categories.Select(category => Mapper.Map<CategoryDto>(category)).ToList();

                return CustomResponseDto<CategoryListDto>.Success(200, new CategoryListDto
                {
                    Categories = categoriesDto
                });
            }
            return CustomResponseDto<CategoryListDto>.Fail(200, "Silinmiş bir kategori bulunamadı!");
        }
    }
}
