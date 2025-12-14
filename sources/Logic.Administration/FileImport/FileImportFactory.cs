using Logic.Administration.Interfaces;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Logic.Administration.FileImport
{
    public class FileImportFactory : IFileImportFactory
    {
        public AFileImport GetFileImport(FileImportTypeEnum fileImportType, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            switch (fileImportType)
            {
                case FileImportTypeEnum.Family:
                    return new FamilyFileImport(httpContextAccessor, unitOfWork);
                default:
                    throw new NotImplementedException($"File import type '{fileImportType}' is not implemented.");
            }
        }
    }
}
