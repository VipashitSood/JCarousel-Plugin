﻿using FluentValidation;
using Nop.Plugin.Widgets.JCarousel.Models.Configuration;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Widgets.JCarousel.Validators
{
    public class JcarouselValidator : BaseNopValidator<JCarouselModel>
    {
        public JcarouselValidator(ILocalizationService localizationService)
        {
            //set validation rules
            RuleFor(model => model.Name)
                .NotEmpty()
                .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Widgets.Jcarousel.Fields.Name.Required"));
 
            RuleFor(x => x.MaxItems)
                .GreaterThanOrEqualTo(1)
                .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Widgets.Jcarousel.Fields.MaxItems.GreaterThanOrEqualZero"));
            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(1)
                .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Widgets.Jcarousel.Fields.DisplayOrder.GreaterThanOrEqualZero")); 
        }

    }
}
