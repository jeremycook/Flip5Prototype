using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Humanizer;

namespace Flip.Areas.Core
{
    public class ConventionalModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            ModelMetadata modelMetadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

            if (modelMetadata.DisplayName == null)
            {
                modelMetadata.DisplayName = (modelMetadata.PropertyName ?? modelMetadata.DataTypeName ?? modelMetadata.ModelType.Name).Humanize();
            }

            return modelMetadata;
        }
    }
}