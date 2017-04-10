using System.ComponentModel;

namespace FormUI.Controllers.Coc
{
    public class IdentityModel : NavigableModel
    {
        // GET
        public string VerifyPath;

        // POST
        [DisplayName("Email address")]
        public string Email { get; set; }
    }
}