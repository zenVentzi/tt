using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.DTOs;
using TT.Lib.Entities;

namespace TT.Lib.DataAccess
{
    //public interface IKeyable<T>
    //{
    //    T Key { get; }
    //}


    /// <summary>
    /// Defines methods for a read only data access layer repository.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IReadRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Returns all <typeparamref name="TEntity"/>s.
        /// </summary>
        /// <returns>All <typeparamref name="TEntity"/>s.</returns>
        Task<IEnumerable<TEntity>> Get();

        /// <summary>
        /// Get the <typeparamref name="TEntity"/> by the given <paramref name="key"/> parameter.
        /// </summary>
        /// <param name="keyValues">The <typeparamref name="object[]"/>.</param>
        /// <returns>The <typeparamref name="TEntity"/> for the given <paramref name="key"/></returns>
        Task<TEntity> GetByKey(params object[] keyValues);

        /// <summary>
        /// Returns <see cref="IQueryable{TEntity}"/> for query the <typeparamref name="TEntity"/>s.
        /// </summary>
        /// <returns>The <typeparamref name="TEntity"/>s.</returns>
        IQueryable<TEntity> Query();
    }

    /// <summary>
    /// Defines methods for a read write data access layer repository.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IReadWriteRepository<TEntity> : IReadRepository<TEntity>
        where TEntity : class, IId
    {
        /// <summary>
        /// Adds an <typeparamref name="TEntity"/> to the <see cref="IReadWriteRepository{TEntity}"/>.
        /// </summary>
        /// <param name="entity">The <typeparamref name="TEntity"/> to  add to the <see cref="IReadWriteRepository{TEntity}"/></param>
        /// <returns>The added <typeparamref name="TEntity"/>.</returns>
        Task<TEntity> Add(TEntity entity);

        /// <summary>
        /// Adds <typeparamref name="TEntity"/>s to the <see cref="IReadWriteRepository{TEntity}"/>.
        /// </summary>
        /// <param name="entities">The <typeparamref name="TEntity"/>s to add to the <see cref="IReadWriteRepository{TEntity}"/>.</param>
        /// <returns>The added <typeparamref name="TEntity"/>s.</returns>
        Task<IEnumerable<TEntity>> AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update an <typeparamref name="TEntity"/> to the <see cref="IReadWriteRepository{TEntity}"/>.
        /// </summary>
        /// <param name="entity">The <typeparamref name="TEntity"/> to update to the <see cref="IReadWriteRepository{TEntity}"/>.</param>
        /// <returns>The updated <typeparamref name="TEntity"/></returns>
        Task Update(TEntity entity);

        //void ClearChangeTracker();

        //Task Clear();

        /// <summary>
        /// Remove a <typeparamref name="TEntity"/> from the <see cref="IReadWriteRepository{TEntity}"/>.
        /// </summary>
        /// <param name="entity">The <typeparamref name="TEntity"/> to remove from the <see cref="IReadWriteRepository{TEntity}"/>.</param>
        Task Remove(TEntity entity);

        /// <summary>
        /// Remos <typeparamref name="TEntity"/>s from the <see cref="IReadWriteRepository{TEntity}"/>.
        /// </summary>
        /// <param name="entities">The <typeparamref name="TEntity"/>s to remove from the <see cref="IReadWriteRepository{TEntity}"/>.</param>
        Task RemoveRange(IEnumerable<TEntity> entities);
    }

    /// <summary>
    /// Defines methods for a read only business logic layer service.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IReadService<TEntity>
    {
        /// <summary>
        /// Returns all <typeparamref name="TEntity"/>s.
        /// </summary>
        /// <returns>All <typeparamref name="TEntity"/>s.</returns>
        Task<IEnumerable<TEntity>> Get();

        /// <summary>
        /// Get the <typeparamref name="TEntity"/> by the given <paramref name="key"/> parameter.
        /// </summary>
        /// <param name="keyValues">The keys.</param>
        /// <returns>The <typeparamref name="TEntity"/> for the given <paramref name="key"/></returns>
        Task<TEntity> GetByKey(params object[] keyValues);
    }

    /// <summary>
    /// Defines methods for a read write business logic layer service.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IReadWriteService<TEntity> : IReadService<TEntity>
    {
        Task<IQueryable<TEntity>> Query();

        /// <summary>
        /// Adds an <typeparamref name="TEntity"/> to the <see cref="IReadWriteService{TEntity}"/>.
        /// </summary>
        /// <param name="entity">The <typeparamref name="TEntity"/> to  add to the <see cref="IReadWriteService{TEntity}"/></param>
        /// <returns>The added <typeparamref name="TEntity"/>.</returns>
        Task<TEntity> Add(TEntity entity);

        /// <summary>
        /// Adds <typeparamref name="TEntity"/>s to the <see cref="IReadWriteService{TEntity}"/>.
        /// </summary>
        /// <param name="entities">The <typeparamref name="TEntity"/>s to add to the <see cref="IReadWriteService{TEntity}"/>.</param>
        /// <returns>The added <typeparamref name="TEntity"/>s.</returns>
        Task<IEnumerable<TEntity>> AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update an <typeparamref name="TEntity"/> to the <see cref="IReadWriteService{TEntity}"/>.
        /// </summary>
        /// <param name="entity">The <typeparamref name="TEntity"/> to update to the <see cref="IReadWriteService{TEntity}"/>.</param>
        /// <returns>The updated <typeparamref name="TEntity"/></returns>
        Task Update(TEntity entity);

        /// <summary>
        /// Remove a <typeparamref name="TEntity"/> from the <see cref="IReadWriteService{TEntity}"/>.
        /// </summary>
        /// <param name="entity">The <typeparamref name="TEntity"/> to remove from the <see cref="IReadWriteService{TEntity}"/>.</param>
        Task Remove(TEntity entity);

        /// <summary>
        /// Remove a <typeparamref name="TEntity"/> from the <see cref="IReadWriteService{TEntity}"/> by the given key / keys/>.
        /// </summary>
        Task RemoveByKey(params object[] keyValues);
    }

    /// <summary>
    /// Provides an <see cref="IReadRepository{TEntity, TKey}"/> implementation with EntityFramework data access.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    public abstract class EntityFrameworkReadRepository<TEntity, TContext> : IReadRepository<TEntity>
        where TEntity : class, IId
        where TContext : DbContext
    {
        /// <summary>
        /// Returns the <see cref="DbSet{TEntity}"/> by the given <typeparamref name="TContext"/>.
        /// </summary>
        /// <param name="context">The Entity Framework context.</param>
        /// <returns>The <see cref="DbSet{TEntity}"/>.</returns>
        protected DbSet<TEntity> GetEntities(TContext context) => context.Set<TEntity>();

        /// <summary>
        /// Returns an initalized <typeparamref name="TContext"/>.
        /// </summary>
        /// <returns>The initalized <typeparamref name="TContext"/>.</returns>
        protected abstract TContext GetDbContext();

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TEntity>> Get() => await this.GetEntities(this.GetDbContext()).ToListAsync();

        /// <inheritdoc/>
        public virtual async Task<TEntity> GetByKey(params object[] keyValues) => await this.GetEntities(this.GetDbContext()).FindAsync(keyValues);

        /// <inheritdoc/>
        public virtual IQueryable<TEntity> Query() => this.GetEntities(this.GetDbContext()).AsQueryable();
    }

    /// <summary>
    /// Provides an <see cref="IReadWriteRepository{TEntity}"/> implementation with EntityFramework data access.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    public abstract class EntityFrameworkReadWriteRepository<TEntity, TContext> : EntityFrameworkReadRepository<TEntity, TContext>, IReadWriteRepository<TEntity>
        where TEntity : class, IId
        where TContext : DbContext
    {
        /// <inheritdoc/>
        public virtual async Task<TEntity> Add(TEntity entity)
        {
            var dbContext = this.GetDbContext();
            this.GetEntities(dbContext).Add(entity);

            await dbContext.SaveChangesAsync();

            return entity;
        }

        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TEntity>> AddRange(IEnumerable<TEntity> entities)
        {
            var dbContext = this.GetDbContext();
            this.GetEntities(dbContext).AddRange(entities);
            await dbContext.SaveChangesAsync();

            return entities;
        }

        /// <inheritdoc/>
        public virtual async Task Clear()
        {
            var dbContext = this.GetDbContext();
            var dbSet = this.GetEntities(dbContext);
            dbSet.RemoveRange(dbSet);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public virtual async Task Remove(TEntity entity)
        {
            var dbContext = this.GetDbContext();
            this.GetEntities(dbContext).Remove(entity);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public virtual async Task RemoveRange(IEnumerable<TEntity> entities)
        {
            var dbContext = this.GetDbContext();
            this.GetEntities(dbContext).RemoveRange(entities);
            await dbContext.SaveChangesAsync();
        }

        public virtual void ClearChangeTracker()
        {
            this.GetDbContext().ChangeTracker.Clear();
        }

        /// <inheritdoc/>
        public virtual async Task Update(TEntity entity)
        {
            var dbContext = this.GetDbContext();
            dbContext.Attach(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }
    }

    public interface IPropertyService : IReadWriteService<Property> { }

    public interface IProductPropertyService : IReadWriteService<ProductProperty> 
    {
        Dictionary<string, object> GetProductPropertyDTOs(IEnumerable<ProductProperty> allProductProperties, IEnumerable<Property> allProperties);
    }

    public interface IProductService : IReadWriteService<Product> 
    {
        ProductDTO ExportProduct(Product product, IEnumerable<Property> allProperties);
        Task<ProductDTO> ExportProduct(string productId);
    }

    public interface IBrandService : IReadWriteService<Brand>
    {
        Task<IEnumerable<BrandDTO>> Export();
    }

    public interface IProductRepository<T> : IReadWriteRepository<T> where T : class, IId { }

    public interface IPropertyRepository<T> : IReadWriteRepository<T> where T : class, IId { }

    public interface IProductPropertyRepository<T> : IReadWriteRepository<T> where T : class, IId { }

    public interface IBrandRepository<T> : IReadWriteRepository<T> where T : class, IId { }

    public class ReadWriteService<T, TKey> : IReadWriteService<T>
        where T : class, IId
    {
        IReadWriteRepository<T> repo;

        public ReadWriteService(IReadWriteRepository<T> repo) 
        {
            this.repo = repo;
        }

        public async Task<IQueryable<T>> Query() => this.repo.Query();

        public async Task<IEnumerable<T>> Get() => await this.repo.Get();

        public async Task Remove(T entity) => await this.repo.Remove(entity);

        public async Task<T> Add(T entity) => await this.repo.Add(entity);

        public async Task Update(T entity) => await this.repo.Update(entity);

        public async Task<IEnumerable<T>> AddRange(IEnumerable<T> entities) => await this.repo.AddRange(entities);

        public async Task<T> GetByKey(params object[] keyValues)
        {
            return await this.repo.Query()
                .FirstOrDefaultAsync(x => x.Id == int.Parse(keyValues[0].ToString()));
        }

        public async Task RemoveByKey(params object[] keyValues)
        {
            var existing = await this.repo.GetByKey(keyValues);

            if (existing == null)
            {
                throw new ArgumentNullException("Product does not exist!");
            }

            await this.repo.Remove(existing);
        }
    }


    public class ReadWriteRepository<TEntity, TKey>
        where TEntity : class
        where TKey : IComparable<TKey>
    {
        protected readonly DbContext dbContext;

        public ReadWriteRepository(TTDbContext dbContext) : base()
        {
            this.dbContext = dbContext;
        }

        protected DbContext GetDbContext() => this.dbContext;

        /// <summary>
        /// Returns the <see cref="DbSet{TEntity}"/> by the given <typeparamref name="TContext"/>.
        /// </summary>
        /// <param name="context">The Entity Framework context.</param>
        /// <returns>The <see cref="DbSet{TEntity}"/>.</returns>
        protected DbSet<TEntity> GetEntities() => this.dbContext.Set<TEntity>();

        public IQueryable<TEntity> Query() => this.GetEntities().AsQueryable();

        public async Task<IEnumerable<TEntity>> Get() => await this.GetEntities().ToListAsync();

        public async Task<TEntity> GetByKey(params object[] keyValues) => await this.GetEntities().FindAsync(keyValues);

        /// <inheritdoc/>
        public async Task Remove(TEntity entity)
        {
            var dbContext = this.GetDbContext();
            this.GetEntities().Remove(entity);
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task Update(TEntity entity)
        {
            var dbContext = this.GetDbContext();
            dbContext.Attach(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task RemoveRange(IEnumerable<TEntity> entities)
        {
            var dbContext = this.GetDbContext();
            this.GetEntities().RemoveRange(entities);
            await dbContext.SaveChangesAsync();
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            var dbContext = this.GetDbContext();

            this.GetEntities().Add(entity);

            await dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<TEntity>> AddRange(IEnumerable<TEntity> entities)
        {
            var dbContext = this.GetDbContext();

            this.GetEntities().AddRange(entities);

            await dbContext.SaveChangesAsync();

            return entities;
        }
    }

    public class BrandRepository : ReadWriteRepository<Brand, int>, IBrandRepository<Brand>
    {
        public BrandRepository(TTDbContext dbContext) : base(dbContext) { }
    }

    public class ProductRepository : ReadWriteRepository<Product, int>, IProductRepository<Product>
    {
        public ProductRepository(TTDbContext dbContext) : base(dbContext) { }
    }

    public class PropertyRepository : ReadWriteRepository<Property, int>, IPropertyRepository<Property>
    {
        public PropertyRepository(TTDbContext dbContext) : base(dbContext) { }
    }

    public class ProductPropertyRepository : ReadWriteRepository<ProductProperty, int>, IProductPropertyRepository<ProductProperty>
    {
        public ProductPropertyRepository(TTDbContext dbContext) : base(dbContext) { }
    }


    public class BrandService : ReadWriteService<Brand, int>, IBrandService
    {
        private readonly TTDbContext dbContext;
        private readonly IBrandRepository<Brand> mainRepository;
        private readonly IProductService productService;
        private readonly IPropertyService propertyService;

        public BrandService(
            TTDbContext dbContext,
            IBrandRepository<Brand> mainRepository,
            IProductService productService,
            IPropertyService propertyService) : base(mainRepository)
        {
            this.dbContext = dbContext;
            this.mainRepository = mainRepository ?? throw new ArgumentNullException(nameof(mainRepository));
            this.productService = productService ?? throw new ArgumentNullException(nameof(productService));
            this.propertyService = propertyService ?? throw new ArgumentNullException(nameof(propertyService));
        }

        public async Task<IEnumerable<BrandDTO>> Export()
        {
            var brands = await this.mainRepository.Get();
            var allProperties = await this.propertyService.Get();

            var brandDTOs = brands.Select(b => ExportBrand(b, allProperties));
            return brandDTOs;
        }

        private BrandDTO ExportBrand(Brand brand, IEnumerable<Property> allProperties) 
        {
            var products = brand.Products;
            var productDTOs = products.Select(p => this.productService.ExportProduct(p, allProperties));
            var brandDTO = new BrandDTO { Name = brand.Name, products = productDTOs };

            return brandDTO;
        }
    }

    public class ProductService : ReadWriteService<Product, int>, IProductService
    {
        private readonly TTDbContext dbContext;
        private readonly IProductRepository<Product> mainRepository;
        private readonly IProductPropertyService productPropertyService;
        private readonly IPropertyService propertyService;

        public ProductService(TTDbContext dbContext, IProductRepository<Product> mainRepository, IProductPropertyService productPropertyService, IPropertyService propertyService) : base(mainRepository)
        {
            this.dbContext = dbContext;
            this.mainRepository = mainRepository ?? throw new ArgumentNullException(nameof(mainRepository));
            this.productPropertyService = productPropertyService;
            this.propertyService = propertyService;
        }

        public ProductDTO ExportProduct(Product product, IEnumerable<Property> allProperties)
        {
            var productPropertyDTOs = this.productPropertyService.GetProductPropertyDTOs(product.Properties, allProperties);
            var productDTO = new ProductDTO { Properties = productPropertyDTOs, Code = product.Key };

            return productDTO;
        }

        public async Task<ProductDTO> ExportProduct(string productId)
        {
            var product = await this.mainRepository.GetByKey(productId);
            var allProperties = await this.propertyService.Get();
            var productPropertyDTOs = this.productPropertyService.GetProductPropertyDTOs(product.Properties, allProperties);
            var productDTO = new ProductDTO { Properties = productPropertyDTOs, Code = product.Key };

            return productDTO;
        }
    }

    public class ProductPropertyService : ReadWriteService<ProductProperty, int>, IProductPropertyService
    {
        private readonly TTDbContext dbContext;
        private readonly IProductPropertyRepository<ProductProperty> mainRepository;
        private readonly IPropertyService propertyService;

        public ProductPropertyService(TTDbContext dbContext, IProductPropertyRepository<ProductProperty> mainRepository, IPropertyService propertyService) : base(mainRepository)
        {
            this.dbContext = dbContext;
            this.mainRepository = mainRepository ?? throw new ArgumentNullException(nameof(mainRepository));
            this.propertyService = propertyService;
        }

        public Dictionary<string, object> GetProductPropertyDTOs(IEnumerable<ProductProperty> allProductProperties, IEnumerable<Property> allProperties)
        {
            var propertiesList = new List<Dictionary<string, object>>();

            foreach (var currentProp in allProductProperties)
            {
                var field = this.GetField(currentProp, allProperties);
                propertiesList.Add(field);
            }

            var merged = this.GetMergedFields(propertiesList);
            return merged;
        }

        private Dictionary<string, object> GetMergedFields(List<Dictionary<string, object>> dictList)
        {
            var merged = dictList[0];
            for (int i = 0; i < dictList.Count() - 2; i++)
            {
                merged = this.MergeDictionaries(merged, dictList[i + 1]);
            }
            
            return merged;
        }

        Dictionary<string, object> MergeDictionaries(Dictionary<string, object> dict1, Dictionary<string, object> dict2)
        {
            var result = new Dictionary<string, object>(dict1);

            foreach (var kvp in dict2)
            {
                if (result.ContainsKey(kvp.Key))
                {
                    // If both values are dictionaries, merge them recursively
                    if (result[kvp.Key] is Dictionary<string, object> subDict1 && kvp.Value is Dictionary<string, object> subDict2)
                    {
                        result[kvp.Key] = MergeDictionaries(subDict1, subDict2);
                    }
                    else
                    {
                        // Otherwise, overwrite or handle the value conflict
                        result[kvp.Key] = kvp.Value; // Overwrite with dict2's value
                    }
                }
                else
                {
                    // If key is not in dict1, just add it
                    result[kvp.Key] = kvp.Value;
                }
            }

            return result;
        }

        private Dictionary<string, object> GetField(ProductProperty property, IEnumerable<Property> allProperties) 
        {
            var current = property.Property;
            var nestedFields = new Stack<string>();

            do
            {
                nestedFields.Push(current.Name);
                current = allProperties.First(p => p.Id == current.ParentId);
            } while (current.ParentId != 1);

            var fieldDict = ArrToDict(nestedFields, property.Value);
            return fieldDict;
        }

        private Dictionary<string, object> ArrToDict(Stack<string> keys, string value) 
        { 
            if (keys.Count() == 1)
            {
                return new Dictionary<string, object>() { { keys.Pop(), value } };
            }

            var currentKey = keys.Pop();

            return new Dictionary<string, object>() { { currentKey, ArrToDict(keys, value) } };
        }
    }

    public class PropertyService : ReadWriteService<Property, int>, IPropertyService
    {
        private readonly TTDbContext dbContext;
        private readonly IPropertyRepository<Property> mainRepository;

        public PropertyService(TTDbContext dbContext, IPropertyRepository<Property> mainRepository) : base(mainRepository)
        {
            this.dbContext = dbContext;
            this.mainRepository = mainRepository ?? throw new ArgumentNullException(nameof(mainRepository));
        }
    }
}
