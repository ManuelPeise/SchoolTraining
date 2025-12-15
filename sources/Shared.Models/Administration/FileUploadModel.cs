using Microsoft.AspNetCore.Components.Forms;

namespace Shared.Models.Administration
{
    public class FileUploadModel
    {
        public string FileName { get; set; } = string.Empty;
        public string? JsonContent { get; set; }
    }
}
