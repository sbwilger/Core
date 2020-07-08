using Microsoft.EntityFrameworkCore;
using sbwilger.Core.ViewModels;
using sbwilger.DAL;
using sbwilger.DAL.Models.Profiles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace sbwilger.Core.Services.Profiles
{
    public interface IExperienceService
    {
        Task<GrantXpViewModel> GrantXpAsync(ulong discordID, ulong guildId, int xpAmount);
    }
    public class ExperienceService : IExperienceService
    {
        private readonly DbContextOptions<RPGContext> _options;
        private readonly IProfileService _profileService;

        public async Task<GrantXpViewModel> GrantXpAsync(ulong discordId, ulong guildId, int xpAmount)
        {
            using var context = new RPGContext(_options);

            Profile profile = await _profileService.GetOrCreateProfileAsync(discordId, guildId).ConfigureAwait(false);

            profile.ExpToLevel -= xpAmount;
            profile.Exp += xpAmount;

            bool levelled = false;

            if(profile.ExpToLevel <= 0)
            {
                float exponent = 1.5f;
                int baseXP = 1000;

                profile.Level++;
                profile.ExpToLevel += (int)MathF.Floor((float)baseXP * MathF.Pow(profile.Level, exponent));

                levelled = true;
            }

            context.Profiles.Update(profile);

            await context.SaveChangesAsync().ConfigureAwait(false);

            return new GrantXpViewModel
            {
                Profile = profile,
                LevelledUp = levelled
            };
        }
    }
}
