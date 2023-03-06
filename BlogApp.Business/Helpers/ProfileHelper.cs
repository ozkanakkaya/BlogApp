using AutoMapper;
using BlogApp.Business.Mapping;

namespace BlogApp.Business.Helpers
{
    public static class ProfileHelper
    {
        public static List<Profile> GetProfiles()
        {
            return new List<Profile>
            {
                new UserProfile(),
            };
        }
    }
}
