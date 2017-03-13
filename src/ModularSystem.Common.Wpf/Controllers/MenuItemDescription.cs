namespace ModularSystem.Common.Wpf.Controllers
{
    public struct MenuItemDescription
    {
        public string[] Path { get; set; }
        public int Order { get; set; }

        public MenuItemDescription(string path, int order)
        {
            Path = path.Split('/');
            Order = order;
        }
    }
}
