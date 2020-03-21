using CT.Backend.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CT.Backend.Shared.ViewModel
{
    public class UserInformationViewModel : UserInformation
    {
        public UserInformationViewModel()
        {

        }
        public UserInformationViewModel(UserInformation baseModel)
        {
            Email = baseModel.Email;
            FirstName = baseModel.FirstName;
            Id = baseModel.Id;
            LastName = baseModel.LastName;
            Location = baseModel.Location;
            RiskScore = baseModel.RiskScore;
            Source = baseModel.Source;
            Token = baseModel.Token;
            AppointmentToken = baseModel.AppointmentToken;
        }
        public string QuestionToken { get; set; }
        public new int? RiskScore {get;}
    }
}
