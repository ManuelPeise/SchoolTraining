using Logic.Import.FileImport.Interfaces;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;

namespace Logic.Import.FileImport
{
    public class FileImporterFactory : IFileImporterFactory
    {
        public AFileImport GetFileImporter(
            FileImportTypeEnum fileImportType, 
            IHttpContextAccessor httpContextAccessor, 
            IUnitOfWork unitOfWork)
        {
            switch (fileImportType)
            {
                case FileImportTypeEnum.Family:
                    return new FamilyFileImporter(httpContextAccessor, unitOfWork);
                case FileImportTypeEnum.Vocabulary:
                    return new VocabularyFileImporter(httpContextAccessor, unitOfWork);
                default:
                    throw new NotImplementedException($"File import type '{fileImportType}' is not implemented.");
            }
        }
    }
}
