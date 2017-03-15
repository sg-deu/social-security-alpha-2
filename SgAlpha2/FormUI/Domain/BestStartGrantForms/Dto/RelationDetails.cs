﻿using System;
using System.ComponentModel;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.Util.Attributes;

namespace FormUI.Domain.BestStartGrantForms.Dto
{
    public class RelationDetails : INationalInsuranceNumberHolder
    {
        public RelationDetails()
        {
            Address = new Address();
        }

        [DisplayName("Title")]
        public string       Title                   { get; set; }

        [DisplayName("Full name")]
        public string       FullName                { get; set; }

        [DisplayName("Date of Birth")]
        [HintText("For example, 18 03 1980")]
        public DateTime?    DateOfBirth             { get; set; }

        [DisplayName("National Insurance number")]
        [HintText("It's on your National Insurance Card, benefit letter, payslip or P60.<br/>For example, 'VO 12 34 56 D'")]
        [UiInputMask(InputMasks.NationalInsuranceNumber)]
        public string       NationalInsuranceNumber { get; set; }

        [DisplayName("Relationship to applicant")]
        public string       RelationshipToApplicant { get; set; }

        [DisplayName("Lives at the same address")]
        public bool         InheritAddress          { get; set; }

        public Address      Address                 { get; set; }

        public void CopyTo(RelationDetails dest, Part part)
        {
            switch (part)
            {
                case Part.Part1:
                    dest.Title = Title;
                    dest.FullName = FullName;
                    dest.DateOfBirth = DateOfBirth;
                    dest.NationalInsuranceNumber = NationalInsuranceNumber;
                    dest.RelationshipToApplicant = RelationshipToApplicant;
                    break;

                case Part.Part2:
                    dest.InheritAddress = InheritAddress;
                    dest.Address = !InheritAddress ? Address.Copy(Address) : null;
                    break;

                default:
                    throw new Exception("Unhandled part : " + part);
            }
        }
    }
}