﻿using MSF.Domain;
using MSF.Application;
using Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MSF.Service
{

    public interface IProductService
    {
        Task<List<ProductViewModel>> GetProducts();

        Task<ProductViewModel> GetProductById(long Id);

        Task<ProductViewModel> SaveProduct(Product product);

        Task<bool> DeleteProduct(long Id);
    }

    internal class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IUnitOfWork unitOfWork;

        public ProductService(IProductRepository productRepository,
            IUnitOfWork unitOfWork )
        {
            this.productRepository = productRepository;
            this.unitOfWork = unitOfWork;
        }

        async Task<bool> IProductService.DeleteProduct(long Id)
        {
            await productRepository.SoftDelete(Id);
            int result = await unitOfWork.CommitAsync();
            return result > 0;
        }

        async Task<ProductViewModel> IProductService.GetProductById(long Id) => await productRepository.GetAsync(Id);
        
        async Task<List<ProductViewModel>> IProductService.GetProducts()
        {
            var products = await productRepository.GetAllAsync();
            return products.Select(p => (ProductViewModel)p).ToList();
        }

        async Task<ProductViewModel> IProductService.SaveProduct(Product product)
        {
            await productRepository.SaveAsync(product);
            await unitOfWork.CommitAsync();

            return product;
        }
    }
}
