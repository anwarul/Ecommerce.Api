﻿using System;
using System.Threading.Tasks;
using Streetwood.Core.Domain.Abstract.Repositories;
using Streetwood.Core.Domain.Entities;
using Streetwood.Core.Extensions;
using Streetwood.Infrastructure.Managers.Abstract;
using Streetwood.Infrastructure.Services.Abstract.Commands;

namespace Streetwood.Infrastructure.Services.Implementations.Commands
{
    internal class ProductCommandService : IProductCommandService
    {
        private readonly IProductCategoryRepository productCategoryRepository;
        private readonly IPathManager pathManager;

        public ProductCommandService(IProductCategoryRepository productCategoryRepository, IPathManager pathManager)
        {
            this.productCategoryRepository = productCategoryRepository;
            this.pathManager = pathManager;
        }

        public async Task<int> AddAsync(string name, string nameEng, decimal price, string description, string descriptionEng, bool acceptCharms, string sizes, Guid productCategoryId)
        {
            var category = await productCategoryRepository.GetAndEnsureExistAsync(productCategoryId);
            var imagesPath = pathManager.GetProductImagesPath(category.UniqueName, name.AppendRandom(5));
            var product = new Product(name, nameEng, price, description, descriptionEng, acceptCharms, sizes, imagesPath);

            category.AddProduct(product);
            await productCategoryRepository.SaveChangesAsync();

            return product.Id;
        }
    }
}
