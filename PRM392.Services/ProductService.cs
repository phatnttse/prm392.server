using AutoMapper;
using PRM392.Repositories.Entities;
using PRM392.Repositories.Enums;
using PRM392.Repositories.Interfaces;
using PRM392.Repositories.Models;
using PRM392.Services.DTOs.Product;
using PRM392.Services.Interfaces;


namespace PRM392.Services
{
    public class ProductService : IProductService
    {
        #region Properties
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStorageService _storageService;
        #endregion

        #region Ctors
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IStorageService storageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storageService = storageService;
        }
        #endregion

        #region CRUD
        public async Task<ApplicationResponse> CreateProduct(CreateUpdateProductDTO body)
        {
            try
            {                
                var product = _mapper.Map<Product>(body);
              
                product.ActiveFlag = (byte)ActiveFlag.Active;

                await _unitOfWork.ProductRepository.AddAsync(product);

                List<ProductImage> images = body.ImageUrls!.Select(url => new ProductImage
                {
                    ProductId = product.Id,
                    ImageUrl = url
                }).ToList();

                List<ProductImage> newImages = await _unitOfWork.ProductImageRepository.AddRangeAsync(images);

                product.Images = newImages;

                await _unitOfWork.SaveChangesAsync();

                return new ApplicationResponse
                {
                    Data = _mapper.Map<ProductDTO>(product),
                    Message = "Product created successfully",
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

        public async Task<ApplicationResponse> DeleteProduct(string id)
        {
            try
            {
                Product product = await _unitOfWork.ProductRepository.GetByIdAsync(id) ?? throw new ApiException("Product not found", System.Net.HttpStatusCode.NotFound);

                product.ActiveFlag = (byte)ActiveFlag.Deleted;

                _unitOfWork.ProductRepository.Update(product);

                await _unitOfWork.SaveChangesAsync();

                return new ApplicationResponse
                {
                    Message = "Product deleted successfully",
                    Success = true,
                    Data = product,
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

        public async Task<ApplicationResponse> GetProductById(string id)
        {
            try
            {
                Product? product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id) ?? throw new ApiException("Product not found", System.Net.HttpStatusCode.NotFound);

                return new ApplicationResponse
                {
                    Data = _mapper.Map<ProductDTO>(product),
                    Message = "Product retrieved successfully",
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

        public async Task<ApplicationResponse> GetProducts()
        {
            try
            {
                List<Product> products = await _unitOfWork.ProductRepository.GetProductsAsync();

                var productDtos = _mapper.Map<List<ProductDTO>>(products);

                return new ApplicationResponse
                {
                    Data = productDtos,
                    Message = "Products retrieved successfully",
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

        public async Task<ApplicationResponse> UpdateProduct(string id, CreateUpdateProductDTO body)
        {
            try
            {
                Product product = await _unitOfWork.ProductRepository.GetByIdAsync(id) ?? throw new ApiException("Product not found", System.Net.HttpStatusCode.NotFound);

                _mapper.Map(body, product);
              
                _unitOfWork.ProductRepository.Update(product);

                await _unitOfWork.SaveChangesAsync();

                return new ApplicationResponse
                {
                    Data = _mapper.Map<ProductDTO>(product),
                    Message = "Product updated successfully",
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
