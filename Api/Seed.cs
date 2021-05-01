using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Api
{
    public static class Seed
    {
        private const int NUMBER_OF_STORES = 40;
        private const string MOCK_DATA_STORES_FILE_PATH = "./MockData/MockDataStores.json";
        private const string MOCK_DATA_SECTIONS_FILE_PATH = @"./MockData/MockDataSections.json";
        private static ApplicationDbContext _context;
        private static IEnumerable<Store> Stores { get; set; }
        private static IEnumerable<Section> Sections { get; set; }
        private static Random rnd = new Random();

        public async static Task Start(IServiceProvider services)
        {
            _context = services.GetRequiredService<ApplicationDbContext>();
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();

            Stores = JsonConvert.DeserializeObject<IEnumerable<Store>>(await File.ReadAllTextAsync(MOCK_DATA_STORES_FILE_PATH))
                .GroupBy(s => s.Name).Select(g => g.First());
            Sections = JsonConvert.DeserializeObject<IEnumerable<Section>>(await File.ReadAllTextAsync(MOCK_DATA_SECTIONS_FILE_PATH))
                .GroupBy(s => s.Name).Select(g => g.First());

            await CreateStores();
        }

        private async static Task CreateStores()
        {
            var storeCount = Stores.Count();
            for (int i = 0; i < NUMBER_OF_STORES; i++)
            {
                if (i >= storeCount) break;
                var store = Stores.Skip(i).First();
                store.MaximumPeopleAllowed = rnd.Next(20, 100);
                store.Sections = CreateSections(rnd.Next(1, 8), store.MaximumPeopleAllowed);
                await _context.AddAsync(store);
            }
            await _context.SaveChangesAsync();
        }

        private static IList<Section> CreateSections(int count, int maximumPeopleAllowed)
        {
            var sections = new List<Section>();
            var sectionCount = Sections.Count();
            for (int i = 0; i < count; i++)
            {
                string pick = null;
                while (pick == null || sections.Any(s => s != null && s.Name == pick))
                {
                    pick = Sections.Skip(rnd.Next(0, sectionCount)).First().Name;
                }
                sections.Add(new Section() { Name = pick });
            }
            return sections;
        }
    }

}
