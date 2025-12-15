using Core.Web.Providers;
using Core.Web.ViewModels;
using Logic.Administration.Interfaces;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Shared.Models.Administration;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Core.Web.Components.Pages.ViewModels
{
    public class FamilyAdministrationViewModel : ViewModelBase
    {
        private readonly IApiHttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        private List<FamilyModel> _originalFamilies = new(); // immutable snapshot (do not mutate)
        private List<FamilyModel> _workingFamilies = new();  // deep-cloned list that UI binds to and mutates
        private readonly HashSet<int> _modifiedFamilyIds = new();
        private bool _isModified;

        public bool SaveCancelButtonsDisabled => !_isModified;
        public List<FamilyModel> Families { get; private set; } = new(); // filtered view over _workingFamilies

        public FamilyAdministrationViewModel(
            IApiHttpClient httpClient,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public override async Task InitializeAsync()
        {
            // Load original data and keep it untouched for comparison
            _originalFamilies = await LoadFamiliesFromApi();

            // Create a deep copy for working instances to avoid mutating originals
            _workingFamilies = _originalFamilies.Select(CloneFamily).ToList();

            // Start with the full working set visible
            Families = _workingFamilies.ToList();

            _modifiedFamilyIds.Clear();
            _isModified = false;
        }

        public void HandleCheckedChanged(int familyId)
        {
            var current = _workingFamilies.FirstOrDefault(f => f.FamilyId == familyId);
            if (current == null) return;

            current.IsActive = !current.IsActive;

            var original = _originalFamilies.FirstOrDefault(f => f.FamilyId == familyId);
            if (original == null) return;

            if (current.IsActive != original.IsActive)
            {
                _modifiedFamilyIds.Add(familyId);
            }
            else
            {
                _modifiedFamilyIds.Remove(familyId);
            }

            _isModified = _modifiedFamilyIds.Count > 0;
        }

        public async Task SaveChanges()
        {
            try
            {
                SetIsLoading(true);

                var modifiedFamilies = _workingFamilies
                    .Where(f => _modifiedFamilyIds.Contains(f.FamilyId))
                    .ToList();

                var response = await _httpClient.PostAsync<List<FamilyModel>>("api/familyadministration/updatefamilies", JsonSerializer.Serialize(modifiedFamilies));

                if (response.IsSuccess)
                {
                    _originalFamilies = response.Data ?? new List<FamilyModel>();

                    _workingFamilies = _originalFamilies.Select(CloneFamily).ToList();

                    Families = _workingFamilies.ToList();

                    _modifiedFamilyIds.Clear();
                    _isModified = false;
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
            finally
            {
                SetIsLoading(false);
            }
        }

        public void ResetChanges()
        {
            var originalLookup = _originalFamilies.ToDictionary(f => f.FamilyId);
            foreach (var f in _workingFamilies)
            {
                if (originalLookup.TryGetValue(f.FamilyId, out var original))
                {
                    f.IsActive = original.IsActive;
                }
            }

            var workingLookup = _workingFamilies.ToDictionary(f => f.FamilyId);
            for (int i = 0; i < Families.Count; i++)
            {
                var id = Families[i].FamilyId;
                if (workingLookup.TryGetValue(id, out var wf))
                {
                    Families[i].IsActive = wf.IsActive;
                }
            }

            _modifiedFamilyIds.Clear();
            _isModified = false;
        }

        public async Task HandleSelectFile(InputFileChangeEventArgs e)
        {
            try
            {
                SetIsLoading(true);

                var token = await GetJwtToken();
                _httpClient.EnsureAthenticationToken(token);

                var file = e.File;

                using (var stream = file.OpenReadStream())
                using (var reader = new StreamReader(stream))
                {
                    var model = new FileUploadModel
                    {
                        FileName = file.Name,
                        JsonContent = await reader.ReadToEndAsync()
                    };

                    var body = JsonSerializer.Serialize(model);

                    var response = await _httpClient.PostAsync<FileUploadModel>(
                        "api/familyadministration/uploadfamilytemplatefile", body, "application/json");
                }
                // Handle success/error based on response.IsSuccess if needed
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
            finally
            {
                SetIsLoading(false);
            }
        }

        public void OnFilterTextChanged(ChangeEventArgs e)
        {
            var filterText = e.Value?.ToString()?.Trim().ToLower() ?? string.Empty;

            Families = string.IsNullOrWhiteSpace(filterText)
                ? _workingFamilies.ToList()
                : _workingFamilies
                    .Where(f => f.Name?.ToLower().StartsWith(filterText, StringComparison.Ordinal) == true)
                    .ToList();
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

        private static FamilyModel CloneFamily(FamilyModel f)
        {
            return new FamilyModel
            {
                FamilyId = f.FamilyId,
                Name = f.Name,
                ContactMailAddress = f.ContactMailAddress,
                LastUdateBy = f.LastUdateBy,
                LastUpdateAt = f.LastUpdateAt,
                IsActive = f.IsActive
            };
        }

    }
}
