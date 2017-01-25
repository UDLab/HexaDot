using System.Collections.Generic;
using HexaDot.Data.ViewModels.Institute;

namespace HexaDot.Data.ViewModels.Validators
{
    public class InstituteEditModelValidator : IInstituteEditModelValidator
    {
        public List<KeyValuePair<string, string>> Validate(InstituteEditViewModel model)
        {
            var result = new List<KeyValuePair<string, string>>();

            if (string.IsNullOrEmpty(model.WebUrl))
            {
                result.Add(new KeyValuePair<string, string>(nameof(model.WebUrl), "You must supply a URL of the Institute."));
            }

            return result;
        }
    }

    public interface IInstituteEditModelValidator
    {
        List<KeyValuePair<string, string>> Validate(InstituteEditViewModel model);
    }
}
