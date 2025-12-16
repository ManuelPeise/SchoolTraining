using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Logic.Import.FileImport
{
    public abstract class AFileImport : LogicBase
    {
        private IUnitOfWork _unitOfWork;

        protected IUnitOfWork UnitOfWork => _unitOfWork;
        protected AFileImport(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork) : base(httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
        }

        public abstract Task<bool> ImportFile(string fileContent, string fileName);

        public JsonSerializerOptions GetSerializerOptions(bool includeDateOptions)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip
            };
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            if (includeDateOptions)
            {
                options.Converters.Add(new FlexibleDateTimeConverter());
            }

            return options;
        }
    }
}
