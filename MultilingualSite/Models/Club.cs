using System.ComponentModel.DataAnnotations;

namespace MultilingualSite.Models
{
    public class Club
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                 ErrorMessageResourceName = "NameRequired")]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resource))]
        public string? Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
               ErrorMessageResourceName = "CityRequired")]
        [Display(Name = "City", ResourceType = typeof(Resources.Resource))]
        public string? City { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource),
                ErrorMessageResourceName = "CountryRequired")]
        [Display(Name = "Country", ResourceType = typeof(Resources.Resource))]
        public string? Country { get; set; }
    }
}