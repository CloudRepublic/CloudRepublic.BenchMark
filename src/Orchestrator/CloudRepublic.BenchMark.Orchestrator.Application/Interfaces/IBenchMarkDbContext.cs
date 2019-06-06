using System.Threading;
using System.Threading.Tasks;
using CloudRepublic.BenchMark.Orchestrator.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudRepublic.BenchMark.Orchestrator.Application.Interfaces
{
    public interface IBenchMarkDbContext
    {
        DbSet<BenchMarkResult> BenchMarkResult { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}