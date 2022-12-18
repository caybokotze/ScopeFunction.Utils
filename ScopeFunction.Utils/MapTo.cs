using System;
using System.Linq;
using System.Runtime.CompilerServices;
using AutoMapper;

namespace ScopeFunction.Utils
{
    public static class AutoMap
    {
        public static TDestination? MapTo<TDestination>(this object? source) where TDestination : class, new()
        {
            if (source is null)
            {
                return null;
            }
            
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap(source.GetType(), typeof(TDestination));
            });

            var mapper = new Mapper(config);

            return mapper.Map(source, new TDestination());
        }
        
        public static TDestination? MapTo<TSource, TDestination>(this TSource? source)
            where TSource : class where TDestination : class
        {
            if (source is null)
            {
                return null;
            }
            
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
            });

            var mapper = new Mapper(config);

            return mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination? MapTo<TSource, TDestination>(this TSource? source, TDestination destination)
            where TSource : class where TDestination : class
        {
            if (source is null)
            {
                return null;
            }
            
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<TSource, TDestination>(); });

            var mapper = new Mapper(config);

            return mapper.Map(source, destination);
        }

        public static TDestination? MapTo<TSource, TDestination>(this TSource? source, TDestination destination,
            Func<TSource, string[]> ignoreSourceValue) where TSource : class where TDestination : class
        {
            if (source is null)
            {
                return null;
            }
            
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<TSource, TDestination>(); });

            var mapper = new Mapper(config);

            var mappings = ignoreSourceValue(source);

            foreach (var property in source.GetType().GetProperties())
            {
                if (!mappings.Contains(property.Name))
                {
                    continue;
                }
                
                var destinationValue = destination.GetType().GetProperty(property.Name)?.GetValue(destination);
                if (destinationValue is null)
                {
                    continue;
                }
                
                property.SetValue(source, destinationValue);
            }

            return mapper.Map(source, destination);
        }
    }
}