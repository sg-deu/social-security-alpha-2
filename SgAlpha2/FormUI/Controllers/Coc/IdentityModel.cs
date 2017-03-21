using System.ComponentModel;

namespace FormUI.Controllers.Coc
{
    public class IdentityModel : NavigableModel
    {
        // POST
        [DisplayName("Email address")]
        public string Email { get; set; }
    }
}