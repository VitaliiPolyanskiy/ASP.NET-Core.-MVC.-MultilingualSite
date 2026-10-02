using MultilingualSite.Models;

namespace MultilingualSite.Services
{
    public interface ILangRead
    {
        List<Language> GetLanguageList();
    }

    public class ReadLangServices : ILangRead
    {
        private readonly List<Language> _languageLists;

        public ReadLangServices(IConfiguration con)
        {
            _languageLists = con.GetSection("Lang").GetChildren()
                .Select(lang => new Language
                {
                    ShortName = lang.Key,
                    Name = lang.Value
                })
                .Where(l => l.Name != null)
                .ToList();
        }

        public List<Language> GetLanguageList() => _languageLists;
    }
}