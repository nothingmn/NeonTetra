//https://raw.githubusercontent.com/aspnet/Configuration/dev/src/Microsoft.Extensions.Configuration.Binder/ConfigurationBinder.cs

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;

//using Microsoft.Extensions.Configuration.Binder;

namespace NeonTetra.Core.Configuration
{
    /// <summary>
    ///     Static helper class that allows binding strongly typed objects to configuration values.
    /// </summary>
    public static class ConfigurationBinder
    {
        /// <summary>
        ///     Attempts to bind the configuration instance to a new instance of type T.
        ///     If this configuration section has a value, that will be used.
        ///     Otherwise binding by matching property names against configuration keys recursively.
        /// </summary>
        /// <typeparam name="T">The type of the new instance to bind.</typeparam>
        /// <param name="configuration">The configuration instance to bind.</param>
        /// <returns>The new instance of T if successful, default(T) otherwise.</returns>
        public static T Get<T>(this IConfiguration configuration, Func<Type, object> activator)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var result = configuration.Get(typeof(T), activator);
            if (result == null) return default;
            return (T) result;
        }

        /// <summary>
        ///     Attempts to bind the configuration instance to a new instance of type T.
        ///     If this configuration section has a value, that will be used.
        ///     Otherwise binding by matching property names against configuration keys recursively.
        /// </summary>
        /// <param name="configuration">The configuration instance to bind.</param>
        /// <param name="type">The type of the new instance to bind.</param>
        /// <returns>The new instance if successful, null otherwise.</returns>
        public static object Get(this IConfiguration configuration, Type type, Func<Type, object> activator)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return BindInstance(type, null, configuration, activator);
        }

        /// <summary>
        ///     Attempts to bind the given object instance to the configuration section specified by the key by matching property
        ///     names against configuration keys recursively.
        /// </summary>
        /// <param name="configuration">The configuration instance to bind.</param>
        /// <param name="key">The key of the configuration section to bind.</param>
        /// <param name="instance">The object to bind.</param>
        public static void Bind(this IConfiguration configuration, string key, object instance,
            Func<Type, object> activator)
        {
            configuration.GetSection(key).Bind(instance, activator);
        }


        private static object DefaultActivator(Type type)
        {
            return Activator.CreateInstance(type);
        }

        /// <summary>
        ///     Attempts to bind the given object instance to configuration values by matching property names against configuration
        ///     keys recursively.
        /// </summary>
        /// <param name="configuration">The configuration instance to bind.</param>
        /// <param name="instance">The object to bind.</param>
        /// <param name="activator"></param>
        public static void Bind(this IConfiguration configuration, object instance, Func<Type, object> activator)
        {
            if (activator == null) activator = DefaultActivator;
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            if (instance != null) BindInstance(instance.GetType(), instance, configuration, activator);
        }

        /// <summary>
        ///     Extracts the value with the specified key and converts it to type T.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="configuration">The configuration.</param>
        /// <param name="key">The key of the configuration section's value to convert.</param>
        /// <returns>The converted value.</returns>
        public static T GetValue<T>(this IConfiguration configuration, string key)
        {
            return GetValue(configuration, key, default(T));
        }

        /// <summary>
        ///     Extracts the value with the specified key and converts it to type T.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="configuration">The configuration.</param>
        /// <param name="key">The key of the configuration section's value to convert.</param>
        /// <param name="defaultValue">The default value to use if no value is found.</param>
        /// <returns>The converted value.</returns>
        public static T GetValue<T>(this IConfiguration configuration, string key, T defaultValue)
        {
            return (T) GetValue(configuration, typeof(T), key, defaultValue);
        }

        /// <summary>
        ///     Extracts the value with the specified key and converts it to the specified type.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="type">The type to convert the value to.</param>
        /// <param name="key">The key of the configuration section's value to convert.</param>
        /// <returns>The converted value.</returns>
        public static object GetValue(this IConfiguration configuration, Type type, string key)
        {
            return GetValue(configuration, type, key, null);
        }

        /// <summary>
        ///     Extracts the value with the specified key and converts it to the specified type.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="type">The type to convert the value to.</param>
        /// <param name="key">The key of the configuration section's value to convert.</param>
        /// <param name="defaultValue">The default value to use if no value is found.</param>
        /// <returns>The converted value.</returns>
        public static object GetValue(this IConfiguration configuration, Type type, string key, object defaultValue)
        {
            var value = configuration.GetSection(key).Value;
            if (value != null) return ConvertValue(type, value);
            return defaultValue;
        }

        private static void BindNonScalar(this IConfiguration configuration, object instance,
            Func<Type, object> activator)
        {
            if (instance != null)
                foreach (var property in GetAllProperties(instance.GetType().GetTypeInfo()))
                    BindProperty(property, instance, configuration, activator);
        }

        private static void BindProperty(PropertyInfo property, object instance, IConfiguration config,
            Func<Type, object> activator)
        {
            // We don't support set only, non public, or indexer properties
            if (property.GetMethod == null ||
                !property.GetMethod.IsPublic ||
                property.GetMethod.GetParameters().Length > 0)
                return;

            var propertyValue = property.GetValue(instance);
            var hasPublicSetter = property.SetMethod != null && property.SetMethod.IsPublic;

            if (propertyValue == null && !hasPublicSetter) return;

            propertyValue = BindInstance(property.PropertyType, propertyValue, config.GetSection(property.Name),
                activator);

            if (propertyValue != null && hasPublicSetter) property.SetValue(instance, propertyValue);
        }

        private static object BindToCollection(TypeInfo typeInfo, IConfiguration config, Func<Type, object> activator)
        {
            var type = typeof(IList<>).MakeGenericType(typeInfo.GenericTypeArguments[0]);
            var instance = activator.Invoke(type);
            BindCollection(instance, type, config, activator);
            return instance;
        }

        // Try to create an array/dictionary instance to back various collection interfaces
        private static object AttemptBindToCollectionInterfaces(Type type, IConfiguration config,
            Func<Type, object> activator)
        {
            var typeInfo = type.GetTypeInfo();

            if (!typeInfo.IsInterface) return null;

            var collectionInterface = FindOpenGenericInterface(typeof(IReadOnlyList<>), type);
            if (collectionInterface != null) return BindToCollection(typeInfo, config, activator);

            collectionInterface = FindOpenGenericInterface(typeof(IReadOnlyDictionary<,>), type);
            if (collectionInterface != null)
            {
                var dictionaryType = typeof(Dictionary<,>).MakeGenericType(typeInfo.GenericTypeArguments[0],
                    typeInfo.GenericTypeArguments[1]);
                var instance = activator.Invoke(dictionaryType);
                BindDictionary(instance, dictionaryType, config, activator);
                return instance;
            }

            collectionInterface = FindOpenGenericInterface(typeof(IDictionary<,>), type);
            if (collectionInterface != null)
            {
                var instance = activator.Invoke(typeof(Dictionary<,>).MakeGenericType(typeInfo.GenericTypeArguments[0],
                    typeInfo.GenericTypeArguments[1]));
                BindDictionary(instance, collectionInterface, config, activator);
                return instance;
            }

            collectionInterface = FindOpenGenericInterface(typeof(IReadOnlyCollection<>), type);
            if (collectionInterface != null) return BindToCollection(typeInfo, config, activator);

            collectionInterface = FindOpenGenericInterface(typeof(ICollection<>), type);
            if (collectionInterface != null) return BindToCollection(typeInfo, config, activator);

            collectionInterface = FindOpenGenericInterface(typeof(IEnumerable<>), type);
            if (collectionInterface != null) return BindToCollection(typeInfo, config, activator);

            return null;
        }

        private static object BindInstance(Type type, object instance, IConfiguration config,
            Func<Type, object> activator)
        {
            // if binding IConfigurationSection, break early
            if (type == typeof(IConfigurationSection)) return config;

            var section = config as IConfigurationSection;
            var configValue = section?.Value;
            Exception error;
            object convertedValue;
            if (configValue != null && TryConvertValue(type, configValue, out convertedValue, out error))
            {
                if (error != null) throw error;

                // Leaf nodes are always reinitialized
                return convertedValue;
            }

            if (config != null && config.GetChildren().Any())
            {
                // If we don't have an instance, try to create one
                if (instance == null)
                {
                    // We are alrady done if binding to a new collection instance worked
                    instance = AttemptBindToCollectionInterfaces(type, config, activator);
                    if (instance != null) return instance;

                    instance = activator.Invoke(type);
                }

                // See if its a Dictionary
                var collectionInterface = FindOpenGenericInterface(typeof(IDictionary<,>), type);
                if (collectionInterface != null)
                {
                    BindDictionary(instance, collectionInterface, config, activator);
                }
                else if (type.IsArray)
                {
                    instance = BindArray((Array) instance, config, activator);
                }
                else
                {
                    // See if its an ICollection
                    collectionInterface = FindOpenGenericInterface(typeof(ICollection<>), type);
                    if (collectionInterface != null)
                        BindCollection(instance, collectionInterface, config, activator);
                    // Something else
                    else
                        BindNonScalar(config, instance, activator);
                }
            }

            return instance;
        }

        private static void BindDictionary(object dictionary, Type dictionaryType, IConfiguration config,
            Func<Type, object> activator)
        {
            var typeInfo = dictionaryType.GetTypeInfo();

            // IDictionary<K,V> is guaranteed to have exactly two parameters
            var keyType = typeInfo.GenericTypeArguments[0];
            var valueType = typeInfo.GenericTypeArguments[1];
            var keyTypeIsEnum = keyType.GetTypeInfo().IsEnum;

            if (keyType != typeof(string) && !keyTypeIsEnum) return;

            var addMethod = typeInfo.GetDeclaredMethod("Add");
            foreach (var child in config.GetChildren())
            {
                var item = BindInstance(
                    valueType,
                    null,
                    child,
                    activator);
                if (item != null)
                {
                    if (keyType == typeof(string))
                    {
                        var key = child.Key;
                        addMethod.Invoke(dictionary, new[] {key, item});
                    }
                    else if (keyTypeIsEnum)
                    {
                        var key = Convert.ToInt32(Enum.Parse(keyType, child.Key));
                        addMethod.Invoke(dictionary, new[] {key, item});
                    }
                }
            }
        }

        private static void BindCollection(object collection, Type collectionType, IConfiguration config,
            Func<Type, object> activator)
        {
            var typeInfo = collectionType.GetTypeInfo();
            var type = collection.GetType().GetTypeInfo();

            // ICollection<T> is guaranteed to have exactly one parameter
            var itemType = typeInfo.GenericTypeArguments[0];
            var addMethod = type.GetMethod("Add");
            var clearMethod = type.GetMethod("Clear"); //just adding items now, make sure the collection is empty
            if (clearMethod != null) clearMethod.Invoke(collection, null);

            foreach (var section in config.GetChildren())
                try
                {
                    var item = BindInstance(
                        itemType,
                        null,
                        section,
                        activator
                    );
                    if (item != null)
                        if (addMethod != null)
                            addMethod.Invoke(collection, new[] {item});
                }
                catch
                {
                }
        }

        private static Array BindArray(Array source, IConfiguration config, Func<Type, object> activator)
        {
            var children = config.GetChildren().ToArray();
            var arrayLength = source.Length;
            var elementType = source.GetType().GetElementType();
            var newArray = Array.CreateInstance(elementType, arrayLength + children.Length);

            // binding to array has to preserve already initialized arrays with values
            if (arrayLength > 0) Array.Copy(source, newArray, arrayLength);

            for (var i = 0; i < children.Length; i++)
                try
                {
                    var item = BindInstance(
                        elementType,
                        null,
                        children[i],
                        activator);
                    if (item != null) newArray.SetValue(item, arrayLength + i);
                }
                catch
                {
                }

            return newArray;
        }

        private static bool TryConvertValue(Type type, string value, out object result, out Exception error)
        {
            error = null;
            result = null;
            if (type == typeof(object))
            {
                result = value;
                return true;
            }

            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (string.IsNullOrEmpty(value)) return true;
                return TryConvertValue(Nullable.GetUnderlyingType(type), value, out result, out error);
            }

            var converter = TypeDescriptor.GetConverter(type);
            if (converter.CanConvertFrom(typeof(string)))
            {
                try
                {
                    result = converter.ConvertFromInvariantString(value);
                }
                catch (Exception ex)
                {
                    error = new InvalidOperationException(string.Concat("Failed to bind:", value, ":", type), ex);
                }

                return true;
            }

            return false;
        }

        private static object ConvertValue(Type type, string value)
        {
            object result;
            Exception error;
            TryConvertValue(type, value, out result, out error);
            if (error != null) throw error;
            return result;
        }

        private static Type FindOpenGenericInterface(Type expected, Type actual)
        {
            var actualTypeInfo = actual.GetTypeInfo();
            if (actualTypeInfo.IsGenericType &&
                actual.GetGenericTypeDefinition() == expected)
                return actual;

            var interfaces = actualTypeInfo.ImplementedInterfaces;
            foreach (var interfaceType in interfaces)
                if (interfaceType.GetTypeInfo().IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() == expected)
                    return interfaceType;
            return null;
        }

        private static IEnumerable<PropertyInfo> GetAllProperties(TypeInfo type)
        {
            var allProperties = new List<PropertyInfo>();

            do
            {
                allProperties.AddRange(type.DeclaredProperties);
                type = type.BaseType.GetTypeInfo();
            } while (type != typeof(object).GetTypeInfo());

            return allProperties;
        }
    }
}