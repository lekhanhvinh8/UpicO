using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Upico.Core.CloudPhoto;

namespace Upico.Core.Services
{
    public interface IPhotoService
    {
        public Task<Photo> AddPhoto(IFormFile file);
        public Task<string> DeletePhoto(string id);
    }
}
