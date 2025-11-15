
namespace PocViseu.Model.ModelView
{
    public class LogSystemModelView
    {
        public string? Description { set; get; }

        public string? Value { set; get; }

        public Int32? Level { get; set; } = 0;

        public string? TraceKey { set; get; }

        //[JsonIgnore]
        //public string? DateInit { set; get; }

        //[JsonIgnore]
        //public string? DateEnd { set; get; }
    }

}
