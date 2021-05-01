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
        private const int NUMBER_OF_STORES = 10;
        private const string MOCK_DATA_STORES_FILE_PATH = "./MockData/MockDataStores.json";
        private const string MOCK_DATA_SECTIONS_FILE_PATH = @"./MockData/MockDataSections.json";
        private static ApplicationDbContext _context;
        private static Random rnd = new Random();
        private static IEnumerable<Store> Stores { get; set; }
        private static IEnumerable<Section> Sections { get; set; }

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
                var numberOfSections = rnd.Next(1, 5);
                var sections = new List<Section>();
                for (int j = 0; j < numberOfSections; j++)
                {
                    string pick = null;
                    while (pick == null || sections.Any(s => s != null && s.Name == pick))
                    {
                        pick = Sections.Skip(rnd.Next(0, Sections.Count())).First().Name;
                    }
                    sections.Add(new Section() { Name = pick });
                }
                var store = Stores.Skip(rnd.Next(0, storeCount)).First();
                store.Sections = sections;
                await _context.AddAsync(store);
                sections = new List<Section>();
            }
            await _context.SaveChangesAsync();
        }

    }

}
