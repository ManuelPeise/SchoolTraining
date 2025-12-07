using Data.Entities;
using Logic.Database;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.Models.Administration;
using System.Text;

namespace Logic.Administration.FileImport
{
    public abstract class AFileImport : LogicBase
    {
        protected IDbContextFactory DbContextFactory;

        protected AFileImport(IHttpContextAccessor httpContextAccessor, IDbContextFactory dbContextFactory) : base(httpContextAccessor)
        {
            DbContextFactory = dbContextFactory;
        }

        public abstract Task<int> Import(string fileContent, string fileName);

        public abstract Task<FileDownloadModel> GetFile(FileImportTypeEnum fileType);

        protected (string? fileName, string? base64String) GetFileTemplate(FileImportTypeEnum fileType)
        {
            byte[] file;

            switch (fileType)
            {
                case FileImportTypeEnum.Family:
                    file = Resx.Files.FamilyImport;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileType));
            }

            if (file == null || file.Length == 0)
            {
                return (null, null);
            }

            return ("FamilyImportTemplate.json", Convert.ToBase64String(file));
        }

        protected async Task SaveImportFile(UnitOfWork unitOfWork, FileImportTypeEnum fileType, string fileName, string fileContent, ImportStatusEnum status)
        {
            await unitOfWork.ImportFileRepository.AddAsync(new ImportFileEntity
            {
                FileName = fileName,
                FileType = fileType,
                FileContent = Encoding.UTF8.GetBytes(fileContent),
                Status = status
            });
        }
    }
}
