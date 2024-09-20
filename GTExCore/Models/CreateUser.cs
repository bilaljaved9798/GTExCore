using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GTCore.Models
{
    public class CreateUser : CustomValidation
    {

        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public decimal AccountBalance { get; set; }

        public int UserTypeID { get; set; }
        public int CreatedbyID { get; set; }
        public string AgentRateC { get; set; }
        public decimal BetLowerLimit { get; set; } = 2000;
        public decimal BetUpperLimit { get; set; } = 25000;
        public decimal BetLowerLimitHorsePlace { get; set; } = 2000;
        public decimal BetUpperLimitHorsePlace { get; set; } = 25000;
        public decimal BetUpperLimitGrayHoundPlace { get; set; } = 25000;
        public decimal BetLowerLimitGrayHoundPlace { get; set; } = 2000;
        public decimal BetUpperLimitGrayHoundWin { get; set; } = 25000;
        public decimal BetLowerLimitGrayHoundWin { get; set; } = 2000;
        public decimal BetUpperLimitMatchOdds { get; set; } = 500000;
        public decimal BetLowerLimitMatchOdds { get; set; } = 2000;
        public decimal BetUpperLimitInningRuns { get; set; } = 500000;

        public decimal BetLowerLimitInningRuns { get; set; } = 2000;
        public decimal BetUpperLimitCompletedMatch { get; set; } = 500000;
        public decimal BetLowerLimitCompletedMatch { get; set; } = 2000;
        public bool isTennisAllowed { get; set; }
        public bool isSoccerAllowed { get; set; }
        public decimal BetUpperLimitMatchOddsSoccer { get; set; } = 500000;
        public decimal BetLowerLimitMatchOddsSoccer { get; set; } = 2000;
        public decimal BetUpperLimitMatchOddsTennis { get; set; } = 500000;
        public decimal BetLowerLimitMatchOddsTennis { get; set; } = 2000;
        public decimal BetUpperLimitTiedMatch { get; set; } = 500000;
        public decimal BetLowerLimitTiedMatch { get; set; } = 2000;
        public decimal BetUpperLimitWinner { get; set; } = 500000;
        public decimal BetLowerLimitWinner { get; set; } = 2000;

    }

}