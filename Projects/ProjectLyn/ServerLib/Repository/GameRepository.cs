using Microsoft.EntityFrameworkCore;
using ServerLib.Model;
using System.Threading.Tasks;

namespace ServerLib
{
    public class GameRepository
    {
        private readonly UserContext _context;

        public GameRepository(UserContext context)
        {
            _context = context;
        }
        public async Task<account?> GetAccountById(int id)
        {
            return await _context.Account.FindAsync(id);
        }

        public async Task<account?> GetAccountByUserId(int userId)
        {
            return await _context.Account.FirstOrDefaultAsync(a => a.Id == userId);
        }

        public async Task<account> CreateAccountAsync(account account)
        {
            _context.Account.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<bool> UpdateAccountAsync(account account)
        {
            _context.Account.Update(account);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAccountAsync(int id)
        {
            var account = await _context.Account.FindAsync(id);
            if (account == null) return false;

            _context.Account.Remove(account);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}