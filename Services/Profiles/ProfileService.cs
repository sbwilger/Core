using Microsoft.EntityFrameworkCore;
using sbwilger.DAL;
using sbwilger.DAL.Models.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sbwilger.Core.Services.Profiles
{
    public interface IProfileService
    {
        Task<Profile> GetOrCreateProfileAsync(ulong DiscordId, ulong guildId);
    }

    public class ProfileService : IProfileService
    {
        private readonly DbContextOptions<RPGContext> _options;

        public ProfileService(DbContextOptions<RPGContext> options)
        {
            _options = options;
        }

        public async Task<Profile> GetOrCreateProfileAsync(ulong discordId, ulong guildId)
        {
            using var context = new RPGContext(_options);

            Profile profile = await context.Profiles.Where(x => x.GuildId == guildId).FirstOrDefaultAsync(x => x.DiscordId == discordId).ConfigureAwait(false);

            if(profile != null)
            {
                return profile;
            }

            profile = new Profile
            {
                DiscordId = discordId,
                GuildId = guildId
            };

            context.Add(profile);

            await context.SaveChangesAsync().ConfigureAwait(false);

            return profile;
        }
    }
}
