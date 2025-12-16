using Data.Entities;
using Data.Entities.Learning;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.Models.Import;
using System.Text.Json;

namespace Logic.Import.FileImport
{
    public class VocabularyFileImporter : AFileImport
    {
        public VocabularyFileImporter(IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork) : base(httpContextAccessor, unitOfWork) { }

        /// <summary>
        /// Imports vocabulary data from the specified file content and updates the database with new or modified
        /// entries.
        /// </summary>
        /// <remarks>If the file content is invalid or fails validation, the import is aborted and no
        /// changes are made to the database. The method logs information and errors related to the import process. No
        /// exceptions are thrown; errors are logged and the method returns <see langword="false"/> on
        /// failure.</remarks>
        /// <param name="fileContent">A string containing the JSON-formatted content of the VocabularyImportModel to import. Must not be null or empty.</param>
        /// <param name="fileName">The name of the file being imported. Used for logging and error reporting purposes. Must not be null or
        /// empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the import
        /// succeeds and the data is valid; otherwise, <see langword="false"/>.</returns>
        public override async Task<bool> ImportFile(string fileContent, string fileName, ImportFileEntity fileEntity)
        {
            try
            {
                var (isValidFileName, fileDate) = ValidateFileName(fileName);

                if (!isValidFileName || fileDate == null)
                {
                    fileEntity.Status = ImportStatusEnum.Failed;
                    UnitOfWork.ImportFileRepository.Update(fileEntity);

                    await LogError($"Vocabulary import from file '{fileName}' failed due to invalid file name format.");
 
                    return false;
                }

                var isDatabaseChanged = false;

                var serializerOptions = GetSerializerOptions(includeDateOptions: false);

                var importModel = JsonSerializer.Deserialize<VocabularyImportModel>(fileContent, serializerOptions);

                if (importModel == null || importModel.Vocabularies == null)
                {
                    fileEntity.Status = ImportStatusEnum.Failed;
                    UnitOfWork.ImportFileRepository.Update(fileEntity);

                    await LogError($"Vocabulary import from file '{fileName}' failed due to invalid file content.");

                    return false;
                }

                if (!ValidateVocabularyModel(importModel))
                {
                    fileEntity.Status = ImportStatusEnum.Failed;
                    UnitOfWork.ImportFileRepository.Update(fileEntity);

                    await LogError($"Vocabulary import from file '{fileName}' failed due to validation errors.");

                    return false;
                }

                foreach (var vocabulary in importModel.Vocabularies)
                {
                    var existingVocabulary = await UnitOfWork.LearningUnitOfWork.VocabularyRepository
                        .GetByAsync(x => x.IdExternal == vocabulary.IdExternal, true);

                    var isNew = existingVocabulary == null;

                    if (isNew)
                    {
                        // Add new vocabulary if required
                        var vocabularyEntity = GetVocabularyEntity(existingVocabulary, vocabulary);

                        if (vocabularyEntity == null) { continue; }

                        await UnitOfWork.LearningUnitOfWork.VocabularyRepository.AddAsync(vocabularyEntity);

                        isDatabaseChanged = true;

                    }
                    else
                    {
                        if (existingVocabulary == null) { continue; }

                        // Update existing vocabulary if required
                        var updatedVocabularyEntity = GetUpdatedVocabularyEntity(existingVocabulary, vocabulary);

                        if (updatedVocabularyEntity == null) { continue; }

                        UnitOfWork.LearningUnitOfWork.VocabularyRepository.Update(existingVocabulary);

                        isDatabaseChanged = true;
                    }
                }

                if (isDatabaseChanged)
                {
                    await LogInfo($"Vocabulary import from file '{fileName}' completed successfully.");

                    // Save import file record
                    fileEntity.Status = ImportStatusEnum.Success;
                    UnitOfWork.ImportFileRepository.Update(fileEntity);

                    await UnitOfWork.SaveChangesAsync(CurrentUser.UserName);
                }

                return true;
            }
            catch (Exception exception)
            {
                await LogError($"One or more errors occurred during vocabulary import: {exception.Message}", exception);

                return false;
            }
        }

        // expecting file name format: Vocabulary_import_YYYYMMDDHHMMSS.json
        private (bool isValid, DateTime? fileDate) ValidateFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return (false, null);
            }

            if (!fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                return (false, null);
            }

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

            var fileNameParts = fileNameWithoutExtension.Split('_');

            if (fileNameParts.Length != 3 || !fileNameParts[0].Equals("Vocabulary", StringComparison.OrdinalIgnoreCase))
            {
                return (false, null);
            }

