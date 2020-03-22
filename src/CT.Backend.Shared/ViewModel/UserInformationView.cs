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
            Name = baseModel.Name;
            Id = baseModel.Id;            
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
