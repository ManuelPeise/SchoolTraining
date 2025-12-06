using Logic.Administration.Interfaces;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Logic.Administration.FileImport
{
    public class FileImportFactory: IFileImportFactory
    {
        public AFileImport GetFileImport(FileImportTypeEnum fileImportType, IHttpContextAccessor httpContextAccessor, IDbContextFactory dbContextFactory)
        {
            switch (fileImportType)
            {
                case FileImportTypeEnum.Family:
                    return new FamilyFileImport(httpContextAccessor, dbContextFactory);
                default:
                    throw new NotImplementedException($"File import type '{fileImportType}' is not implemented.");
            }
        }
    }
}
