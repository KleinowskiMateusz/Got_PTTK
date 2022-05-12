using Microsoft.AspNetCore.Http;

namespace KsiazeczkaPttk.DAL.Interfaces
{
    public interface IFileService
    {
        FileStream GetPhoto(string fileName, string rootPath);
        Task<string> SaveFile(IFormFile file, string rootPath);
        void RemoveFile(string url, string rootPath);
    }
}
