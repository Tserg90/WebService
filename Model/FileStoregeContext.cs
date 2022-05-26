using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebService.Model
{
    public class FileStoregeContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(local);Initial Catalog=WebServiceDb;Integrated Security=True");
        }
        public DbSet<FileInDb> Files { get; set; }

        public async void Save( string filename, string mime, byte[] content, CancellationToken cancellationToken)
        {
            await Files.AddAsync(new FileInDb
            {
                fileName = filename,
                fileMime = mime,
                fileContent = content
            }, cancellationToken);
            await SaveChangesAsync(cancellationToken);
        }
        public async Task<(byte[], string)> Load( string filename, CancellationToken cancellationToken)
        {
            var file = await Files
                .Where(f => f.fileName == filename)
                .Select<FileInDb, (byte[], string)>(f => new(f.fileContent, f.fileMime))
                .SingleOrDefaultAsync(cancellationToken);
            return file;
        }
        public async Task<IReadOnlyCollection<string>> GetFiles(CancellationToken cancellationToken)
        {
            return await Files.Select(f => f.fileName).ToArrayAsync(cancellationToken);
        }
    }
}
