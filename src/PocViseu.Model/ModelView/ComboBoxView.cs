using Microsoft.EntityFrameworkCore;

namespace PocViseu.Model.ModelView
{
    [Keyless]
    public class ComboBoxView
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Param { get; set; }
    }
}
