﻿using System;
using System.Collections.Generic;
using System.Linq;
using HotChocolate.Language;
using HotChocolate.Types.Descriptors.Definitions;

namespace HotChocolate.Types.Descriptors
{
    public class EnumTypeDescriptor
        : DescriptorBase<EnumTypeDefinition>
        , IEnumTypeDescriptor
    {
        public EnumTypeDescriptor(IDescriptorContext context, NameString name)
            : base(context)
        {
            Definition.ClrType = typeof(object);
            Definition.Name = name.EnsureNotEmpty(nameof(name));
        }

        public EnumTypeDescriptor(IDescriptorContext context, Type enumType)
            : base(context)
        {
            Definition.ClrType = enumType
                ?? throw new ArgumentNullException(nameof(enumType));
            Definition.Name = context.Naming.GetTypeName(enumType);
            Definition.Description =
                context.Naming.GetTypeDescription(enumType);
        }

        protected override EnumTypeDefinition Definition { get; }

        protected ICollection<EnumValueDescriptor> Values { get; } =
            new List<EnumValueDescriptor>();

        protected override void OnCreateDefinition(
            EnumTypeDefinition definition)
        {
            Dictionary<object, EnumValueDefinition> values =
                Values.Select(t => t.CreateDefinition())
                   .ToDictionary(t => t.Value);
            AddImplicitValues(definition, values);

            definition.Values.Clear();

            foreach (EnumValueDefinition value in values.Values)
            {
                definition.Values.Add(value);
            }
        }

        protected void AddImplicitValues(
            EnumTypeDefinition typeDefinition,
            IDictionary<object, EnumValueDefinition> values)
        {
            if (typeDefinition.Values.IsImplicitBinding()
                && typeDefinition.ClrType != typeof(object)
                && typeDefinition.ClrType.IsEnum)
            {
                foreach (object value in Enum.GetValues(typeDefinition.ClrType))
                {
                    EnumValueDefinition valueDefinition =
                        EnumValueDescriptor.New(Context, value)
                            .CreateDefinition();

                    if (!values.ContainsKey(valueDefinition.Value))
                    {
                        values.Add(valueDefinition.Value, valueDefinition);
                    }
                }
            }
        }

        public IEnumTypeDescriptor SyntaxNode(
            EnumTypeDefinitionNode enumTypeDefinition)
        {
            Definition.SyntaxNode = enumTypeDefinition;
            return this;
        }

        public IEnumTypeDescriptor Name(NameString value)
        {
            Definition.Name = value.EnsureNotEmpty(nameof(value));
            return this;
        }

        public IEnumTypeDescriptor Description(string value)
        {
            Definition.Description = value;
            return this;
        }

        public IEnumTypeDescriptor BindItems(
            BindingBehavior behavior)
        {
            Definition.Values.BindingBehavior = behavior;
            return this;
        }

        public IEnumValueDescriptor Item<T>(T value)
        {
            if (Definition.ClrType == null)
            {
                Definition.ClrType = typeof(T);
            }

            var descriptor = new EnumValueDescriptor(Context, value);
            Values.Add(descriptor);
            return descriptor;
        }

        public IEnumTypeDescriptor Directive<T>(T instance)
            where T : class
        {
            Definition.AddDirective(instance);
            return this;
        }

        public IEnumTypeDescriptor Directive<T>()
            where T : class, new()
        {
            Definition.AddDirective(new T());
            return this;
        }

        public IEnumTypeDescriptor Directive(
            NameString name, params ArgumentNode[] arguments)
        {
            Definition.AddDirective(name, arguments);
            return this;
        }
    }
}