namespace FormUI.Controllers.Helpers.Controls
{
    public abstract class InputControl : Control
    {
        protected ControlContext ControlContext;

        public InputControl(ControlContext controlContext)
        {
            ControlContext = controlContext;
        }
    }
}