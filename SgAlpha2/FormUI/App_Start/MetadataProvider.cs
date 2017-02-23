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
    }

    public class MetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var metaData = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

            var hint = attributes.OfType<HintTextAttribute>().FirstOrDefault();

            if (hint != null)
                metaData.AdditionalValues.Add(Metadata.HintText, hint.HintText);

            var uiLength = attributes.OfType<UiLengthAttribute>().FirstOrDefault();

            if (uiLength != null)
                metaData.AdditionalValues.Add(Metadata.MaxLength, uiLength.MaxLength);

            return metaData;
        }
    }
}