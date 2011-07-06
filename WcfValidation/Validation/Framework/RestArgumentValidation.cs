using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel.Web;
using FluentValidation;
using FluentValidation.Results;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using WcfValidation.Validation.Attributes;

namespace WcfValidation.Validation.Framework
{
    [Serializable]
    [ProvideAspectRole(StandardRoles.Validation)]
    public sealed class RestArgumentValidation : MethodInterceptionAspect
    {
        private readonly List<ParameterValidation> _validationRules = new List<ParameterValidation>();

        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            foreach (var parameterInfo in method.GetParameters())
            {
                var validationAttribute = (from attributeType in parameterInfo.GetCustomAttributes(true)
                                           where typeof(RestArgumentValidatorBase).IsAssignableFrom(attributeType.GetType())
                                           select attributeType).Cast<RestArgumentValidatorBase>().FirstOrDefault();

                if(validationAttribute != null)
                {
                    _validationRules.Add(new ParameterValidation
                                             {
                                                 Parameter = parameterInfo,
                                                 DefaultValue = validationAttribute.DefaultValue,
                                                 ValidationRules = validationAttribute.Validators
                                             });
                }
            }
        }

        public override void OnInvoke(MethodInterceptionArgs args)
        {
            var errorMessage = new List<ErrorMessage>();            
            Arguments arguments = args.Arguments;

            foreach (var parameterValidation in _validationRules)
            {
                foreach (var validatorType in parameterValidation.ValidationRules)
                {
                    var result = ValidateArgument(validatorType, arguments[parameterValidation.Parameter.Position]);

                    if (result != null && !parameterValidation.HasDefaultValue)
                    {
                        var message = new ErrorMessage
                                          {
                                              ParameterName = parameterValidation.Parameter.Name
                                          };
                        foreach (var validationFailure in result)
                        {
                            message.Errors.Add(validationFailure.ToString());
                        }
                        errorMessage.Add(message);
                    }
                    else if(result != null)
                    {
                        args.Arguments.SetArgument(parameterValidation.Parameter.Position, parameterValidation.DefaultValue());
                    }
                }
            }

            if(errorMessage.Any())
            {
                throw new WebFaultException<List<ErrorMessage>>(errorMessage, HttpStatusCode.BadRequest);                
            }

            base.OnInvoke(args);
        }


        static IEnumerable<ValidationFailure> ValidateArgument(Type validatorType, object argument)
        {
            IValidator validator = (IValidator) Activator.CreateInstance(validatorType, null);

            var result = validator.Validate(argument);
            if (!result.IsValid)
            {
                return result.Errors;
            }
            return null;

        }

        [Serializable]
        class ParameterValidation
        {
            public ParameterInfo Parameter { get; set; }

            public Func<object> DefaultValue { get; set; }

            public bool HasDefaultValue
            {
                get
                {
                    return DefaultValue != null;
                }
            }

            public IEnumerable<Type> ValidationRules { get; set; }
        }
    }
}