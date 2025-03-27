using AutoMapper;
using PRM392.Repositories.Entities;
using PRM392.Repositories.Enums;
using PRM392.Repositories.Interfaces;
using PRM392.Repositories.Models;
using PRM392.Services.DTOs.Category;
using PRM392.Services.Interfaces;
using PRM392.Utils;

namespace PRM392.Services
{
    public class CategoryService : ICategoryService
    {
        #region Properties
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion

        #region Ctors
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApplicationResponse> CreateCategory(CreateUpdateCategoryDTO body)
        {
            try
            {
                var category = _mapper.Map<Category>(body);

                category.ActiveFlag = (byte)ActiveFlag.Active;

                await _unitOfWork.CategoryRepository.AddAsync(category);

                await _unitOfWork.SaveChangesAsync();

                return new ApplicationResponse
                {
                    Data = _mapper.Map<CategoryDTO>(category),
                    Message = "Category created successfully",
                    Success = true,
                    StatusCode = System.Net.HttpStatusCode.Created
                };
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }
      
        public async Task<ApplicationResponse> DeleteCategory(string id)
        {
            try
            {
                Category cate = await _unitOfWork.CategoryRepository.GetByIdAsync(id) ?? throw new ApiException("Category is not found", System.Net.HttpStatusCode.NotFound);

                _unitOfWork.CategoryRepository.Delete(cate);
                
                await _unitOfWork.SaveChangesAsync();

                return new ApplicationResponse
                {
                    Message = "Category is deleted successfully",
                    Success = true,
                    Data = cate,
                    StatusCode = System.Net.HttpStatusCode.OK
                };

            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ApplicationResponse> GetCategories()
        {
            try
            {
                List<Category> categories = await _unitOfWork.CategoryRepository.GetAllAsync();

                var cateDTOs = _mapper.Map<List<CategoryDTO>>(categories);
             
                return new ApplicationResponse
                {
                    Data = cateDTOs,
                    Message = "Category list retrieved successfully",
                    Success = true,
                    StatusCode = System.Net.HttpStatusCode.OK
                };

            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ApplicationResponse> GetCategoryById(string id)
        {
            try
            {
                Category? category = await _unitOfWork.CategoryRepository.GetByIdAsync(id) ?? throw new ApiException("Category is not found", System.Net.HttpStatusCode.NotFound);

                return new ApplicationResponse
                {
                    Data = _mapper.Map<CategoryDTO>(category),
                    Message = "Category is retrieved successfully",
                    Success = true,
                    StatusCode = System.Net.HttpStatusCode.OK
                };

            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ApplicationResponse> UpdateCategory(string id, CreateUpdateCategoryDTO body)
        {
            try
            {

                Category cate = await _unitOfWork.CategoryRepository.GetByIdAsync(id) ?? throw new ApiException("Category is not found", System.Net.HttpStatusCode.NotFound);

                _mapper.Map(body, cate);

                _unitOfWork.CategoryRepository.Update(cate);

                await _unitOfWork.SaveChangesAsync();

                return new ApplicationResponse
                {
                    Data = _mapper.Map<CategoryDTO>(cate),
                    Message = "Category is updated successfully",
                    Success = true,
                    StatusCode = System.Net.HttpStatusCode.OK
                };

            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }
        #endregion
    }
}
