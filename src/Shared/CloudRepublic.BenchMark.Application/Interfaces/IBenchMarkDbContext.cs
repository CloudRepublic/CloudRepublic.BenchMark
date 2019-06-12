using System.Threading;
using System.Threading.Tasks;
using CloudRepublic.BenchMark.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudRepublic.BenchMark.Application.Interfaces
{
    public interface IBenchMarkDbContext
    {
        DbSet<BenchMarkResult> BenchMarkResult { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}