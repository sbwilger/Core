﻿using System;
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
        private readonly RPGContext _context;

        public ItemService(RPGContext context)
        {
            _context = context;
        }

        public async Task CreateNewItemAsync(Item item)
        {
            await _context.AddAsync(item).ConfigureAwait(false);

            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<Item> GetItemByName(string itemName)
        {
            itemName = itemName.ToLower();

            return await _context.Items.FirstOrDefaultAsync(x => x.Name.ToLower() == itemName).ConfigureAwait(false);
        }

    }
}
