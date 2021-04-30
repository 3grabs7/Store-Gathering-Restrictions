using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ShopGatheringRestrictionsApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopGatheringRestrictionsApi
{
    public static class Seed
    {
        private const int NUMBER_OF_STORES = 10;
        private const string MOCK_DATA_STORES_FILE_PATH = "./MockData/MockDataStores.json";
        private const string MOCK_DATA_SECTIONS_FILE_PATH = @"./MockData/MockDataSections.json";
        private static ApplicationDbContext _context;
        private static Random rnd = new Random();
        private static IEnumerable<string> StoreNames { get; set; }
        private static IEnumerable<string> SectionNames { get; set; }
        private enum Section
        {

        }
        public async static Task Start(IServiceProvider services)
        {
            _context = services.GetRequiredService<ApplicationDbContext>();
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();

            StoreNames = JsonConvert.DeserializeObject<IEnumerable<StoreNames>>(await File.ReadAllTextAsync(MOCK_DATA_STORES_FILE_PATH))
                .Select(s => s.Store)
                .Distinct();
            SectionNames = JsonConvert.DeserializeObject<IEnumerable<SectionNames>>(await File.ReadAllTextAsync(MOCK_DATA_SECTIONS_FILE_PATH))
                .Select(s => s.Section)
                .Distinct();

            CreateStores();
            await _context.SaveChangesAsync();
        }

        private static void CreateStores()
        {
            var numberOfSections = rnd.Next(1, 5);
            var sections = new string[numberOfSections];
            for (int i = 0; i < SectionNames.Count(); i++)
            {
                string pick = null;
                while (pick == null || !SectionNames.Contains(pick))
                {
                    pick = SectionNames.Skip(rnd.Next(0, SectionNames.Count())).First();
                }
                sections[i] = pick;
            }
            for (int i = 0; i < NUMBER_OF_STORES; i++)
            {
                var store = new Store()
                {
                    Name = StoreNames.Skip(rnd.Next(0, StoreNames.Count())).First()
                };
            }
        }
        private static void GenerateSections(Store store)
        {

        }
    }
    public class StoreNames
    {
        public string Store { get; set; }
    }
    public class SectionNames
    {
        public string Section { get; set; }
    }
}
