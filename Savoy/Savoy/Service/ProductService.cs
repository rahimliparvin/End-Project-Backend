using Microsoft.EntityFrameworkCore;
using Savoy.Data;
using Savoy.Models;
using Savoy.Service.Interfaces;

namespace Savoy.Service
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllAsync() => await _context.Products.Include(m => m.ProductImages).
                                                                                         Include(m => m.ProductCategories).
                                                                                         ThenInclude(m => m.Category).
                                                                                         Include(m => m.ProductTags).
                                                                                         ThenInclude(m => m.Tag).
                                                                                         Include(m => m.ProductColors).
                                                                                         ThenInclude(m => m.Color).
                                                                                         ToListAsync();

        public async Task<Product> GetFullDataByIdAsync(int id) => await _context.Products.Include(m => m.ProductImages).
                                                                                    Include(m => m.ProductCategories).
                                                                                    ThenInclude(m => m.Category).
                                                                                    Include(m => m.ProductTags).
                                                                                    ThenInclude(m => m.Tag).
                                                                                    Include(m => m.ProductColors).
                                                                                    ThenInclude(m => m.Color).
                                                                                    FirstOrDefaultAsync(m => m.Id == id);

        public async Task<int> GetCountAsync() => await _context.Products.CountAsync();

        public async Task<IEnumerable<Product>> GetPaginationDatas(int page, int take) => await _context.Products.Include(m => m.ProductImages).
                                                                                                                 Include(m => m.ProductCategories).
                                                                                                                 ThenInclude(m => m.Category).
                                                                                                                 Skip((page * take) - take).
                                                                                                                 Take(take).
                                                                                                                 ToListAsync();

        public async Task<IEnumerable<Product>> GetPaginationData(int? categoryId, int? tagId, int? colorId, int page, int take, string searchText)
        {
            if (categoryId == null)
            {

                if (searchText == null)
                {
                    if (colorId != null && tagId != null)
                    {
                        return await _context.Products.Include(m => m.ProductImages).Include(m => m.ProductCategories).
                                                                                     ThenInclude(m => m.Category).
                                                                                     Where(x => x.ProductColors.Any(xt => xt.ColorId == colorId)).
                                                                                     Where(x => x.ProductTags.Any(xt => xt.TagId == tagId)).
                                                                                     Skip((page * take) - take).
                                                                                     Take(take).
                                                                                     ToListAsync();
                    }
                    if (colorId != null && tagId == null)
                    {
                        return await _context.Products.Include(m => m.ProductImages).Include(m => m.ProductCategories).
                                                                                     ThenInclude(m => m.Category).
                                                                                     Where(x => x.ProductColors.Any(xt => xt.ColorId == colorId)).
                                                                                     Skip((page * take) - take).
                                                                                     Take(take).
                                                                                     ToListAsync();
                    }
                    if (colorId == null && tagId != null)
                    {
                        return await _context.Products.Include(m => m.ProductImages).Include(m => m.ProductCategories).
                                                                                     ThenInclude(m => m.Category).
                                                                                     Where(x => x.ProductTags.Any(xt => xt.TagId == tagId)).
                                                                                     Skip((page * take) - take).
                                                                                     Take(take).
                                                                                     ToListAsync();
                    }

                    return await _context.Products.Include(m => m.ProductImages).Include(m => m.ProductCategories).
                                                                           ThenInclude(m => m.Category).
                                                                           Skip((page * take) - take).
                                                                           Take(take).
                                                                           ToListAsync();
                }

                if (colorId != null && tagId != null)
                {
                    return await _context.Products.Where(m => !m.SoftDelete && m.Name.ToLower().
                                                                                 Trim().Contains(searchText.ToLower().Trim())).Include(m => m.ProductImages).Include(m => m.ProductCategories).
                                                                                 ThenInclude(m => m.Category).
                                                                                 Where(x => x.ProductColors.Any(xt => xt.ColorId == colorId)).
                                                                                 Where(x => x.ProductTags.Any(xt => xt.TagId == tagId)).
                                                                                 Skip((page * take) - take).
                                                                                 Take(take).
                                                                                 ToListAsync();
                }
                if (colorId != null && tagId == null)
                {
                    return await _context.Products.Include(m => m.ProductImages).Include(m => m.ProductCategories).
                                                                                 ThenInclude(m => m.Category).
                                                                                 Where(x => x.ProductColors.Any(xt => xt.ColorId == colorId)).
                                                                                 Where(m => !m.SoftDelete && m.Name.ToLower().
                                                                                 Trim().Contains(searchText.ToLower().Trim())).
                                                                                 Skip((page * take) - take).
                                                                                 Take(take).
                                                                                 ToListAsync();
                }
                if (colorId == null && tagId != null)
                {
                    return await _context.Products.Include(m => m.ProductImages).Include(m => m.ProductCategories).
                                                                                 ThenInclude(m => m.Category).
                                                                                 Where(x => x.ProductTags.Any(xt => xt.TagId == tagId)).
                                                                                 Where(m => !m.SoftDelete && m.Name.ToLower().
                                                                                 Trim().Contains(searchText.ToLower().Trim())).
                                                                                 Skip((page * take) - take).
                                                                                 Take(take).
                                                                                 ToListAsync();
                }

                return await _context.Products.Where(m => !m.SoftDelete && m.Name.ToLower().
                                                                       Trim().Contains(searchText.ToLower().Trim())).Include(m => m.ProductImages).Include(m => m.ProductCategories).
                                                                       ThenInclude(m => m.Category).
                                                                      
                                                                       Skip((page * take) - take).
                                                                       Take(take).
                                                                       ToListAsync();

            }
            else
            {
                if (searchText == null)
                {
                    if (colorId != null && tagId != null)
                    {
                        return await _context.Products.Where(x => x.ProductCategories.Any(xt => xt.CategoryId == categoryId)).
                                                                                      Include(m=>m.ProductImages).
                                                                                      Where(x => x.ProductColors.Any(xt => xt.ColorId == colorId)).
                                                                                      Where(x => x.ProductTags.Any(xt => xt.TagId == tagId)).
                                                                                      Skip((page * take) - take).
                                                                                      Take(take).
                                                                                      ToListAsync();
                    }
                    if (colorId != null && tagId == null)
                    {
                        return await _context.Products.Where(x => x.ProductCategories.Any(xt => xt.CategoryId == categoryId)).
                                                                                      Include(m => m.ProductImages).
                                                                                      Where(x => x.ProductColors.Any(xt => xt.ColorId == colorId)).
                                                                                      Skip((page * take) - take).
                                                                                      Take(take).
                                                                                      ToListAsync();
                    }
                    if (colorId == null && tagId != null)
                    {
                        return await _context.Products.Where(x => x.ProductCategories.Any(xt => xt.CategoryId == categoryId)).
                                                                                     Include(m => m.ProductImages).
                                                                                     Where(x => x.ProductTags.Any(xt => xt.TagId == tagId)).
                                                                                     Skip((page * take) - take).
                                                                                     Take(take).
                                                                                     ToListAsync();
                    }

                    return await _context.Products.Where(x => x.ProductCategories.Any(xt => xt.CategoryId == categoryId)).
                                                                           Include(m => m.ProductImages).
                                                                           Include(m => m.ProductImages).
                                                                           Skip((page * take) - take).
                                                                           Take(take).
                                                                           ToListAsync();
                }


                if (colorId != null && tagId != null)
                {
                    return await _context.Products.Where(x => x.ProductCategories.Any(xt => xt.CategoryId == categoryId)).
                                                                                 Include(m => m.ProductImages).
                                                                                 Where(x => x.ProductColors.Any(xt => xt.ColorId == colorId)).
                                                                                 Where(x => x.ProductTags.Any(xt => xt.TagId == tagId)).
                                                                                 Where(m => !m.SoftDelete && m.Name.ToLower().
                                                                                 Trim().Contains(searchText.ToLower().Trim())).
                                                                                 Skip((page * take) - take).
                                                                                 Take(take).
                                                                                 ToListAsync();
                }
                if (colorId != null && tagId == null)
                {
                    return await _context.Products.Where(x => x.ProductCategories.Any(xt => xt.CategoryId == categoryId)).
                                                                                 Include(m => m.ProductImages).
                                                                                 Where(x => x.ProductColors.Any(xt => xt.ColorId == colorId)).
                                                                                 Where(m => !m.SoftDelete && m.Name.ToLower().
                                                                                 Trim().Contains(searchText.ToLower().Trim())).
                                                                                 Skip((page * take) - take).
                                                                                 Take(take).
                                                                                 ToListAsync();
                }
                if (colorId == null && tagId != null)
                {
                    return await _context.Products.Where(x => x.ProductCategories.Any(xt => xt.CategoryId == categoryId)).
                                                                                 Include(m => m.ProductImages).
                                                                                 Where(x => x.ProductTags.Any(xt => xt.TagId == tagId)).
                                                                                 Where(m => !m.SoftDelete && m.Name.ToLower().
                                                                                 Trim().Contains(searchText.ToLower().Trim())).
                                                                                 Skip((page * take) - take).
                                                                                 Take(take).
                                                                                 ToListAsync();
                }

                return await _context.Products.Where(x => x.ProductCategories.Any(xt => xt.CategoryId == categoryId)).
                                                                       Include(m => m.ProductImages).
                                                                       Where(m => !m.SoftDelete && m.Name.ToLower().
                                                                       Trim().Contains(searchText.ToLower().Trim())).
                                                                       Skip((page * take) - take).
                                                                       Take(take).
                                                                       ToListAsync();
            }

        }

        public async Task<int> GetCategoryIdProductCountAsync(int? categoryId) => await _context.Products.Where(x => x.ProductCategories.Any(xt => xt.CategoryId == categoryId)).
                                                                                                          CountAsync();


    }
}



//Products.Where(x => x.ProductCategories.Any(xt => xt.CategoryId == categoryId))