using System;
using AutoMapper;

namespace ScopeFunction.Utils
{
    public static class AutoMap
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource objectSource)
            where TSource : class where TDestination : class
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<TSource, TDestination>(); });

            var mapper = new Mapper(config);

            return mapper.Map<TSource, TDestination>(objectSource);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource objectSource, TDestination destination)
            where TSource : class where TDestination : class
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<TSource, TDestination>(); });

            var mapper = new Mapper(config);

            return mapper.Map(objectSource, destination);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource objectSource, TDestination destination,
            Action<TSource> overrideSource) where TSource : class where TDestination : class
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<TSource, TDestination>(); });

            var mapper = new Mapper(config);

            overrideSource(objectSource);

            return mapper.Map(objectSource, destination);
        }
    }
}