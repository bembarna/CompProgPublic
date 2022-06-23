using CompProgEdu.Core.Data;
using FluentValidation;
using FluentValidation.Validators;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CompProgEdu.Core.Features.Extensions
{
    public static class ValidationExtension
    {
        public static IRuleBuilderOptions<TRequest, int> EntityMustExist<TRequest, TEntity>(this IRuleBuilder<TRequest, int> ruleBuilder, DataContext dataContext)
        where TEntity : class, IAuditableEntity

        {
            return ruleBuilder.Must(MustExist)
                .WithMessage($"{typeof(TEntity).Name} could not be found!");

            bool MustExist(int entityId)
            {
                var entity = dataContext.Set<TEntity>().FirstOrDefault(x => x.Id == entityId);
                
                var condition = entity == null;

                return !condition;
            }
        }

        public static IRuleBuilderOptions<TRequest, T> IsUniqueWithDbList<TRequest, T>(this IRuleBuilder<TRequest, T> ruleBuilder, List<T> itemList)

        {
            return ruleBuilder.Must(EntityMustBeUnique)
                .WithMessage($"Must be unique");

            bool EntityMustBeUnique(T item)
            {
               var test = itemList.Contains(item);

                return !test;
            }
        }


        public static IRuleBuilderOptions<TRequest, T> IsUnique<TRequest, TEntity, T>(this IRuleBuilder<TRequest, T> ruleBuilder, DataContext dataContext)
            where TEntity : class, IAuditableEntity
        {
            var items = dataContext.Set<TEntity>().ToList();

            return ruleBuilder.SetValidator(new UniqueValidator<TEntity>(items));
        }


        public static IRuleBuilderOptions<TRequest, string> Password<TRequest>(this IRuleBuilder<TRequest, string> ruleBuilder)
        {
            var hasAtLeastEightCharacters = new Regex(@"(?=.{8,}$)");
            var hasAtLeastOneNumber = new Regex(@"[0-9]+");
            var hasAtLeastOneUpperChar = new Regex(@"[A-Z]+");
            var hasAtLeastOneLowerChar = new Regex(@"[a-z]+");
            var hasAtLeastOneSpecialChar = new Regex(@"(?=.*\W)");

            return ruleBuilder
                .NotEmpty()
                .Must(x => hasAtLeastEightCharacters.IsMatch(x ?? ""))
                .WithMessage("Password must be at least eight characters long.")
                .Must(x => hasAtLeastOneNumber.IsMatch(x ?? ""))
                .WithMessage("Password must contain at least one number.")
                .Must(x => hasAtLeastOneUpperChar.IsMatch(x ?? ""))
                .WithMessage("Password must contain at least one uppercase character.")
                .Must(x => hasAtLeastOneLowerChar.IsMatch(x ?? ""))
                .WithMessage("Password must contain at least one lowercase character.")
                .Must(x => hasAtLeastOneSpecialChar.IsMatch(x ?? ""))
                .WithMessage("Password must contain at least one special character.");
        }
    }

    public class UniqueValidator<T> : PropertyValidator
    where T : class, IAuditableEntity
    {
        private readonly T _tEntity;
        private readonly List<T> _items;

        public UniqueValidator(List<T> items)
          : base("{PropertyName} must be unique")
        {
            _items = items;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var identifiablePropName = MethodExtensions.GetPropertyNameAsString(
                () => _tEntity.Id);
            var editedProperty = context.InstanceToValidate;
            var editedPropertyId = editedProperty.GetType().GetProperty(identifiablePropName)?.GetValue(editedProperty, null) ?? 0;
            var newValue = context.PropertyValue;
            var property =  typeof(T).GetTypeInfo().GetProperty(context.PropertyName);
            var newList = _items.Where(x => x.Id != (int)editedPropertyId).Select(x => property.GetValue(x)).ToList();      
            return !newList.Contains(newValue);
        }
    }

}
