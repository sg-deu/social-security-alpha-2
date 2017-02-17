using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FormUI.Domain.Util;

namespace FormUI.App_Start
{
    public class MetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var metaData = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

            var hint = attributes.OfType<HintTextAttribute>().FirstOrDefault();

            if (hint != null)
                metaData.AdditionalValues.Add("HintText", hint.HintText);

            return metaData;
        }
    }
}