            if (!DateTime.TryParseExact(fileNameParts[2], "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out var fileDate))
            {
                return (false, null);
            }

            return (true, fileDate);
        }

        private bool ValidateVocabularyModel(VocabularyImportModel? importModel)
        {
            if (importModel == null)
            {
                return false;
            }

            var existingExternalIds = importModel.Vocabularies
                .Select(v => new { Id = v.IdExternal })
                .GroupBy(x => x.Id)
                .ToList();

            if (existingExternalIds.Any(g => g.Count() > 1))
            {
                return false;
            }

            return true;
        }

        private VocabularyEntity GetVocabularyEntity(VocabularyEntity? existing, VocabularyModel vocabulary)
        {
            return new VocabularyEntity
            {
                IdExternal = vocabulary.IdExternal,
                DanishValue = vocabulary.DanishValue,
                DanishExampleSentence = vocabulary.DanishExampleSentence,
                DanishPhoneticSpelling = vocabulary.DanishPhoneticSpelling,
                EnglishValue = vocabulary.EnglishValue,
                EnglishExampleSentence = vocabulary.EnglishExampleSentence,
                EnglishPhoneticSpelling = vocabulary.EnglishPhoneticSpelling,
                FrechValue = vocabulary.FrechValue,
                FrenchExampleSentence = vocabulary.FrenchExampleSentence,
                FrenchPhoneticSpelling = vocabulary.FrenchPhoneticSpelling,
                GermanValue = vocabulary.GermanValue,
                GermanExampleSentence = vocabulary.GermanExampleSentence,
                GermanPhoneticSpelling = vocabulary.GermanPhoneticSpelling,
            };
        }

        private VocabularyEntity? GetUpdatedVocabularyEntity(VocabularyEntity existingVocabulary, VocabularyModel vocabulary)
        {
            bool isUpdated = false;
            if (existingVocabulary.DanishValue != vocabulary.DanishValue)
            {
                existingVocabulary.DanishValue = vocabulary.DanishValue;
                isUpdated = true;
            }
            if (existingVocabulary.DanishExampleSentence != vocabulary.DanishExampleSentence)
            {
                existingVocabulary.DanishExampleSentence = vocabulary.DanishExampleSentence;
                isUpdated = true;
            }
            if (existingVocabulary.DanishPhoneticSpelling != vocabulary.DanishPhoneticSpelling)
            {
                existingVocabulary.DanishPhoneticSpelling = vocabulary.DanishPhoneticSpelling;
                isUpdated = true;
            }
            if (existingVocabulary.EnglishValue != vocabulary.EnglishValue)
            {
                existingVocabulary.EnglishValue = vocabulary.EnglishValue;
                isUpdated = true;
            }
            if (existingVocabulary.EnglishExampleSentence != vocabulary.EnglishExampleSentence)
            {
                existingVocabulary.EnglishExampleSentence = vocabulary.EnglishExampleSentence;
                isUpdated = true;
            }
            if (existingVocabulary.EnglishPhoneticSpelling != vocabulary.EnglishPhoneticSpelling)
            {
                existingVocabulary.EnglishPhoneticSpelling = vocabulary.EnglishPhoneticSpelling;
                isUpdated = true;
            }
            if (existingVocabulary.FrechValue != vocabulary.FrechValue)
            {
                existingVocabulary.FrechValue = vocabulary.FrechValue;
                isUpdated = true;
            }
            if (existingVocabulary.FrenchExampleSentence != vocabulary.FrenchExampleSentence)
            {
                existingVocabulary.FrenchExampleSentence = vocabulary.FrenchExampleSentence;
                isUpdated = true;
            }
            if (existingVocabulary.FrenchPhoneticSpelling != vocabulary.FrenchPhoneticSpelling)
            {
                existingVocabulary.FrenchPhoneticSpelling = vocabulary.FrenchPhoneticSpelling;
                isUpdated = true;
            }
            if (existingVocabulary.GermanValue != vocabulary.GermanValue)
            {
                existingVocabulary.GermanValue = vocabulary.GermanValue;
                isUpdated = true;
            }
            if (existingVocabulary.GermanExampleSentence != vocabulary.GermanExampleSentence)
            {
                existingVocabulary.GermanExampleSentence = vocabulary.GermanExampleSentence;
                isUpdated = true;
            }
            if (existingVocabulary.GermanPhoneticSpelling != vocabulary.GermanPhoneticSpelling)
            {
                existingVocabulary.GermanPhoneticSpelling = vocabulary.GermanPhoneticSpelling;
                isUpdated = true;
            }

            if (!isUpdated) { return null; }

            return existingVocabulary;
        }

        private async Task LogInfo(string message)
        {
            await UnitOfWork.LogMessage(new LogMessageEntity
            {
                Message = message,
                ExeptionMessage = string.Empty,
                StackTrace = string.Empty,
                Module = nameof(FamilyFileImporter),
                LogLevel = LogLevelEnum.Info,
                TimeStamp = DateTime.UtcNow,
            }, false, CurrentUser.FamilyId);
        }

        private async Task LogError(string message, Exception? exception = null)
        {
            await UnitOfWork.LogMessage(new LogMessageEntity
            {
                Message = message,
                ExeptionMessage = exception?.Message ?? string.Empty,
                StackTrace = exception?.StackTrace ?? string.Empty,
                Module = nameof(FamilyFileImporter),
                LogLevel = LogLevelEnum.Error,
                TimeStamp = DateTime.UtcNow,
            }, true, CurrentUser.FamilyId);
        }
    }
}
