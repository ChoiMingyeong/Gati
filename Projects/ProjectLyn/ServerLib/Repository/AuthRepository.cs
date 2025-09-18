using Microsoft.EntityFrameworkCore;
using ServerLib.Model;
using System.Threading.Tasks;

namespace ServerLib
{
    public class AuthRepository
    {
        private readonly AuthContext _context;

        public AuthRepository(AuthContext context)
        {
            _context = context;
        }

        public async Task<auth?> GetAuthById(int id)
        {
            return await _context.Auth.FindAsync(id);
        }

        public async Task<auth?> GetAuthByEmail(string email)
        {
            return await _context.Auth.FirstOrDefaultAsync(a => a.UserEmail == email);
        }

        public async Task<auth> CreateAuthAsync(auth auth)
        {
            _context.Auth.Add(auth);
            await _context.SaveChangesAsync();
            return auth;
        }

        public async Task<bool> UpdateAuthAsync(auth auth)
        {
            _context.Auth.Update(auth);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAuthAsync(int id)
        {
            var auth = await _context.Auth.FindAsync(id);
            if (auth == null) return false;

            _context.Auth.Remove(auth);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}