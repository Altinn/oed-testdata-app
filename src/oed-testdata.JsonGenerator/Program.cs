// See https://aka.ms/new-console-template for more information

using oed_testdata.JsonGenerator.Models.Da;
using oed_testdata.JsonGenerator.Models.Tenor;
using System.Text.Json;

var cliArgs = Environment.GetCommandLineArgs();
if (cliArgs.Length is < 2 or > 3)
{
    Console.WriteLine($"Usage: {Path.GetFileName(cliArgs[0])} <tenor-file.json> [(optional)output-path]");
    return -1;
}

var tenorFile = cliArgs[1];
try
{
    var tenorPerson = await TenorPersonFile.ValidateAndDeserialize(tenorFile);
    var daData = DaFile.New(tenorPerson.id, Guid.NewGuid());

    var daCase = daData.DaCaseList.Single();
    daCase.Parter = tenorPerson.tenorRelasjoner.freg
        .Select(freg =>
            new Parter
            {
                Nin = freg.id,
                Role = PartRoleConverter.Convert(freg.tenorRelasjonsnavn),
                Formuesfullmakt = true,
                PaatarGjeldsansvar = false,
                GodkjennerSkifteattest = false
            })
        .ToArray();

    var metadata = new EstateMetadata
    {
        Persons = tenorPerson.tenorRelasjoner.freg
            .Select(freg =>
                new EstateMetadataPerson
                {
                    Nin = freg.id,
                    Name = $"{freg.fornavn} {freg.etternavn}"
                })
            .ToArray()
    };

    await DaFile.SerializeAndWrite(daData, tenorPerson.visningnavn);
    await MetadataFile.SerializeAndWrite(metadata, tenorPerson.id);
}
catch (ArgumentException ae)
{
    Console.WriteLine(ae.Message);
    return -1;
}

return 0;
