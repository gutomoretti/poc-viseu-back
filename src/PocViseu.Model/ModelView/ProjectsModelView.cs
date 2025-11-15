using System.Text.Json.Serialization;

namespace PocViseu.Model.ModelView
{
    public class ProjectsModelView
    {
        public Int64? Id { get; set; }

        public string? Customer { set; get; }

        public List<string?> Projects { set; get; }

        [JsonIgnore]
        public Int64? CustomerId { get; set; }
    }
}
