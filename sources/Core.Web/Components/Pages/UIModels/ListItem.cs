namespace Core.Web.Components.Pages.UIModels
{
    public class ListItem<TModel> where TModel : class
    {
        public int Id { get; set; }
        public TModel? Model { get; set; }
        public string ClassName { get; set; } = "list-group-item";
    }
}
