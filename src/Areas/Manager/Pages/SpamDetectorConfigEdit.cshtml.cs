using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Piranha.Manager;

namespace Zon3.SpamDetector.Areas.Manager.Pages
{
    [Authorize(Policy = Permission.Config)]
    public class SpamDetectorConfigEdit : PageModel
    {
    }
}