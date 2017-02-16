﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FluentAssertions;
using FormUI.App_Start;
using NUnit.Framework;

namespace FormUI.Tests.App_Start
{
    [TestFixture]
    public class BinderTests
    {
        [Test]
        public void Bind()
        {
            Bind("01", "02", "2003").Should().Be(new DateTime(2003, 02, 01));
            Bind("02", "01", "2003").Should().Be(new DateTime(2003, 01, 02));
            Bind("31", "12", "2003").Should().Be(new DateTime(2003, 12, 31));

            Bind(null, "02", "2003").Should().BeNull();
            Bind("01", null, "2003").Should().BeNull();
            Bind("01", "02", null).Should().BeNull();

            Bind("xx", "02", "2003").Should().BeNull();
            Bind("01", "xx", "2003").Should().BeNull();
            Bind("01", "02", "xxxx").Should().BeNull();

            Bind("00", "02", "2003").Should().BeNull();
            Bind("31", "02", "2003").Should().BeNull();
            Bind("01", "00", "2003").Should().BeNull();
            Bind("01", "13", "2003").Should().BeNull();
        }

        [Test]
        public void AddsModelErrors()
        {
            BindState("01", "02", "2003").IsValid.Should().BeTrue();
            BindState("", "", "").IsValid.Should().BeTrue("it is valid to leave all three empty in case the entry is optional");

            BindState("x", "02", "2003").IsValid.Should().BeFalse("should report day as invalid");
            BindState("01", "x", "2003").IsValid.Should().BeFalse("should report month as invalid");
            BindState("01", "02", "x").IsValid.Should().BeFalse("should report year as invalid");
        }

        private ModelStateDictionary BindState(string dayText, string monthText, string yearText)
        {
            var ct = new ControllerContext();

            var bc = NewModelBindingContext();
            bc.ModelName = "TestDateTime";
            bc.ValueProvider = new DummyValueProvider(bc.ModelName, dayText, monthText, yearText);

            var binder = new Binder();
            var nullableDateTime = binder.BindModel(ct, bc);

            return bc.ModelState;
        }

        private DateTime? Bind(string dayText, string monthText, string yearText)
        {
            var ct = new ControllerContext();

            var bc = NewModelBindingContext();
            bc.ModelName = "TestDateTime";
            bc.ValueProvider = new DummyValueProvider(bc.ModelName, dayText, monthText, yearText);

            var binder = new Binder();
            var nullableDateTime = binder.BindModel(ct, bc);

            if (nullableDateTime == null)
                return null;

            return (DateTime)nullableDateTime;
        }

        private class DummyValueProvider : IValueProvider
        {
            private IDictionary<string, string> _values = new Dictionary<string, string>();

            public DummyValueProvider(string modelName, string day, string month, string year)
            {
                if (day != null)
                    _values.Add(modelName + "_day", day);

                if (month != null)
                    _values.Add(modelName + "_month", month);

                if (year != null)
                    _values.Add(modelName + "_year", year);
            }

            public bool ContainsPrefix(string prefix)
            {
                return _values.ContainsKey(prefix);
            }

            public ValueProviderResult GetValue(string key)
            {
                return new ValueProviderResult(null, _values[key], null);
            }
        }

        private ModelBindingContext NewModelBindingContext()
        {
            return new ModelBindingContext { ModelMetadata = new ModelMetadata(new FakeModelMetadataProvider(), null, null, typeof(DateTime?), null) };
        }

        private class FakeModelMetadataProvider : ModelMetadataProvider
        {
            public override IEnumerable<ModelMetadata> GetMetadataForProperties(object container, Type containerType)
            {
                return null;
            }

            public override ModelMetadata GetMetadataForProperty(Func<object> modelAccessor, Type containerType, string propertyName)
            {
                return null;
            }

            public override ModelMetadata GetMetadataForType(Func<object> modelAccessor, Type modelType)
            {
                return null;
            }
        }
    }
}