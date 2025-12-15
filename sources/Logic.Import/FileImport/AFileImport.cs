using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;

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
    }
}
