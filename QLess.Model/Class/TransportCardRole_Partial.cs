using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLess.Model
{
    public partial class TransportCardRole : IdentityRole, IRole<int>
    {
        int IRole<int>.Id => TransportCardRoleId;
    }

    public sealed class TransportCardRoleStore : IRoleStore<TransportCardRole>
    {
        private readonly QLessEntities _db;

        public TransportCardRoleStore(IOwinContext context)
        {
            _db = context.Get<QLessEntities>();
        }

        public void Dispose()
        {
            _db?.Dispose();
        }

        public Task CreateAsync(TransportCardRole role)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TransportCardRole role)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TransportCardRole role)
        {
            throw new NotImplementedException();
        }

        public async Task<TransportCardRole> FindByIdAsync(string roleId)
        {
            return await _db.TransportCardRoles.FindAsync(roleId);
        }

        public async Task<TransportCardRole> FindByNameAsync(string roleName)
        {
            return await _db.TransportCardRoles.FirstOrDefaultAsync(r => r.TransportCardRoleName == roleName);
        }
    }
}
