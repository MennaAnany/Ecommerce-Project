using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;

namespace API.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public ProductRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
        }


        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<ProductDto> GetProductBySlugAsync(string slug)
        {
            var product = await _context.Products
                        .FirstOrDefaultAsync(p => p.Slug == slug);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<PagedList<ProductDto>> GetProductsAsync(ProductParams productParams)
        {
            IQueryable<ProductDto> query = _context.Products.AsQueryable().ProjectTo<ProductDto>(_mapper.ConfigurationProvider);

            if (!string.IsNullOrEmpty(productParams.Category))
            {
                query = query.Where(p => p.Category == productParams.Category);
            }

            switch (productParams.PriceRange)
            {
                case "high":
                    query = query.OrderByDescending(p => p.Price);
                    break;

                case "low":
                    query = query.OrderBy(p => p.Price);
                    break;
                default:
                    query = query.OrderBy(p => p.Id);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(productParams.SearchFilter))
            {
                query = query.Where(p => p.Name.Contains(productParams.SearchFilter));
            }

            return await PagedList<ProductDto>.CreateAsync(query, productParams.PageNumber, productParams.PageSize);
        }

        public async Task<StatisticsDto> FindMostBoughtProduct(List<Order> orders)
        {
            var mostBoughtProduct = orders
                .SelectMany(order => order.OrderItems)
                .GroupBy(orderItem => orderItem.Product)
                .OrderByDescending(group => group.Sum(item => item.Quantity))
                .FirstOrDefault();

            if (mostBoughtProduct != null)
                return new StatisticsDto
                {
                    Id = mostBoughtProduct.Key.Id,
                    Name = mostBoughtProduct.Key.Name,
                    Slug = mostBoughtProduct.Key.Slug,
                    Price = mostBoughtProduct.Key.Price,
                    Quantity = mostBoughtProduct.Sum(item => item.Quantity)
                };

            return null;

        }
    } 
}
