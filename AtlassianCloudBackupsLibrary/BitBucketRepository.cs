namespace AtlassianCloudBackupsLibrary
{
    using Newtonsoft.Json.Linq;

    public class BitBucketRepository
    {
        public string Name { get; set; }
        public string SourceControlProvider { get; set; }
        public bool HasWiki { get; set; }
        public string CloneUrl { get; set; }
        public string SshCloneUrl { get; set; }

        public static BitBucketRepository Create(JToken token)
        {
            var result = new BitBucketRepository();
            
            dynamic jsonValues = token;

            result.Name = jsonValues.slug;
            result.SourceControlProvider = jsonValues.scm;
            result.HasWiki = jsonValues.has_wiki;

            var cloneUrls = jsonValues.links.clone;

            foreach (var child in cloneUrls.Children())
            {
                if (child.name == "https")
                {
                    result.CloneUrl = child.href;
                }

                if (child.name == "ssh")
                {
                    result.SshCloneUrl = child.href;
                }
            }

            return result;
        }
    }
}
