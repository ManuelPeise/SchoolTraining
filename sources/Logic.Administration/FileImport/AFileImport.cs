using Data.Entities;
using Logic.Database;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using System.Text;

namespace Logic.Administration.FileImport
{
    public abstract class AFileImport: LogicBase
    {
        protected IDbContextFactory DbContextFactory;
        
        protected AFileImport(IHttpContextAccessor httpContextAccessor, IDbContextFactory dbContextFactory):base(httpContextAccessor)
        {
          DbContextFactory = dbContextFactory;
        }

        protected abstract Task<int> Import(FileStream fileStream);

        protected abstract Task<Stream?> GetFile(FileImportTypeEnum fileType);
        
        protected Stream? GetFileTemplate(FileImportTypeEnum fileType)
        {
            byte[] file;

            switch(fileType)
            {
                case FileImportTypeEnum.Family:
                   file = Resx.Files.FamilyImport;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileType));
            }

            if(file == null || file.Length == 0)
            {
                return null;
            }

            using(var memoryStream = new MemoryStream(file))
            {
                return memoryStream;
            } 
        }

        protected async Task SaveImportFile(UnitOfWork unitOfWork, FileImportTypeEnum fileType, string fileContent, ImportStatusEnum status)
        {
            await unitOfWork.ImportFileRepository.AddAsync(new ImportFileEntity
            {
                FileName = Path.GetRandomFileName(),
                FileType = fileType,
                FileContent = Encoding.UTF8.GetBytes(fileContent),
                Status = status
            });
        }
    }
}
