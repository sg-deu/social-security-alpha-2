namespace FormUI.Controllers.Helpers.Controls
{
    public abstract class InputControl : Control
    {
        public InputControl(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id    { get; protected set; }
        public string Name  { get; protected set; }
    }
}