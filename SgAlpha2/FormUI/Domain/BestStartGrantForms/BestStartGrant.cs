﻿using System;
using FormUI.Domain.BestStartGrantForms.Dto;
using FormUI.Domain.Forms;

namespace FormUI.Domain.BestStartGrantForms
{
    public class BestStartGrant : Form
    {
        public BestStartGrant() : base(Guid.NewGuid().ToString())
        {
        }

        public AboutYou AboutYou { get; set; }
    }
}