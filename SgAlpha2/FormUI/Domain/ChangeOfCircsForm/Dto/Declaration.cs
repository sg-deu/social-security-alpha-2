﻿using System.ComponentModel;

namespace FormUI.Domain.ChangeOfCircsForm.Dto
{
    public class Declaration
    {
        [DisplayName("I agree with the 4 statements.")]
        public bool AgreedToLegalStatement { get; set; }
    }
}