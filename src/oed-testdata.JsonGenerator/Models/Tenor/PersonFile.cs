using System.Text.Json;

namespace oed_testdata.JsonGenerator.Models.Tenor
{
    internal static class TenorPersonFile
    {
        public static async Task<Person> ValidateAndDeserialize(string tenorFile)
        {
            if (!File.Exists(tenorFile))
                throw new ArgumentException($"File does not exist: {tenorFile}");

            await using var filestream = File.OpenRead(tenorFile);

            var person = await JsonSerializer.DeserializeAsync<Person>(filestream);
            if (person is null)
                throw new ArgumentException($"Not a valid tenor person file: {tenorFile}");

            if (person.personstatus != "doed")
                throw new ArgumentException($"Person in tenor file is not dead ({person.personstatus}): {tenorFile}");

            return person;
        }
    }

}
