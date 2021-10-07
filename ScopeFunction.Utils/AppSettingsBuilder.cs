using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using PeanutButter.DuckTyping.Exceptions;
using PeanutButter.DuckTyping.Extensions;
using PeanutButter.Utils;

namespace ScopeFunction.Utils
{
    public static class AppSettingsBuilder
    {
        public static T CreateAppSettings<T>() where T : class
        {
            return GetSettingsFrom<T>(
                CreateConfigurationRoot()
            );
        }

        public static IConfigurationRoot CreateConfigurationRoot()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            const string deployConfig = "appsettings.deploy.json";
            if (CanLoad(deployConfig))
            {
                builder.AddJsonFile(deployConfig);
            }

            return builder.Build();
        }

        private static bool CanLoad(string deployConfig)
        {
            if (!File.Exists(deployConfig)) return false;

            var lines = File.ReadAllLines(deployConfig);
            var re = new Regex("#{.+}");
            return lines.All(l => !re.Match(l).Success);
        }

        private static T GetSettingsFrom<T>(
            IConfigurationRoot config) where T : class
        {
            var providedConfig = config
                ?.GetSection("Settings")
                ?.GetSection("ConnectionStrings")
                ?.GetChildren()
                ?.ToDictionary(s => s.Key, s => s.Value);
            try
            {
                return providedConfig.FuzzyDuckAs<T>(true);
            }
            catch (UnDuckableException ex)
            {
                Console.WriteLine(ex.Errors.JoinWith("\n"));
                throw;
            }
        }
    }
}