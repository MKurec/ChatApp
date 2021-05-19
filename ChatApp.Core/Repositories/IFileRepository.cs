using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Infrastructure.Repositories
{
    public interface IFileRepository
    {
        Task<FileStream> GetAsync(string path,string defaultpath);
        Task AddAsync(Image image, string path, Guid id);

        Task DeleteAsync(string path);
    }
}
