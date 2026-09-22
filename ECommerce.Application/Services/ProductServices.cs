using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class ProductServices:IProductService
    {
        private readonly IProductRepository _repository;

        public ProductServices(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
           var products = await _repository.GetAllProductsAsync();

            var productDto = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                ProductImage = p.ProductImage,

            }).ToList();
            return productDto;
        }
        public async Task<List<ProductDto>> GetFilteredProductsAsync(string? name, string? category, string? sortBy, int pageNumber = 1, int pageSize = 12)
        {
            var products = await _repository.GetFilteredProductsAsync(name, category , sortBy,  pageNumber ,pageSize );
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                ProductImage = p.ProductImage,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? ""

            }).ToList();
        }
      
        public async Task<ProductDto?> GetProductByIdAsync(int id) 
        {
            var product = await _repository.GetProductByIdAsync(id);
            if (product == null)
            {
                return null;
            }

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ProductImage = product.ProductImage
            };
        }

        public async Task<ProductDto> AddAsync(CreateProductDto createProductDto)
        {
            var product = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                Stock = createProductDto.Stock,
                ProductImage = createProductDto.ProductImage,
                CategoryId = createProductDto.CategoryId,
            };

            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ProductImage = product.ProductImage,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? ""
            };
        }

        public async Task<ProductDto?> UpdateAsync( int id , UpdateProductDto updateProductDto)
        {
            var product = await _repository.GetProductByIdAsync(id);

            if (product == null)
            {
                return null;
            }

            product.Name = updateProductDto.Name;
            product.Description = updateProductDto.Description;
            product.Price = updateProductDto.Price;
            product.Stock = updateProductDto.Stock;
            product.ProductImage = updateProductDto.ProductImage;
            product.CategoryId = updateProductDto.CategoryId;

            await _repository.UpdateAsync(product);
            await _repository.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ProductImage = product.ProductImage,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? ""
            };
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _repository.GetProductByIdAsync(id);

            if (product != null)
            {
                await _repository.DeleteAsync(id);
                await _repository.SaveChangesAsync();
            }
           
        }
        public async Task<int> GetFilteredProductsCountAsync(string? name, string? category)
        {
            return await _repository.GetFilteredProductsCountAsync(name, category);
        }
        public async Task SaveChangesAsync()
        {
            await _repository.SaveChangesAsync();
        }


    }
}
