using Data.Layer.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Layer.Contexts.Configurations
{
    public class UsersConfigurations : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> appUser)
        {
            appUser.OwnsOne(o => o.Address, a => a.WithOwner());
        }
    }
}
