using Piranha.Manager.Extend;

namespace Zon3.SpamDetector
{
    public static class Actions
    {
        public sealed class ToolbarActions
        {
            public ActionList<ToolbarAction> SpamDetectorConfigEdit { get; private set; } = new ActionList<ToolbarAction>
            {
                new ToolbarAction
                {
                    InternalId = "Info",
                    ActionView = "Partial/Actions/_SpamDetectorInfo"
                }
            };

            internal ToolbarActions() { }
        }

        public static ToolbarActions Toolbars { get; private set; } = new ToolbarActions();
    }
}