using Core.Web.Providers;
using Core.Web.ViewModels;
using Logic.Administration.Interfaces;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Shared.Models.Administration;
using System.Text.Json;

namespace Core.Web.Components.Pages.ViewModels
{
    public class FamilyAndUserViewModel : ViewModelBase
    {
        private readonly IApiHttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public List<FamilyModel> Families { get; set; } = new();

        public FamilyAndUserViewModel(
            IApiHttpClient httpClient,
            AuthenticationStateProvider authenticationStateProvider,
            IFamilyAdministrationService familyAdministrationService)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public override async Task InitializeAsync()
        {
            Families = await LoadFamiliesFromApi();
        }

        public async Task HandleSelectFile(InputFileChangeEventArgs e)
        {
            try
            {
                SetIsLoading(true);

                var token = await GetJwtToken();

                _httpClient.EnsureAthenticationToken(token);

                var model = new FileUploadModel
                {
                    File = e.File
                };

                var response = await _httpClient.PostAsync<FileUploadModel>("api/familyadministration/uploadfamilytemplatefile", JsonSerializer.Serialize(model));

                if (response.IsSuccess)
                {
                    // Handle success if needed
                }
                else
                {
                    // Handle error if needed
                }
            }
            finally
            {
                SetIsLoading(true);
            }
        }

        public async Task<FileDownloadModel?> GetFileFromServer()
        {
            try
            {
                SetIsLoading(true);

                var token = await GetJwtToken();

                _httpClient.EnsureAthenticationToken(token);

                var response = await _httpClient.GetAsync<FileDownloadModel>("api/familyadministration/downloadfamilyimporttemplate");

                return response?.Data ?? null;
            }
            finally
            {
                SetIsLoading(false);
            }
        }

        private async Task<List<FamilyModel>> LoadFamiliesFromApi()
        {
            try
            {
                SetIsLoading(true);

                var token = await GetJwtToken();

                _httpClient.EnsureAthenticationToken(token);

                var response = await _httpClient.GetAsync<List<FamilyModel>>("api/familyadministration/getfamilies");

                return response.Data ?? new List<FamilyModel>();
            }
            finally
            {
                SetIsLoading(false);
            }
        }

        private async Task<string?> GetJwtToken()
        {
            var token = await ((CustomAuthenticationStateProvider)_authenticationStateProvider).GetJwtTokenHeader();

            return token;
        }
    }
}
