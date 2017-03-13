﻿using FluentAssertions;
using FormUI.Domain.BestStartGrantForms.Commands;
using FormUI.Domain.BestStartGrantForms.Dto;
using NUnit.Framework;

namespace FormUI.Tests.Domain.BestStartGrantForms.Dto
{
    [TestFixture]
    public class RelationDetailsTests : AbstractTest
    {
        [Test]
        public void CopyTo_CoversAllDetails()
        {
            var src = RelationDetailsBuilder.NewValid(Part.Part2);
            var dest = new RelationDetails();

            src.CopyTo(dest, Part.Part1);

            dest.Title.Should().Be(src.Title);

            src.CopyTo(dest, Part.Part2);

            dest.ShouldBeEquivalentTo(src);
        }
    }
}