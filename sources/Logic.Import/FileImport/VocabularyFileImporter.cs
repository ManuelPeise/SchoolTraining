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

        public override async Task<bool> ImportFile(string fileContent, string fileName)
        {
            try
            {
                var isDatabaseChanged = false;

                var serializerOptions = GetSerializerOptions(includeDateOptions: false);

                var vocabularies = JsonSerializer.Deserialize<List<VocabularyModel>>(fileContent, serializerOptions);

                foreach (var vocabulary in vocabularies!)
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
