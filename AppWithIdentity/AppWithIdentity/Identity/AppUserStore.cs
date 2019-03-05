using Dapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AppWithIdentity.Identity
{
    public class AppUserStore : IUserStore<AppUser>, IUserPasswordStore<AppUser>
    {
        public static DbConnection GetDbConnection()
        {
            var connection = new SqlConnection("Data Source=localhost;Initial Catalog=test;User ID=AirWatchAdmin;Password=AirWatchAdmin;");
            connection.Open();
            return connection;
        }

        public async Task<IdentityResult> CreateAsync(AppUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetDbConnection())
            {
                await connection.ExecuteAsync("insert into appuser (id, username, normalizedUserName, passwordhash) values (@id, @username, @normalizedUserName, @passwordhash)",
                    new {
                        id = user.Id,
                        username = user.UserName,
                        normalizedUserName = user.NormalizedName,
                        passwordhash = user.PasswordHash
                    }
                    );
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(AppUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetDbConnection())
            {
                await connection.ExecuteAsync("delete appuser where username = @username", new { username = user.UserName });
            }

            return IdentityResult.Success;
        }

        public void Dispose()
        {
        }

        public async Task<AppUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using (var connection = GetDbConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<AppUser>("select * from appuser where id = @id", new { id = userId});
            }
        }

        public async Task<AppUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using (var connection = GetDbConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<AppUser>("select * from appuser where username = @username", new { username = normalizedUserName });
            }
        }

        public Task<string> GetNormalizedUserNameAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedName);
        }

        public Task<string> GetPasswordHashAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetUserIdAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<bool> HasPasswordAsync(AppUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetNormalizedUserNameAsync(AppUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(AppUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(AppUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(AppUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetDbConnection())
            {
                await connection.ExecuteAsync("update appuser set id = @id, username= @username, normalizedUserName=@normalizedUserName, passwordhash=@passwordhash",
                    new
                    {
                        id = user.Id,
                        username = user.UserName,
                        normalizedUserName = user.NormalizedName,
                        passwordhash = user.PasswordHash
                    }
                    );
            }

            return IdentityResult.Success;
        }
    }
}
