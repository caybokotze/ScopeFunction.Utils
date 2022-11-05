using System;
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
        
        public static TDestination? MapTo<TSource, TDestination>(this TSource? objectSource)
            where TSource : class where TDestination : class
        {
            if (objectSource is null)
            {
                return null;
            }
            
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
            });

            var mapper = new Mapper(config);

            return mapper.Map<TSource, TDestination>(objectSource);
        }

        public static TDestination? MapTo<TSource, TDestination>(this TSource? objectSource, TDestination destination)
            where TSource : class where TDestination : class
        {
            if (objectSource is null)
            {
                return null;
            }
            
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<TSource, TDestination>(); });

            var mapper = new Mapper(config);

            return mapper.Map(objectSource, destination);
        }

        public static TDestination? MapTo<TSource, TDestination>(this TSource? objectSource, TDestination destination,
            Action<TSource> overrideSource) where TSource : class where TDestination : class
        {
            if (objectSource is null)
            {
                return null;
            }
            
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<TSource, TDestination>(); });

            var mapper = new Mapper(config);

            overrideSource(objectSource);

            return mapper.Map(objectSource, destination);
        }
    }
}