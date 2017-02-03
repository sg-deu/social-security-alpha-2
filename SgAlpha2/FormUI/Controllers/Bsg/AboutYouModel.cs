using System;

namespace FormUI.Controllers.Bsg
{
    public class AboutYouModel
    {
        public string   Title                       { get; set; }

        public string   FirstName                   { get; set; }

        public string   OtherNames                  { get; set; }

        public string   SurnameOrFamilyName         { get; set; }

        public DateTime DateOfBirth                 { get; set; }

        public string   NationalInsuranceNumberText { get; set; }
    }
}