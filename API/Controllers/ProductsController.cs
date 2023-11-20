using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            if (createProductDto.Quantity < 0 || createProductDto.Price < 0)
                return BadRequest("Quantity and Price must not be less than 0");

            createProductDto.Price = Math.Round(createProductDto.Price, 2);

            var product = _mapper.Map<Product>(createProductDto);

            product.Category = await _categoryRepository.GetCategoryNameAsync(createProductDto.Category);

            _unitOfWork.ProductRepository.AddProduct(product);

            return (await _unitOfWork.Complete()) ? product : BadRequest("Cannot create product");

        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);

            if (product == null) return BadRequest("Product not found");

            _unitOfWork.ProductRepository.DeleteProduct(product);

            return (await _unitOfWork.Complete()) ? NoContent() : BadRequest("Cannot delete product");

        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagedList<ProductDto>>> GetProducts([FromQuery] ProductParams productsParams)
        {
            var products = await _unitOfWork.ProductRepository.GetProductsAsync(productsParams);

            Response.AddPaginationHeader(new PaginationHeader(products.CurrentPage, products.PageSize, products.TotalCount, products.TotalPages));

            return products;
        }

        [HttpGet("{slug}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDto>> GetProduct(string slug)
        {
            var product = await _unitOfWork.ProductRepository.GetProductBySlugAsync(slug);

            return product == null ? NotFound("Product not found") : product;
        }

        [HttpPatch("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<Product>> UpdateProduct(int id,UpdateProductDto updateProductDto)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);

            if (product == null) return BadRequest("Product not found");

            _mapper.Map(updateProductDto, product);

            if (updateProductDto.Quantity < 0 || updateProductDto.Price < 0)
                return BadRequest("Quantity and Price must not be less than 0");

            _unitOfWork.ProductRepository.UpdateProduct(product);

            return (await _unitOfWork.Complete()) ? NoContent() : BadRequest("Cannot update product");

        }
    }
}
