using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Logic.Import.FileImport.Interfaces
{
    public interface IFileImporterFactory
    {
        AFileImport GetFileImporter(FileImportTypeEnum fileImportType, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork);
    }
}
