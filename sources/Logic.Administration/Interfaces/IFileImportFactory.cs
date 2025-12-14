using Logic.Administration.FileImport;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Logic.Administration.Interfaces
{
    public interface IFileImportFactory
    {
        AFileImport GetFileImport(FileImportTypeEnum fileImportType, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork);
    }
}
