using Piranha.Manager.Extend;

namespace Zon3.SpamDetector
{
    /// <summary>
    /// Manager actions for SpamDetectorService.<
    /// </summary>
    public static class Actions
    {
        /// <summary>
        /// Manager toolbar actions for SpamDetectorService.
        /// </summary>
        public sealed class ToolbarActions
        {
            /// <summary>
            /// Gets the toolbar actions for the config edit view.
            /// </summary>
            public ActionList<ToolbarAction> SpamDetectorConfigEdit { get; } = new()
            {
                new ToolbarAction
                {
                    InternalId = "Info",
                    ActionView = "Partial/Actions/_SpamDetectorInfo"
                },
                new ToolbarAction
                {
                    InternalId = "Save",
                    ActionView = "Partial/Actions/_SpamDetectorConfigSave"
                }
            };

            internal ToolbarActions() { }
        }

        /// <summary>
        /// Static getter for the toolbar actions for the config edit view.
        /// </summary>
        public static ToolbarActions GetFor { get; } = new();
    }
}