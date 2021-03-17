using Piranha.Models;
using System.Threading.Tasks;

namespace Zon3.SpamDetector
{
    public interface ISpamDetector
    {
        public Task<CommentReview> ReviewAsync(Comment comment);
    }
}
