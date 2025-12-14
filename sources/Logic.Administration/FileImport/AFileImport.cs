using Data.Entities;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.Models;
using System.Text;

namespace Logic.Administration.FileImport
{
    public abstract class AFileImport : LogicBase
    {
        private IUnitOfWork _unitOfWork;

        protected IUnitOfWork UnitOfWork => _unitOfWork;
        protected AFileImport(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork) : base(httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
        }

        public abstract Task<int> Import(string fileContent, string fileName);

        public abstract Task<FileResponse> GetFile(FileImportTypeEnum fileType);

        public async Task SaveImportFile(FileImportTypeEnum fileType, string fileName, string fileContent, ImportStatusEnum status)
        {
            var filenameParts = Path.GetFileNameWithoutExtension(fileName).Split('_');

            if(DateTime.TryParseExact(filenameParts.Last(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime fileDate))
            {
                var existing = await _unitOfWork.ImportFileRepository.GetByAsync(i => i.FileName == fileName && i.FileDate == fileDate && i.FileType == fileType);

                if (existing != null)
                {
                    existing.FileContent = Encoding.UTF8.GetBytes(fileContent);
                    existing.Status = status;
                    _unitOfWork.ImportFileRepository.Update(existing);

                    return;
                }
                else
                {
                    await _unitOfWork.ImportFileRepository.AddAsync(new ImportFileEntity
                    {
                        FileName = fileName,
                        FileType = fileType,
                        FileContent = Encoding.UTF8.GetBytes(fileContent),
                        Status = status
                    });
                }

                await _unitOfWork.SaveChangesAsync(CurrentUser.UserName);
            }
            else
            {
                await _unitOfWork.LogMessage(new Data.Entities.LogMessageEntity
                {
                    Message = $"Failed to save import file {fileName} - invalid date format.",
                    CreatedBy = CurrentUser.UserName,
                    CreatedAt = DateTime.UtcNow,
                    LogLevel = LogLevelEnum.Error
                }, true);

                throw new FormatException("Invalid date format in file name.");
            }
        }

        protected (string? fileName, string? base64String) GetFileTemplate(FileImportTypeEnum fileType)
        {
            byte[] file;

            switch (fileType)
            {
                case FileImportTypeEnum.Family:
                    file = Resx.Files.FamilyImportTemplate;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileType));
            }

            if (file == null || file.Length == 0)
            {
                return (null, null);
            }

            return ("FamilyImport_FamilyName_YYYYMMDD.json", Convert.ToBase64String(file));
        }

       
    }
}
