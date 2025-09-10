using System.ComponentModel.DataAnnotations;

namespace Constants.Enums;
public enum BnsLocationType : byte
{
    [Display(Name = "область")] Region = 0,
    [Display(Name = "город")] City = 1,
    [Display(Name = "село")] Village = 2,
    [Display(Name = "город. адм")] CityAdm = 3,
    [Display(Name = "район")] District = 4
}
