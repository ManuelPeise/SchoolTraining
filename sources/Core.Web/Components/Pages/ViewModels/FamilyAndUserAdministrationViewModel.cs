using Core.Web.Components.Pages.UIModels;
using Core.Web.Providers;
using Core.Web.ViewModels;
using Logic.Administration.Interfaces;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Shared.Models.Administration;
using System.Text.Json;

namespace Core.Web.Components.Pages.ViewModels
{
    public class FamilyAndUserAdministrationViewModel : ViewModelBase
    {
        private readonly IApiHttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public ListItem<FamilyModel> SelectedFamily { get; set; } = new();
        private List<ListItem<FamilyModel>> _familyListItems = new();
        public List<ListItem<FamilyModel>> FamilyListItems { get; set; } = new();

        public FamilyAndUserAdministrationViewModel(
            IApiHttpClient httpClient,
            AuthenticationStateProvider authenticationStateProvider,
            IFamilyAdministrationService familyAdministrationService)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public override async Task InitializeAsync()
        {
            var families = await LoadFamiliesFromApi();

            _familyListItems = ConvertFamiliesToListItemCollectionl(families);

            FamilyListItems = _familyListItems;
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

        public void OnFilterTextChanged(ChangeEventArgs e)
        {
            var listitemCopy = _familyListItems;
            var filterText = e.Value?.ToString()?.ToLower() ?? string.Empty;

            FamilyListItems = string.IsNullOrEmpty(filterText) ? 
                _familyListItems : 
                _familyListItems
                .Where(f => f.Model != null && f.Model.Name.ToLower().StartsWith(filterText, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public void OnSelectFamily(int id)
        {
            FamilyListItems.ForEach(f => f.ClassName = "list-group-item");
            
            var selected = FamilyListItems.FirstOrDefault(f => f.Id == id)?? new ListItem<FamilyModel>();
            selected.ClassName = "list-group-item active";

            SelectedFamily = selected;
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

        private List<ListItem<FamilyModel>> ConvertFamiliesToListItemCollectionl(List<FamilyModel> families)
        {
            var collection = new List<ListItem<FamilyModel>>();
            
            families.ForEach(family =>
            {
                collection.Add(new ListItem<FamilyModel>
                {
                    Id = family.FamilyId,
                    Model = family
                });
            });

            // return GenerateDummyFamilies(20);

            return collection;
        }

        /// <summary>
        /// Generates a list of dummy family items for testing or demonstration purposes.
        /// </summary>
        /// <param name="count">The number of family items to generate. Must be greater than or equal to 0.</param>
        /// <returns>A list containing the specified number of dummy family items. The list will be empty if count is 0.</returns>
        private List<ListItem<FamilyModel>> GenerateDummyFamilies(int count)
        {
            var list = new List<ListItem<FamilyModel>>(capacity: count);
            var rnd = new Random();

            for (var i = 1; i <= count; i++)
            {
                list.Add(new ListItem<FamilyModel>
                {
                    Id = i,
                    Model = new FamilyModel
                    {
                        FamilyId = i,
                        Name = $"Family {i}",
                        ContactMailAddress = $"family-{i}@gmx.de"
                    }
                });
            }

            return list;
        }

        private async Task<string?> GetJwtToken()
        {
            var token = await ((CustomAuthenticationStateProvider)_authenticationStateProvider).GetJwtTokenHeader();

            return token;
        }
    }
}
