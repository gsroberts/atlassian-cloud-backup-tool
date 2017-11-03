namespace AtlassianCloudBackupsLibrary
{
    using System.Collections.Generic;

    public class RepositoryCollection : List<BitBucketRepository>
    {
        public static RepositoryCollection CreateFromJson(string json)
        {
            var collection = new RepositoryCollection();



            return collection;
        }
    }
}
