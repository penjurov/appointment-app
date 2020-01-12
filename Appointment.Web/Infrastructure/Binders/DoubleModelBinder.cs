using System;
using System.Globalization;
using System.Web.Mvc;

namespace Appointment.Web.Infrastructure.Binders
{
    public class DoubleModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                if (!string.IsNullOrEmpty(valueResult.AttemptedValue))
                {
                    actualValue = Convert.ToDouble(valueResult.AttemptedValue, CultureInfo.InvariantCulture);
                }
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
}