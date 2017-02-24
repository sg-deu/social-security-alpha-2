using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FormUI.Domain.Util.Attributes;

namespace FormUI.App_Start
{
    public static class Metadata
    {
        public const string HintText    = "HintText";
        public const string MaxLength   = "MaxLength";
        public const string InputMask   = "InputMask";
    }

    public class MetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var metaData = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
            var attributeList = attributes.ToList();

            Each<HintTextAttribute>(attributeList, a =>
                metaData.AdditionalValues.Add(Metadata.HintText, a.HintText));

            Each<UiLengthAttribute>(attributeList, a =>
                metaData.AdditionalValues.Add(Metadata.MaxLength, a.MaxLength));

            Each<UiInputMaskAttribute>(attributeList, a =>
                metaData.AdditionalValues.Add(Metadata.InputMask, a.InputMask));

            return metaData;
        }

        private static void Each<T>(IList<Attribute> attributes, Action<T> action)
        {
            attributes.OfType<T>().ToList().ForEach(action);
        }
    }
}