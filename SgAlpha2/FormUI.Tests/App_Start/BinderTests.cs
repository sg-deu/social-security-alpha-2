using System;
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
        public void BindDate()
        {
            BindDate("01", "02", "2003").Should().Be(new DateTime(2003, 02, 01));
            BindDate("02", "01", "2003").Should().Be(new DateTime(2003, 01, 02));
            BindDate("31", "12", "2003").Should().Be(new DateTime(2003, 12, 31));

            BindDate(null, "02", "2003").Should().BeNull();
            BindDate("01", null, "2003").Should().BeNull();
            BindDate("01", "02", null).Should().BeNull();

            BindDate("xx", "02", "2003").Should().BeNull();
            BindDate("01", "xx", "2003").Should().BeNull();
            BindDate("01", "02", "xxxx").Should().BeNull();

            BindDate("00", "02", "2003").Should().BeNull();
            BindDate("31", "02", "2003").Should().BeNull();
            BindDate("01", "00", "2003").Should().BeNull();
            BindDate("01", "13", "2003").Should().BeNull();
        }

        [Test]
        public void BindDate_AddsModelErrors()
        {
            BindDateState("01", "02", "2003").IsValid.Should().BeTrue();
            BindDateState("", "", "").IsValid.Should().BeTrue("it is valid to leave all three empty in case the entry is optional");

            BindDateState("x", "02", "2003").IsValid.Should().BeFalse("should report day as invalid");
            BindDateState("01", "x", "2003").IsValid.Should().BeFalse("should report month as invalid");
            BindDateState("01", "02", "x").IsValid.Should().BeFalse("should report year as invalid");
        }

        [Test]
        public void BindInt()
        {
            BindInt("1").Should().Be(1);
            BindInt("01").Should().Be(1);

            BindInt("").Should().BeNull();
            BindInt("x").Should().BeNull();
        }

        [Test]
        public void BindInt_AddsModelErrors()
        {
            BindIntState("01").IsValid.Should().BeTrue();
            BindIntState("").IsValid.Should().BeTrue();

            BindIntState("x").IsValid.Should().BeFalse();
        }

        private ModelStateDictionary BindDateState(string dayText, string monthText, string yearText)
        {
            var ct = new ControllerContext();

            var bc = NewModelBindingContext();
            bc.ModelName = "TestDateTime";
            bc.ValueProvider = new DummyValueProvider(bc.ModelName, dayText, monthText, yearText);

            var binder = new Binder.DateTimeBinder();
            var nullableDateTime = binder.BindModel(ct, bc);

            return bc.ModelState;
        }

        private ModelStateDictionary BindIntState(string intText)
        {
            var ct = new ControllerContext();

            var bc = NewModelBindingContext();
            bc.ModelName = "TestInt";
            bc.ValueProvider = new DummyValueProvider(bc.ModelName, intText);

            var binder = new Binder.IntBinder();
            var nullableDateTime = binder.BindModel(ct, bc);

            return bc.ModelState;
        }

        private DateTime? BindDate(string dayText, string monthText, string yearText)
        {
            var ct = new ControllerContext();

            var bc = NewModelBindingContext();
            bc.ModelName = "TestDateTime";
            bc.ValueProvider = new DummyValueProvider(bc.ModelName, dayText, monthText, yearText);

            var binder = new Binder.DateTimeBinder();
            var nullableDateTime = binder.BindModel(ct, bc);

            return (nullableDateTime != null) ? (DateTime?)nullableDateTime : null;
        }

        private int? BindInt(string intText)
        {
            var ct = new ControllerContext();

            var bc = NewModelBindingContext();
            bc.ModelName = "TestInt";
            bc.ValueProvider = new DummyValueProvider(bc.ModelName, intText);

            var binder = new Binder.IntBinder();
            var nullableInt = binder.BindModel(ct, bc);

            return (nullableInt != null) ? (int?)nullableInt : null;
        }

        private class DummyValueProvider : IValueProvider
        {
            private IDictionary<string, string> _values = new Dictionary<string, string>();

            public DummyValueProvider(string modelName, string value)
            {
                _values.Add(modelName, value);
            }

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
