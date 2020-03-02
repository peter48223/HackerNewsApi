using System.Threading.Tasks;

namespace HackerNewsApi
{
    public interface IHttp
    {
        Task<int[]> GetStories();
        Task<Story> GetStory(int id);
    }
}
