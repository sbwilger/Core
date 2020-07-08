using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using sbwilger.DAL;
using sbwilger.DAL.Models.Items;

namespace sbwilger.Core.Services.Items
{
    public interface IItemService
    {
        Task CreateNewItemAsync(Item item);

        Task<Item> GetItemByName(string itemName);
    }

    public class ItemService : IItemService
    {
        private readonly DbContextOptions<RPGContext> _options;

        public ItemService(DbContextOptions<RPGContext> options)
        {
            _options = options;
        }

        public async Task CreateNewItemAsync(Item item)
        {
            using var context = new RPGContext(_options);

            context.Add(item);

            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<Item> GetItemByName(string itemName)
        {
            using var context = new RPGContext(_options);

            return await context.Items.FirstOrDefaultAsync(x => x.Name.ToLower() == itemName.ToLower()).ConfigureAwait(false);
        }

    }
}
